import { Contact } from '../../../types/listing/contact';

export const generateInitials = (contact:Contact) => {
    let str: string = "";
    if (contact.firstName) {
        str += contact.firstName.substr(0, 1);
    }
    if (contact.lastName) {
        if(str.length > 0){
            str += ' ';
        }
        str += contact.lastName.substr(0, 1);
    }
    return str;
}