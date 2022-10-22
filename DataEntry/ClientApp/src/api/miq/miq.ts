import { AutoCompleteResult, AutoCompleteRequest } from '../../types/common/auto-complete';
import { findMiQProperties } from '../glQueries';
import { postData } from '../glAxios';
import { MIQSearchResult } from '../../types/miq/miqSearchResultInt';

// this is called by the searchable input
export const getMIQProperties = (request: AutoCompleteRequest): Promise<AutoCompleteResult[]> => {

    const input: string = request.value.trim();

    const miqCountryCode = request.countryCodes && request.countryCodes.length > 0 ? request.countryCodes[0] : "";

    return new Promise((resolve, reject) => {
        postData(findMiQProperties(input, miqCountryCode)).then((response: any) => {
            if (response.data && response.data.searchEdpProperties) {
                resolve(convertUserResults(response.data.searchEdpProperties));
            } else {
                reject('Error searching...')
            }
        }).catch(() => {
            reject('Error searching...')
        });
    });
}


// convert the user list results to get them in the form the searchable input needs to display
export const convertUserResults = (properties: MIQSearchResult[]) => {

    const converted: AutoCompleteResult[] = [];

    properties.forEach((property: MIQSearchResult) => {
        if (property) {
            converted.push({ name: convertAddressToLabel(property) , value: property });
        }

    });
    return converted;
}


// convert miq selection result to a label the autocomplete component can utilize
const convertAddressToLabel = (property: MIQSearchResult) => {
    return (
        (property.street1 ? property.street1 + ' ' : '') +
        (property.name ? property.name + ', ' : '') + 
        (property.city ? property.city + (property.stateProvince ? ' ' : '') : '') + 
        (property.stateProvince ?  property.stateProvince + '' : '') + 
        (property.postalCode ? ', ' + property.postalCode : '')
    );
}