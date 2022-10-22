import { ActionPayload } from '../../../types/common/action';
import { State, MainMessageType } from '../../../types/state';

import { postData } from '../../../api/glAxios';
import { countAssigned } from '../../../api/glQueries';
import { setMainMessage, clearMessage } from '../system/set-main-message';
import { MainMessaging, AlertMessaging, User } from '../../../types/state';
import { setAlertMessage } from '../../../redux/actions/system/set-alert-message';
import { AlertMessagingType } from '../../../types/state';


// constants
export const ASSIGNED_LISTINGS_COUNT = 'ASSIGNED_LISTINGS_COUNT';

// types
export type AssignedListingsCountAction = ActionPayload<number> & {
    type: typeof ASSIGNED_LISTINGS_COUNT
};

export const assignedListingsCount = (payload:number) : AssignedListingsCountAction => ({
    type: ASSIGNED_LISTINGS_COUNT,
    payload
});

export const getAssignedListingsCount = () => (dispatch: Function, getState: () => State) => { 
 

    postData(countAssigned())
        .then((response: any) => {
            dispatch(clearMessage);
            if (typeof response.data === 'undefined'){
                dispatch(setMainMessage({show: true, type: MainMessageType.ERROR, message: "There was an error getting assigned listing count."}));
            }else if(response.data.count && response.data.count > 0){

                const assignedMsg:string = "You have " + response.data.count.toString() + " Listing(s) assigned to you";
                dispatch(setAlertMessage({ show: true, message: assignedMsg, type: AlertMessagingType.NOTICE, allowClose: true }));
            }
    });
};

