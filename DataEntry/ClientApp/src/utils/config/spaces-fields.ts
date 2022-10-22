import { Config } from '../../types/config/config';
import { SpacesListingTypeFields, SpacesPropertyTypeFields } from '../../types/config/spaces/spaces';

export const findSpacesFields = (config:Config, propertyType:string, listingType:string):SpacesListingTypeFields | undefined => {
    
    if(!config || !propertyType || propertyType.length === 0 || !listingType || propertyType.length === 0){
        return;
    }

    let fieldSetups:SpacesListingTypeFields | undefined;
    
    if(config && config.spaces && config.spaces.fields){
        config.spaces.fields.forEach((spacesPropertyTypeField:SpacesPropertyTypeFields) => {
            if(spacesPropertyTypeField.propertyTypes.indexOf(propertyType) > -1){
                if(spacesPropertyTypeField[listingType]){
                    fieldSetups = spacesPropertyTypeField[listingType];
                }
            }
        });
    }
    return fieldSetups;
}

