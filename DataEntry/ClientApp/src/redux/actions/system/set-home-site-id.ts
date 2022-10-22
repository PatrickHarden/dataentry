import { ActionPayload } from '../../../types/common/action';

// constants
export const SET_HOME_SITE_ID = 'SET_HOME_SITE_ID';

// types
export type SetHomeSiteIdAction = ActionPayload<string> & {
    type: typeof SET_HOME_SITE_ID
};

export const setHomeSiteId = (payload:string) : SetHomeSiteIdAction => ({
    type: SET_HOME_SITE_ID,
    payload
});