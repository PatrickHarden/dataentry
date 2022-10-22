import { ActionPayload } from '../../../types/common/action';
import { State, MainMessageType } from '../../../types/state';
import { Team } from '../../../types/team/team';
import defaultTeam from '../../../api/defaults/team';
import { setMainMessage, clearMessage } from '../system/set-main-message';
import cloneDeep from 'clone-deep';
import { authContext } from '../../../adalConfig';

// constants
export const CURRENT_TEAM_LOADED = 'CURRENT_TEAM_LOADED';

// types
export type CurrentTeamLoadedAction = ActionPayload<Team> & {
    type: typeof CURRENT_TEAM_LOADED
};

export const currentTeamLoaded = (payload: Team): CurrentTeamLoadedAction => ({
    type: CURRENT_TEAM_LOADED,
    payload
});

const getData = (id: string) => {
    // const query = pullSingleTeam(parseInt(id, 10));
    // const data = postData(query);
    const data = {"id": "0", "name": "test", "users" : []}
    return data;
}

export const loadCurrentTeam = (id: string) => (dispatch: Function, getState: () => State) => {

    if (id !== undefined && id.length > 0) {
    
        // getData(id).then(result => {
        //     if (result.data && result.data.team){
        //         const team:Team = result.data.team;
        //         dispatch(currentTeamLoaded(team));
        //         clearMessage(dispatch);
        //     } else {
        //       //  dispatch(setMainMessage({ show: true, type: MainMessageType.ERROR, message: "Unauthorized content", home:true }));
        //     }
        // })
    } else {
        // default listing
        const newTeam = cloneDeep(defaultTeam);
        const currentUser = authContext.getCachedUser();
        newTeam.users.push({teamMemberId: "", firstName: currentUser.profile.given_name , lastName: currentUser.profile.family_name, email: currentUser.userName});
        dispatch(currentTeamLoaded(newTeam));
        clearMessage(dispatch);
    }
}
