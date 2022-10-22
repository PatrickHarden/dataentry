import { ActionPayload } from '../../../types/common/action';
import { FeatureFlags } from '../../../types/state';

// constants
export const UPDATE_FEATURE_FLAGS = 'UPDATE_FEATURE_FLAGS';

// types
export type UpdateFeatureFlagsAction = ActionPayload<FeatureFlags> & {
    type: typeof UPDATE_FEATURE_FLAGS;
};

export const updateFeatureFlags = (payload:FeatureFlags) => ({
    type: UPDATE_FEATURE_FLAGS,
    payload
});