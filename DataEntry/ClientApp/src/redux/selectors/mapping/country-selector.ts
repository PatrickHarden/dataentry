import { State } from '../../../types/state';

export const countrySelector = (state:State) => {
    return state.mapping.country;
}