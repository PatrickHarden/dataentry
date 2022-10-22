import { setSaveAction, SET_SAVE_ACTION} from '../../actions/system/set-save-action';
import { ActionType } from 'typesafe-actions';

export type SetSaveActionType = ActionType<typeof setSaveAction>;

export default(state = {}, action: SetSaveActionType) => {
    switch(action.type){
        case SET_SAVE_ACTION:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}