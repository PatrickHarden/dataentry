import { setMIQExportMessage, SET_MIQ_EXPORT_MESSAGE } from '../../actions/miq/set-miq-export-message';
import { ActionType } from 'typesafe-actions';

export type SetMIQExportMessageAction = ActionType<typeof setMIQExportMessage>;

export default(state = {}, action: SetMIQExportMessageAction) => {
    switch(action.type){
        case SET_MIQ_EXPORT_MESSAGE:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}