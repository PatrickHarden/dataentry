import { ActionPayload } from '../../../types/common/action';

// constants
export const SET_ANALYTICS_BOOLEAN = 'SET_ANALYTICS_BOOLEAN';

// types
export type SetAnalyticsBooleanAction = ActionPayload<boolean> & {
    type: typeof SET_ANALYTICS_BOOLEAN
};

export const setAnalyticsBoolean = (payload:boolean) : SetAnalyticsBooleanAction => ({
    type: SET_ANALYTICS_BOOLEAN,
    payload
});