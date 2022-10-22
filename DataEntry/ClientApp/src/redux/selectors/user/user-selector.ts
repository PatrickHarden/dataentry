import { State } from '../../../types/state';

export const headerSelector = (state:State) => {
    return state.system.user.name;
}