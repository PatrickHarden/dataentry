import { setMIQStatus, SET_MIQ_STATUS } from '../../actions/miq/set-miq-status';
import { ActionType } from 'typesafe-actions';

export type SetMIQStatusAction = ActionType<typeof setMIQStatus>;

export default(state = {}, action: SetMIQStatusAction) => {
    switch(action.type){
        case SET_MIQ_STATUS:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}