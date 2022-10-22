import { ActionPayload } from '../../../types/common/action';
import { State } from '../../../types/state';

// constants
export const SET_SKIP = 'SET_SKIP';
export const SET_MORE_RECORDS = 'SET_MORE_RECORDS';
export const SET_TAKE = 'SET_TAKE';

// types
export type SetSkipAction = ActionPayload<number> & {
    type: typeof SET_SKIP
};

export type SetMoreRecordsAction = ActionPayload<boolean> & {
    type: typeof SET_MORE_RECORDS
};

export type SetTakeAction = ActionPayload<number> & {
    type: typeof SET_TAKE
};

export const setSkip = (payload:number) : SetSkipAction => ({
    type: SET_SKIP,
    payload
});

export const setMoreRecords = (payload:boolean) : SetMoreRecordsAction => ({
    type: SET_MORE_RECORDS,
    payload
});

export const setTake = (payload:number) : SetTakeAction => ({
    type: SET_TAKE,
    payload
});

export const checkTake = (newTake:number) => (dispatch: Function, getState: () => State) => {
    // check the current take
    const currentTake = getState().pagedListings.take;
    if(newTake !== currentTake){
        dispatch(setTake(newTake));
    }
}