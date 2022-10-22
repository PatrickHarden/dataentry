// these are limitations based on data type on the server (.NET limitations based on field mappings)
export enum ServerDataType {
    INTEGER = "INTEGER",
    LONG = "LONG",
    DECIMAL_CURRENCY = "DECIMAL_CURRENCY"     /* one trillion dollars, arbitrary for now */
}

// created a separate function for this since the values needed to match from the config as a string
export const getRestriction = (dataType:ServerDataType) => {
    if(dataType){
        if(dataType === ServerDataType.INTEGER){
            return 2147483647;
        }else if(dataType === ServerDataType.LONG){
            return 9223372036854775807;
        }else if(dataType === ServerDataType.DECIMAL_CURRENCY){
            return 1000000000000.99     /* one trillion dollars, arbitrary for now */
        }
    }
    return 9223372036854775807; // default to long if no data type set
}