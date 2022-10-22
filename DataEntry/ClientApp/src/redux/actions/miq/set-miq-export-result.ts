import { ActionPayload } from '../../../types/common/action';
import { AutoCompleteResult } from '../../../types/common/auto-complete';
import { importEDPProperties } from '../../../api/glQueries';
import { postData } from '../../../api/glAxios';
import { setMiqCurrentRecordsResult } from './set-miq-current-records-result';
import { State } from '../../../types/state';
import { Listing } from '../../../types/listing/listing';
import { Config } from '../../../types/config/config';
import { Space } from '../../../types/listing/space';
import { setMIQStatus } from './set-miq-status';
import { showMIQExportError } from './set-miq-export-message';

// constants
export const SET_MIQ_EXPORT_RESULT = 'SET_MIQ_EXPORT_RESULT';

// types
export type SetMIQExportResultAction = ActionPayload<string | undefined> & {
    type: typeof SET_MIQ_EXPORT_RESULT
};

export const setMIQExportResult = (payload:string | undefined) : SetMIQExportResultAction => ({
    type: SET_MIQ_EXPORT_RESULT,
    payload
});

export const selectMIQExportResult = (id:string) => (dispatch: Function, getState: () => State) => {
    // record the selection in memory
    dispatch(setMIQExportResult(id));
    dispatch(setMIQStatus({loading: true, error: false, message: ""}));

    const region: any = getState().mapping.selectedCountry

    postData(importEDPProperties(parseInt(id, 10), region)).then((result:any) => {

        dispatch(setMIQStatus({loading: false, error: false, message: ""}));

        if(result && result.data){
            let listings:Listing[] = result.data.getEdpImportProperty;
            if(listings &&listings.length > 0){
                listings = prepareResults(listings, getState().system.configDetails.config);
                dispatch(setMiqCurrentRecordsResult(listings));
            }else{
                dispatch(showMIQExportError("Error","The requested listing returned no data from MIQ"));
            }
        }else{
            dispatch(showMIQExportError("Error","There was a problem loading the requested listing."));
        }
    });
}

const prepareResults = (listings:Listing[], config:Config) => {

    // massage the data back as needed
    // (1) reconcile any regional data that needs to be single
    const convertRegional:boolean = !config.languages || config.languages.length > 0;   

    listings.forEach((listing:Listing) => {

        if(convertRegional){
            // TODO: we may need to convert the following: headline, property description, building description, highlights
            // spaces
            if(listing.spaces && listing.spaces.length > 0){
                listing.spaces.forEach((space:Space) => {
                    if(!space.nameSingle || !(space.nameSingle.trim().length === 0) && space.name){
                        if(space.name[0]){  // assumption is that we only have one text field coming in during a "single" region situation
                            space.nameSingle = space.name[0].text;
                        }
                    }
                });       
            }
        }
    });

    return listings;
}