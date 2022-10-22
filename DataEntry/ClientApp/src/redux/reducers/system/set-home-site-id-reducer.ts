import { setHomeSiteId, SET_HOME_SITE_ID } from '../../actions/system/set-home-site-id';
import { ActionType } from 'typesafe-actions';

export type SetHomeSiteIdAction = ActionType<typeof setHomeSiteId>;

export default(state = {}, action: SetHomeSiteIdAction) => {
    switch(action.type){
        case SET_HOME_SITE_ID:
            return action.payload;
        break;

        default:
            return state;
    }
}