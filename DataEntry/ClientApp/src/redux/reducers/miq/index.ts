import {combineReducers} from 'redux';
import miqSearchSelection from './miq-search-selection';
import miqCurrentRecords from './miq-current-records';
import miqSetPendingRecord from './miq-pending-add-edit-record';
import miqStatus from './miq-status';
import miqExportMessage from './miq-export-message-reducer';
import miqRedirect from './miq-redirect-reducer';
import miqSpaces from './miq-spaces-reducer';

export default combineReducers({
    selectedSearchOption: miqSearchSelection,
    currentRecords: miqCurrentRecords,
    pendingAddEditRecord: miqSetPendingRecord,
    exportMessage: miqExportMessage,
    status: miqStatus,
    redirect: miqRedirect,
    spaces: miqSpaces
});