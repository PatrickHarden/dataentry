import { ActionPayload } from '../../../types/common/action';
import { Listing } from '../../../types/listing/listing';
import { State } from '../../../types/state';

// constants
export const SET_MIQ_SPACES = 'SET_MIQ_SPACES';

// types
export type SetMiqSpaces = ActionPayload<Listing[]> & {
    type: typeof SET_MIQ_SPACES
}

export const setMiqSpaces = (payload:any[]) : SetMiqSpaces => ({
    type: SET_MIQ_SPACES,
    payload
});

export const setMiqSpacesResult = (spaces:any, selected: boolean) => (dispatch: Function, getState: () => State) => {
    const currentSpaces = getState().miq.spaces;
    // determine whether to delete/add
    if (getState().miq.spaces.length > 0){
        getState().miq.spaces.forEach((space, index) => {
            if (space && space.miqId && selected && space.miqId !== spaces.miqId){
                currentSpaces.push(spaces)
            } else if (space && space.miqId && !selected && space.miqId === spaces.miqId){
                delete currentSpaces[index];
            }
        })
    } else if (selected) {
        currentSpaces.push(spaces)
    }

    // remove empty elements
    const filtered = currentSpaces.filter(el => {
        return el != null;
    });

    dispatch(setMiqSpaces(filtered));
}