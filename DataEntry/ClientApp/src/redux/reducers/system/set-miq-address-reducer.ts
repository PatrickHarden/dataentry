import { setMiqAddress, SET_MIQ_ADDRESS} from '../../actions/system/set-miq-address';
import { ActionType } from 'typesafe-actions';

export type SetMiqAddressAction = ActionType<typeof setMiqAddress>;

export default(state = {}, action:SetMiqAddressAction) => {
    switch(action.type){
        case SET_MIQ_ADDRESS:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}