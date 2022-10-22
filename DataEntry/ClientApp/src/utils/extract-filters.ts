import { FilterSetup, Filter, FilterType } from '../types/listing/filters';

// utility function to extract the raw filters from the UI filter setup selections (and piggyback on the search)
export const extractFilters = (setups:FilterSetup[], search:string):Filter[] => {
    const filters:Filter[] = [];

    // we treat the search value like a filter and create a filter object to send to the backend
    if(search && search.trim().length > 0){
        filters.push({
            type: FilterType.KEYWORD.valueOf(),
            value: search
        })
    }

    // go through the other filter setups, include the filter data if selected and there is a filter attached to the setup data (i.e., for "View All" there won't be)
    if(setups){
        setups.forEach((setup:FilterSetup) => {
            if(setup.selected && setup.filter){
                filters.push(setup.filter);
            }                
        });
    }

    return filters;
}