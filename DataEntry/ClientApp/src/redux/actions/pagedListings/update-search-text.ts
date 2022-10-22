import { ActionPayload } from '../../../types/common/action';
import { State } from '../../../types/state';
import { resetPagedListings, getListingsPaged } from './load-listings-paged';
import { extractFilters } from '../../../utils/extract-filters';
import { FilterSetup, Filter } from '../../../types/listing/filters';

// constants
export const UPDATE_SEARCH = 'UPDATE_SEARCH';

// types
export type UpdateSearchAction = ActionPayload<string> & {
    type: typeof UPDATE_SEARCH
};

export const changeSearch = (payload:string) : UpdateSearchAction => ({
    type: UPDATE_SEARCH,
    payload
});

export const searchChanged = (searchText:string) => (dispatch: Function, getState: () => State) => {      
    // if filters are changed, we need to perform a few actions:
    // (1) ensure that the search text has actually changed
    const currentSearchText:string = getState().pagedListings.searchText;
    if(searchText.trim() !== currentSearchText.trim()){
        // if yes, then (1) reset the current paged listings (this will clear out the skip too)
        resetPagedListings(dispatch, false);
        // ensure we change the search text in redux for next time
        dispatch(changeSearch(searchText));
        // grab our current filters, and then pull a new set of listings
        const filterSetups:FilterSetup[] = getState().pagedListings.filters;
        const backendFilters:Filter[] = extractFilters(filterSetups, searchText);
        dispatch(getListingsPaged(0, getState().pagedListings.take, backendFilters));
    }    
}