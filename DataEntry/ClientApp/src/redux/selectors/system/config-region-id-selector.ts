import { State } from '../../../types/state';

export const regionIdSelector = (state:State) => {
    return state.mapping.selectedCountry;
}