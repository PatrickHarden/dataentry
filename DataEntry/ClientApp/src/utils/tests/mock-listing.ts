import { Listing } from '../../types/listing/listing'
import { ReplaceProperty, findAndReplace } from './replace-json'

export const getMockListing = (replacements?:ReplaceProperty[]):Listing => {

    let listing:Listing = {
        isDeleted: false,
        propertyId: "someid",
        externalId: "anotherId",
        pending: false,
        published: false,
        state: "",
        editable: true,
        propertyName: "Test Property",
        propertyRecordName: "Test Property",
        propertyType: "office",
        propertySubType: "",
        propertyUseClass: "",
        listingType: "sale",
        street: "some street",
        street2: "",
        city: "some city",
        stateOrProvince: "tx",
        country: "us",
        postalCode: "12345",
        energyRating: "",
        buildingDescriptionSingle: "",
        buildingDescription: [],
        locationDescriptionSingle: "",
        locationDescription: [],
        status: "",
        video: "",
        walkThrough: "",
        importedData: "",
        website: "",
        operator: "", 
        floors: null,
        yearBuilt: null,
        headlineSingle: "",
        headline: [],
        photos: [],
        brochures: [],
        floorplans: [],
        highlights: [],
        pointsOfInterests: [], 
        transportationTypes: [],
        specifications: {
            leaseType: "",
            measure: "",
            leaseTerm: "",
            minSpace: 0,
            maxSpace: 0,
            totalSpace: 0,
            minPrice: 0,
            maxPrice: 0,
            salePrice: 0,
            contactBrokerForPrice: false,
            showPriceWithUoM: false,
            currencyCode: "$"
        },
        microMarket: "", 
        microMarkets: [],
        spacesCount: 0,
        spaces: [],
        contacts: [],
        externalPublishUrl : "",
        triggers: { "addressChange": 0}, 
        datasource: {}
    }

    if(replacements && replacements.length > 0){
        listing = findAndReplace(listing, replacements);
    }

    return listing;
}   