import { ActionPayload } from '../../../types/common/action';
import { State, User } from '../../../types/state';
import { postData } from '../../../api/glAxios';
import { defaultRegionID, isAdmin } from '../../../api/glQueries';


// constants
export const SET_ADMIN = 'SET_ADMIN';

// types
export type SetAdminAction = ActionPayload<User> & {
    type: typeof SET_ADMIN
};

export const setAdminLoaded = (payload:User) : SetAdminAction => ({
    type: SET_ADMIN,
    payload
});

export async function checkAdmin(state:State) {

    const user = state.system.user;
    const region = state.mapping.selectedCountry || defaultRegionID;

    let userData:any;

    if(user && (!user.isAdmin || !user.isAdmin === null)){

        return await postData(isAdmin(region))
            .then((response: any) => {

                if(response && response.data.isAdmin !== null){
                    userData = Object.assign({},user);
                    userData.isAdmin = response.data.isAdmin;
                    return userData;
                }else{
                    return userData;
                }
        }); 
    }else {
        return userData;
    }  
}