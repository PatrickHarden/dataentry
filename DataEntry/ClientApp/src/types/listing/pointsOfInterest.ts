export interface PointOfInterest{
    interestKind: string,
    places: PointOfInterestPlace[]
}

export interface PointOfInterestPlace {
    name: string,
    type: string,
    distances: number | null,
    distanceUnits: string,
    duration: number | null,
    travelMode: string,
    order: number
}