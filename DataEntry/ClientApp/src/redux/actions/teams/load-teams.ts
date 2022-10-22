import { ActionPayload } from '../../../types/common/action';
import { State, MainMessageType } from '../../../types/state';
import { Team } from '../../../types/team/team';
import { postData } from '../../../api/glAxios';
import { getAllTeamsForUser } from '../../../api/glQueries';
import { setMainMessage, clearMessage } from '../system/set-main-message';
import { authContext } from '../../../adalConfig';

// constants
export const TEAMS_LOADED = 'LOAD_TEAMS';

// types
export type TeamsLoadedAction = ActionPayload<Team[]> & {
    type: typeof TEAMS_LOADED
};

export const teamsLoaded = (payload:Team[]) : TeamsLoadedAction => ({
    type: TEAMS_LOADED,
    payload
});



export const loadTeams = () => (dispatch: Function, getState: () => State) => {
  
    dispatch(setMainMessage({ show: true, type: MainMessageType.LOADING, message: "Loading Teams..." }));
    const currentUser = authContext.getCachedUser();
    postData(getAllTeamsForUser())
        .then((response: any) => {
            if (typeof response.data === 'undefined'){
                dispatch(setMainMessage({show: true, type: MainMessageType.ERROR, message: "There was an error loading teams."}));
            // }else if(response.data.teams && response.data.teams.length === 0){
            //     dispatch(setMainMessage({ show: true, type: MainMessageType.NOTICE, message: "There are no existing listings.", messageLine2: "Go ahead and create one." }));
            }else{
                clearMessage(dispatch);
                  
                
                const dbTeams:Team[] = response.data.teams.map(
                    (m:any) => ({
                        "id": m.name, 
                        "name":m.name, 
                        "users": m.users.map(
                            (user:any) => ({
                                "firstName": user.firstName? user.firstName : "",
                                "lastName" : user.lastName? user.lastName : "",
                                "email" : user.id? user.id : "",
                            })
                        ) 
                    })
                );
                
                dispatch(teamsLoaded(dbTeams));
            }
    });
}