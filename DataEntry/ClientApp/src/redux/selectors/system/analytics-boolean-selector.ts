import { State } from '../../../types/state';

export const analyticsBooleanSelector = (state:State) => {
    return state.system.analytics;
}