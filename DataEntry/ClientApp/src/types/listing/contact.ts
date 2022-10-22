import { GLFile } from './file';

export interface Contact {
    contactId: any,
    tempId?: boolean,
    firstName: string,
    lastName: string,
    phone: string,
    phone2?: string,
    email: string,
    location: string,
    additionalFields: {
        license: string
    },
    license?:string,
    avatar?: string
}