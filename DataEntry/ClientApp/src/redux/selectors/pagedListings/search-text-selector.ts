import { State } from '../../../types/state';

export const searchTextSelector = (state:State) => {
    return state.pagedListings.searchText;
}