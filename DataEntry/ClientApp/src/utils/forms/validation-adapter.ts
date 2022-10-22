import * as Yup from 'yup';
import { Validation, FieldType, MessageKey } from '../../types/forms/validation';

/* 
    The validation adapter's job is to take a validation json and turn it into a yup schema.
    This may be project specific to allow for different json formats for validations between projects. 
    Yup reference: https://github.com/jquense/yup
*/

export const convertValidationJSON = (validations:object) => {

    const schemaObj = {};

    Object.keys(validations).forEach(fieldName => {
        const validationObject:Validation = validations[fieldName];
        schemaObj[fieldName] = createValidation(fieldName,validationObject);
    });

    return Yup.object().shape(schemaObj);   
}

const getValidatonMessage = (messages:object, validation:string, fieldName:string, defaultMessage:string) => {
    // extract a custom message for a validation, if it exists (and fallback on the defaultMessage if not)
    if(messages !== undefined && messages[validation] !== undefined && messages[validation].length > 0){
        return messages[validation];
    }
    return fieldName + ": " + defaultMessage;
}

const createValidation = (fieldName:string, validation:Validation) => {

    let yupValidation:any;
    const messages:object = validation.messages;

    switch(validation.type){

        case FieldType.STRING:
            yupValidation = Yup.string();
            // min length
            if(validation.min !== undefined){
                yupValidation = yupValidation.min(validation.min, getValidatonMessage(messages,MessageKey.MIN,fieldName,'Minimum length not met: ' + validation.min));
            }
            // max length
            if(validation.max !== undefined){
                yupValidation = yupValidation.max(validation.max, getValidatonMessage(messages,MessageKey.MAX,fieldName,'Maximum length exceeded: ' + validation.max));
            }
            // regex
            if(validation.regex !== undefined){
                yupValidation = yupValidation.matches(validation.regex, getValidatonMessage(messages,MessageKey.REGEX,fieldName,'Regex failure on this field.'));
            }
        break;

        case FieldType.NUMBER:
            yupValidation = Yup.number();
            if(validation.min !== undefined){
                yupValidation = yupValidation.min(validation.min, getValidatonMessage(messages,MessageKey.MIN,fieldName,'Minimum value required: ' + validation.min));
            }
            if(validation.max !== undefined){
                yupValidation = yupValidation.max(validation.max, getValidatonMessage(messages,MessageKey.MAX,fieldName,'Maximum value exceeded: ' + validation.max));
            }
        break;

        case FieldType.DATE:
            yupValidation = Yup.date();
            if(validation.min !== undefined){
                yupValidation = yupValidation.min(validation.min, getValidatonMessage(messages,MessageKey.MIN,fieldName,'Minimum date exceeded: ' + validation.min));
            }
            if(validation.max !== undefined){
                yupValidation = yupValidation.max(validation.max, getValidatonMessage(messages,MessageKey.MAX,fieldName,'Maximum date exceeded: ' + validation.max));
            }
        break;

        case FieldType.BOOLEAN:
            yupValidation = Yup.boolean();
        break;

        case FieldType.ARRAY:
            yupValidation = Yup.array();
            if(validation.min !== undefined){
                yupValidation = yupValidation.min(validation.min, getValidatonMessage(messages,MessageKey.MIN,fieldName,'Minimum number of items required: ' + validation.min));
            }
            if(validation.max !== undefined){
                yupValidation = yupValidation.max(validation.max, getValidatonMessage(messages,MessageKey.MAX,fieldName,'Maximum number of items exceeded: ' + validation.max));
            }
        break;
    }

    // common validations
    if(yupValidation !== undefined){
        // required
        if(validation.required !== undefined && validation.required === true){
            yupValidation = yupValidation.required(getValidatonMessage(messages,MessageKey.REQUIRED,fieldName,'Required field'));
        }
        // not one of (not in)
        if(validation.notOneOf !== undefined && validation.notOneOf.length > 0){
            yupValidation = yupValidation.notOneOf(getValidatonMessage(messages,MessageKey.NOTONEOF,fieldName,'This value is not allowed'));
        }
    }

    return yupValidation;
}