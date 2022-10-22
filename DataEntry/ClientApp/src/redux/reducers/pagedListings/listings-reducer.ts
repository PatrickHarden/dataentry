import {listingsLoadedPaged, LISTINGS_LOADED_PAGED} from '../../actions/pagedListings/load-listings-paged';
import { ActionType } from 'typesafe-actions';

export type ListingsLoadedPagedAction = ActionType<typeof listingsLoadedPaged>;

export default(state = {}, action: ListingsLoadedPagedAction) => {
    switch(action.type){
        case LISTINGS_LOADED_PAGED:
            return [...action.payload];
        break;

        default:
            return state;
    }
}