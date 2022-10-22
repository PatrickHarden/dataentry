import { State } from '../../../types/state';

export const currentListingSelector = (state:State) => {
    return state.entry.currentListing;
}