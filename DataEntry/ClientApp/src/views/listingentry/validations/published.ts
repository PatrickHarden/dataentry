import { FieldType } from '../../../types/forms/validation';
import { Listing } from '../../../types/listing/listing';
import { createCoordinates, checkCoordinates } from '../../../utils/map';
import { GLFile } from '../../../types/listing/file';
import { checkFilesThatNeedProcessing } from '../../../utils/image-check';
import { Space } from '../../../types/listing/space';
import { FeatureFlags } from '../../../types/state';

export interface ManualError {
    error: boolean,
    message: string
}

export interface PublishedExtraInfo{
    manual: object,
    errors: string[];
}

export const publishedPropertyValidations:object = {
    'propertyRecordName': {
        type: FieldType.STRING,
        required: true,
        messages: {
            'required': 'A property record name is required.'
        }
    },
    'propertyName': {
        type: FieldType.STRING,
        required: false,
        messages: {
            'required': 'A property name is required.'
        }
    },
    'listingType': {
        type: FieldType.STRING,
        required: true,
        messages: {
            'required': 'A listing type is required.'
        }
    },
    'propertyType': {
        type: FieldType.STRING,
        required: true,
        messages: {
            'required': 'A property type is required.'
        }
    },
    'street': {
        type: FieldType.STRING,
        required: true,
        length: 3,
        messages: {
            'required': 'A street address is required.',
            'length': 'A street address with at least 3 characters is required.'
        }
    },
    'city': {
        type: FieldType.STRING,
        required: true,
        messages: {
            'required': 'A city is required.'
        }
    },
    'postalCode': {
        type: FieldType.STRING,
        required: true,
        messages: {
            'required': 'A postal code is required.'
        }
    },
    'headline': {
        type: FieldType.STRING,
        required: false,
        messages: {
            'required': 'A headline is required.'
        }
    },
    'buildingDescription': {
        type: FieldType.STRING,
        required: true,
        messages: {
            'required': 'A property description is required.'
        }
    },
    'photos': {
        type: FieldType.ARRAY,
        required: true,
        messages: {
            'required': 'At least one photo is required.'
        }
    },
    'contacts': {
        type: FieldType.ARRAY,
        required: true,
        messages: {
            'required': 'At least one contact is required.'
        }
    }
}

export const extraPublishedValidationFunc = (listing:Listing, featureFlags:FeatureFlags, errors:string[]) : PublishedExtraInfo => {

    // provide validations outside of yup
    const manualErrorObj:object = {};
    let isOnePhotoActive = false;
    listing.photos.map((p: GLFile) => {
        if (p.active === true) { isOnePhotoActive = true }
    })
    if(!listing.photos || listing.photos.length === 0 || isOnePhotoActive === false){
        const photos:ManualError = { error: true, message: '- At least one photo is required.'}; 
        const photosFieldName = 'photos';
        manualErrorObj[photosFieldName] = photos;
        errors.push("At least one photo must be included.");
    }
    if(!checkCoordinates(createCoordinates(listing))){
        const coords:ManualError = { error: true, message: 'An address that displays on the map is required.'};
        const coordsFieldName = 'coords';
        manualErrorObj[coordsFieldName] = coords;
        errors.push("Coordinates are required.");
    }
    if(!listing.contacts || listing.contacts.length === 0){
        const contacts:ManualError = { error: true, message: 'At least one contact is required.'}; 
        const contactsFieldName = 'contacts';
        manualErrorObj[contactsFieldName] = contacts;
        errors.push("At least one contact is required.");
    }
    // if watermark detection is enabled in the config, we need to ensure all images have been processed.
    if(featureFlags.watermarkDetectionFeatureFlag === true){

        if(listing.photos && checkFilesThatNeedProcessing([],listing.photos).length > 0){
            const photos:ManualError = { error: true, message: '- Publishing cannot complete until watermark detection is resolved. If images have been flagged for watermarks, please either remove them from your listing, select override if you have permission to use the photo, or reach out to the Global Listings team for help.'}; 
            const photosFieldName = 'photos';
            manualErrorObj[photosFieldName] = photos;
            errors.push("One or more photos under specifications cannot be published due to watermark detection (processing or errors).");
        }
        // floorplans  ::: NOTE: Floorplans have been disabled for watermarking, but we may need to make this dynamic if we decide to include them again.
        /*
        if(listing.floorplans && checkFilesThatNeedProcessing([],listing.floorplans).length > 0){
            const floorplans:ManualError = { error: true, message: '- Publishing cannot complete until watermark detection is resolved. If images have been flagged for watermarks, please either remove them from your listing, select override if you have permission to use the photo, or reach out to the Global Listings team for help.'}; 
            const floorplansFieldName = 'floorplans';
            manualErrorObj[floorplansFieldName] = floorplans;
            errors.push("One or more floorplans under specifications cannot be published due to watermark detection (processing or errors).");
        }
        */

        // spaces
        let spaceErrorCount:number = 0;
        if(listing.spaces && listing.spaces.length > 0){
            listing.spaces.forEach((space:Space) => {
                if(space.photos && space.photos.length > 0){
                    const photoProcessing:number[] = checkFilesThatNeedProcessing([],space.photos);
                    spaceErrorCount += photoProcessing.length;
                }
                /*
                if(space.floorplans && space.floorplans.length > 0){
                    const fpProcessing:number[] = checkFilesThatNeedProcessing([],space.floorplans);
                    spaceErrorCount += fpProcessing.length;
                }
                */
            });
        }
        if(spaceErrorCount > 0){
            const spaceErrors:ManualError = { error: true, message: '- Publishing cannot complete until watermark detection is resolved.  One ore more spaces below contains an image that has been flagged for watermarks. Please either remove them from your listing, select override if you have permission to use the photo, or reach out to the Global Listings team for help.'}; 
            const spacesFieldName = 'spaces';
            manualErrorObj[spacesFieldName] = spaceErrors;
            errors.push("One or more spaces have photos that cannot be published due to watermark detection (processing or errors).");
        }
    }
    
    return {
        'manual': manualErrorObj,
        'errors': errors
    };
}