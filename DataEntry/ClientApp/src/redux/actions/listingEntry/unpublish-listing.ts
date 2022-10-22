import { Listing } from '../../../types/listing/listing';
import { postData } from '../../../api/glAxios';
import { unpublish } from '../../../api/glQueries';
import { setMainMessage, clearMessage } from '../system/set-main-message';
import { MainMessageType } from '../../../types/state';
import { setAlertMessage } from '../../../redux/actions/system/set-alert-message';
import { AlertMessagingType } from '../../../types/state';
import { push } from 'connected-react-router';
import { setMIQStatus } from '../miq/set-miq-status';
import { showMIQExportError } from '../miq/set-miq-export-message';
import { selectMIQExportResult } from '../miq/set-miq-export-result';


export enum UnpublishType {
    LISTINGENTRY = "LISTINGENTRY",
    MIQ_NEW = "MIQ_NEW",
    MIQ_EXISTING = "MIQ_EXISTING"
}

const unpublishData = (id: any) => {
    const query = unpublish(id)
    const data = postData(query);
    return data;
}

const handleError = (listing: Listing, unpublishType: UnpublishType, dispatch: Function) => {
    if (unpublishType === UnpublishType.LISTINGENTRY) {
        dispatch(setMainMessage({ show: true, type: MainMessageType.ERROR, message: "There was an error while unpublishing." }));
    }
    else if (unpublishType === UnpublishType.MIQ_EXISTING || unpublishType === UnpublishType.MIQ_NEW) {
        dispatch(showMIQExportError("Error", "There was an error while unpublishing."));
        dispatch(setMIQStatus({ loading: false, error: false, message: "" }));
    }
}

export const unpublishListing = (listing: Listing, unpublishType: UnpublishType) => (dispatch: Function) => {
    dispatch(setMainMessage({ show: true, type: MainMessageType.SAVING, message: "Unpublishing Listing..." }));
    unpublishData(listing.id)
        .then((result: any) => {
            if (result.data && result.data.unpublishListing) {
                clearMessage(dispatch);

                let message = "Listing Successfully Submitted for Unpublishing";
                if (unpublishType === UnpublishType.MIQ_NEW) {
                    message = "Listing queued to be unpublished. We saved a copy of this listing in your listings to be edited in the future. If not longer needed you can delete the property.";
                } else if (unpublishType === UnpublishType.MIQ_EXISTING) {
                    message = "Listing queued to be unpublished.";
                }

                dispatch(setAlertMessage({
                    show: true,
                    message: message,
                    type: AlertMessagingType.SUCCESS,
                    allowClose: true
                }));

                if (unpublishType === UnpublishType.LISTINGENTRY) {
                    dispatch(push('/'));
                }
                else if (unpublishType === UnpublishType.MIQ_EXISTING || unpublishType === UnpublishType.MIQ_NEW) {
                    if (listing.miqId) {
                        // reload listings to get updated publishing states
                        dispatch(selectMIQExportResult(listing.miqId));
                    } else {
                        // it should always have an miqId, but if it doesn't then hide the loading prompt to return control to
                        dispatch(setMIQStatus({ loading: false, error: false, message: "" }));
                    }
                }
            }
            else {
                // error state
                handleError(listing, unpublishType, dispatch);
            }
        })
        .catch((reason: any) => {
            handleError(listing, unpublishType, dispatch);
        });
}