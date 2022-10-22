import { ActionPayload } from '../../../types/common/action';

// constants
export const SET_COUNTRY = 'SET_COUNTRY';

// types
export type SetCountryAction = ActionPayload<string> & {
    type: typeof SET_COUNTRY
};

export const setCountry = (payload:string) : SetCountryAction => ({
    type: SET_COUNTRY,
    payload
});