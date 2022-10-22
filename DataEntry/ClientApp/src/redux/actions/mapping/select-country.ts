import { ActionPayload } from '../../../types/common/action';

// constants
export const SELECT_COUNTRY = 'SELECT_COUNTRY';

// types
export type SelectCountryAction = ActionPayload<string> & {
    type: typeof SELECT_COUNTRY
};

export const selectCountry = (payload:string) : SelectCountryAction => ({
    type: SELECT_COUNTRY,
    payload
});