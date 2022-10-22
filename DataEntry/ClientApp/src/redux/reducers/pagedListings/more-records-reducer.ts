import {setMoreRecords, SET_MORE_RECORDS} from '../../actions/pagedListings/set-paging-params';
import { ActionType } from 'typesafe-actions';

export type SetMoreRecordsAction = ActionType<typeof setMoreRecords>;

export default(state = {}, action: SetMoreRecordsAction) => {
    switch(action.type){
        case SET_MORE_RECORDS:
            return action.payload;
        break;

        default:
            return state;
    }
}