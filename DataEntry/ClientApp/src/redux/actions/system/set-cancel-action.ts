import { ActionPayload } from '../../../types/common/action';
import { RedirectAction } from '../../../types/state';

// constants
export const SET_CANCEL_ACTION = 'SET_CANCEL_ACTION';

// types
export type SetCancelAction = ActionPayload<RedirectAction> & {
    type: typeof SET_CANCEL_ACTION
};

export const setCancelAction = (payload:RedirectAction) : SetCancelAction => ({
    type: SET_CANCEL_ACTION,
    payload
});