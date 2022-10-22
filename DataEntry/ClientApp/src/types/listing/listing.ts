import { Highlight } from './highlight';
import { Specifications } from './specifications';
import { Space } from './space';
import { Contact } from './contact';
import { GLFile } from './file';
import { MicroMarket } from './microMarket';
import { TeamMember } from '../team/teamMember';
import { Team } from '../team/team';
import { ChargesType } from '../../views/listingentry/chargesAndModifiers/charges-and-modifiers'
import { SizeType } from '../../views/listingentry/sizesAndMeasurements/sizes-and-measurements';
import { DataSource } from './datasource';
import { MultiLangString } from './multi-lang-string';
import { Parkings } from './parkings';
import { PointOfInterest } from './pointsOfInterest';
import { TransportationType } from './transportationType';
import { ListingAssignment } from './listingAssignment';
import { ExternalRating } from './externalRating';

export interface Listing{
    id?: any,
    addresses?: any,
    externalId: string|null,
    miqId?: string,
    configId?: string,
    isDeleted: boolean,
    propertyId: string,
    pending: boolean,
    published: boolean,
    state:string,
    editable: boolean,
    propertyName: string,
    propertyRecordName: string,
    propertyType: string,
    propertySubType: string,
    propertyUseClass: string,
    listingType: string,
    street?: string,
    street2?: string,
    city?: string,
    stateOrProvince?: string,
    country?: string,
    postalCode?: string,
    lat?: number,
    lng?: number,
    energyRating?: string,
    epcRating?: string,
    wellRating?: string,
    leedRating?: string,
    externalRatings: ExternalRating[],
    buildingDescriptionSingle: string,
    buildingDescription: MultiLangString[],
    locationDescriptionSingle: string,
    locationDescription: MultiLangString[],
    status: string,
    availableFrom?: string | null,
    syndicationFlag?: boolean,
    syndicationMarket?: string,
    video: string,
    walkThrough: string,
    importedData: string,
    website: string,
    operator: string, 
    floors: number | null,
    yearBuilt: number | null,
    propertySizes?: SizeType[]
    chargesAndModifiers?: ChargesType[],
    headlineSingle: string,
    parkings?: Parkings,
    pointsOfInterests: PointOfInterest[],
    transportationTypes: TransportationType[],
    headline: MultiLangString[],
    photos: GLFile[],
    brochures: GLFile[],
    floorplans: GLFile[],
    epcGraphs?: GLFile[] | null,
    highlights: Highlight[],
    specifications: Specifications,
    microMarket: string,    // 9/3/19: Ryan (Adding this in, UI only for now to support new requirements)
    microMarkets: MicroMarket[],
    spacesCount: number,
    spaces: Space[],
    contacts: Contact[],
    users?: TeamMember[],
    userList?: string[], // array of emails only for saving to database
    teams?: Team[];
    teamList?: string[], // array of team names only for saving to database
    owner?: string,
    aspects?: string[],
    externalPublishUrl : string,
    triggers: RenderTrigger,  // front end only, to help us with knowing when to render some items
    previewSearchApiEndPoint?: string,
    externalPreviewUrl?: string,
    datasource: DataSource,
    listingAssignment?: ListingAssignment,
    dateCreated?: Date,
    dateUpdated?: Date,
    datePublished?: Date,
    dateListed?: Date,
    alternatePostalAddresses?: AlternatePostalAddresses[]
}

interface RenderTrigger{
    addressChange: number
}

export interface AlternatePostalAddresses{
    street?: string,
    street2?: string,
    city?: string,
    stateOrProvince?: string,
    postalCode?: string,
    country?: string,
    lat?: number,
    lng?: number
}