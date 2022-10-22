import { setTake, SET_TAKE} from '../../actions/pagedListings/set-paging-params';
import { ActionType } from 'typesafe-actions';

export type SetTakeAction = ActionType<typeof setTake>;

export default(state = {}, action: SetTakeAction) => {
    switch(action.type){
        case SET_TAKE:
            return action.payload;
        break;

        default:
            return state;
    }
}