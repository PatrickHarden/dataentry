import { State } from '../../../types/state';

export const entryPageScrollPositionSelector = (state:State) => {
    return state.system.entryPageScrollPosition;
}