import { ActionPayload } from '../../../types/common/action';
import { AlertMessaging } from '../../../types/state';

// constants
export const SET_ALERT_MESSAGE = 'SET_ALERT_MESSAGE';

// types
export type SetAlertMessageAction = ActionPayload<AlertMessaging> & {
    type: typeof SET_ALERT_MESSAGE
};

export const setAlertMessage = (payload:AlertMessaging) : SetAlertMessageAction => ({
    type: SET_ALERT_MESSAGE,
    payload
});

export const clearAlertMessage = (dispatch:Function) => {
    const message:AlertMessaging = {
        show: false,
        message: ""
    }
    dispatch(setAlertMessage(message));
}