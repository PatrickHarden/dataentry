import React, {FC } from 'react';
import { connect } from 'formik';

interface GLFieldAdapterProps {
    field: any,
    use: any,
    formik: any
}
  
const GLFieldAdapter: FC<GLFieldAdapterProps> = ({
    field,
    ...props
}) => {

    if(!props.use || !field){
        return <></>;
    }

    const { setFieldValue, setFieldTouched, touched, errors } = props.formik;

    // handlers : these are methods that handle values the component needs to set and validate using formik.
    const blurHandler = (value: any) => {
        setFieldValue(field.name,value,false);
        setFieldTouched(field.name,true);
    }

    const changeHandler = (value:any) => {
        setFieldValue(field.name,value,false);
        setFieldTouched(field.name,true);
    }

    // this is where the adapter does its work : taking the formik values and passing down to our components
    let error = touched[field.name] && errors[field.name];
    let errorMessage = "";
    const defaultValue = field.value;  // default value of the field
    const name = field.name;

    if(error){
        error = true;
    }

    // manual error check - if it exists, we assume there is an error
    const manualErrorName:string = "manualErrors";
    if(props[manualErrorName] && props[manualErrorName][name]){
        error = true;
        errorMessage = props[manualErrorName][name].message;
    }

    // create the component we are trying to render, passing down any props and mapped props
    return React.createElement(props.use, {...props, error, errorMessage, name, defaultValue, blurHandler, changeHandler}, props.children);
};

export default connect(GLFieldAdapter);