import { State } from '../../../types/state';

export const currentProcessingImagesCheck = (state:State) => {
    return state.entry.processingImagesCheck;
}