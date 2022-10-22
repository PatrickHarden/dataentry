import { Listing } from '../../types/listing/listing';

export const updateRenderTrigger = (change:any, field:string) => {
    change.triggers = {};
    change.triggers[field] = new Date().getTime();
    return change;
}

export const checkTrigger = (current:Listing,changed:Listing,field:string) => {
    let triggered = false;
    if(changed.triggers){
        if(!current.triggers){
            triggered = true;
        }else if(current.triggers[field] !== changed.triggers[field]){
            triggered = true;
        }
    }
    return triggered;
}