import { State } from '../../../types/state';

export const moreRecordsSelector = (state:State) => {
    return state.pagedListings.moreRecords;
}