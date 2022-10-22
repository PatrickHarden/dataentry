import { Contact } from '../../../types/listing/contact';

// create blank contact
export const createBlankContact = () => {
    const blankContact:Contact = {
        'contactId': undefined,
        'tempId': false,
        'firstName': '',
        'lastName': '',
        'email': '',
        'phone': '',
        'location': '',
        'additionalFields': {
            'license': ''
        }
    };
    return blankContact;
}