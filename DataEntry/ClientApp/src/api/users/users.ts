import { AutoCompleteResult, AutoCompleteRequest } from '../../types/common/auto-complete';
import { getUsers } from '../glQueries';
import { postData } from '../glAxios';
import { TeamMember} from '../../types/team/teamMember'
import { UserSearchFilter } from '../../types/team/userSearchFilter'

// this is called by the searchable input
export const getUsersForSearch = (request:AutoCompleteRequest): Promise<AutoCompleteResult[]> => {

        const input:string = request.value.trim();
        const inputFilter: UserSearchFilter = { term: request.value.trim(), blacklist: request.extraData? JSON.parse(request.extraData) : []}
        const service = new google.maps.places.AutocompleteService();                
    
        return new Promise((resolve, reject) => {
            postData(getUsers(inputFilter))
            .then((response: any) => {
                if (typeof response.data === 'undefined'){
                    // error on user search... 
                    resolve([]);
                }else{     
                    const dbUsers:TeamMember[] = response.data.users.map(
                        (m:any) => ({
                            "teamMemberId": m.id, 
                            "firstName": m.firstName, 
                            "lastName": m.lastName,
                            "email" : m.id
                        })
                    );

                    resolve(convertUserResults(dbUsers));
                }
        });

    });    
}

// convert the user list results to get them in the form the searchable input needs to display
export const convertUserResults = (dbUsers:TeamMember[]) => {

    const converted:AutoCompleteResult[] = [];

    dbUsers.forEach((result:TeamMember) => {
        converted.push({ name: result.firstName + " " + result.lastName, value: result});
    });

    return converted;
}