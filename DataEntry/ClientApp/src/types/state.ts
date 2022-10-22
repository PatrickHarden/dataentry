import { Listing } from './listing/listing';
import { Team } from './team/team';
import { Contact } from './listing/contact';
import { ConfigDetails } from './config/config';
import { DataSource } from './listing/datasource';
import { GLFile } from './listing/file';
import { Filter } from './listing/filters';
import { MIQSearchResult } from './miq/miqSearchResultInt';
import { MIQStatus } from './miq/miqStatus';
import { MIQExportMessage } from './miq/miqExportMessaging';
import { InsightsRecord } from './insights/insights-record';

export interface State {
    router: {
        location: {
            pathname: string,
            search: string,
            hash: string,
            key: string,
        }
        action: string
    }
    pagedListings: PagedListings,
    entry: EntryPage,
    mapping: Mapping,
    system: System,
    contacts: Contacts,
    teams : TeamsPage,
    miq: MIQ,
    insights: Insights,
    featureFlags: FeatureFlags
}

export interface PagedListings {
    listings: Listing[],
    assignedListings: Listing[],
    skip: number,
    take: number,
    moreRecords: boolean,
    searchText: string,
    filters: Filter[]
}

export interface EntryPage {
    currentListing: Listing,
    navbar: NavBarState,
    pendingOperations: EntryPendingOperations,
    processingImagesCheck: ProcessingImagesCheck,
    refreshImages: GLFile[],
    spaces?: any
}

export interface NavBarState {
    listingDirty: boolean,
    previewStatus: PreviewStatusMessage,
    previewStatusTimerId: number
}

export interface ProcessingImagesCheck {
    pendingCheck: boolean,
    listingId: number | null,
    imageIds: number[] | null,
    processingTimerId: number | null
}

export interface EntryPendingOperations {
    savePending: boolean
}

export enum PreviewStatusMessage {
    BLANK,
    LOADING,
    AVAILABLE,
    UNAVAILABLE,
    FAILED,
    OUTDATED,
    SAVE_TO_GENERATE
}

export interface User {
    name: string,
    isAdmin: boolean | null
}

export interface Mapping {
    country: string,
    selectedCountry?: string,
    countryCode?: string
}

export interface TeamsPage {
    allTeams: Team[],
    currentTeam: Team
}

export interface FeatureFlags {
    previewFeatureFlag: boolean,
    watermarkDetectionFeatureFlag: boolean,
    miqImportFeatureFlag: boolean,
    miqLimitSearchToCountryCodeFeatureFlag: boolean,
    autoCalculateSizeAndPrice?: boolean
} 

export interface System {
    configDetails: ConfigDetails,
    mainMessage: MainMessaging,
    alertMessage : AlertMessaging,
    confirmDialog: ConfirmDialogParams,
    analytics: boolean,
    suffix: ListingSuffix,
    entryPageScrollPosition: number,
    dataSourcePopup: DataSourcePopupParams,
    reloadListingId: number,
    isDuplicatingListing: boolean,
    user: User,
    cancelAction: RedirectAction,
    saveAction: RedirectAction,
    homeSiteId?: string,
    setMiqAddress?: any
}

export interface RedirectAction {
    goto?: string
}

export interface MainMessaging {    /* main message meaning a display to the user that takes up the content area of the screen */
    show: boolean,
    type?: MainMessageType,
    message?: string,
    messageLine2?: string
    home?: boolean
}

export enum MainMessageType {
    LOADING = "LOADING",
    SAVING = "SAVING",
    ERROR = "ERROR",
    NOTICE = "NOTICE",
    NONE = "NONE"
}

export interface AlertMessaging {   /* alerts will only be visible in a small part of the screen */
    show: boolean,
    message?: string,
    type?: AlertMessagingType,
    allowClose?: boolean
}

export enum AlertMessagingType {
    ERROR = "ERROR",
    WARNING = "WARNING",
    NOTICE = "NOTICE",
    SUCCESS = "SUCCESS",
    NONE = "NONE"
}

export interface ConfirmDialogParams {
    show: boolean,
    showConfirmButton?: boolean,
    showCopyButton?: boolean,
    title?: string,
    message?: string,
    scrollable?: boolean,
    cancelTxt?: string,
    confirmTxt?: string,
    copyTxt?: string,
    cancelFunc?(): void,
    confirmFunc?(): void
}

export interface ListingSuffix{
    measure: string,
    leaseTerm: string 
}

export interface Contacts {
    allContacts: Contact[]
}

export interface DataSourcePopupParams {
    show: boolean,
    action: string,
    datasource: DataSource,
    confirmFunc?(popupdata: any): void
}

export interface MIQ {
    show: boolean,
    selectedSearchOption: MIQSearchResult,
    currentRecords: Listing[],
    pendingAddEditRecord: Listing,
    status: MIQStatus,
    exportMessage: MIQExportMessage,
    redirect: string,
    spaces: any[]
}

export interface Insights {
    loading: MainMessaging,
    current: InsightsRecord
}