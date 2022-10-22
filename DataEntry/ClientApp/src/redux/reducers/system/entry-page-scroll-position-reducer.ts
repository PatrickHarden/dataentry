import { setEntryPageScrollPosition, SET_SCROLL_POSITION} from '../../actions/system/entry-page-scroll-position';
import { ActionType } from 'typesafe-actions';

export type SetEntryPageScrollPositionAction = ActionType<typeof setEntryPageScrollPosition>;

export default(state = {}, action: SetEntryPageScrollPositionAction) => {
    switch(action.type){
        case SET_SCROLL_POSITION:
            return action.payload;
        break;

        default:
            return state;
    }
}