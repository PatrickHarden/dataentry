export interface Column {
    dataField?: string,
    renderFunction?: (data: any, dataField?:string, defaultValue?: string) => void,
    header?: string,
    size?: number,
    staticDisplay?: string,
    defaultValue?: string
}