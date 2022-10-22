import { State } from '../../../types/state';

export const routeSelector = (state:State) => {
    return state.router.location.pathname;
}