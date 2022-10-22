import 'react-app-polyfill/ie11';
import 'core-js/es/symbol/iterator';
import 'airbnb-js-shims';
import ReactDOM from 'react-dom';
import React from 'react';
import './index.scss';
import buildStore from './app/createStore';
import { createBrowserHistory } from 'history';
import { Provider } from 'react-redux';
import App from './app/App';
import GLAnalytics from './components/analytics/gl-analytics';
import { PreviewStatusMessage } from './types/state';

// Create browser history to use in the Redux store
let baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
if (baseUrl === null) {
  baseUrl = '';   // ts fallback
}
const history = createBrowserHistory({ basename: baseUrl });

// initialize our store
// note: the .net boilerplate had the following for initialStore: 
// Get the application-wide store instance, prepopulating with state from the server where availab
// const initialState = window.initialReduxState;
const initialState = {
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
      loaded: false,
      error: false,
      forced: false
    },
    confirmDialog: {
      show: false
    },
    setMiqAddress: {
      show: false
    },
    analytics: false,
    suffix: '',
    entryPageScrollPosition: 0,
    reloadListingId: -1,
    isDuplicatingListing: false,
    user: { name: '', isAdmin: null },
    cancelAction: { goto: "/" },
    saveAction: {}
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
    miqImportFeatureFlag: false,
    miqLimitSearchToCountryCodeFeatureFlag: true,
  },
  miq: {
    selectedSearchOption: null,
    currentRecords: null,
    pendingAddEditRecord: null,
    status: { loading: false, error: false },
    exportMessage: { show: false },
    redirect: "",
    spaces: []
  },
  insights: {
    insightsLoading: {
      loading: false
    },
    current: undefined
  }
}

const store = buildStore(initialState, history);

// the element in the index we are replacing
const rootElement = document.getElementById('root');

ReactDOM.render(
  <Provider store={store}>
    <GLAnalytics>
      <App history={history} />
    </GLAnalytics>
  </Provider>,
  rootElement);