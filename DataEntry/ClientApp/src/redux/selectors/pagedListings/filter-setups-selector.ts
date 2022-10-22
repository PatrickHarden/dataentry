import { State, User } from '../../../types/state';
import { userSelector } from '../system/user-selector';
import { createSelector } from 'reselect';
import { FilterSetup, FilterType } from '../../../types/listing/filters';
import { bulkUploadFilter, deletedFilter, importFromMIQFilter, standardFilters } from '../../../api/filters/filter-settings';
import { featureFlagSelector } from '../featureFlags/feature-flag-selector';
import { FeatureFlags } from '../../../types/state';
import { configSelector } from '../system/config-selector';
import { Config } from '../../../types/config/config';
import { PropertyTypeOption } from '../../../types/common/option';

export const filtersSelector = (state:State) => {
    return state.pagedListings.filters;
}

export const filterSetupsSelector = createSelector(
    filtersSelector,
    userSelector,
    featureFlagSelector,
    configSelector,
    (currentFilters: FilterSetup[], user:User, featureFlags:FeatureFlags, config:Config):FilterSetup[] => {

        const filters:FilterSetup[] = [...standardFilters];  // standard filters are always included
        
        // individual filters setup based on data
        // import from miq filter (dependent on the feature flag)
        if(featureFlags && featureFlags.miqImportFeatureFlag){
            filters.push(importFromMIQFilter);
        }

        // delete filter: dependent on the user being an admin
        if(user && user.isAdmin){
            filters.push(deletedFilter);
        }

        // push a divider for visual separation
        filters.push({divider: true});

        // add the property types as filter options from the config
        if(config && config.propertyType && config.propertyType.options){
            let propertyTypes : PropertyTypeOption[] = [];
            // get sort options for SG Region.
            if(config && config.sortType && config.sortType.show && config.sortType.options){
                propertyTypes =  [...config.sortType.options];
            }
            else {
                propertyTypes =  [...config.propertyType.options];
            }

    
            if (config.propertyType && config.propertyType.alphabeticalSort === true) 
            {
                propertyTypes.sort((a, b) => a.label.localeCompare(b.label));
            }
            
            propertyTypes.forEach((option:PropertyTypeOption) => {
                filters.push({
                    "uid": option.label.toLowerCase().replace(/\s/g, ''),
                    "label": option.label,
                    "selected": false,
                    "allowMultiple": true,
                    "filter": {
                        type: FilterType.PROPERTY_TYPE.toString(),
                        value: option.value
                    },
                    "category": "propertyType"
                });
            });
        }

        // finally, add the bulk upload filter
        filters.push(bulkUploadFilter);

        // now that we have our setups, we need to ensure our new setups have any previous selected filters selected, so reconcile them
        if(currentFilters && currentFilters.length > 0){
            const selectedFilters:object = {};
            currentFilters.forEach((currentFilter:FilterSetup) => {
                if(currentFilter.selected && currentFilter.uid){
                    selectedFilters[currentFilter.uid] = true;
                }
            });
            
            // loop through the newly created filters and mark any selected needed
            filters.forEach((filterSetup:FilterSetup) => {
                if(filterSetup.uid && selectedFilters[filterSetup.uid]){
                    filterSetup.selected = true;
                }
            });
        }

        return filters;
    }
);