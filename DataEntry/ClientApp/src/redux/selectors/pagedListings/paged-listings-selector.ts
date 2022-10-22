import { State } from '../../../types/state';

export const pagedListingsSelector = (state:State) => {
    return state.pagedListings.listings;
}