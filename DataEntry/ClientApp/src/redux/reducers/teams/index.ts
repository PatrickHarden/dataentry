import {combineReducers} from 'redux';
import currentTeamReducer from './current-team-reducer';
import allTeamsReducer from './all-teams-reducer';

export default combineReducers({
    currentTeam: currentTeamReducer,
    allTeams: allTeamsReducer
});