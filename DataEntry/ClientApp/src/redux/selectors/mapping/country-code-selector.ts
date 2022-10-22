import { State } from '../../../types/state';

export const countryCodeSelector = (state:State) => {
    return state.mapping.countryCode;
}