import {currentListingLoaded,CURRENT_LISTING_LOADED} from '../../actions/listingEntry/load-current-listing';
import { ActionType } from 'typesafe-actions';

export type CurrentListingLoadedAction = ActionType<typeof currentListingLoaded>;

export default(state = {}, action: CurrentListingLoadedAction) => {
    switch(action.type){
        case CURRENT_LISTING_LOADED:
            return {...state,...action.payload};
        break;

        default:
            return state;
    }
}