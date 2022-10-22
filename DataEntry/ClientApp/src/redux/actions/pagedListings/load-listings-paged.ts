import { ActionPayload } from '../../../types/common/action';
import { State, MainMessageType } from '../../../types/state';
import { Listing } from '../../../types/listing/listing';
import { postData } from '../../../api/glAxios';
import { pullListingsPaged } from '../../../api/glQueries';
import { setSkip, setMoreRecords } from './set-paging-params';
import { setMainMessage, clearMessage } from '../system/set-main-message';
import { Filter } from '../../../types/listing/filters';
import { authContext } from '../../../adalConfig';
import { TeamMember } from '../../../types/team/teamMember';

// constants
export const LISTINGS_LOADED_PAGED = 'LISTINGS_LOADED_PAGED';
export const ASSIGNED_LISTINGS_LOADED_PAGED = 'ASSIGNED_LISTINGS_LOADED_PAGED';

// types
export type ListingsLoadedPagedAction = ActionPayload<Listing[]> & {
    type: typeof LISTINGS_LOADED_PAGED
};

export const listingsLoadedPaged = (payload:Listing[]) : ListingsLoadedPagedAction => ({
    type: LISTINGS_LOADED_PAGED,
    payload
});

export type AssignedListingsLoadedPagedAction = ActionPayload<Listing[]> & {
    type: typeof ASSIGNED_LISTINGS_LOADED_PAGED
};

export const assignedlistingsLoadedPaged = (payload:Listing[]) : AssignedListingsLoadedPagedAction => ({
    type: ASSIGNED_LISTINGS_LOADED_PAGED,
    payload
});

export const getListingsPaged = (skip:number, take:number, filters:Filter[]) => (dispatch: Function, getState: () => State) => { 

    const currentNormalListings:Listing[] = getState().pagedListings.listings;
    const currentAssignedListings:Listing[] = getState().pagedListings.assignedListings;
    const currentSkip:number = getState().pagedListings.skip;

    if(skip === 0){
        dispatch(setMainMessage({show: true, type: MainMessageType.LOADING, message: "Loading..."}));
    }   

    const siteId = (typeof getState().mapping.selectedCountry === 'string') ? getState().mapping.selectedCountry : "00000000-0000-0000-0000-000000000001";
    

    postData(pullListingsPaged(skip,take,filters, siteId))
        .then((response: any) => {
            dispatch(clearMessage);
            if (typeof response.data === 'undefined'){
                dispatch(setMainMessage({show: true, type: MainMessageType.ERROR, message: "There was an error loading your listings."}));
            }else if(response.data.listings && response.data.listings.length === 0){

                // we need to know why its 0: no listings at all, no listings given this search/filter combo, or just at the end of the recordset
                if(getState().pagedListings.listings && getState().pagedListings.listings.length === 0){

                    if(skip === 0 && (!filters || filters === null || filters.length === 0)){
                        // no filters applied, we have no listings, and the skip is 0 - so we don't have any listings
                        dispatch(setMainMessage({show: true, type:MainMessageType.NOTICE, message: "There are no existing listings.",messageLine2: "Go ahead and create one."}));
                    }else if(filters && filters !== null && filters.length > 0){
                        // otherwise, we assume a search/filter is causing the listing return to be 0, and since we know we dont have listings we can safely make that assumption
                        dispatch(setMainMessage({show: true, type:MainMessageType.NOTICE, message: "There are no listings using the current search/filters.",messageLine2: "Please change or clear you search/filters."}))
                    }  
                }
                dispatch(setMoreRecords(false));
            }else{
                const loadedListings:Listing[] = response.data.listings;
                // figure out which "buckets" the listings should fall in
                const loadedNormal:Listing[] = [];
                const loadedAssigned:Listing[] = [];
                if(loadedListings){
                    loadedListings.forEach((listing:Listing) => {
                        // to fall in the "assigned" bucket:
                        // (1) the assignment flag needs to be true (isAssigned)
                        // (2) the logged in user cannot be the assigner
                        // (3) the logged in user must be an actual team member, indicating assignment (otherwise admin gets all assignments)
                        if(isAssigned(listing) && !isAssigner(listing) && isTeamMember(listing)){
                            loadedAssigned.push(listing);
                        }else{
                            loadedNormal.push(listing);
                        }
                    }); 
                }
                dispatch(listingsLoadedPaged([...currentNormalListings, ...loadedNormal]));
                dispatch(assignedlistingsLoadedPaged([...currentAssignedListings, ...loadedAssigned]))
                dispatch(setSkip(currentSkip + loadedListings.length));
                if(loadedListings.length < take){   // if the loaded listing length is less than the take, we are out of records
                    dispatch(setMoreRecords(false));
                }
            }
    });
};

export const resetPagedListings = (dispatch: Function, reload: boolean) => {
    dispatch(setSkip(0));
    dispatch(setMoreRecords(true));
    dispatch(listingsLoadedPaged([]));
    dispatch(assignedlistingsLoadedPaged([]));

    if(reload){
        dispatch(reloadListingsPaged);
    }
}

// this function will reload the listings based on the current skip, take, and filters
export const reloadListingsPaged = (dispatch: Function, getState: () => State) => {

    const skip:number = getState().pagedListings.skip;
    const take:number = getState().pagedListings.take;
    const filters:Filter[] = getState().pagedListings.filters;

    dispatch(getListingsPaged(skip,take,filters));
} 

// helper functions to help divide into buckets
const isAssigned = (listing:Listing):boolean => {
    if(listing && listing.listingAssignment && listing.listingAssignment.assignmentFlag){
        return true;
    }
    return false;
}

const isAssigner = (listing:Listing):boolean => {
    const user:any = authContext.getCachedUser();
    const email:string = user && user.userName ? user.userName.toLowerCase() : "";
    if(listing && listing.listingAssignment && listing.listingAssignment.assignedBy 
        && (email.length === 0 || email === listing.listingAssignment.assignedBy.toLowerCase())){
            return true;
    }
    return false;
}

const isTeamMember = (listing:Listing):boolean => {
    if(listing.users){
        const user:any = authContext.getCachedUser();
        const email:string = user && user.userName ? user.userName.toLowerCase() : "";
        const members:TeamMember[] = listing.users.filter(userObj => userObj.id && userObj.id.toLowerCase() === email);
        if(members.length > 0){
            return true;
        }
    }
    return false;
}