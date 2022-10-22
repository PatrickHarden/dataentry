import { AutoCompleteResult, AutoCompleteRequest } from '../../../types/common/auto-complete';
import axios from 'axios';
import { Address } from '../../../types/listing/address';
import { escapeRegexCharacters } from '../../apiUtil';

const placesURL = "https://api.mapbox.com/geocoding/v5/mapbox.places/";
const accessToken = "pk.eyJ1IjoibGlnbGRldnMiLCJhIjoiY2p3amthb2R4MGR6cjQ5cW41MWsxdHB3ciJ9.cmINTRsP78I-obiEXYoNqw";

interface MapboxPlaceResult {
    place_name: string,
    result: any
}

interface MapboxContextResult {
    id: string,
    text: string
}

export const getMapboxPlaces = (request:AutoCompleteRequest): Promise<AutoCompleteResult[]> => {

    const escapedValue = escapeRegexCharacters(request.value.trim());   
    const urlValue:string = encodeURI(escapedValue);

    const cachebuster = new Date().getTime();
    const autocomplete = true;
    const country = "us";
    const types = "country%2Caddress";

    // note: append "&country=" + country
    // to restrict to country

    const requestURL:string = placesURL + urlValue + ".json?access_token=" + accessToken + "&cachebuster=" + 
        cachebuster + "&autocomplete=" + autocomplete + "&types=" + types;

    return axios.get(requestURL).then(res => {
        return convertMapboxResults(res.data.features);
    });
}

export const convertMapboxResults = (results:MapboxPlaceResult[]) : AutoCompleteResult[] => {
    
    const converted:AutoCompleteResult[] = [];

    results.forEach((result:MapboxPlaceResult) => {
        converted.push({ name: result.place_name, value: result});
    });

    return converted;
}

const extractMapboxContextResult = (contexts:MapboxContextResult[],search:string):string => {
    let foundContextVaue:string = "";
    contexts.forEach((context:MapboxContextResult) => {
        if(context && context.id && context.id.indexOf(search) > -1){
            foundContextVaue = context.text;
            return;
        }
    });
    return foundContextVaue;
}

export const createAddressFromMapboxResult = (result: AutoCompleteResult):Address => {

    // mapbox : name, value 
    /* 
        value: 
            center: [0] (lng), [1] (lat),
            context: [
                {id: "postcode...."},   i.e. 80109
                {id: "place..."}, i.e., "Castle Rock"
                {id: "region..."}, i.e., "Colorado"
                {id: "country..."}, i.e., "United States"
            ],
            address: ""  i.e., "3188",
            text: "" i.e., "W. Calypso Court"

    */

    const lng = result.value.center[0];
    const lat = result.value.center[1];

    const city:string = extractMapboxContextResult(result.value.context,"place");
    const postalCode:string = extractMapboxContextResult(result.value.context,"postcode");
    const stateOrProvince:string = extractMapboxContextResult(result.value.context,"region");
    const country:string = extractMapboxContextResult(result.value.context,"country");
    const street = (result.value.address ? result.value.address + " " : "") + (result.value.text ? result.value.text : "");
        
    return {
        'street': street,
        'city': city,
        'postalCode': postalCode,
        'stateOrProvince': stateOrProvince,
        'country': country,
        'lat': lat,
        'lng': lng
    }
}