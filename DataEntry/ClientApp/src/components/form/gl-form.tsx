import React, { FC, useContext, useEffect } from 'react'
import { Formik } from 'formik';
import { FormContext } from './gl-form-context';
import styled from 'styled-components';

interface Props {
    initVals: object,
    validationAdapter: Function,
    validationJSON: object,
    changeHandler?: Function,
    children?: any,
    forceValidate?: boolean,
    showErrors?: boolean
}

const GLForm: FC<Props> = (props) => {

    const { initVals, validationJSON, children, changeHandler, showErrors } = props;
    const validationSchema = props.validationAdapter(validationJSON);
    
    let validateForm:Function;
    let errorsRef:object;

    let { forceValidate } = props;

    const formControllerContext = useContext(FormContext); 

    const sleep = (ms:number) => new Promise(resolve => setTimeout(resolve,ms));

    const changeValues = (values: object) => {
        // if a change handler is passed from the embedding view, use that and let it handle what to do with the values
        if (changeHandler !== undefined) {
            changeHandler(values);
        } else {
            formControllerContext.onFormChange(values);
        }
    }

    const randomizeKey = ():string => {
        // this is a hacky way to get the form to update since enableReinitialize in the current version appears broken
        return "r" + new Date().getTime();
    }

    const setValidateForm = (formikProps:any) => {
        validateForm = formikProps.validateForm;
    }

    const touchAll = (formikProps:any) => {
        // this gets called before a force validate in order to ensure all fields are accounted for
        if(validationJSON){
            Object.keys(validationJSON).forEach(fieldName => {
                if(validationJSON[fieldName].required){
                    formikProps.touched[fieldName] = true;
                }
            });
        }
        
    }

    const setErrors = (errors:object) => {
        errorsRef = errors;
    }

    const finishValidation = () => {
        const messages:any = [];
        Object.keys(errorsRef).forEach(fieldName => {
            messages.push(errorsRef[fieldName]);
        });
        formControllerContext.validateComplete(messages);
    }

    useEffect(() => {
        if((forceValidate || showErrors) && validateForm){

            if(forceValidate){
                forceValidate = false;
                validateForm().then(sleep(100).then(() => finishValidation()));
            }else if(showErrors){
                validateForm();
            }
        }
    });

    return (
        <Formik 
            key={randomizeKey()}
            initialValues={initVals}
            validationSchema={validationSchema}
            enableReinitialize={true}
            onSubmit={(values) => {
                changeValues(values);
            }}
            validate={(values) => {
                changeValues(values);   // note: this is the current method to bubble values up
            }}
            render={(formikProps) => {
                if(!validateForm){
                    setValidateForm(formikProps);
                }
                setErrors(formikProps.errors);
                if(forceValidate || showErrors){
                    touchAll(formikProps);
                }
                return (
                    <BaseForm>
                        {children}
                    </BaseForm>  
                );
        }} />
    );
};

const BaseForm = styled.div`
  background-color: #fdfdfd;
  text-align: left;
`;

export default React.memo(GLForm);