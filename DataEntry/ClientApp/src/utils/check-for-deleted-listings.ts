import { Listing } from '../types/listing/listing';


export const checkForDeletedListings = (listings: Listing[]) => {
    let result: boolean = false;
    for (const listing of listings){
        if (listing.isDeleted){
            result = true;
            break;
        }
    }

    return result;
}