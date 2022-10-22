import { AutoCompleteResult, AutoCompleteRequest } from '../../types/common/auto-complete';
import { escapeRegexCharacters } from '../apiUtil';
import { Contact } from '../../types/listing/contact';

// remote data provider 
export const searchContacts = (request:AutoCompleteRequest): Promise<AutoCompleteResult[]> | undefined => {

    // const escapedValue = escapeRegexCharacters(request.value.trim());   
    // const urlValue:string = encodeURI(escapedValue);

    // for now, return a static list of contacts.  switch over to a remote data provider when the api is available.
    return undefined;
}

// static data provider (remove this when we have an API available)
export const searchContactsStatic = (request:AutoCompleteRequest): AutoCompleteResult[] => {

    let contacts:Contact[] = [];
  
    if(request.useSearchData){
        contacts = request.useSearchData;
    }

    if(!request.value){
        return [];
    }

    const escapedValue = escapeRegexCharacters(request.value.trim());
    const regex = new RegExp('^' + escapedValue, 'i');

    // todo: a more efficient method here would be to probaly use matches and then find matches...if we
    // for whatever reason don't move this to the back end, we should re-factor

    const firstNameResults = contacts.filter(contact => regex.test(contact.firstName));
    const lastNameResults = contacts.filter(contact => regex.test(contact.lastName));
    const emailResults = contacts.filter(contact => regex.test(contact.email));
    let filteredResults = [...firstNameResults, ...lastNameResults, ...emailResults];
    filteredResults = unique(filteredResults);

    // limit # of selections to 6
    if(filteredResults && filteredResults.length > 6){
        filteredResults = filteredResults.splice(0,6);
    }

    return convertContactResults(filteredResults);
}

const unique = (array:Contact[]) => {
    const a = array.concat();
    for(let i=0; i<a.length; ++i) {
        for(let j=i+1; j<a.length; ++j) {
            if(a[i] === a[j]){
                a.splice(j--, 1);
            }   
        }
    }
    return a;
}

export const convertContactResults = (results:Contact[]) : AutoCompleteResult[] => {
    
    const converted:AutoCompleteResult[] = [];

    results.forEach((result:Contact) => {
        let displayName = (result.firstName? result.firstName : '') + " " + (result.lastName? result.lastName : '');   // todo: this may need to be flushed out more
        if(result.email){
            displayName += " (" + result.email + ")";
        }
        converted.push({ name: displayName, value: result});
    });

    return converted;
}

export const saveContact = (contact: Contact) : Contact => {
    // if we don't have an id, assign one [this would happen in the server]
    if(!contact.contactId){
        contact.contactId = "temp_cid_" + new Date().getTime();
        contact.tempId = true;
    }
    // todo: add save contact logic / axios call.  we may need to return a promise here.
    return contact;
}