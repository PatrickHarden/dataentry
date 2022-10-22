import { UPDATE_SPACES } from '../../actions/listingEntry/update-spaces';

export default(state = [], action: any) => {
    switch(action.type){
        case UPDATE_SPACES:
            return action.payload;
        break;

        default:
            return state;
    }
}