import { State } from '../../../types/state';

export const currentInsightsRecordSelector = (state:State) => {
    return state.insights.current;
}