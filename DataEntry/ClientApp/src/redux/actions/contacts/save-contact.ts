import { Contact } from '../../../types/listing/contact';
import { saveContact } from '../../../api/glQueries';
import { postData } from '../../../api/glAxios';


// thunk
export const saveContactFromModal = (contact:Contact):Promise<Contact> => {

    return new Promise((resolve, reject) => {
        postData(saveContact(contact)).then((result: any) => {
            if(!result.data || !result.data.saveContact){
                reject("There was an error saving this contact.");
            }else{
                resolve(result.data.saveContact);
            }
        }).catch((error:any) => {
            reject("There was an error saving this contact.");
        });
    });    
}
