
import { Option, ChargeTypeOption, SizeTypeOption, PropertyTypeOption, StateOrProvinceOption } from '../common/option';
import { SpecsConfig } from './specs/specs';
import { SpacesConfig } from './spaces/spaces';
import { GridConfig } from './common/grid';
import { FormDateFieldProps } from '../../components/form-date-field/form-date-field'

export interface ConfigDetails {
    loaded: boolean,
    forced: boolean,
    error: string,
    config: Config,
    homeSiteId?: string,
    intercomm?: boolean,
    intercommAppId?: string,
    aiKey: string,
    regions: Region[],
    propertyTypeColors: Map<string,string>,  /* a lookup for our property type colors */
    propertyTypeLabels: Map<string,string> /* a lookup for our property type labels */
}

export interface Region {
    countryCode: string
    cultureCode: string
    homeSiteID: string
    iD: string
    listingPrefix: string
    name: string
    previewPrefix: string
    previewSiteID: string
}

export interface Config {
    intercomm?:boolean,
    intercommAppId?:string,
    supportLink?: boolean,   /* Link for submitting a support request to CBRE */
    siteId: string,
    miqCountryCode: string,
    featureFlags: FeatureFlagsConfig,
    defaultMeasurement: string,
    addToTeam: Teams,
    map: MapConfig,
    coordinates: Coordinates,
    propertyType: PropertyTypeFieldConfig<PropertyTypeOption>,
    sortType: PropertyTypeFieldConfig<PropertyTypeOption>,
    listingTypes: FieldConfig<Option>,
    propertySubType: FieldConfig<Option>,
    propertyUseClass: FieldConfig<Option>,
    city: FieldSizeConfig,
    country: FieldConfig<Option>,
    stateOrProvince: StateFieldConfig,
    postalCode: FieldSizeConfig,
    highlight?: HighlightConfig,
    syndication?: SyndicationConfig,
    microMarkets: FieldConfig<Option>,
    specifications: SpecsConfig,
    spaces: SpacesConfig,
    contacts: ContactsConfig,
    validations: ValidationConfig,
    teamsEnabled: boolean,
    exportEnabled: boolean,
    duplicateEnabled: boolean,
    googleTrackingId?: string,
    aspects: AspectsConfig,
    chargesAndModifiers: ChargesConfig,
    parkings: ParkingsConfig,
    pointsOfInterests?: POIConfig,
    transportationTypes?: TTConfig,
    sizesandmeasurements: SizeTypeConfig,
    currencyCode: string,
    currencySymbol: string,
    watermark: boolean,
    displayMIQId?: boolean,
    insightsEnabled?: boolean,
    publishedPropertyPrefix?:string,
    amenities: any,
    dataSource: any,
    floorsField?: FloorsField,
    yearField?: YearField,
    energyRating?: EnergyRatingConfig,
    epcRating?: EpcRatingConfig,
    leedRating?: LeedRatingConfig,
    wellRating?: WellRatingConfig,
    status?: StatusConfig,
    availableFrom?: AvailableFromConfig,
    languages?: string[],
    defaultCultureCode?: string,
    translations? : Translation[],
    headlineSingle? : HeadlineSingleConfig,
    epcGraphs? : EpcGraphsConfig,
    currencies: CurrencyConfig
}

export interface Translation {
    cultureCode: string,
    languageName: string,
    plHeadline: string,
    plBuildingDescription: string,
    plLocationDescription: string

}
export interface Property {
    propertyType: string,
    listingType: string,
    teamName: string
}

export interface Teams {
    properties: Property[];
}
export interface FeatureFlagsConfig {
    leaseRateTypeEnabled: boolean,
    defaultWaterMarkTrue?: boolean,
    overrides?: Overrides,
    unpublishFromMIQExport?: boolean,
    autoCalculateSizeAndPrice?: boolean,
    hideStateOrProvince?: boolean
}

export interface Overrides{
    uatImportFromMIQRequireAdmin?: boolean
}

export interface FloorsField {
    show: boolean
}

export interface YearField {
    show: boolean
}
export interface Coordinates {
    lat: number,
    lng: number
}

export interface MapConfig {
    component: string,
    places: PlacesConfig
    addressComponents?: AddressComponentsConfig
}

export interface SyndicationConfig {
    show: boolean,
    label?: string
    markets?: SyndicationMarketConfig[]
}

export interface SyndicationMarketConfig extends FieldConfig<Option> {
    propertyType: string
    grid: GridConfig
}

export interface HighlightConfig {
    label: string,
    addButtonLabel: string
}

export interface PlacesConfig {
    api: string,
    limitSearch: boolean,
    limitCountryCodes: string[]
}

export interface AddressComponentsConfig {
    cityFieldName?: string
}

export interface ContactsConfig {
    phoneMask: string,
    homeOffice: FieldConfig<Option>
}

export enum ConfigFieldType {
    FORM_INPUT = "FORM_INPUT",
    FORM_TEXT_AREA = "FORM_TEXT_AREA",
    FORM_TABBED_TEXT_AREA = "FORM_TABBED_TEXT_AREA",
    FORM_SELECT = "FORM_SELECT",
    FORM_DATE_FIELD = "FORM_DATE_FIELD",
    FORM_CHECKBOX = "FORM_CHECKBOX",
    CONDITIONAL_INCLUDE = "CONDITIONAL_INCLUDE",
    FORM_SPACE_DESC = "FORM_SPACE_DESC",
    FORM_SPACE_NAME = "FORM_SPACE_NAME",
}

export interface SizeTypeConfig {
    show: boolean,
    header: string,
    use?: string,
    label?: string,
    required?: boolean,
    sizeType?: SizeTypeOption[],
    unitofmeasure?: Option[],
    size?: number
}

export interface ChargesConfig {
    show: boolean,
    label?: string,
    required: boolean,
    chargesType: ChargeTypeOption[],
    chargeModifier: Option[],
    term: Option[],
    perUnit: Option[],
    enablePerUnit: boolean,
    enableYear: boolean
}

export interface ParkingsConfig {
    show: boolean,
    label?: string,
    required: boolean,
    ratioPerUnit: Option[],
    parkingType: Option[],
    interval: Option[]
}

export interface POIConfig {
    show: boolean,
    label?: string,
    required: boolean,
    interestKinds: string[],
    travelMode: Option[],
    distanceUnits: Option[]
}

export interface TTConfig {
    show: boolean, 
    label?: string,
    required: boolean, 
    types: string[],
    travelMode: Option[],
    distanceUnits: Option[]
}

export interface FieldConfig<T extends Option> {
    show: boolean,
    label: string,
    required: boolean,
    size?: string,
    options: T[],
    fieldType?: ConfigFieldType
}

export interface PropertyTypeFieldConfig<T extends Option> extends DropdownFieldConfig<T> {
    clearSpacesOn: string[] // the values [from the options] where we need to clear UI fields
}

export interface DropdownFieldConfig<T extends Option> extends FieldConfig<T> {
    alphabeticalSort?: boolean
}

export interface StateFieldConfig {
    show: boolean,
    label: string,
    required: boolean,
    defaultCountry?: string,
    size?: string,
    countryStates: StateOrProvinceOption[]
}

export interface FieldSizeConfig {
    show: boolean,
    size: string
}

export interface ValidationConfig {
    unpublished: object,
    published: object
}

export interface AspectsConfig {
    show: boolean,
    label: string,
    required: boolean,
    options: AspectConfig[]
}

export interface HeadlineSingleConfig {
    show: boolean,
    label?: string,
    required: boolean
}

export interface AspectConfig {
    id: string,
    display: string,
    default: boolean
}

export interface StatusConfig extends FieldConfig<Option> {
    prompt: string,
    grid: GridConfig,
    defaultValue: string
}

export interface AvailableFromConfig extends FormDateFieldProps {
    show: boolean,
    grid: GridConfig
}

export interface EnergyRatingConfig extends FieldConfig<Option> {
    prompt: string,
    grid: GridConfig
}

export interface LeedRatingConfig extends FieldConfig<Option> {
    prompt: string,
    grid: GridConfig
}

export interface WellRatingConfig extends FieldConfig<Option> {
    prompt: string,
    grid: GridConfig
}

export interface EpcRatingConfig extends FieldConfig<Option> {
    prompt: string,
    grid: GridConfig
}

export interface EpcGraphsConfig {
    show: boolean,
    title: string,
    label: string,
    accepted: string,
    description: string,
    showPrimary: boolean,
    allowWatermarkingDetect: boolean
}

export interface CurrencyConfig {
    options: Option[],
    defaultValue: string
}