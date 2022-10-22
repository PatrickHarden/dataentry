import { State } from '../../../types/state';

export const takeSelector = (state:State) => {
    return state.pagedListings.take;
}