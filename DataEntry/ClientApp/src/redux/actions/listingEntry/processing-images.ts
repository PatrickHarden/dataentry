import { ActionPayload } from '../../../types/common/action';
import { ProcessingImagesCheck } from '../../../types/state';
import { GLFile } from '../../../types/listing/file';

// constants
export const SET_IMAGES_PROCESSING = 'SET_IMAGES_PROCESSING';
export const REFRESH_IMAGES = 'REFRESH_IMAGES';

// types
export type SetImagesProcessingAction = ActionPayload<ProcessingImagesCheck> & {
    type: typeof SET_IMAGES_PROCESSING;
};

export type RefreshImagesAction = ActionPayload<GLFile[]> & {
    type: typeof REFRESH_IMAGES
};

export const setImagesProcessing = (payload:ProcessingImagesCheck) => ({
    type: SET_IMAGES_PROCESSING,
    payload
});

export const refreshImages = (payload:GLFile[]) => ({
    type: REFRESH_IMAGES,
    payload
});