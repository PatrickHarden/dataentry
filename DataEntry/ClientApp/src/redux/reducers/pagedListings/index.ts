import {combineReducers} from 'redux';
import listingsReducer from './listings-reducer';
import assignedListingsReducer from './assigned-listings-reducer';
import skipReducer from './skip-reducer';
import takeReducer from './take-reducer';
import moreRecordsReducer from './more-records-reducer';
import searchTextReducer from './search-text-reducer';
import filtersReducer from './filters-reducer';

export default combineReducers({
    listings: listingsReducer,
    assignedListings: assignedListingsReducer,
    skip: skipReducer,
    take: takeReducer,
    moreRecords: moreRecordsReducer,
    searchText: searchTextReducer,
    filters: filtersReducer
});