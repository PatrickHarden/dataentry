import { setMiqSpaces, SET_MIQ_SPACES } from '../../actions/miq/set-miq-spaces'
import { ActionType } from 'typesafe-actions';

export type SetMIQSpaces = ActionType<typeof setMiqSpaces>;

export default(state = {}, action: SetMIQSpaces) => {
    switch(action.type){
        case SET_MIQ_SPACES:
            return action.payload;
        break;

        default:
            return state;
    }
}