export let initialState = {
    initialState: {
        listings: [],
        skip: 0,
        take: 8,
        moreRecords: true,
        filters: [],
        searchText: ''
    },
    entry: {
        navbar: {
            listingDirty: false,
            previewStatus: 0,
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
        refreshImages: []
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
            error: false,
            forced: false,
            config: {}
        },
        confirmDialog: {
            show: false
        },
        analytics: false,
        suffix: '',
        entryPageScrollPosition: 0,
        reloadListingId: -1,
        user: { name: '', isAdmin: null },
    },
    contacts: {
        allContacts: []
    },
    teams: {
        allTeams: []
    },
    featureFlags: {
        previewFeatureFlag: false,
        watermarkDetectionFeatureFlag: false,
        miqImportFeatureFlag: false
    }
};