import { WatermarkProcessStatus, GLFile } from '../types/listing/file';
import { Listing } from '../types/listing/listing';
import { Space } from '../types/listing/space';
import { FeatureFlags } from '../types/state';

// these are the statuses that would place a file in the "pending": if its processing, if there IS a watermark (not allowed), if there was an error
export const problemWatermarkStatuses:number[] = [
    WatermarkProcessStatus.WATERMARK, 
    WatermarkProcessStatus.WATERMARK_ERROR,
    WatermarkProcessStatus.CRE_WATERMARK,
    WatermarkProcessStatus.NOT_PROCESSED,
    WatermarkProcessStatus.PROCESSING,
    WatermarkProcessStatus.READY_TO_PROCESS
];  

// these are statuses that the bucket will refresh on when changed
export const refreshBucketStatuses:number[] = [
    WatermarkProcessStatus.NO_WATERMARK,
    WatermarkProcessStatus.WATERMARK,
    WatermarkProcessStatus.WATERMARK_ERROR,
    WatermarkProcessStatus.CRE_WATERMARK,
    WatermarkProcessStatus.NOT_PROCESSED,
    WatermarkProcessStatus.PROCESSING,
    WatermarkProcessStatus.READY_TO_PROCESS
]

export const extractImagesThatNeedProcessing = (listing:Listing) => {

    // extract a list of ids of images that need to be processed for a listing.
    let imageIds:number[] = [];

    // photos under specifications
    if(listing && listing.photos){
        imageIds = checkFilesThatNeedProcessing(imageIds,listing.photos);
    }

    // floorplans under specifications
    if(listing && listing.floorplans){
        imageIds = checkFilesThatNeedProcessing(imageIds,listing.floorplans);
    }

    // loop through spaces and extract photos and floorplans
    if(listing && listing.spaces){
        listing.spaces.forEach((space:Space) => {
            if(space && space.photos){
                imageIds = checkFilesThatNeedProcessing(imageIds,space.photos);
            }
            if(space && space.floorplans){
                imageIds = checkFilesThatNeedProcessing(imageIds,space.floorplans);
            }
        });
    }

    return imageIds;
}

export const checkFilesThatNeedProcessing = (imageIds:number[],fileArr:GLFile[]):number[] => {
    if(fileArr){
        fileArr.forEach((file:GLFile) => {
            imageIds = checkFile(imageIds,file);
        });
    }
    return imageIds;
}

export const checkFileForWatermarkProblems = (file:GLFile, featureFlags: FeatureFlags, allowWatermarkingDetect: boolean):boolean => {

    if(!featureFlags.watermarkDetectionFeatureFlag || !allowWatermarkingDetect){     // if not detection enabled, will always be false
        return false;
    }

    if(file && file.userOverride === true){     // if a user has overriden, this file won't fall in the pending bucket
        return false;
    }

    if(file && file.watermarkProcessStatus !== undefined && file.watermarkProcessStatus !== null && problemWatermarkStatuses.indexOf(file.watermarkProcessStatus) > -1){
        return true;
    }
    return false;
}

const checkFile = (ids:number[], file:GLFile) => {

    if(file.userOverride && file.userOverride === true && file.id && file.id > 0){
        return ids;     // overrides are ignored
    }else{
        if(file.watermarkProcessStatus !== undefined && file.watermarkProcessStatus !== null && problemWatermarkStatuses.indexOf(file.watermarkProcessStatus) > -1 && file.id && file.id > 0){
            ids.push(file.id);
        }
    }
    return ids;
}