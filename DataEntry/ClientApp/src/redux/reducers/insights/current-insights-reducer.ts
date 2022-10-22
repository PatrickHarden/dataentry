import { setCurrentInsightsRecord, SET_CURRENT_INSIGHTS_RECORD } from '../../actions/insights/load-insights-action';
import { ActionType } from 'typesafe-actions';

export type SetCurrentInsightsRecord = ActionType<typeof setCurrentInsightsRecord>;

export default(state = {}, action: SetCurrentInsightsRecord) => {
    switch(action.type){
        case SET_CURRENT_INSIGHTS_RECORD:
            return Object.assign({}, action.payload);
        break;

        default:
            return state;
    }
}