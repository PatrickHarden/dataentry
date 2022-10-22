import { State } from '../../../types/state';

export const allContactsSelector = (state:State) => {
    return state.contacts.allContacts;
}