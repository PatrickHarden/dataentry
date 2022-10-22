import { UPDATE_SAVE_PENDING } from '../../actions/listingEntry/update-pending-operation';

export default(state = {}, action: any) => {
    switch(action.type){
        case UPDATE_SAVE_PENDING:
            const dirty = { 'savePending': action.payload }; 
            return {...state,...dirty};
        break;

        default:
            return state;
    }
}