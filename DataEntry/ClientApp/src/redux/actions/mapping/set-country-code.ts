import { ActionPayload } from '../../../types/common/action';

// constants
export const SET_COUNTRY_CODE = 'SET_COUNTRY_CODE';

// types
export type SetCountryCodeAction = ActionPayload<string> & {
    type: typeof SET_COUNTRY_CODE
};

export const setCountryCode = (payload:string) : SetCountryCodeAction => ({
    type: SET_COUNTRY_CODE,
    payload
});