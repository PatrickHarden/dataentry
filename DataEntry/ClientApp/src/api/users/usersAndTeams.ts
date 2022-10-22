import { AutoCompleteResult, AutoCompleteRequest } from '../../types/common/auto-complete';
import { getUsersOrTeams } from '../glQueries';
import { postData } from '../glAxios';
import { ListingTeamMember} from '../../types/listing/listingTeamMember'
import { UserSearchFilter } from '../../types/team/userSearchFilter'

// this is called by the searchable input
export const getUsersAndTeamsForSearch = (request:AutoCompleteRequest): Promise<AutoCompleteResult[]> => {

        const input:string = request.value.trim();
        const inputFilter: UserSearchFilter = { term: request.value.trim(), blacklist: request.extraData? JSON.parse(request.extraData) : []}
        const service = new google.maps.places.AutocompleteService();

        return new Promise((resolve, reject) => {
            postData(getUsersOrTeams(inputFilter))
            .then((response: any) => {
                if (typeof response.data === 'undefined'){
                    // error on user search... 
                    resolve([]);
                }else{     
                    const dbMembers:ListingTeamMember[] = response.data.claimants.map(
                        (m:any) => ({
                            "name": m.name,
                            "firstName": m.firstName, 
                            "lastName": m.lastName,
                            "fullName": m.fullName,
                            "isTeam": m.isTeam
                        })
                    );

                    resolve(convertUserResults(dbMembers));
                }
        });

    });    
}

// convert the user list results to get them in the form the searchable input needs to display
export const convertUserResults = (dbMembers:ListingTeamMember[]) => {

    const converted:AutoCompleteResult[] = [];

    dbMembers.forEach((result:ListingTeamMember) => {
        if (result.isTeam) {
            converted.push({ name: result.name, value: result});
        } 
        else {
            converted.push({ name: result.firstName + " " + result.lastName, value: result});
        }
        
    });

    return converted;
}