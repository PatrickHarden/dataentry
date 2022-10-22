import { State } from '../../../types/state';

export const selectedCountrySelector = (state:State) => {
    return state.mapping.selectedCountry;
}