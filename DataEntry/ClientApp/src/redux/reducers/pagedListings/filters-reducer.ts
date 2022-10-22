import {CHANGE_FILTERS, SETUP_FILTERS} from '../../actions/pagedListings/change-filters';

export default(state = {}, action: any) => {
    switch(action.type){
        case CHANGE_FILTERS:
            return [...action.payload];
        break;

        case SETUP_FILTERS:
            return [...action.payload];
        break;

        default:
            return state;
    }
}