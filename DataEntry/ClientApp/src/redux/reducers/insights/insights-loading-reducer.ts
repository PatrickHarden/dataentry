import { setInsightsLoading, SET_INSIGHTS_LOADING } from '../../actions/insights/load-insights-action';
import { ActionType } from 'typesafe-actions';

export type SetInsightsLoading = ActionType<typeof setInsightsLoading>;

export default(state = {}, action: SetInsightsLoading) => {
    switch(action.type){
        case SET_INSIGHTS_LOADING:
            return Object.assign({}, action.payload);
        break;

        default:
            return state;
    }
}