import { setMainMessage, SET_MAIN_MESSAGE} from '../../actions/system/set-main-message';
import { ActionType } from 'typesafe-actions';

export type SetMainMessageAction = ActionType<typeof setMainMessage>;

export default(state = {}, action: SetMainMessageAction) => {
    switch(action.type){
        case SET_MAIN_MESSAGE:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}