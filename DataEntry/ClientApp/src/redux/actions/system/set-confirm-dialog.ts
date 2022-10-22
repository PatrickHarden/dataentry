import { ActionPayload } from '../../../types/common/action';
import { ConfirmDialogParams } from '../../../types/state';

// constants
export const SET_CONFIRM_DIALOG = 'SET_CONFIRM_DIALOG';

// types
export type SetConfirmDialogAction = ActionPayload<ConfirmDialogParams> & {
    type: typeof SET_CONFIRM_DIALOG
};

export const setConfirmDialog = (payload:ConfirmDialogParams) : SetConfirmDialogAction => ({
    type: SET_CONFIRM_DIALOG,
    payload
});

export const clearConfirmDialog = (dispatch:Function) => {
    const dialogSetting:ConfirmDialogParams = {
        show: false,
        title: undefined,
        message: undefined,
        cancelTxt: undefined,
        confirmTxt : undefined,
        cancelFunc: undefined,
        confirmFunc: undefined,
        showConfirmButton: undefined,
        showCopyButton: undefined
    }
    dispatch(setConfirmDialog(dialogSetting));
}