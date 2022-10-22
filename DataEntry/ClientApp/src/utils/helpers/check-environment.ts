import { Config } from "../../types/config/config";

export interface Environments {
    localhost: boolean,
    dev: boolean,
    uat: boolean,
    prod: boolean
}

export const checkEnvironment = (config:Config, environments:Environments):boolean => {
    const url: string = window.location.href;
    if(config){
        if(environments.localhost && url.indexOf("localhost") > -1){
            return true;
        }else if(environments.dev && url.indexOf("dev") > -1){
            return true;
        }else if(environments.uat && url.indexOf("uat") > -1){
            return true;
        }else if(environments.prod){    // if we are checking on prod, and we've gotten this far, assumption is we must be in prod
            return true;
        }
    }
    return false;
}