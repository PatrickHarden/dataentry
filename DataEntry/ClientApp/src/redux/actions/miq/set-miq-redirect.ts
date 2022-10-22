import { ActionPayload } from "../../../types/common/action";
import { State } from "../../../types/state";
import { checkEnvironment } from "../../../utils/helpers/check-environment";

export const SET_MIQ_REDIRECT = 'SET_MIQ_REDIRECT ';

// types
export type SetMIQRedirectAction = ActionPayload<string | undefined> & {
    type: typeof SET_MIQ_REDIRECT
};

export const dispatchMIQRedirect = (payload:string | undefined) : SetMIQRedirectAction => ({
    type: SET_MIQ_REDIRECT,
    payload
});

export const setMIQRedirect = (redirect:string)  => (dispatch: Function, getState: () => State) => { 
    if(!redirect || redirect.trim().length === 0){
        const config = getState().system.configDetails.config;
        if(checkEnvironment(config, {localhost: true, dev: false, uat: false, prod: false})){
            redirect = "https://marketiq-uat.cbre.com/";
        }else if(checkEnvironment(config, {localhost: false, dev: true, uat: false, prod: false})){
            redirect = "https://marketiq-dev.cbre.com/";
        }else if(checkEnvironment(config, {localhost: false, dev: false, uat: true, prod: false})){
            redirect = "https://marketiq-uat.cbre.com/";
        }else{
            redirect = "https://marketiq.cbre.com/";
        }
    }
    // record the selection in memory
    dispatch(dispatchMIQRedirect(redirect));
}