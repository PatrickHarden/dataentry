import { State } from '../../../types/state';

export const miqCurrentSelectionSelector = (state:State) => {
    return state.miq.selectedSearchOption;
}