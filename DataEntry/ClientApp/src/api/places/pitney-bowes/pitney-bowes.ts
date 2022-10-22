import { AutoCompleteResult, AutoCompleteRequest } from '../../../types/common/auto-complete';
import axios, { AxiosRequestConfig } from 'axios';
import { Address } from '../../../types/listing/address';
import { escapeRegexCharacters } from '../../apiUtil';
import queryString from 'query-string';

interface PitneyBowesUnit {
    formattedUnitAddress: string
}

interface PitneyBowesRange {
    placeName: string,
    units: PitneyBowesUnit[]
}

interface PitneyBowesGeometry {
    type: string,
    coordinates: number[]
}

interface PitneyBowesAddress {
    formattedAddress: string,
    mainAddressLine: string,
    placeName: string,
    areaName1: string,
    areaName2: string,
    areaName3: string,
    areaName4: string,
    postCode: string,
    postCodeExt: string,
    country: string,
    addressNumber: string,
    streetName: string,
    unitType: string,
    unitValue: string
}

interface PitneyBowesAddressResult {
    address: PitneyBowesAddress,
    geometry: PitneyBowesGeometry,
    totalUnitCount: number,
    ranges: PitneyBowesRange[]
}

const geosearchURL = "https://api.pitneybowes.com/location-intelligence/geosearch/v2/locations?";

const apiKey:string = "Rmc57nYFglzdoRhJ9qGZKaBzU0hNxtEE";
const secret:string = "ZkJj5K5TyuuC3Zgw";
const base64value:string = "Um1jNTduWUZnbHpkb1JoSjlxR1pLYUJ6VTBoTnh0RUU6WmtKajVLNVR5dXVDM1pndw==";
const token:any = undefined;

export const getPitneyPlaces = (request:AutoCompleteRequest): Promise<AutoCompleteResult[]> => {

    const escapedValue = escapeRegexCharacters(request.value.trim());   
    const urlValue:string = encodeURI(escapedValue);
    let country:string = "USA";
    if(request.extraData){
        country = request.extraData;
    }    
    // const cachebuster = new Date().getTime();
    // const autocomplete = true;
    // 
    // const types = "country%2Caddress";

    // https://api.pitneybowes.com/location-intelligence/geosearch/v2/locations?searchText=1%20Global%20V&country=AUS

    const requestBody = {
        'grant_type': 'client_credentials'
    }

    const postConfig:AxiosRequestConfig = {
        method: 'POST',
        url: 'https://api.pitneybowes.com/oauth/token',
        headers: {
            'Authorization': 'Basic ' + base64value,
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: queryString.stringify(requestBody)
    }

    return axios(postConfig).then((ores:any) => {

        // tokens expire after 36 seconds, so we just grab them every time for now...we should add logic to check a token instead
        
        const accessToken = ores.data.access_token;

        const getConfig:AxiosRequestConfig = {
            method: 'GET',
            url: geosearchURL + "searchText=" + urlValue + "&country=" + country,
            headers: {
                'Authorization': 'Bearer ' + accessToken
            }
        }

        return axios(getConfig).then(res => {
            return convertPitneyBowesResults(res);
        });
    });
}

export const convertPitneyBowesResults = (result:any) : AutoCompleteResult[] => {
    
    const converted:AutoCompleteResult[] = [];  

    if(result && result.data && result.data.location){
        const addressResults:PitneyBowesAddressResult[] = result.data.location;
        addressResults.forEach((addressResult:PitneyBowesAddressResult) => {
            converted.push({ name: addressResult.address.formattedAddress, value: addressResult});
        });
    }

    return converted;
}

export const createAddressFromPitneyBowesResult = (result: AutoCompleteResult):Address => {

    /*
        location: [0] {
            address: PitneyBowesAddressResult
            geometry: PitneyBowesGeometry,
            totalUnitCount: number
            ranges: PitneyBowesRange[]
        }
    */

    if(result.value && result.value){

        const location:PitneyBowesAddressResult = result.value;

        const lng = location.geometry.coordinates[0];
        const lat = location.geometry.coordinates[1];

        const city:string = location.address.areaName3;
        const postalCode:string = location.address.postCode;
        const stateOrProvince:string = location.address.areaName1;
        const country:string = location.address.country;
        const street = location.address.mainAddressLine;

        return{ 
            'street': street,
            'city': city,
            'postalCode': postalCode,
            'stateOrProvince': stateOrProvince,
            'country': country,
            'lat': lat,
            'lng': lng 
        }
    }else{
        return {
            'street': 'error',
            'city': 'error',
            'postalCode': 'error',
            'stateOrProvince': 'error',
            'country': 'error',
            'lat': 0,
            'lng': 0 
        }
    }

}