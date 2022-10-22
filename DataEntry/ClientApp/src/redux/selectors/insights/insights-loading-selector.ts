import { State } from '../../../types/state';

export const insightsLoadingSelector = (state:State) => {
    return state.insights.loading;
}