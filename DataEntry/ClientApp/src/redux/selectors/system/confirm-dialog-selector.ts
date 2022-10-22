import { State } from '../../../types/state';

export const confirmDialogSelector = (state:State) => {
    return state.system.confirmDialog;
}