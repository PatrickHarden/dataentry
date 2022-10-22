import { Listing } from '../../../types/listing/listing';
import { postData } from '../../../api/glAxios';
import { publish } from '../../../api/glQueries';
import { push } from 'connected-react-router';
import { clearMessage } from '../system/set-main-message';
import { setAlertMessage } from '../../../redux/actions/system/set-alert-message';
import { AlertMessagingType } from '../../../types/state';
import { currentListingLoaded} from '../../../redux/actions/listingEntry/load-current-listing';
import { updateSavePending } from './update-pending-operation';
import { resetPagedListings } from '../pagedListings/load-listings-paged';

const publishData = (id: any) => {
    const query = publish(id);
    const data = postData(query);
    return data;
}

export const publishListing = (listing:Listing) => (dispatch: Function) => {

    publishData(listing.id).then((result:any) => {
        if (result.data && result.data.publishListing){
            clearMessage(dispatch);
            dispatch(setAlertMessage({show: true, message: "Listing Successfully Submitted for Publishing", type: AlertMessagingType.SUCCESS, allowClose: true}));
            // reset paging and reload 
            resetPagedListings(dispatch, true);
            dispatch(push('/'));
            dispatch(updateSavePending(false));
        } 
        else {
            // error state
            clearMessage(dispatch);        
            dispatch(setAlertMessage({show: true, message: "There was an Error while publishing this listing.", type: AlertMessagingType.ERROR, allowClose: true}));
            dispatch(currentListingLoaded(listing));  
            dispatch(updateSavePending(false)); 
        }
    });
}