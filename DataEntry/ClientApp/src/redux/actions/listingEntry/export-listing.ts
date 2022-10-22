import { Listing } from '../../../types/listing/listing';
import { setConfirmDialog } from '../system/set-confirm-dialog';
import cloneDeep from 'clone-deep';
import { Highlight } from '../../../types/listing/highlight';
import { Space } from '../../../types/listing/space';
import { Contact } from '../../../types/listing/contact';
import { Config } from '../../../types/config/config';

export const exportListing = (listing: Listing, config: Config) => (dispatch: Function) => {
    const filteredListing: Listing = filterListingData(listing, config);
    const formattedListing: string = formatListing(filteredListing);
    dispatch(
        setConfirmDialog(
            {
                show: true,
                title: "Exported Listing",
                message: formattedListing,
                cancelTxt: "Close",
                showConfirmButton: false,
                showCopyButton: true
            }));
}

function filterListingData(listing:Listing, config:Config):Listing {
    const filteredListing: any = cloneDeep(listing);

    delete filteredListing.aspects;
    delete filteredListing.brochures;
    delete filteredListing.bulkUploadFileName;
    delete filteredListing.chargesAndModifiers;
    delete filteredListing.datasource;
    delete filteredListing.dataSource;
    delete filteredListing.dateCreated;
    delete filteredListing.dateUpdated;
    delete filteredListing.externalPreviewUrl;
    delete filteredListing.externalPublishUrl;
    delete filteredListing.floorplans;
    delete filteredListing.floors;
    delete filteredListing.yearBuilt;
    delete filteredListing.id;
    delete filteredListing.isBulkUpload;
    delete filteredListing.isDeleted;
    delete filteredListing.microMarkets;
    delete filteredListing.operator;
    delete filteredListing.owner;
    delete filteredListing.photos;
    delete filteredListing.previewSearchApiEndPoint;
    delete filteredListing.propertyRecordName;
    delete filteredListing.propertySubType;
    delete filteredListing.published;
    delete filteredListing.spacesCount;
    delete filteredListing.state;
    delete filteredListing.teamList;
    delete filteredListing.teams;
    delete filteredListing.userList;
    delete filteredListing.users;
    delete filteredListing.epcGraphs;

    if (filteredListing.contacts) {
        filteredListing.contacts.forEach((v: Contact) => {
            delete v.contactId;
            delete v.avatar;

            // Remove empty license number
            if (v.additionalFields && v.additionalFields.license && v.additionalFields.license === "0") {
                delete v.additionalFields.license;
            }
        });
    }

    // Remove sort order from highlights
    filteredListing.highlights = filteredListing.highlights.map((v: Highlight) => v.value);

    if (filteredListing.spaces) {
        filteredListing.spaces.forEach((v: Space) => {
            delete v.brochures;
            delete v.floorplans;
            delete v.id;
            delete v.photos;

            if (v.specifications !== null) {
                delete v.specifications.bedrooms;
                delete v.specifications.contactBrokerForPrice;
                delete v.specifications.currencyCode;
                delete v.specifications.taxModifer;
            }
        });
    }

    if (filteredListing.specifications !== null) {
        delete filteredListing.specifications.bedrooms;
        delete filteredListing.specifications.contactBrokerForPrice;
        delete filteredListing.specifications.currencyCode;
        delete filteredListing.specifications.taxModifer;
    }

    return filteredListing;
}

function formatListing(obj:object): string {
    let result: string = "";
    if (obj === null || obj === undefined) {
        return result;
    }
    if (Array.isArray(obj)) {
        const innerResult: string[] = [];
        let isComplex: boolean = false;
        (obj as object[]).forEach(value => {
            const formattedValue = formatListing(value);
            if (formattedValue.indexOf("\n") !== -1) {
                isComplex = true;
            }
            innerResult.push(formatListing(value));
        });
        result = innerResult.join(isComplex ? "\n\n" : "\n");
    }
    else if (typeof obj === "object") {
        const lines: string[] = [];
        for (const prop in obj) {
            if (!Object.prototype.hasOwnProperty.call(obj, prop)) {
                continue;
            }
            const value = obj[prop];
            const innerResult = formatListing(value).replace(/\n/g, "\n\t");
            if (innerResult.length > 0) {
                lines.push(`${prop}:${innerResult.indexOf("\n") !== -1 ? "\n\t" : " "}${innerResult}`);
            }
        }
        result = lines.join("\n");
    }
    else {
        result = (obj as object).toString();
    }
    return result;
}