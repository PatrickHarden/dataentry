import { GeoCoordinates } from '../types/map/maps';
import { Listing } from '../types/listing/listing';

export const createCoordinates = (data:Listing): GeoCoordinates | undefined => {
    if(data.lat !== undefined && data.lng !== undefined){
        return {
            lat: data.lat,
            lng: data.lng
        };
    }
    return {lat:1, lng:1};
}

export const checkCoordinates = (coordinate:GeoCoordinates | undefined):boolean => {
    // todo: we can surely simplify this.
    let validLat:boolean = false;
    let validLng:boolean = false;
    if(coordinate){
        
        if(coordinate.lng && coordinate.lng > -180 && coordinate.lng < 180){
            validLng = true;
        }
        
        if(coordinate.lat && coordinate.lat > -90 && coordinate.lat < 90){
            validLat = true;
        }
    }
    return validLat && validLng;
}