import {createContext} from 'react';

// form context using context API to pass around functions and any other values we need
export const FormContext = createContext({
    onFormChange: (values:object) => {
        // default
    },
    validateComplete : (errors:string[]) => {
        // default
    }
});