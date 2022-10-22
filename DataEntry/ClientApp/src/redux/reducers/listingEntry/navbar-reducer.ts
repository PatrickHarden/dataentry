import { UPDATE_LISTING_DIRTY, UPDATE_PREVIEW_STATUS, UPDATE_PREVIEW_STATUS_TIMER_ID } from '../../actions/listingEntry/update-navbar';

export default(state = {}, action: any) => {
    switch(action.type){
        case UPDATE_LISTING_DIRTY:
            const dirty = { 'listingDirty': action.payload }; 
            return {...state,...dirty};
        break;
        
        case UPDATE_PREVIEW_STATUS: 
            const previewStatus = { 'previewStatus': action.payload }; 
            return {...state,...previewStatus};
        break;

        case UPDATE_PREVIEW_STATUS_TIMER_ID:
            const timerId = { 'previewStatusTimerId': action.payload };
            return {...state,...timerId};
        break;

        default:
            return state;
    }
}