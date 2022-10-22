import { State } from '../../../types/state';

export const isDuplicatingListingSelector = (state:State) => {
    return state.system.isDuplicatingListing;
}