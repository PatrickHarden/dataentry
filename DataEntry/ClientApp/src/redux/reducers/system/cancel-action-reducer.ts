import { setCancelAction, SET_CANCEL_ACTION} from '../../actions/system/set-cancel-action';
import { ActionType } from 'typesafe-actions';

export type SetCancelActionType = ActionType<typeof setCancelAction>;

export default(state = {}, action: SetCancelActionType) => {
    switch(action.type){
        case SET_CANCEL_ACTION:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}