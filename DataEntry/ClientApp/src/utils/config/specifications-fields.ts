import { SpecsListingTypeFields, SpecsPropertyTypeFields } from '../../types/config/specs/specs';
import { Config } from '../../types/config/config';

export const findSpecificationFields = (config:Config, propertyType:string, listingType:string):SpecsListingTypeFields | undefined => {

    if(!config || !propertyType || propertyType.length === 0 || !listingType || propertyType.length === 0){
        return;
    }

    let fieldSetups:SpecsListingTypeFields | undefined;

    // find the correct field configuration
    if(config && config.specifications){
        // find the correct place where we will pull the fields from using the property type

        Object.keys(config.specifications).forEach(key => {

            if(config.specifications[key]){

                const propertyTypeCandidate:SpecsPropertyTypeFields = config.specifications[key];

                if(propertyTypeCandidate.values && propertyTypeCandidate.values.indexOf(propertyType) > -1){
                    // we know we have the right propertyType bucket, now find the listing type bucket
                    if(propertyTypeCandidate[listingType]){
                        fieldSetups = propertyTypeCandidate[listingType];
                    }
                }
            }
        });
    }

    return fieldSetups;
}