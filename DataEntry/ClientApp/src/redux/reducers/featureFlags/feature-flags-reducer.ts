import { updateFeatureFlags, UPDATE_FEATURE_FLAGS} from '../../actions/featureFlags/update-feature-flags';
import { ActionType } from 'typesafe-actions';

export type UpdateFeatureFlagAction = ActionType<typeof updateFeatureFlags>;

export default(state = {}, action: UpdateFeatureFlagAction) => {
    switch(action.type){
        case UPDATE_FEATURE_FLAGS:
            return {...state,...action.payload}
        break;

        default:
            return state;
    }
}