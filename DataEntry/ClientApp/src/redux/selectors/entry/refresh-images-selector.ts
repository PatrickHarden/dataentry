import { State } from '../../../types/state';
import { GLFile } from '../../../types/listing/file';

export const refreshImagesSelector = (state:State):GLFile[] => {
    return state.entry.refreshImages;
}