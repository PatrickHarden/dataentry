import { State } from '../../../types/state';

export const alertMessageSelector = (state:State) => {
    return state.system.alertMessage;
}