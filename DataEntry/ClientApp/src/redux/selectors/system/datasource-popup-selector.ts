import { State } from '../../../types/state';

export const dataSourcePopupSelector = (state:State) => {
    return state.system.dataSourcePopup;
}