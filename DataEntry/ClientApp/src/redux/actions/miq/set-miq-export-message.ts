import { ActionPayload } from '../../../types/common/action';
import { State } from '../../../types/state';
import { MIQExportMessage, MIQResultType } from '../../../types/miq/miqExportMessaging';

// constants
export const SET_MIQ_EXPORT_MESSAGE = 'SET_MIQ_EXPORTSET_MIQ_EXPORT_MESSAGE_RESULT';

// types
export type SetMiqExportMessageAction = ActionPayload<MIQExportMessage> & {
    type: typeof SET_MIQ_EXPORT_MESSAGE
};

export const setMIQExportMessage = (payload:MIQExportMessage) : SetMiqExportMessageAction => ({
    type: SET_MIQ_EXPORT_MESSAGE,
    payload
});

export const showMIQExportSuccess = (header: string, body: string) => (dispatch: Function, getState: () => State) => {
    const exportMessage:MIQExportMessage = {
        "show": true,
        "header": header,
        "body": body,
        "type": MIQResultType.SUCCESS
    }
    dispatch(setMIQExportMessage(exportMessage));
}

export const showMIQExportError = (header: string, body: string) => (dispatch: Function, getState: () => State) => {
    const exportMessage:MIQExportMessage = {
        "show": true,
        "header": header,
        "body": body,
        "type": MIQResultType.ERROR
    }
    dispatch(setMIQExportMessage(exportMessage));
}

export const clearMIQExportMessage = () => (dispatch: Function, getState: () => State) => {
    const exportMessage:MIQExportMessage = { show: false };
    dispatch(setMIQExportMessage(exportMessage));
}