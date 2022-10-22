import { State } from '../../../types/state';

export const miqExportMessageSelector = (state:State) => {
    return state.miq.exportMessage;
}