// this file is responsible for setting up the initial state for the application
// this can also be leveraged in tests to mock the store up
import { State, PreviewStatusMessage } from '../types/state'
import { ReplaceProperty, findAndReplace } from '../utils/tests/replace-json';
import { Config } from '../types/config/config';
import { getMockListing } from '../utils/tests/mock-listing';

// for tests, if you need to pass in any replacement objects, you can do so here
export const getInitialStore = (mockConfig:Config, replacements?:ReplaceProperty[]):any => {

    let initialStore = {
        pagedListings: {
          listings: [],
          assignedListings: [],
          skip: 0,
          take: 8,
          moreRecords: true,
          filters: [],
          searchText: ''
        },
        entry: {
          navbar: {
            listingDirty: false,
            previewStatus: PreviewStatusMessage.BLANK,
            previewStatusTimerId: 0
          },
          pendingOperations: {
            savePending: false
          },
          processingImagesCheck: {
            pendingCheck: false,
            listingId: null,
            imageIds: null,
            processingTimerId: null
          },
          refreshImages: [],
          currentListing: getMockListing()
        },
        mapping: {
          country: "SGP"
        },
        system: {
          mainMessage: {
            show: false
          },
          alertMessage: {
            show: false
          },
          configDetails: {
            loaded: true,
            error: "",
            forced: false,
            config: mockConfig,
            aiKey: '',
            propertyTypeColors: new Map(),
            propertyTypeLabels: new Map()
          },
          confirmDialog: {
            show: false
          },
          analytics: false,
          suffix: {
              measure: "sqft",
              leaseTerm: "annual"
          },
          entryPageScrollPosition: 0,
          reloadListingId: -1,
          isDuplicatingListing: false,
          user: { name: '', isAdmin: null },
          cancelAction: { goto: "/" },
          saveAction: {},
          dataSourcePopup: {
            show: false,
            action: "",
            datasource: {
              datasources: []
            }
          }
        },
        contacts: {
          allContacts: []
        },
        teams: {
          currentTeam: {
            id: "",
            name: "",
            users: [],
          },
          allTeams: []
        },
        featureFlags: {
          previewFeatureFlag: false,
          watermarkDetectionFeatureFlag: false,
          miqImportFeatureFlag: false
        },
        router: {
            location: {
                pathname: "",
                search: "",
                hash: "",
                key: ""
            },
            action: ""
        },
        miq: {
          selectedSearchOption: {},
          currentRecords: [],
          pendingAddEditRecord: {},
          status: { loading: false, error: false },
          exportMessage: { show: false }
        },
        insights: {
          insightsLoading: {
            show: false
          },
          current: undefined
        }
    }

    // replacements (for testing generally)
    if(replacements && replacements.length > 0){
      initialStore = findAndReplace(initialStore, replacements);
    }

    return initialStore;
}