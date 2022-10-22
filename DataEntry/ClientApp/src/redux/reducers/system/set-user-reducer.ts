import { setAdminLoaded, SET_ADMIN } from '../../actions/user/get-admin';
import { ActionType } from 'typesafe-actions';

export type UserLoadedAction = ActionType<typeof setAdminLoaded>;

export default(state = {}, action: UserLoadedAction) => {
    switch(action.type){
        case SET_ADMIN:
            return action.payload;
        default:
            return state;
    }
}