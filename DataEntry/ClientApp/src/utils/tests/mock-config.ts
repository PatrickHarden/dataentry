import { Config } from '../../types/config/config';
import { ReplaceProperty, findAndReplace } from './replace-json';

export enum TestConfig {
    US = "us-comm",
    SG = "sg-comm",
    IN = "in-comm",
    NL = "nl-comm"
}

export const getMockConfig = (testConfigToLoad:TestConfig, replacements?:ReplaceProperty[]):Config => {

    // load the test config using the enum/site id combo
    let config:Config = require("../../config/" + testConfigToLoad.valueOf() + ".json");

    if(replacements && replacements.length > 0){
        config = findAndReplace(config, replacements);
    }

    return config;
}   