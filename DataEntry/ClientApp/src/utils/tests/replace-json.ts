import { set } from "lodash";

export interface ReplaceProperty {
    propertyName: string,
    replaceWith: any
}

// this function takes an object and locates the path indicated in "replace" and then
// (a) finds the property specified
export const findAndReplace = <T extends Object>(data:T, replacements:ReplaceProperty[]):T =>  {

    replacements.forEach((replaceProperty:ReplaceProperty) => {
        const { propertyName, replaceWith } = replaceProperty;
        if(!data){
            return;
        }
        set(data, propertyName, replaceWith);
    });
    return data;
}   