import { State } from '../../../types/state';

export const configDetailsSelector = (state:State) => {
    return state.system.configDetails;
}