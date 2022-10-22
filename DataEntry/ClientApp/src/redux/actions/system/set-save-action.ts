import { ActionPayload } from '../../../types/common/action';
import { RedirectAction } from '../../../types/state';

// constants
export const SET_SAVE_ACTION = 'SET_SAVE_ACTION';

// types
export type SetSaveAction = ActionPayload<RedirectAction> & {
    type: typeof SET_SAVE_ACTION
};

export const setSaveAction = (payload:RedirectAction) : SetSaveAction => ({
    type: SET_SAVE_ACTION,
    payload
});