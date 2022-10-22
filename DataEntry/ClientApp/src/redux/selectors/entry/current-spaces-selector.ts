import { State } from '../../../types/state';

export const currentSpacesSelector = (state:State) => {
    return state.entry.spaces;
}