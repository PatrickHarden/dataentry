import { faCommentDollar } from '@fortawesome/free-solid-svg-icons'

/* a common dropdown option */

export interface Option {
    label: string,
    value: any,
    order: number
}

export interface StateOrProvinceOption {
    countryCode: string,
    options: Option[]
}

export interface ChargeTypeOption extends Option {
    modifiers?: any,
    terms?: any,
    amount?: boolean
}

export interface SizeTypeOption extends Option {
    unit?: any,
    size?: boolean
}

export interface PropertyTypeOption extends Option {
    color: string,
    subPropertyType?: string,
    useClass?: string
}