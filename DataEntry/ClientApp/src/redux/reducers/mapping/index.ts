import {combineReducers} from 'redux';
import countryReducer from './country-reducer';
import setCountry from './set-country';
import setCountryCode from './set-country-code-reducer';

export default combineReducers({
    country: countryReducer,
    selectedCountry: setCountry,
    countryCode: setCountryCode
});