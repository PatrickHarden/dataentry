import { ActionPayload } from '../../../types/common/action';
import { MIQStatus } from '../../../types/miq/miqStatus';

// constants
export const SET_MIQ_STATUS = 'SET_MIQ_STATUS';

// types
export type SetMIQStatusAction = ActionPayload<MIQStatus> & {
    type: typeof SET_MIQ_STATUS
};

export const setMIQStatus = (payload:MIQStatus) : SetMIQStatusAction => ({
    type: SET_MIQ_STATUS,
    payload
});