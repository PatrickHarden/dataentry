import { setMiqCurrentRecordsResult, SET_MIQ_CURRENT_RECORDS_RESULT } from '../../actions/miq/set-miq-current-records-result';
import { ActionType } from 'typesafe-actions';

export type SetMIQCurrentRecords = ActionType<typeof setMiqCurrentRecordsResult>;

export default(state = {}, action: SetMIQCurrentRecords) => {
    switch(action.type){
        case SET_MIQ_CURRENT_RECORDS_RESULT:
            return [...action.payload];
        break;

        default:
            return state;
    }
}