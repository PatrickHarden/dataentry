import { setIsDuplicatingListing, IS_DUPLICATING_LISTING } from '../../actions/system/set-is-duplicating-listing';
import { ActionType } from 'typesafe-actions';

export type SetIsDuplicatingListingAction = ActionType<typeof setIsDuplicatingListing>;

export default(state = {}, action: SetIsDuplicatingListingAction) => {
    switch(action.type){
        case IS_DUPLICATING_LISTING:
            return action.payload;
        break;

        default:
            return state;
    }
}