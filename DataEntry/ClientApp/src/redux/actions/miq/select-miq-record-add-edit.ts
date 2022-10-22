import { ActionPayload } from '../../../types/common/action';
import { Listing } from '../../../types/listing/listing';
import { State } from '../../../types/state';
import { push } from 'connected-react-router';

// constants
export const SELECT_MIQ_RECORD_ADD_EDIT = 'SELECT_MIQ_RECORD_ADD_EDIT';

// types
export type SetMIQAddEditAction = ActionPayload<Listing | null> & {
    type: typeof SELECT_MIQ_RECORD_ADD_EDIT
};

export const setMIQAddEditRecord = (payload:Listing | null) : SetMIQAddEditAction => ({
    type: SELECT_MIQ_RECORD_ADD_EDIT,
    payload
});

export const setPendingMIQAddEditRecord = (record:Listing)  => (dispatch: Function, getState: () => State) => { 
    // record the selection in memory
    dispatch(setMIQAddEditRecord(record));

    // redirect the user to the appropriate page using connected-react-router
    if(record.id > 0){   // edit path
        dispatch(push('/le/' + record.id));
    }else{ // create path
        dispatch(push('/le'));
    }
}