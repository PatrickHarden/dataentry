import {combineReducers} from 'redux';
import allContactsReducer from './all-contacts-reducer';

export default combineReducers({
    allContacts: allContactsReducer
});