import { State, MainMessageType } from '../../../types/state';
import { Team } from '../../../types/team/team';
import { setMainMessage, clearMessage } from '../system/set-main-message';
import { postData } from '../../../api/glAxios'
import { updateTeamQuery, createNewTeamQuery } from '../../../api/glQueries'
import { push } from 'connected-react-router';
import { setAlertMessage } from '../../../redux/actions/system/set-alert-message';
import { AlertMessagingType } from '../../../types/state';
import { loadTeams} from './load-teams';

const saveData = (team:Team) => {
    let data:any;
    if (team.id){
        const query = updateTeamQuery(team);
        // console.log(query);
        data = postData(query)
    } else {
        const query = createNewTeamQuery(team)
        data = postData(query);
    }
    return data;
}

// thunks
export const saveTeam = (team:Team) => (dispatch: Function, getState: () => State) => {

    dispatch(setMainMessage({ show: true, type: MainMessageType.SAVING, message: "Saving Team..." }));

    saveData(team).then((result: any) => {
        // if data returns a create team or update team object, it worked & update teams     
        if (result && result.data ){
            
            dispatch(loadTeams());
            
        } else{
            // error state
            clearMessage(dispatch);
            
            dispatch(setAlertMessage({show: true, message: "There was an Error while saving team.", type: AlertMessagingType.ERROR, allowClose: true}));
        }
    })
}