import { AutoCompleteResult, AutoCompleteRequest } from '../../../types/common/auto-complete';
import { Address } from '../../../types/listing/address';
import { GeoCoordinates } from '../../../types/map/maps';
import { AddressComponentsConfig } from "../../../types/config/config"

// this is called by the searchable input
export const getGooglePlaces = (request:AutoCompleteRequest): Promise<AutoCompleteResult[]> => {

    const input:string = request.value.trim();
    const service = new google.maps.places.AutocompleteService();

    // todo: figure out how to actually remove the restrictions if they aren't set instead of defaulting to the US
    let componentRestrictions = {
        country: ['us']
    };
    if(request.countryCodes){
        componentRestrictions = {
            country: request.countryCodes
        }
    }

    return new Promise((resolve, reject) => {
        if(request.value.trim().length === 0){  // if we don't have a request value, then just return immediately without hitting service
            resolve([]);
            return;
        }
        service.getPlacePredictions({input, componentRestrictions},
            (predictions, status) => {
                if (status !== google.maps.places.PlacesServiceStatus.OK) {
                    // for now, just send back a blank set of results. we should probably handle this gracefully with the component when we build the no results out
                    // we need to send back a blank array though, otherwise the previous results stay up if they are still typing.
                    resolve([]); 
                }
                if (predictions != null) {
                    resolve(convertGoogleResults(predictions));
                }
            });
        }
    ); 
}

// convert the google results to get them in the form the searchable input needs to display
export const convertGoogleResults = (predictions: google.maps.places.AutocompletePrediction[]) => {

    const converted:AutoCompleteResult[] = [];

    predictions.forEach((result:google.maps.places.AutocompletePrediction) => {
        converted.push({ name: result.description, value: result});
    });

    return converted;
}

export const createAddressFromGoogleResultUsingLatLng = async(result: {lat:number, lng:number}):Promise<Address> => {

    const request:google.maps.GeocoderRequest = {
        location: {lat: result.lat, lng: result.lng}
    };

    const addresses = await geocode(request);
    return parseGoogleResult(addresses);
}

// this method is called once a user actually selects an address
export const createAddressFromGoogleResult = async(result: AutoCompleteResult, addressComponentsConfig?:AddressComponentsConfig):Promise<Address> => {

    // if the user selects a result, we need to geocode
    const request:google.maps.GeocoderRequest = {
        placeId: result.value.place_id
    };

    const addresses = await geocode(request);
    return parseGoogleResult(addresses, addressComponentsConfig);
}

const parseGoogleResult = (addresses:google.maps.GeocoderResult[], addressComponentsConfig?:AddressComponentsConfig):Address => {
    if(addresses.length > 0){
        
        const geocodingResult:google.maps.GeocoderResult = addresses[0];
        
        const { bounds, viewport } = geocodingResult.geometry;
        const googleBounds = bounds || viewport;
        const coords:GeoCoordinates = convertGoogleLatLng(googleBounds.getCenter());
        const addressComponents = geocodingResult.address_components;

        const cityFieldName = (addressComponentsConfig && addressComponentsConfig.cityFieldName !== "") ? addressComponentsConfig.cityFieldName! : "locality";
        const city:string = extractResult(addressComponents, cityFieldName,'long_name');

        const postalCode:string = extractResult(addressComponents,'postal_code','long_name');
        let stateOrProvince:string = extractResult(addressComponents,'administrative_area_level_1','short_name');
        if(stateOrProvince && stateOrProvince.length > 0){
            stateOrProvince = stateOrProvince.split('.').join(""); // remove periods from state abbreviations, if they exist
            stateOrProvince = stateOrProvince.toUpperCase(); // make all abbreviations upper case
        }

        const country:string = extractResult(addressComponents,'country','short_name');

        const streetNumber:string = extractResult(addressComponents,'street_number','long_name');
        const route:string = extractResult(addressComponents,'route','long_name');

        let street = streetNumber ? streetNumber + ' ' + route : route;

        // fallback for street
        // point of interest
        if(!street || street.length === 0){
            street = extractResult(addressComponents,'point_of_interest','long_name');
        }
        // final fallback is the entire formatted address
        if(!street || street.length === 0){
            street = geocodingResult.formatted_address;
        }    

        return {
            'street': street,
            'city': city,
            'postalCode': postalCode,
            'stateOrProvince': stateOrProvince,
            'country': country,
            'lat': coords.lat,
            'lng': coords.lng
        }
    }else{
        throw new Error('Geocoding Failed');
    }
}

// convenience function to help us extract data from the google object
const extractResult = (addressComponents:google.maps.GeocoderAddressComponent[], fieldName:string, use:string) => {
    let extracted:string = "";
    addressComponents.forEach((addressPart:google.maps.GeocoderAddressComponent) => {
        if(addressPart.types && addressPart.types.indexOf(fieldName) > -1){
            extracted = addressPart[use];
        }
    });
    return extracted;
}

// extra geocoding service since they aren't passed back by the prediction service
const geocode = async (
    request: google.maps.GeocoderRequest
  ): Promise<google.maps.GeocoderResult[]> => {

    const geocoder: google.maps.Geocoder = new google.maps.Geocoder();

    return new Promise((resolve, reject) => {
      geocoder.geocode(request, (results, status) => {
        if (results && status === google.maps.GeocoderStatus.OK) {
          resolve(results);
        } else {
          reject(status);
        }
      });
    });
};

const convertGoogleLatLng = (latLng: google.maps.LatLng): GeoCoordinates => {
    return { lat: latLng.lat(), lng: latLng.lng() };
}