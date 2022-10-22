import {combineReducers} from 'redux';
import mainMessagingReducer from './main-messaging-reducer';
import alertMessagingReducer from './alert-messaging-reducer';
import configDetailsReducer from './config-details-reducer';
import confirmDialogReducer from './confirm-dialog-reducer';
import setAnalyticsReducer from './set-analytics-boolean';
import setSuffixReducer from './set-suffix-string';
import entryPageScrollPositionReducer from './entry-page-scroll-position-reducer';
import reloadEntryIdReducer from './reload-listing-id-reducer';
import dataSourcePopupReducer from './datasource-popup-reducer';
import userReducer from '../system/set-user-reducer';
import cancelActionReducer from '../system/cancel-action-reducer';
import saveActionReducer from '../system/save-action-reducer';
import setHomeSiteId from '../system/set-home-site-id-reducer';
import isDuplicatingListingReducer from './is-duplicating-listing-reducer';
import setMiqAddressAction from './set-miq-address-reducer';

export default combineReducers({
    mainMessage: mainMessagingReducer,
    alertMessage: alertMessagingReducer,
    configDetails: configDetailsReducer,
    confirmDialog : confirmDialogReducer,
    analytics: setAnalyticsReducer,
    suffix: setSuffixReducer,
    entryPageScrollPosition: entryPageScrollPositionReducer,
    reloadListingId: reloadEntryIdReducer,
    isDuplicatingListing: isDuplicatingListingReducer,
    dataSourcePopup: dataSourcePopupReducer,
    user: userReducer,
    cancelAction: cancelActionReducer,
    saveAction: saveActionReducer,
    homeSiteId: setHomeSiteId,
    setMiqAddress: setMiqAddressAction
});