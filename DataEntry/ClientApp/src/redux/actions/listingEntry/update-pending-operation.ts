import { ActionPayload } from '../../../types/common/action';

// constants
export const UPDATE_SAVE_PENDING = 'UPDATE_SAVE_PENDING';

// types
export type UpdatingSavePendingAction = ActionPayload<boolean> & {
    type: typeof UPDATE_SAVE_PENDING;
};

export const updateSavePending = (payload: boolean): UpdatingSavePendingAction => ({
    type: UPDATE_SAVE_PENDING,
    payload
}); 