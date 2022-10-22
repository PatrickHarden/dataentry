import { State } from '../../../types/state';

export const userSelector = (state: State) => {
    return state.system.user;
}