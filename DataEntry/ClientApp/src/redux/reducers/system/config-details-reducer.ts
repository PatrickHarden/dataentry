import { configDetailsLoaded, CONFIG_DETAILS_LOADED } from '../../actions/system/load-config-details';
import { ActionType } from 'typesafe-actions';

export type ConfigLoadedAction = ActionType<typeof configDetailsLoaded>;

export default(state = {}, action: ConfigLoadedAction) => {
    switch(action.type){
        case CONFIG_DETAILS_LOADED:
            return Object.assign({},action.payload);
        break;

        default:
            return state;
    }
}