import {allContactsLoaded,ALL_CONTACTS_LOADED} from '../../actions/contacts/refresh-all-contacts';
import { ActionType } from 'typesafe-actions';

export type CurrentListingLoadedAction = ActionType<typeof allContactsLoaded>;

export default(state = {}, action: CurrentListingLoadedAction) => {
    switch(action.type){
        case ALL_CONTACTS_LOADED:
            return [...action.payload];
        break;

        default:
            return state;
    }
}