import React, {FC, useState} from 'react'
import { useDispatch } from 'react-redux';
import { FormContext } from './gl-form-context';

interface OwnProps {
    changeHandler : (values:object, preventRerender?:boolean) => void,
    validateComplete : (errors:string[]) => void
}

const FormController: FC<OwnProps> = (props) => {

    const { changeHandler, children, validateComplete } = props;
    
    const [formValues, setFormValues] = useState({});

    const changeFormValues = (values:object, preventRerender?:boolean) => {
        // merge in the changes passed from the child view/form
        const fvCopy = Object.assign({}, formValues);
        Object.keys(values).forEach(key => {
            fvCopy[key] = values[key];
        });
        changeHandler(values, preventRerender);
    }

    const finishForcedValidation = (errors:string[]) => {
        validateComplete(errors);
    }

    return (
        <FormContext.Provider value={{onFormChange: changeFormValues, validateComplete: finishForcedValidation}}>
            {children}
        </FormContext.Provider>
    );
};

export default FormController;