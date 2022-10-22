import { Team } from '../../../types/team/team';
import { postData } from '../../../api/glAxios'
import { deleteTeamQuery } from '../../../api/glQueries'
import { push } from 'connected-react-router';
import { loadTeams} from './load-teams';

const deleteData = (id: string) => {
    const query = deleteTeamQuery(id)
    const data = postData(query);
    return data;
}

export const deleteTeam = (team:Team) => (dispatch: Function) => {

    deleteData(team.id).then((result: any) => {
        if (result.data){
            dispatch(loadTeams());
        }
    })
}