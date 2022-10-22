import { State } from '../../../types/state';
import { setMiqCurrentRecordsResult } from './set-miq-current-records-result';
import { setMIQSearchResult } from './set-miq-search-result';

export const clearMIQData = ()  => (dispatch: Function, getState: () => State) => { 
    // clear out our current search result
    dispatch(setMIQSearchResult(undefined));
    // clear out current records
    dispatch(setMiqCurrentRecordsResult([]));
}