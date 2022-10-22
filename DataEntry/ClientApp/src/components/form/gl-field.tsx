import React from 'react'
import { Field } from 'formik';
import GLFieldAdapter from './gl-field-adapter';

interface GLFieldProps {
    use: any        // todo: figure out the type that works for our components.
}

const GLField : <T extends object>(p: T & GLFieldProps) => React.ReactElement = (props) => {
    return (
        <Field {...props} component={GLFieldAdapter}/>
    );
};

export default GLField;