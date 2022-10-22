import { setAlertMessage, SET_ALERT_MESSAGE} from '../../actions/system/set-alert-message';
import { ActionType } from 'typesafe-actions';

export type SetAlertMessageAction = ActionType<typeof setAlertMessage>;

export default(state = {}, action: SetAlertMessageAction) => {
    switch(action.type){
        case SET_ALERT_MESSAGE:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}