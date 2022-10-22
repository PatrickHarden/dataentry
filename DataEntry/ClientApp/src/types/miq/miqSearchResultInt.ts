export interface MIQSearchResult {
    id: number,
    name: string,
    street1: string,
    street2?: string | null,
    city: string,
    postalCode: string,
    propertyType: string[],
    stateProvince: string
}