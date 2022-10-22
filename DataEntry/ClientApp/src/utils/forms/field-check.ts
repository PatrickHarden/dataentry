export const fieldCheckStrVal = (value:string, acceptedValues:string[]) => {
    if(acceptedValues !== undefined){
        if(acceptedValues.indexOf(value) > -1){
            return true;
        }
    }
    return false;
}