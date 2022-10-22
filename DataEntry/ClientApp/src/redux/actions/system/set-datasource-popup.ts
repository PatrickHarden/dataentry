import { DataSourcePopupParams } from '../../../types/state';
import { DataSource } from '../../../types/listing/datasource';

// constants
export const SET_DATASOURCE_POPUP = 'SET_DATASOURCE_POPUP';


export const setDatasourcePopup = (payload:DataSourcePopupParams) => ({
    type: SET_DATASOURCE_POPUP,
    payload
});

export const clearDatasourcePopup = (dispatch:Function) => {
    const dialogSetting:DataSourcePopupParams = {
        show: false,
        action: 'create',
        datasource: {datasources:[],other:''},
        confirmFunc: undefined
    }
    dispatch(setDatasourcePopup(dialogSetting));
}