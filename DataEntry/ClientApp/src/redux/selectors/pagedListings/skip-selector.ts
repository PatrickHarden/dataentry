import { State } from '../../../types/state';

export const skipSelector = (state:State) => {
    return state.pagedListings.skip;
}