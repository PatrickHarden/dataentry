import { Listing } from '../../../types/listing/listing';
import { State } from '../../../types/state';

export const miqPendingAddEditRecordSelector = (state:State):Listing => {
    return state.miq.pendingAddEditRecord;
}