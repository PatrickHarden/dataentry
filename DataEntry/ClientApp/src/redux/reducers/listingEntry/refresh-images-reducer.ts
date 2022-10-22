import { REFRESH_IMAGES } from '../../actions/listingEntry/processing-images';

export default(state = {}, action: any) => {
    switch(action.type){
        case REFRESH_IMAGES:
            return [...action.payload];
        break;

        default:
            return state;
    }
}