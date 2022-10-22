import { ActionPayload } from '../../../types/common/action';

// constants
export const SET_SCROLL_POSITION = 'SET_SCROLL_POSITION';

// types
export type SetScrollPositionAction = ActionPayload<number> & {
    type: typeof SET_SCROLL_POSITION
};

export const setEntryPageScrollPosition = (payload:number) : SetScrollPositionAction => ({
    type: SET_SCROLL_POSITION,
    payload
});