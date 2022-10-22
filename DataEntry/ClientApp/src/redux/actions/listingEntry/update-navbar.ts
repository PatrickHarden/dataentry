import { ActionPayload } from '../../../types/common/action';
import { NavBarState, State, PreviewStatusMessage } from '../../../types/state';
import { Listing } from '../../../types/listing/listing';
import moment from 'moment';

// constants
export const UPDATE_LISTING_DIRTY = 'UPDATE_LISTING_DIRTY';
export const UPDATE_PREVIEW_STATUS = 'UPDATE_PREVIEW_STATUS';
export const UPDATE_PREVIEW_STATUS_TIMER_ID = 'UPDATE_PREVIEW_STATUS_TIMER_ID';

// types
export type UpdateListingDirtyAction = ActionPayload<boolean> & {
    type: typeof UPDATE_LISTING_DIRTY;
};

export type UpdatePreviewStatusAction = ActionPayload<PreviewStatusMessage> & {
    type: typeof UPDATE_PREVIEW_STATUS
};

export type UpdatePreviewStatusTimerIdAction = ActionPayload<number> & {
    type: typeof UPDATE_PREVIEW_STATUS_TIMER_ID
};

export const updateListingDirty = (payload:boolean): UpdateListingDirtyAction => ({
    type: UPDATE_LISTING_DIRTY,
    payload
});

export const updatePreviewStatus = (payload: PreviewStatusMessage): UpdatePreviewStatusAction => ({
    type: UPDATE_PREVIEW_STATUS,
    payload
});

export const updatePreviewStatusTimerId = (payload: number): UpdatePreviewStatusTimerIdAction => ({
    type: UPDATE_PREVIEW_STATUS_TIMER_ID,
    payload
});

// important note: if calling this externally (outside of the preview loop), grab the timerId from the store.
export const stopPreviewPing = (dispatch:Function, timerId:number) => {
    if(timerId > 0){
        clearInterval(timerId);
        dispatch(updatePreviewStatusTimerId(0));
    }
}

const checkForPreview = (url: string): Promise<any> => {
   return new Promise((resolve, reject) => {
      
        fetch(url, {
        method: "GET",
        headers: {
          "Content-Type": "text/plain"
        }
      }).then(response => {
        const responseJSON = response.json();
        resolve(responseJSON);
      }).catch((error) => {
        reject(error.message);
      });


    });
}

export const setDirtyListing = (dirty: boolean) => (dispatch: Function, getState: () => State) => {
    if(dirty === true){
        // if dirty, we can stop asking for a preview to save pings
        stopPreviewPing(dispatch,getState().entry.navbar.previewStatusTimerId);
    }
    dispatch(updateListingDirty(dirty));
}

export const checkPreviewStatus = (dispatch:Function, store:State, savedListing:Listing) => {

    if(store && store.featureFlags.previewFeatureFlag === false){
        return;
    }

    const navbarState:NavBarState = store.entry.navbar;

    // if we have an existing timer id, clear it out before we create a new one
    stopPreviewPing(dispatch, navbarState.previewStatusTimerId);

    // don't start the timer in the following circumstances:
    // (1) state is publishing or unpublishing

    if (savedListing.state && (savedListing.state.toLowerCase() === "publishing" || savedListing.state.toLowerCase() === "unpublishing")) {
        return;
    }

    if(!savedListing.id){
        // if we don't have a listing id, it's new
        dispatch(updatePreviewStatus(PreviewStatusMessage.SAVE_TO_GENERATE));
        return;
    }

    const checkForPreviewInterval = () => {

        if (savedListing.previewSearchApiEndPoint) {
            checkForPreview(savedListing.previewSearchApiEndPoint).then((response: any) => {
             
                let lastUpdated;
                let storeApiDate;

                if(response.Found){
                    lastUpdated = response.Documents[0][0]["Common.LastUpdated"];
                    storeApiDate = new Date(lastUpdated);
                }
               
                // outdated preview upon publish fix
                // preview save time within last 3 seconds of save time would be current preview
                if (response.Found && storeApiDate && savedListing.dateUpdated && (storeApiDate < (moment(savedListing.dateUpdated).subtract(3, 'seconds').toDate()))) {
                    dispatch(updatePreviewStatus(PreviewStatusMessage.OUTDATED));
                } else if (response.Found) {
                    dispatch(updatePreviewStatus(PreviewStatusMessage.AVAILABLE));
                    stopPreviewPing(dispatch, timerId);
                } else {
                    dispatch(updatePreviewStatus(PreviewStatusMessage.UNAVAILABLE));
                }
            })
            .catch((ex) => {
                console.log(ex);
                dispatch(updatePreviewStatus(PreviewStatusMessage.FAILED));
            });
        }
    }

    dispatch(updatePreviewStatus(PreviewStatusMessage.LOADING));    // default state before we start timer

    checkForPreviewInterval();
    // we need to keep pinging the preview status until we get a response
    const timerId:number = setInterval(() => {
        checkForPreviewInterval();
    }, 30000); // 30 seconds
    dispatch(updatePreviewStatusTimerId(timerId));  // record the timer id in redux 
};