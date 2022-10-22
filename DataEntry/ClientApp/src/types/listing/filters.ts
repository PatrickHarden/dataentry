export interface Filter {
    type: string,
    value: string
}

export enum FilterType {
    KEYWORD = "Keyword", /* search */
    PROPERTY_TYPE = "PropertyType",   /* values vary per site, front end will pass in based on config to match db value */
    PUBLISHING_STATUS = "PublishingStatus",     /* draft / published /pending /deleted (UI will allow only one) */
    BULK_UPLOAD = "BulkUploadOnly",
    DELETED = "Deleted",
    MIQ = "MiqOnly"
}

export interface FilterSetup {
    uid?: string,
    label?: string,
    selected?: boolean,
    value?: string,
    allowMultiple?: boolean,
    category?: string,
    clearAll?: boolean,
    filter?: Filter
    divider?: boolean
}