import { State } from '../../../types/state';

export const mainMessageSelector = (state:State) => {
    return state.system.mainMessage;
}