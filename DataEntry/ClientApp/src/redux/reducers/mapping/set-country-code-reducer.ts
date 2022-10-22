import { setCountryCode, SET_COUNTRY_CODE} from '../../actions/mapping/set-country-code';
import { ActionType } from 'typesafe-actions';

export type SetCountryCodeAction = ActionType<typeof setCountryCode>;

export default(state = {}, action: SetCountryCodeAction) => {
    switch(action.type){
        case SET_COUNTRY_CODE:
            return action.payload;
        break;

        default:
            return state;
    }
}