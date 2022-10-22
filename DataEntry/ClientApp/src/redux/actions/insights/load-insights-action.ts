import { postData } from '../../../api/glAxios';
import { pullSingleListing } from '../../../api/glQueries';
import { ActionPayload } from '../../../types/common/action';
import { InsightsRecord } from '../../../types/insights/insights-record';
import { Listing } from '../../../types/listing/listing';
import { State, MainMessaging, MainMessageType } from '../../../types/state';
import { configSelector } from '../../selectors/system/config-selector';
import { massage } from '../listingEntry/load-current-listing';

// constants
export const SET_INSIGHTS_LOADING = 'SET_INSIGHTS_LOADING';
export const SET_CURRENT_INSIGHTS_RECORD = 'SET_CURRENT_INSIGHTS_RECORD';

// types
export type SetInsightsLoadingAction = ActionPayload<MainMessaging> & {
    type: typeof SET_INSIGHTS_LOADING
};

export type SetCurrentInsightsRecord = ActionPayload<InsightsRecord> & {
    type: typeof SET_CURRENT_INSIGHTS_RECORD
};

// functions
export const setInsightsLoading = (payload:MainMessaging) : SetInsightsLoadingAction => ({
    type: SET_INSIGHTS_LOADING,
    payload
});

export const setCurrentInsightsRecord = (payload:InsightsRecord | undefined) => ({
    type: SET_CURRENT_INSIGHTS_RECORD,
    payload
});

export const loadCurrentInsightsRecord = (id:number)  => (dispatch: Function, getState: () => State) => { 
    const config = configSelector(getState());
    
    // set loading indicator for insights
    dispatch(setInsightsLoading({ show: true, type: MainMessageType.LOADING, message: "Loading Analytics..." }));

    // load the insights record from our API
    // note: currently, we are just loading the existing listing and then wrapping with the InsightsRecord object,
    // but when we create the back end API we will switch this call over to the new API call

    postData(pullSingleListing(id)).then(result => {
        if (result.data && result.data.listing){
            const listing:Listing = massage(result.data.listing, config);
            const record:InsightsRecord = {
                'listing': listing
            };
            dispatch(setInsightsLoading({show: false}));
            dispatch(setCurrentInsightsRecord(record));
        }else{
            dispatch(setInsightsLoading({show: false}));
            dispatch(setCurrentInsightsRecord(undefined));
        }
    });
}