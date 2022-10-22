import { State } from '../../../types/state';

export const assignedListingsSelector = (state:State) => {
    return state.pagedListings.assignedListings;
}