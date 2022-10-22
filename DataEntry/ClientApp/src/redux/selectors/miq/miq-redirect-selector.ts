import { State } from '../../../types/state';

export const miqRedirectSelector = (state:State):string => {
    return state.miq.redirect;
}