import { State } from '../../../types/state';

export const reloadListingIdSelector = (state:State) => {
    return state.system.reloadListingId;
}