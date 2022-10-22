import { SET_IMAGES_PROCESSING } from '../../actions/listingEntry/processing-images';

export default(state = {}, action: any) => {
    switch(action.type){
        case SET_IMAGES_PROCESSING:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}