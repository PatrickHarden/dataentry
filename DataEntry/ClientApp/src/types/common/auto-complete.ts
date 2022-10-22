export interface AutoCompleteRequest {
    value: any,
    dataAdapter?: Function,
    extraData?: string,
    useSearchData?: any,
    countryCodes?: string[]
}

export interface AutoCompleteResult {
    name: string,
    value: any
}