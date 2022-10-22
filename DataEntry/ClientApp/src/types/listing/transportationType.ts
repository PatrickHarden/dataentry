export interface TransportationType {
    type: string,
    places: TransportationPlace[]
}

export interface TransportationPlace {
    name: string,
    distances: number | null,
    distanceUnits: string,
    duration: number | null,
    travelMode: string,
    order: number
}