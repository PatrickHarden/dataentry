import { ActionPayload } from '../../../types/common/action';
import { MainMessaging, State, MainMessageType } from '../../../types/state';

// constants
export const SET_MIQ_ADDRESS = 'SET_MIQ_ADDRESS';

// types
export type setMiqAddressAction = ActionPayload<MainMessaging> & {
    type: typeof SET_MIQ_ADDRESS
};

export const setMiqAddress = (payload:MainMessaging) : setMiqAddressAction => ({
    type: SET_MIQ_ADDRESS,
    payload
});

export const clearMiqAddressPopup = (dispatch:Function) => {
    const message:MainMessaging = {
                show: false,
                type: MainMessageType.NONE,
                message: ""
            }
            dispatch(setMiqAddress(message));
}