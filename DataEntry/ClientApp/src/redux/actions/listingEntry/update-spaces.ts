import { ActionPayload } from '../../../types/common/action';
import { Space } from '../../../types/listing/space';
// constants
export const UPDATE_SPACES = 'UPDATE_SPACES';

// types
export type UpdateSpaces = ActionPayload<Space[]> & {
    type: typeof UPDATE_SPACES;
};

export const updateSpaces = (payload: Space[]): UpdateSpaces => ({
    type: UPDATE_SPACES,
    payload
}); 