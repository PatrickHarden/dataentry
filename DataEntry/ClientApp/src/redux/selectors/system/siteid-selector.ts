import { State } from '../../../types/state';

export const configSiteIdSelector = (state:State) => {
    if (state.system.configDetails.config && state.system.configDetails.config.siteId){
        return state.system.configDetails.config.siteId;
    } else {
        return undefined;
    }
}

export const homeSiteIdSelector = (state:State) => {
    return state.system.homeSiteId;
}