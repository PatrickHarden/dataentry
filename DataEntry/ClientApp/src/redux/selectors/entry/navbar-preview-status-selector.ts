import { State, PreviewStatusMessage, FeatureFlags } from '../../../types/state';
import { createSelector } from 'reselect';
import { currentListingSelector } from './current-listing-selector';
import { Listing } from '../../../types/listing/listing';
import { featureFlagSelector } from '../featureFlags/feature-flag-selector';

export const previewStatusSelector = (state:State) => {
    return state.entry.navbar.previewStatus
}

export const listingDirtySelector = (state:State) => {
    return state.entry.navbar.listingDirty;
}

export const navbarPreviewStatusSelector = createSelector(
    currentListingSelector,
    previewStatusSelector,
    listingDirtySelector,
    featureFlagSelector,
    (currentListing:Listing, previewStatus: PreviewStatusMessage, listingDirty:boolean, featureFlags:FeatureFlags):PreviewStatusMessage => {
        
        if(!previewStatus){
            return PreviewStatusMessage.BLANK; 
        }

        // if the listing is in a state of publishing or unpublishing, we won't show a preview status message
        if (currentListing.state && (currentListing.state.toLowerCase() === "publishing" 
            || currentListing.state.toLowerCase() === "unpublishing")) {
            return PreviewStatusMessage.BLANK;      
        }

        if (featureFlags && featureFlags.previewFeatureFlag) {

            // the user has to save before we check for the preview status again.
            if (listingDirty){  
                return PreviewStatusMessage.SAVE_TO_GENERATE;
            }

            // otherwise, the status message we've set in redux is what we will show
            return previewStatus;
        }

        return PreviewStatusMessage.BLANK;
    }
);

