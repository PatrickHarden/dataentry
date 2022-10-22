// this will be a place we can set our labels (eventually hook i18n in - instead of label, pass in an i18n param and resolve)
// for now, we are just checking to see if its required to add an asterisk
export const getLabel = (fieldName:string, label:string, validations:object) => {
    return label + checkForAsterisk(fieldName,validations);
}

export const checkForAsterisk = (fieldName:string, validations:object):string => {
    // check to see if field is required and pass an asterisk (*) back if so
    if(validations[fieldName]){
        if(validations[fieldName].required){
            return "*";
        }
    }
    return "";
}