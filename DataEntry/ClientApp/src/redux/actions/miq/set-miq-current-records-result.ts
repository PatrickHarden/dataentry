import { ActionPayload } from '../../../types/common/action';
import { Listing } from '../../../types/listing/listing';

// constants
export const SET_MIQ_CURRENT_RECORDS_RESULT = 'SET_MIQ_CURRENT_RECORDS_RESULT';

// types
export type SetMiqCurrentRecordsResultAction = ActionPayload<Listing[]> & {
    type: typeof SET_MIQ_CURRENT_RECORDS_RESULT
};

export const setMiqCurrentRecordsResult = (payload:Listing[]) : SetMiqCurrentRecordsResultAction => ({
    type: SET_MIQ_CURRENT_RECORDS_RESULT,
    payload
});