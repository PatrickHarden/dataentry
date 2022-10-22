import { MIQStatus } from '../../../types/miq/miqStatus';
import { State } from '../../../types/state';

export const miqStatusSelector = (state:State):MIQStatus => {
    return state.miq.status;
}