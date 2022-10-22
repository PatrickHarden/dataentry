import { setReloadListingId, RELOAD_LISTING_ID} from '../../actions/system/set-reload-listing-id';
import { ActionType } from 'typesafe-actions';

export type SetReloadListingIdAction = ActionType<typeof setReloadListingId>;

export default(state = {}, action: SetReloadListingIdAction) => {
    switch(action.type){
        case RELOAD_LISTING_ID:
            return action.payload;
        break;

        default:
            return state;
    }
}