import { State } from '../../../types/state';

export const cancelActionSelector = (state:State) => {
    return state.system.cancelAction;
}