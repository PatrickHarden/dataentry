import { setSkip, SET_SKIP} from '../../actions/pagedListings/set-paging-params';
import { ActionType } from 'typesafe-actions';

export type SetSkipAction = ActionType<typeof setSkip>;

export default(state = {}, action: SetSkipAction) => {
    switch(action.type){
        case SET_SKIP:
            return action.payload;
        break;

        default:
            return state;
    }
}