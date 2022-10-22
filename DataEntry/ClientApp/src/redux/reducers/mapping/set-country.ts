import { setCountry, SET_COUNTRY} from '../../actions/mapping/set-country';
import { ActionType } from 'typesafe-actions';

export type SetCountryAction = ActionType<typeof setCountry>;

export default(state = null, action: SetCountryAction) => {
    switch(action.type){
        case SET_COUNTRY:
            return action.payload;
        break;

        default:
            return state;
    }
}