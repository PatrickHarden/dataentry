import { setMIQAddEditRecord, SELECT_MIQ_RECORD_ADD_EDIT } from '../../actions/miq/select-miq-record-add-edit';
import { ActionType } from 'typesafe-actions';

export type SetMIQAddEditRecord = ActionType<typeof setMIQAddEditRecord>;

export default(state = {}, action: SetMIQAddEditRecord) => {
    switch(action.type){
        case SELECT_MIQ_RECORD_ADD_EDIT:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}