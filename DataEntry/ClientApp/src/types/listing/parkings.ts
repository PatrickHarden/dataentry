export interface Parkings {
    ratio: number | null,
    ratioPer: number | null,
    ratioPerUnit: string,
    parkingDetails: ParkingDetails[]
}
 
export interface ParkingDetails {
    parkingType: string,
    parkingSpace: number | null,
    amount: number | null,
    interval: string,
    currencyCode: string
}
