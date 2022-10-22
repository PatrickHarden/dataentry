import { State } from '../../../types/state';

export const miqCurrentRecordsSelector = (state:State) => {
    return state.miq.currentRecords;
}