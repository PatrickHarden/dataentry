import { selectCountry, SELECT_COUNTRY} from '../../actions/mapping/select-country';
import { ActionType } from 'typesafe-actions';

export type SelectCountryAction = ActionType<typeof selectCountry>;

export default(state = {}, action: SelectCountryAction) => {
    switch(action.type){
        case SELECT_COUNTRY:
            return action.payload;
        break;

        default:
            return state;
    }
}