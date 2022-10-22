import pagedListings from './pagedListings';
import entry from './listingEntry';
import mapping from './mapping';
import system from './system';
import contacts from './contacts';
import teams from './teams';
import miq from './miq';
import insights from './insights';
import featureFlagsReducer from './featureFlags/feature-flags-reducer';

export default {
    pagedListings,
    entry,
    mapping,
    system,
    contacts,
    teams,
    miq,
    insights,
    featureFlags: featureFlagsReducer
}