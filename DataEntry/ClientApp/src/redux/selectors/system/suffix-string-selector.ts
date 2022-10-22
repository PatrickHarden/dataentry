import { State, ListingSuffix } from '../../../types/state';

export const suffixSelector = (state:State):ListingSuffix => {
    return state.system.suffix;
}