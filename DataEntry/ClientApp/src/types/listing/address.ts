import { Option } from '../common/option';

export interface Address {
    street?: string,
    street2?: string,
    city?: string,
    stateOrProvince?: string,
    postalCode?: string,
    country?:string,
    lat?: number,
    lng?: number
}