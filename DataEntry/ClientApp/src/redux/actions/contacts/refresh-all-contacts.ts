import { ActionPayload } from '../../../types/common/action';
import { State } from '../../../types/state';
import { Contact } from '../../../types/listing/contact';
import { postData } from '../../../api/glAxios';
import { getAllContacts } from '../../../api/glQueries';

// constants
export const ALL_CONTACTS_LOADED = 'ALL_CONTACTS_LOADED';

// types
export type AllContactsLoadedAction = ActionPayload<Contact[]> & {
    type: typeof ALL_CONTACTS_LOADED
};

export const allContactsLoaded = (payload: Contact[]): AllContactsLoadedAction => ({
    type: ALL_CONTACTS_LOADED,
    payload
});

export const refreshAllContacts = () => (dispatch: Function, getState: () => State) => {
    postData(getAllContacts())
        .then((response: any) => {
            if(response.data && response.data.contacts){
                dispatch(allContactsLoaded(response.data.contacts));
            }
    });
}