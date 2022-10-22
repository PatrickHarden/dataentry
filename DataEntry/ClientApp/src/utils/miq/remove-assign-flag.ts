import { Listing } from '../../types/listing/listing';

export const checkAssignFlag = (listing:Listing):boolean => {
    if(listing && listing.listingAssignment && listing.listingAssignment.assignmentFlag){
        return true;
    }
    return false;
}