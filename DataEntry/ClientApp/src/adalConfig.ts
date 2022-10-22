import { AuthenticationContext, adalFetch, withAdalLogin, AdalConfig } from 'react-adal';
import { environmentConfigs } from "./config/adal-setups";

export const findURLMatch = ():AdalConfig => {
  const matchingConfig = environmentConfigs.urlMatches.filter((urlMatch:any) => urlMatch.urls && urlMatch.urls.indexOf(window.location.hostname) > -1);
  let config = environmentConfigs.default.adalConfig;
  if(matchingConfig.length > 0){
    config = matchingConfig[0].adalConfig;
  }
  return {
    tenant: config.tenant,
    clientId: config.clientId,
    endpoints: config.endpoints,
    cacheLocation: config.cacheLocation && config.cacheLocation === "localStorage" || config.cacheLocation === "sessionStorage" ? config.cacheLocation : undefined,
    redirectUri: window.location.origin
  }
}

export const adalConfig = findURLMatch();

export const authContext = new AuthenticationContext(adalConfig);

authContext.log = (loglevel, message, error) => {
  console.log(`loglevel: ${loglevel}, message: ${message}, error: ${error}`);
}

let endpoint:string = '';
if (adalConfig !== undefined){
  if (adalConfig.endpoints !== undefined) {
      endpoint = adalConfig.endpoints.api
  }
}

export const adalApiFetch = (fetch: any, url: string, options: any = {}) => {
  return adalFetch(authContext, endpoint, fetch, url, options);
}

export const withAdalLoginApi = withAdalLogin(authContext, endpoint)