import {changeSearch,UPDATE_SEARCH} from '../../actions/pagedListings/update-search-text'
import { ActionType } from 'typesafe-actions';

export type UpdateSearchAction = ActionType<typeof changeSearch>;

export default(state = {}, action: UpdateSearchAction) => {
    switch(action.type){
        case UPDATE_SEARCH:
            return action.payload;
        break;

        default:
            return state;
    }
}