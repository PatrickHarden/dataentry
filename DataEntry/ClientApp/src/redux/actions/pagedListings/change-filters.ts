import { ActionPayload } from '../../../types/common/action';
import { FilterSetup, Filter } from '../../../types/listing/filters';
import { State } from '../../../types/state';
import { resetPagedListings, getListingsPaged } from './load-listings-paged';
import { extractFilters } from '../../../utils/extract-filters';

// constants
export const CHANGE_FILTERS = 'CHANGE_FILTERS';
export const SETUP_FILTERS = 'SETUP_FILTERS';

// types
export type ChangeFiltersAction = ActionPayload<FilterSetup[]> & {
    type: typeof CHANGE_FILTERS
};

export type SetupFiltersAction = ActionPayload<FilterSetup[]> & {
    type: typeof SETUP_FILTERS
};

export const changeFilters = (payload:FilterSetup[]) : ChangeFiltersAction => ({
    type: CHANGE_FILTERS,
    payload
});

export const setupFilters = (payload:FilterSetup[]) : SetupFiltersAction => ({
    type: SETUP_FILTERS,
    payload
});

export const filtersChanged = (filterSetups:FilterSetup[]) => (dispatch: Function, getState: () => State) => {
    // (1) We need to clear out the current set of listings using the clear function - this will also reset the skip to 0
    resetPagedListings(dispatch, false);
    // (2) ensure we've changed the current filters in redux for next time (and to affect the dropdown menu)
    dispatch(changeFilters(filterSetups));
    // (3) Pull the new set of paged listings (pull in any search text to create that filter)
    const searchText = getState().pagedListings.searchText;
    const backendFilters:Filter[] = extractFilters(filterSetups, searchText);
    dispatch(getListingsPaged(0, getState().pagedListings.take, backendFilters)); 
}