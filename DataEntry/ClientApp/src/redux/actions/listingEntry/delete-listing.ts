import { Listing } from '../../../types/listing/listing';
import { postData } from '../../../api/glAxios'
import { deleteListingQuery } from '../../../api/glQueries'
import { push } from 'connected-react-router';
import { setMainMessage, clearMessage } from '../system/set-main-message';
import { MainMessageType } from '../../../types/state';
import { resetPagedListings } from '../pagedListings/load-listings-paged';

const deleteData = (id: any) => {
    const query = deleteListingQuery(id)
    const data = postData(query);
    return data;
}

export const deleteListing = (listing:Listing) => (dispatch: Function) => {
    // grab the id from the url
    const url = window.location.href;
    const urlArray = url.split('/');
    const id = parseInt(urlArray[urlArray.length - 1], 10);
    // show a message to the user to let them know deletion is in progress
    dispatch(setMainMessage({ show: true, type: MainMessageType.SAVING, message: "Deleting Listing..." }));
    deleteData(id).then((result: any) => {
        if (result.data && result.data.deleteListing){
            clearMessage(dispatch); // clear out the deleting message
            // reset paging and reload 
            resetPagedListings(dispatch, true);
            dispatch(push('/'));
        } else {
            dispatch(push('/'));
        }
    })
}