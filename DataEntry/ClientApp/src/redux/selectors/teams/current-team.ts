import { State } from '../../../types/state';

export const currentTeamSelector = (state:State) => {
    return state.teams.currentTeam;
}