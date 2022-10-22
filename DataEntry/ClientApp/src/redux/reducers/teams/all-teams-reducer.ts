import {teamsLoaded,TEAMS_LOADED} from '../../actions/teams/load-teams';
import { ActionType } from 'typesafe-actions';

export type TeamsLoadedAction = ActionType<typeof teamsLoaded>;

export default(state = {}, action: TeamsLoadedAction) => {
    switch(action.type){
        case TEAMS_LOADED:
            return [...action.payload];
        break;

        default:
            return state;
    }
}