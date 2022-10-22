import { setAnalyticsBoolean, SET_ANALYTICS_BOOLEAN } from '../../actions/system/set-analytics-boolean';
import { ActionType } from 'typesafe-actions';

export type SetAnalyticsBooleanAction = ActionType<typeof setAnalyticsBoolean>;

export default(state = {}, action: SetAnalyticsBooleanAction) => {
    switch(action.type){
        case SET_ANALYTICS_BOOLEAN:
            return action.payload;
        break;

        default:
            return state;
    }
}