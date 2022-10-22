import { State } from '../../../types/state';

export const allTeamsSelector = (state:State) => {
    return state.teams.allTeams;
}