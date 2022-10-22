import { ActionPayload } from '../../../types/common/action';

// constants
export const IS_DUPLICATING_LISTING = 'IS_DUPLICATING_LISTING';

// types
export type SetIsDuplicatingListingAction = ActionPayload<Boolean> & {
    type: typeof IS_DUPLICATING_LISTING
};

export const setIsDuplicatingListing = (payload:Boolean) : SetIsDuplicatingListingAction => ({
    type: IS_DUPLICATING_LISTING,
    payload
});