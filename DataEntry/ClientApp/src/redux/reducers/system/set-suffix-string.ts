import { setListingSuffix, SET_LISTING_SUFFIX } from '../../actions/system/set-suffix-string';
import { ActionType } from 'typesafe-actions';

export type SetSuffixStringAction = ActionType<typeof setListingSuffix>;

export default(state = {}, action: SetSuffixStringAction) => {
    switch(action.type){
        case SET_LISTING_SUFFIX:
            return action.payload;
        break;

        default:
            return state;
    }
}