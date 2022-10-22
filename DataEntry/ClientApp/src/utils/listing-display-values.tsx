/* This is a set of utilities to help us format data for listings (Listing) */
import React from 'react';
import { Listing } from '../types/listing/listing';
import { Specifications } from '../types/listing/specifications';
import { formatDecimals } from './forms/format-decimals';
import { formatNumberWithCommas } from './forms/format-number-commas';

// local formatters

// price
const priceStringFormatter = (currencySymbol: string, value: number): string => {
    let result = "" + value;
    result = formatDecimals(result, 2);
    result = formatNumberWithCommas(result);
    result = currencySymbol + result;
    return result;
}

// combinator
const combineFields = (rootString: string, appendString?: string, appendAfter?: string) => {
    appendString ? rootString += appendString : rootString = rootString;
    appendString && appendAfter ? rootString += appendAfter : rootString = rootString;
    return rootString;
}

// utility methods to extract key pieces of listing data

// "sizes"
export const findSizeUsingSpecs = (specifications:Specifications) => {
    let returnStr = "-";
    if (specifications) {
        if (specifications.totalSpace) {
            returnStr = " " + specifications.totalSpace + " ";
            returnStr += specifications.measure && specifications.measure.length > 0 ? specifications.measure.toUpperCase() : "";
        }
    }
    returnStr = formatNumberWithCommas(returnStr);
    return returnStr;
}

// "prices"
export const findPriceUsingSpecs = (currencySymbol:string, listingType:string, specifications:Specifications) => {
    let returnStr = "";
    if (specifications) {
        if ((listingType === "sale" || listingType === "saleLease" || listingType === "investment") && specifications.salePrice) {
            returnStr += priceStringFormatter(currencySymbol, specifications.salePrice);
        }
        else if (listingType === "lease" || listingType === "saleLease") {
            if (specifications.minPrice) {
                returnStr += priceStringFormatter(currencySymbol, specifications.minPrice);
            }
            if (specifications.maxPrice) {
                if (returnStr.length > 0) {
                    returnStr += " - ";
                }
                returnStr += priceStringFormatter(currencySymbol, specifications.maxPrice);
            }
            if (returnStr.length > 0 && specifications.measure && specifications.measure.length > 0) {
                returnStr += " / " + specifications.measure.toUpperCase();
            }
        }
    }
    if (returnStr.length === 0) {
        returnStr = "-";
    }
    return returnStr;
}

export const renderAddressDom = (listing:Listing, WrapperElement: any) => {
    let addressLine1:string = "";
    let addressLine2:string = "";
    let cityStateZip:string = "";
    
    // address line 1
    addressLine1 = combineFields(addressLine1,listing.street," ");
    addressLine2 = combineFields(addressLine2,listing.street2," ");
    // city/state/zip
    cityStateZip = combineFields(cityStateZip,listing.city);
    if(listing.city && listing.stateOrProvince){
        cityStateZip += ", ";
    }
    cityStateZip = combineFields(cityStateZip,listing.stateOrProvince," ");
    cityStateZip = combineFields(cityStateZip,listing.postalCode);

    return (
        <>
            { addressLine1 && addressLine1.trim().length > 0 && <WrapperElement data-testid="property-result-view-address-line-1">{addressLine1}</WrapperElement> }
            { addressLine2 && addressLine2.trim().length > 0 && <WrapperElement data-testid="property-result-view-address-line-2">{addressLine2}</WrapperElement> }
            { cityStateZip && cityStateZip.trim().length > 0 && <WrapperElement data-testid="property-result-view-address-cityStateZip">{cityStateZip}</WrapperElement> }
        </>
    );
}