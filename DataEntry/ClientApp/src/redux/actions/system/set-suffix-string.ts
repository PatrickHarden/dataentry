import { ActionPayload } from '../../../types/common/action';
import { ListingSuffix } from '../../../types/state';

// constants
export const SET_LISTING_SUFFIX = 'SET_LISTING_SUFFIX';

// types
export type SetListingSuffixAction = ActionPayload<ListingSuffix> & {
    type: typeof SET_LISTING_SUFFIX
};

export const setListingSuffix = (payload:ListingSuffix) : SetListingSuffixAction => ({
    type: SET_LISTING_SUFFIX,
    payload
});