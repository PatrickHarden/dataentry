import { GLFile } from '../../../types/listing/file';
import { Listing } from '../../../types/listing/listing';
import { State } from '../../../types/state';
import { convertBase64ToFile, uploadFile } from '../../../utils/files/file-util';
import { setPendingMIQAddEditRecord } from './select-miq-record-add-edit';
import { setMIQStatus } from './set-miq-status';
import { loadConfig } from '../../../redux/actions/system/load-config-details';
import CountryCodes from '../../../config/country-code-config-map.json';
import { checkDatesForTimeZone, convertListingDataToSupportMultiRegional } from '../../../utils/listing/convert-listing-data';
import { Space } from '../../../types/listing/space';
import { Config } from '../../../types/config/config';
import { saveListing, SaveType } from '../listingEntry/save-listing';

export const prepareRecordForAddEdit = (record: Listing, siteId: string | undefined) => (dispatch: Function, getState: () => State) => {
    window.scrollTo({ top: 0 });

    convertFilesAndDispatch(record, siteId, dispatch, getState())
        .then(() => dispatch(setPendingMIQAddEditRecord(record)));
}

export const prepareRecordForCreateAndUnpublish = (record: Listing, siteId: string | undefined, config: Config) => (dispatch: Function, getState: () => State) => {
    window.scrollTo({ top: 0 });

    convertFilesAndDispatch(record, siteId, dispatch, getState())
        .then(() => {
            dispatch(setMIQStatus({ loading: true, processingFiles: true, error: false, message: "" }));
            dispatch(saveListing(record,SaveType.UNPUBLISH_FROM_MIQ,config))
        });
}

async function convertFilesAndDispatch(record: Listing, siteId: string | undefined, dispatch: Function, state:State) {

    // load config according to the record's country
    // for (const [key, value] of Object.entries(CountryCodes)) {
    //     if (siteId){
    //         if (key.toLowerCase() === record.country.toLowerCase() && value.toLowerCase() !== siteId.toLowerCase()){ // ensure no unneccesary load
    //             dispatch(loadConfig(value))
    //         }
    //     }
    // }
    // if the record is new (id = 0), we need to check through the files and if there are photos 
    // that need to be converted from base64 to file and uploaded, we will do that and manipulate the record
    // before sending the user to the add/edit screen.

    // if we only have one language, pass the record through our multi-regional conversion to ensure we have it setup correctly
    const config:Config = state.system.configDetails.config;
    if(!config.languages || (config.languages && config.languages.length === 1)){
        record = convertListingDataToSupportMultiRegional(record, state.system.configDetails.config);
    }
    // secondly, we get dates with a timezone of 00:00:00 which doesnt work with our components, so massage that data to current time zone
    record = checkDatesForTimeZone(record);
    
    if (record.id === 0) {

        // first, because our photos have a primary flag, ensure any photo sections have only one primary image selected
        // no other sections currently have the concept of a primary
        checkPrimaryPhotos(record);

        let filesToUpload:any[] = [];

        // compile a list of files we need to upload from our various sections
        // photos
        filesToUpload = filesToUpload.concat(checkFiles(record, record.photos));
        // floorplans
        filesToUpload = filesToUpload.concat(checkFiles(record, record.floorplans));
        // brochures
        filesToUpload = filesToUpload.concat(checkFiles(record, record.brochures));
        // brochures
        if (record.epcGraphs) {
            filesToUpload = filesToUpload.concat(checkFiles(record, record.epcGraphs));
        }

        // perform the actual upload
        if(filesToUpload.length > 0){
            dispatch(setMIQStatus({ loading: true, processingFiles: true, error: false, message: "" }));
            await uploadFiles(state, record, filesToUpload, dispatch).then(() => {
                dispatch(setMIQStatus({ loading: false, error: false, message: "" }));
            }); 
        }
    }
}

const checkFiles = (record:Listing, files:GLFile[]) => {

    let filesToUpload:any[] = [];

    // this is a new record from MIQ, so we need to check the assets to see if they need to be converted
    if(files && files.length > 0){
        filesToUpload = files;
    }
    if(record.spaces && record.spaces.length > 0){
        record.spaces.forEach((space:Space) => {
            if(space.photos && space.photos.length > 0){ filesToUpload = filesToUpload.concat(space.photos); }
            if(space.floorplans && space.floorplans.length > 0){ filesToUpload = filesToUpload.concat(space.floorplans); }
            if(space.brochures && space.brochures.length > 0){ filesToUpload = filesToUpload.concat(space.brochures); }
        });
    }
    return filesToUpload;
}

const checkPrimaryPhotos = (record:Listing) => {
    if(record.photos){
        ensureOnePrimary(record.photos);
    }
    if(record.spaces){
        record.spaces.forEach((space:Space) => {
            // photos
            if(space.photos){ ensureOnePrimary(space.photos); }
        });
    }
}

const ensureOnePrimary = (photos:GLFile[]) => {
    const primaryCheck = photos.filter((photo:GLFile) => photo.primary);
    if(primaryCheck.length === 0 && photos.length > 0){
        photos[0].primary = true;
    }
    if(primaryCheck.length > 1){
        primaryCheck.forEach((photo:GLFile) => photo.primary = false);
        primaryCheck[0].primary = true;
    }
}

async function uploadFiles(state:State, record: Listing, filesToUpload: GLFile[], dispatch: Function) {
    for (const file of filesToUpload) {
        if (file.base64String && file.base64String.length > 0) {
            let fileExtension:string | undefined;
            if(file.url){
                fileExtension = file.url.split('.').pop();
            }
            if(fileExtension === file.url){
                fileExtension = undefined;
            }
            const fileToUpload: File | undefined = convertBase64ToFile(file.base64String, file.displayText, fileExtension);

            if (fileToUpload) {
                await uploadFile(fileToUpload, state.featureFlags.watermarkDetectionFeatureFlag ? state.featureFlags.watermarkDetectionFeatureFlag : false).then(fileResult => {
                    if (fileResult) {
                        // since we got a result, setup the necessary information on the file
                        file.id = fileResult.id;
                        file.url = fileResult.url;
                        if(fileExtension && fileExtension !== "pdf"){
                            file.watermarkProcessStatus = fileResult.watermarkProcessStatus;
                        }
                        file.base64String = '';
                    } else {
                        file.errorDisplay = 'Error Uploading File';
                    }
                });
            } else {
                file.errorDisplay = 'Error Converting File';
            }
        }
    }
}