import { ActionPayload } from '../../../types/common/action';
import { State, FeatureFlags, System, User } from '../../../types/state';
import { postData } from '../../../api/glAxios';
import { loadAppSettings, pullRegions } from '../../../api/glQueries';
import { ConfigDetails, Config } from '../../../types/config/config';
import { PropertyTypeOption } from '../../../types/common/option';
import { updateFeatureFlags } from '../featureFlags/update-feature-flags';
import { checkEnvironment } from '../../../utils/helpers/check-environment';
import { checkAdmin, setAdminLoaded } from '../user/get-admin';
import { setHomeSiteId } from './set-home-site-id';
import { loadGoogleMaps } from './load-google-maps';

// constants
export const CONFIG_DETAILS_LOADED = 'CONFIG_DETAILS_LOADED';

// types
export type AllContactsLoadedAction = ActionPayload<ConfigDetails> & {
    type: typeof CONFIG_DETAILS_LOADED
};

export const configDetailsLoaded = (payload: ConfigDetails): AllContactsLoadedAction => ({
    type: CONFIG_DETAILS_LOADED,
    payload
});

// setup a property type color map for easy reference later on
const setupPropertyTypeColors = (config:Config) => {
    const colors:Map<string,string> = new Map();
    if(config && config.propertyType && config.propertyType.options){
        config.propertyType.options.forEach((option:PropertyTypeOption) => {
            if(option.color && option.color.length > 0){
                colors.set(option.value,option.color);
            }
        });
    }
    return colors;
}

// setup a property type labels
const setupPropertyTypeLabels = (config:Config) => {
    const labels:Map<string,string> = new Map();
    if(config && config.propertyType && config.propertyType.options){
        config.propertyType.options.forEach((option:PropertyTypeOption) => {
            if(option.label && option.label.length > 0){
                labels.set(option.value,option.label);
            }
        });
    }
    return labels;
}

// setup listing filters
export const loadConfig = (siteId?:string, overrideHomeSiteId?:boolean) => (dispatch: Function, getState: () => State) => {
    const query = pullRegions();

    if (getState().system.configDetails.regions){
        setupAdmin(dispatch, getState(), siteId, getState().system.configDetails.regions);
        if(overrideHomeSiteId && siteId){
            dispatch(setHomeSiteId(siteId));
        }
    } else {
        postData(query).then((response: any) => {
            const results = response.data.regions;
            setupAdmin(dispatch, getState(), siteId, results);
        if(overrideHomeSiteId && siteId){
            dispatch(setHomeSiteId(siteId));
        }
        })
    }
}

// get admin first
async function setupAdmin(dispatch: Function, state:State, siteId?:string, regions?: any) {

    
    checkAdmin(state).then((adminCheck:any) => {

        const isAdmin:boolean = adminCheck && adminCheck.isAdmin ? adminCheck.isAdmin : false;
        setupEnvironment(dispatch, state, isAdmin, siteId, regions);

        if(adminCheck){
            dispatch(setAdminLoaded(adminCheck));
        }
    });
}

// then setup our environment
const setupEnvironment = (dispatch:Function, state:State, isAdmin:boolean, siteId?:string, regions?: any) => {
    
    // todo: don't want to use any here, but typescript is not playing nice
    const configDetails:any = {
        loaded: false,
        forced: false,
        error: "",
        aiKey: "",
        regions: regions
    }
    if(siteId && siteId.length > 0){

        // we are forcing the site id instead of loading from graphql (i.e., switch [this should only happen in localhost/dev/uat])
        try{
            const config:Config = require("../../../config/" + siteId + ".json");
            configDetails.loaded = true;
            configDetails.forced = true;
            configDetails.config = config;
            configDetails.propertyTypeColors = setupPropertyTypeColors(config);
            configDetails.propertyTypeLabels = setupPropertyTypeLabels(config);

            // feature flags
            let ff:FeatureFlags = {
                previewFeatureFlag: false,
                watermarkDetectionFeatureFlag: false,
                miqImportFeatureFlag: false,
                miqLimitSearchToCountryCodeFeatureFlag: true
            };
            if(state.featureFlags){
                ff = state.featureFlags;
            }            
            ff = checkForFeatureFlagOverrides(config, isAdmin, ff);

            dispatch(updateFeatureFlags(ff));


        }catch(error){
            if (typeof(Storage) !== "undefined") { localStorage.removeItem("siteId"); } // prevents getting stuck on a siteid where the config doesn't exist
            configDetails.error = "There was an error loading the requested configuration - please double check the siteId being passed";
        }
        dispatch(configDetailsLoaded(configDetails));
    }else{
        // we don't have a site id passed in, so load from graphql (or local storage) and get the configured default site id
        const query = loadAppSettings();
        postData(query)
            .then((response: any) => {
                if(response.data){
                    // Use SiteId from local storage first. If it's not there then fallback to graphql.
                    const graphQLSiteId:string = typeof(Storage) !== "undefined" && localStorage.getItem("siteId") ? localStorage.getItem("siteId") : response.data.configs.homeSiteId;
                    const aiKey:string = response.data.configs.aiKey;
                
                    const previewFlag:boolean = JSON.parse(response.data.configs.previewFeatureFlag);
                    const wdFlag:boolean = JSON.parse(response.data.configs.watermarkDetectionFeatureFlag);
                    const miqImportFlag:boolean = JSON.parse(response.data.configs.miqImportFeatureFlag);
                    const miqLimitSearchFlag:boolean = JSON.parse(response.data.configs.miqLimitSearchToCountryCodeFeatureFlag);
                    const googleMapsKey:string = response.data.configs.googleMapsKey;
                    const googleMapsChannel:string = response.data.configs.googleMapsChannel;

                    try{
                        const config:Config = require("../../../config/" + graphQLSiteId + ".json");
                        configDetails.loaded = true;
                        configDetails.config = config;
                        configDetails.aiKey = aiKey;
                        configDetails.propertyTypeColors = setupPropertyTypeColors(config);
                        configDetails.propertyTypeLabels = setupPropertyTypeLabels(config);
                        dispatch(setHomeSiteId(graphQLSiteId))

                        // update the feature flags in the store based on data loaded from the backend
                        let featureFlags:FeatureFlags = {
                            previewFeatureFlag: previewFlag,
                            watermarkDetectionFeatureFlag: wdFlag,
                            miqImportFeatureFlag: miqImportFlag,
                            miqLimitSearchToCountryCodeFeatureFlag: miqLimitSearchFlag 
                        };
                        featureFlags = checkForFeatureFlagOverrides(config, isAdmin, featureFlags);

                        dispatch(updateFeatureFlags(featureFlags));

                        // load google maps 
                        loadGoogleMaps(googleMapsKey, googleMapsChannel)

                    }catch(error){
                        configDetails.error = "There was an error loading the site configuration.";
                    }
                    dispatch(configDetailsLoaded(configDetails));
                }
        });
    }
}

const checkForFeatureFlagOverrides = (config:Config, isAdmin:boolean | null, currentFeatureFlags: FeatureFlags):FeatureFlags => {
    // individual client config overrides
    // use case 1: India needs MIQ import in UAT (ADMINS only)
    if(config.featureFlags && config.featureFlags.overrides 
        && config.featureFlags.overrides.uatImportFromMIQRequireAdmin 
        && checkEnvironment(config,{localhost: true, dev: true, uat: true, prod: false})
        && isAdmin){
            currentFeatureFlags.miqImportFeatureFlag = config.featureFlags.overrides.uatImportFromMIQRequireAdmin;
    }
    return currentFeatureFlags;
}   