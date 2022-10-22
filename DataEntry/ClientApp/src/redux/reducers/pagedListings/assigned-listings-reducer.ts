import {assignedlistingsLoadedPaged, ASSIGNED_LISTINGS_LOADED_PAGED} from '../../actions/pagedListings/load-listings-paged';
import { ActionType } from 'typesafe-actions';

export type AssignedListingsLoadedAction = ActionType<typeof assignedlistingsLoadedPaged>;

export default(state = {}, action: AssignedListingsLoadedAction) => {
    switch(action.type){
        case ASSIGNED_LISTINGS_LOADED_PAGED:
            return [...action.payload];
        break;

        default:
            return state;
    }
}