import {combineReducers} from 'redux';
import insightsLoadingReducer from './insights-loading-reducer';
import insightsCurrentReducer from './current-insights-reducer';

export default combineReducers({
    loading: insightsLoadingReducer,
    current: insightsCurrentReducer
});