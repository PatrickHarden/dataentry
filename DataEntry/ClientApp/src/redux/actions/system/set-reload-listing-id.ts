import { ActionPayload } from '../../../types/common/action';

// constants
export const RELOAD_LISTING_ID = 'RELOAD_LISTING_ID';

// types
export type SetReloadListingIdAction = ActionPayload<number> & {
    type: typeof RELOAD_LISTING_ID
};

export const setReloadListingId = (payload:number) : SetReloadListingIdAction => ({
    type: RELOAD_LISTING_ID,
    payload
});