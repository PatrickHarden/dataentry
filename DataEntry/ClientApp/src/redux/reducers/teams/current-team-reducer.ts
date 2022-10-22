import {currentTeamLoaded,CURRENT_TEAM_LOADED} from '../../actions/teams/load-current-team';
import { ActionType } from 'typesafe-actions';

export type CurrentTeamLoadedAction = ActionType<typeof currentTeamLoaded>;

export default(state = {}, action: CurrentTeamLoadedAction) => {
    switch(action.type){
        case CURRENT_TEAM_LOADED:
            return {...state,...action.payload}
        break;

        default:
            return state;
    }
}