import { setMIQSearchResult, SET_MIQ_SEARCH_RESULT } from '../../actions/miq/set-miq-search-result';
import { ActionType } from 'typesafe-actions';

export type SetMiQSelectedProperty = ActionType<typeof setMIQSearchResult>;

export default(state = {}, action: SetMiQSelectedProperty) => {
    switch(action.type){
        case SET_MIQ_SEARCH_RESULT:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}