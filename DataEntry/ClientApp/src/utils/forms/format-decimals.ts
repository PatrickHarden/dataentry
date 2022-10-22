export const formatDecimals = (value:any, restriction:number) => {
    if(value && value.toString().indexOf(".") > -1){
        value = Number(value).toFixed(restriction);
    }
    return value;
}