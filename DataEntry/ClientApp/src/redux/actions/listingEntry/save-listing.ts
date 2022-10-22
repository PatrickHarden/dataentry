import { State, MainMessageType, RedirectAction } from '../../../types/state';
import { Listing } from '../../../types/listing/listing';
import { setMainMessage, clearMessage } from '../system/set-main-message';
import { postData } from '../../../api/glAxios'
import { updateListingQuery, createNewListingQuery } from '../../../api/glQueries'
import { publishListing } from '../listingEntry/publish-listing';
import { exportListing } from '../listingEntry/export-listing';
import { setAlertMessage } from '../../../redux/actions/system/set-alert-message';
import { AlertMessagingType } from '../../../types/state';
import { Config } from '../../../types/config/config';
import { currentListingLoaded} from '../../../redux/actions/listingEntry/load-current-listing';
import { setReloadListingId } from '../system/set-reload-listing-id';
import { updateSavePending } from './update-pending-operation';
import { resetPagedListings } from '../pagedListings/load-listings-paged';
import { clearMIQData } from '../miq/clear-miq-data';
import { checkAssignFlag } from '../../../utils/miq/remove-assign-flag';
import { showMIQExportError, showMIQExportSuccess } from '../miq/set-miq-export-message';
import { getAssignedListingsCount } from '../miq/get-assigned-listings-count';
import { push } from 'connected-react-router';
import { authContext } from '../../../adalConfig';
import { TeamMember } from '../../../types/team/teamMember';
import { setIsDuplicatingListing } from '../system/set-is-duplicating-listing';
import { unpublishListing, UnpublishType } from './unpublish-listing';
import { setMIQStatus } from '../miq/set-miq-status';
import moment from 'moment';
import { Space } from '../../../types/listing/space'

const saveData = (listing:Listing, config:Config, regionId?: string) => {
    let data:any;

    if (listing.id){
        const query = updateListingQuery(listing, config, regionId);
        data = postData(query)
    } else {    
        const query = createNewListingQuery(listing, config, regionId)
        data = postData(query);
    }
    
    return data;
}

export enum SaveType {
    SAVE = "SAVE",
    PUBLISH = "PUBLISH",
    EXPORT = "EXPORT",
    ASSIGN = "ASSIGN",
    DUPLICATE = "DUPLICATE", // Save the current listing before duplicating. On success begin a 2nd save with type = SAVEDUPLICATE
    SAVEDUPLICATE = "SAVEDUPLICATE", // Save a duplicate of the listing by calling CreateListing with the same data
    UNPUBLISH_FROM_MIQ = "UNPUBLISH",
}

// thunks
export const saveListing = (listing:Listing, saveType:SaveType, config:Config) => (dispatch: Function, getState: () => State) => {
    
    console.log("saveListing", listing);
    // this helps us ensure only one save operation is occurring at a time.
    if(getState().entry.pendingOperations.savePending === true && saveType !== "SAVEDUPLICATE"){
        return;
    }
    
    if (!listing.id && !listing.owner) {
        const currentUser = authContext.getCachedUser();
        listing.owner = currentUser.userName;
        const ownerUser: TeamMember = { 
            teamMemberId: "", 
            firstName: currentUser.profile.given_name, 
            lastName: currentUser.profile.family_name, 
            id: currentUser.userName, 
            email: currentUser.userName };
        if (!listing.users) {
            listing.users = [];
        }
        listing.users.push(ownerUser);
    }

    // Check if the currect authContext user exists in the listing's user data, if not, add user to the listing before save
    if (listing.users && listing.users.length > 0){
        const currentUser = authContext.getCachedUser();
        let matchFound = false;
        listing.users.forEach((user: TeamMember) => { // do an email match
            if (user.id === currentUser.userName || user.email === currentUser.userName || user.email === currentUser.profile.upn || user.id === currentUser.profile.unique_name){
                matchFound = true;
            }
        })
        
        if (!matchFound){
            const ownerUser: TeamMember = { 
                teamMemberId: "", 
                firstName: currentUser.profile.given_name, 
                lastName: currentUser.profile.family_name, 
                id: currentUser.userName, 
                email: currentUser.userName 
            };
            listing.users.push(ownerUser);
        }
    }

    if (getState().miq.pendingAddEditRecord) {
        listing.importedData = JSON.stringify(getState().miq.pendingAddEditRecord);
    }

    // set save pending = true to avoid double operations occurring
    dispatch(updateSavePending(true));  

    let messageToUser = "Saving Listing...";
    if(saveType === SaveType.PUBLISH){
        messageToUser = "Publishing Listing...";
    }else if(saveType === SaveType.EXPORT){
        messageToUser = "Exporting...";
    }else if (saveType === SaveType.UNPUBLISH_FROM_MIQ){
        messageToUser = "Unpublishing Listing...";
    }else if (saveType === SaveType.SAVEDUPLICATE){
        messageToUser = "Duplicating Listing...";
    }
    dispatch(setMainMessage({ show: true, type: MainMessageType.SAVING, message: messageToUser }));

    // check for MIQ assignment to turn off the assigned flag on save
    let assignFlagChange:boolean = false;
    if(saveType !== SaveType.ASSIGN && checkAssignFlag(listing)){
        assignFlagChange = true;
        if(listing && listing.listingAssignment){
            listing.listingAssignment.assignmentFlag = false;
        }
    }

    // if the property is toggled as "Send to Toimitilat" and availablelFrom value is null then populating
    // the value with current date while publishing a record for Finland
    if( listing.syndicationFlag && getState().mapping.countryCode === "fi" && saveType === SaveType.PUBLISH)
    {
        if(!listing.availableFrom)
        {
            listing.availableFrom = moment().local().format("YYYY-MM-DDT11:00:00");
        }
        
        if(listing.spaces && listing.spaces.length > 0)
        {
            listing.spaces.forEach((space: Space) => {
                if (!space.availableFrom) {
                    space.availableFrom = moment().local().format("YYYY-MM-DDT11:00:00");
                }
            })
        }
    }
    const regionId = getState().mapping.selectedCountry;

    saveData(listing, config, regionId).then((result: any) => {

        // if data returns a create listing or update listing object, it worked & push to home      
        if (result && result.data && (result.data.createListing || result.data.updateListing)){
            
            resetPagedListings(dispatch, false);   // this will force a refresh on the listing page  
            dispatch(clearMIQData());     // ensure any miq data screens are reset

            // This flag is used by load-current-listing to indicate that the user clicked the duplicate button after we save changes to the original listing.
            dispatch(setIsDuplicatingListing(saveType === SaveType.DUPLICATE));

            if (result.data.createListing){
                clearMessage(dispatch);
                listing.id = result.data.createListing.id;     
                if (saveType !== SaveType.ASSIGN) {
                    dispatch(currentListingLoaded(listing));
                }
            }

            if (saveType === SaveType.PUBLISH) {
                if(assignFlagChange) { dispatch(getAssignedListingsCount) };
                dispatch(publishListing(listing));
            } else if (saveType === SaveType.EXPORT) {
                dispatch(exportListing(listing, config));
                dispatch(updateSavePending(false));
                clearMessage(dispatch); // this probably should be moved to the export thunk
            } else if (saveType === SaveType.ASSIGN) {
                clearMessage(dispatch);
                dispatch(setAlertMessage({ show: true, message: 'Listing Assigned Successfully', type: AlertMessagingType.NOTICE, allowClose: true }));
                dispatch(showMIQExportSuccess("Listing has been assigned to","the user(s) you have selected."));
                dispatch(updateSavePending(false));
            } else if (saveType === SaveType.DUPLICATE) {
                dispatch(setAlertMessage({ show: true, message: 'A copy has been made of your listing.  Be sure to save after making any necessary updates.', type: AlertMessagingType.NOTICE, allowClose: false }));
                dispatch(push(`/le/${listing.id}`));
                dispatch(setReloadListingId(listing.id));
                dispatch(updateSavePending(false));
            } else if (saveType === SaveType.SAVEDUPLICATE) {
                dispatch(setAlertMessage({ show: true, message: 'A copy of your listing has been saved.', type: AlertMessagingType.NOTICE, allowClose: true }));
                dispatch(push(`/le/${listing.id}`));
                dispatch(setReloadListingId(listing.id));
                dispatch(updateSavePending(false));
            } else if (saveType === SaveType.UNPUBLISH_FROM_MIQ) {
                dispatch(unpublishListing(listing, UnpublishType.MIQ_NEW));
            } else {
                clearMessage(dispatch);
                if(assignFlagChange) { dispatch(getAssignedListingsCount) };
                dispatch(setAlertMessage({ show: true, message: 'Listing Saved Successfully', type: AlertMessagingType.NOTICE, allowClose: true }));
                
                const saveAction:RedirectAction = getState().system.saveAction;
                if(saveAction && saveAction.goto){
                    dispatch(showMIQExportSuccess("Save Successful", "Your MIQ Listing has been created in Global Listings"));
                    dispatch(push(saveAction.goto));
                }else{
                    dispatch(push(`/le/${listing.id}`));
                    dispatch(setReloadListingId(listing.id));
                }
                dispatch(updateSavePending(false));
            }
        } else{
            // error state
            clearMessage(dispatch);
            dispatch(updateSavePending(false));
            
            if(saveType === SaveType.ASSIGN){
                dispatch(setAlertMessage({show: true, message: "There was an Error while saving.", type: AlertMessagingType.ERROR, allowClose: true}));
                dispatch(showMIQExportError("Error","There was an error assigning this listing."));
            } else if (saveType === SaveType.UNPUBLISH_FROM_MIQ) {
                dispatch(setAlertMessage({show: true, message: "Unable to automatically save a copy of your listing. Please use the create button to resolve the issues and unpublish the listing.", type: AlertMessagingType.ERROR, allowClose: true}));
                dispatch(setMIQStatus({loading: false, error: false, message: ""}));
            } else {
                dispatch(currentListingLoaded(listing));    // ensure the UI is refreshed with the copy we tried to save, otherwise the user loses anything they've done
                let errorMessage = "There was an Error while saving.";
                if (saveType === SaveType.DUPLICATE) {
                    errorMessage = "There was an error while duplicating your listing.  Be sure to save after making any necessary updates.";
                }
                dispatch(setAlertMessage({show: true, message: errorMessage, type: AlertMessagingType.ERROR, allowClose: true}));
            }
        }
    })
}