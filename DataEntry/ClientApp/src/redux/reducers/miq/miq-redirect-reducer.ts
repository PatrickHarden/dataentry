import { dispatchMIQRedirect, SET_MIQ_REDIRECT } from '../../actions/miq/set-miq-redirect';
import { ActionType } from 'typesafe-actions';

export type SetMIQRedirect = ActionType<typeof dispatchMIQRedirect>;

export default(state = {}, action: SetMIQRedirect) => {
    switch(action.type){
        case SET_MIQ_REDIRECT:
            return action.payload;
        break;

        default:
            return state;
    }
}