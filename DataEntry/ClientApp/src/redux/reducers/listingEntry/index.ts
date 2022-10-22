import {combineReducers} from 'redux';
import currentListingReducer from './current-listing-reducer';
import navbarReducer from './navbar-reducer';
import pendingOperationsReducer from './entry-pending-operations-reducer';
import processingImagesCheckReducer from './processing-images-reducer';
import refreshImagesReducer from './refresh-images-reducer';
import updateSpacesReducer from './update-spaces-reducer';

export default combineReducers({
    currentListing: currentListingReducer,
    navbar: navbarReducer,
    pendingOperations: pendingOperationsReducer,
    processingImagesCheck: processingImagesCheckReducer,
    refreshImages: refreshImagesReducer,
    spaces: updateSpacesReducer
});