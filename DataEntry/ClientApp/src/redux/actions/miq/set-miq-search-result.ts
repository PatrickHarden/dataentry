import { ActionPayload } from '../../../types/common/action';
import { AutoCompleteResult } from '../../../types/common/auto-complete';
import { importEDPProperties } from '../../../api/glQueries';
import { postData } from '../../../api/glAxios';
import { setMiqCurrentRecordsResult } from './set-miq-current-records-result';
import { State } from '../../../types/state';
import { Listing,AlternatePostalAddresses } from '../../../types/listing/listing';
import { Config } from '../../../types/config/config';
import { Space } from '../../../types/listing/space';
import { setMIQStatus } from './set-miq-status';
import { showMIQExportError } from './set-miq-export-message';
import { useEffect } from 'react';

// constants
export const SET_MIQ_SEARCH_RESULT = 'SET_MIQ_SEARCH_RESULT';

// types
export type SetMIQSearchResultAction = ActionPayload<AutoCompleteResult | undefined> & {
    type: typeof SET_MIQ_SEARCH_RESULT
};

export const setMIQSearchResult = (payload:AutoCompleteResult | undefined) : SetMIQSearchResultAction => ({
    type: SET_MIQ_SEARCH_RESULT,
    payload
});

export const selectMIQSearchResult = (selection:AutoCompleteResult) => (dispatch: Function, getState: () => State) => {
    // record the selection in memory
    dispatch(setMIQSearchResult(selection));
    dispatch(setMIQStatus({loading: true, error: false, message: ""}));
    const region = getState().mapping.selectedCountry;

    postData(importEDPProperties(selection.value.id, region)).then((result:any) => {

        dispatch(setMIQStatus({loading: false, error: false, message: ""}));

        if(result && result.data){
            const listings:Listing[] = result.data.getEdpImportProperty;
                if (listings && listings[0].alternatePostalAddresses && listings[0].alternatePostalAddresses.length > 0) {
                    const propertyAddress: AlternatePostalAddresses = {
                        street: listings[0].street,
                        street2: listings[0].street2,
                        city: listings[0].city,
                        stateOrProvince: listings[0].stateOrProvince,
                        postalCode: listings[0].postalCode,
                        country: listings[0].country,
                        lat: listings[0].lat,
                        lng: listings[0].lng,
                    }
                    listings[0].alternatePostalAddresses.push(propertyAddress);
                }

            dispatch(setMiqCurrentRecordsResult(listings));
        }else{
            // todo: throw error
            dispatch(showMIQExportError("Error","There was a problem loading the requested listing."));
        }
    });
}