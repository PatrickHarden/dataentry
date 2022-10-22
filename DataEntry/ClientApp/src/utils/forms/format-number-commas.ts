export const formatNumberWithCommas = (value:any) => {
    if(typeof value !== "string" && typeof value.toString === "function"){
        value = value.toString();
    }
    // we need to separate out the decimal values so they aren't formatted with commas
    const parts = value.split(".");
    if(parts[0]){
        parts[0] = parts[0].replace(/,/g, '');  // remove any commas the user may have entered
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");  // and format the number correctly
    
        if(parts[1]){
            return parts[0] + "." + parts[1];
        }
        return parts[0];
    }
    return value;
}