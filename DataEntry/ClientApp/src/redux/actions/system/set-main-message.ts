import { ActionPayload } from '../../../types/common/action';
import { MainMessaging, State, MainMessageType } from '../../../types/state';

// constants
export const SET_MAIN_MESSAGE = 'SET_MAIN_MESSAGE';

// types
export type SetMainMessageAction = ActionPayload<MainMessaging> & {
    type: typeof SET_MAIN_MESSAGE
};

export const setMainMessage = (payload:MainMessaging) : SetMainMessageAction => ({
    type: SET_MAIN_MESSAGE,
    payload
});

export const clearMessage = (dispatch:Function) => {
    const message:MainMessaging = {
        show: false,
        type: MainMessageType.NONE,
        message: ""
    }
    dispatch(setMainMessage(message));
}