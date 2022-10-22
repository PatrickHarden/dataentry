import { setDatasourcePopup, SET_DATASOURCE_POPUP} from '../../actions/system/set-datasource-popup';
import { ActionType } from 'typesafe-actions';

export type SetDatasourcePopupAction = ActionType<typeof setDatasourcePopup>;

export default(state = {}, action: SetDatasourcePopupAction) => {
    switch(action.type){
        case SET_DATASOURCE_POPUP:
            return Object.assign({},action.payload);
        break;
        default:
            return state;
    }
}