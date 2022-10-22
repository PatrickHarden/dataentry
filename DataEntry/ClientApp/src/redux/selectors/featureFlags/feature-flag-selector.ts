import { State } from '../../../types/state';

export const featureFlagSelector = (state:State) => {
    return state.featureFlags;
}