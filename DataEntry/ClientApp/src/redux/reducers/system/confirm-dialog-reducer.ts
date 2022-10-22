import { setConfirmDialog, SET_CONFIRM_DIALOG} from '../../actions/system/set-confirm-dialog';
import { ActionType } from 'typesafe-actions';

export type SetConfirmDialogAction = ActionType<typeof setConfirmDialog>;

export default(state = {}, action: SetConfirmDialogAction) => {
    switch(action.type){
        case SET_CONFIRM_DIALOG:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}