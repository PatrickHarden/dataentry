import { Config } from "../../types/config/config";
import { GLFile } from "../../types/listing/file";
import { Listing } from "../../types/listing/listing";
import { Space } from "../../types/listing/space";

export const convertListingDataToSupportMultiRegional = (listing:Listing, config:Config) => {

    let defaultCultureCode:string = "";
    if (config && config.defaultCultureCode) {
        defaultCultureCode = config.defaultCultureCode;
    }

    listing.headlineSingle = "";
    if (listing.headline && listing.headline.length > 0){
        listing.headlineSingle = listing.headline[0].text;
        if (defaultCultureCode !== "") {
            const headlineDefault = listing.headline.filter((d: any) => {
                return d.cultureCode === defaultCultureCode;
            });
            if (headlineDefault.length > 0) {
                listing.headlineSingle = headlineDefault[0].text;
            }
        }
    }

    listing.buildingDescriptionSingle = "";
    if (listing.buildingDescription && listing.buildingDescription.length>0){
        listing.buildingDescriptionSingle = listing.buildingDescription[0].text;
        if (defaultCultureCode !== "") {
            const buildingDescriptionDefault = listing.buildingDescription.filter((d: any) => {
                return d.cultureCode === defaultCultureCode;
            });
            if (buildingDescriptionDefault.length > 0) {
                listing.buildingDescriptionSingle = buildingDescriptionDefault[0].text;
            }
        }
    }

    listing.locationDescriptionSingle = "";
    if (listing.locationDescription && listing.locationDescription.length>0){
        listing.locationDescriptionSingle = listing.locationDescription[0].text;
        if (defaultCultureCode !== "") {    
            const locationDescriptionDefault = listing.locationDescription.filter((d: any) => {
                return d.cultureCode === defaultCultureCode;
            });
            if (locationDescriptionDefault.length > 0) {
                listing.locationDescriptionSingle = locationDescriptionDefault[0].text;
            }
        }
    }

    if (listing.spaces) {
        listing.spaces.map((space: any) => {
            space.brochures.map((p: GLFile) => { p.primary = false });
            space.floorplans.map((p: GLFile) => { p.primary = false });

            space.nameSingle = "";
            if (space.name && space.name.length>0){
                space.nameSingle = space.name[0].text;
                if (defaultCultureCode !== "") {
                    const spaceNameDefault = space.name.filter((d: any) => {
                        return d.cultureCode === defaultCultureCode;
                    });
                    if (spaceNameDefault.length > 0) {
                        space.nameSingle = spaceNameDefault[0].text;
                    }
                }
            }

            space.spaceDescriptionSingle = "";
            if (space.spaceDescription && space.spaceDescription.length>0){
                space.spaceDescriptionSingle = space.spaceDescription[0].text;
                if (defaultCultureCode !== "") {
                    const spaceDescriptionDefault = space.spaceDescription.filter((d: any) => {
                        return d.cultureCode === defaultCultureCode;
                    });
                    if (spaceDescriptionDefault.length > 0) {
                        space.spaceDescriptionSingle = spaceDescriptionDefault[0].text;
                    }
                }
            }
       
        })
    }

    if (listing.highlights) {

        // first determine language configuration to only pull the highlights in supported languages
        let supportedLangs:any[] = [];
        if(config && config.languages){
            supportedLangs = config.languages;
        } else if (config && config.defaultCultureCode) {
            supportedLangs.push(config.defaultCultureCode);
        } else {
            supportedLangs.push("en-US"); // temp default language
        }
        

        const tempSortedLangHighlights:any[] = [];

        
        // sort only languages in config by order number
        listing.highlights.forEach((highlight:any) => {
            if(supportedLangs.includes(highlight.cultureCode)) {
               
                // using loop for performance/IE support 
                let i = 0;
                for (i = 0; i < tempSortedLangHighlights.length; i++) {
                    if (highlight.order.value >= tempSortedLangHighlights[i].order){
                        break;
                    }
                }
                tempSortedLangHighlights.splice(i, 0, highlight);
            }
                        
        });

        // convert sorted array to UI format
        const convertedHighlights:any[] = [];
        let x = 0;
        let highlightMultiLangValues:any[] = [];
        for (x = 0; x < tempSortedLangHighlights.length; x++) {
            
            highlightMultiLangValues.push({...tempSortedLangHighlights[x]});
            if ((x === (tempSortedLangHighlights.length-1)) || (tempSortedLangHighlights[x+1].order > tempSortedLangHighlights[x].order)){
                convertedHighlights.push({order: convertedHighlights.length, value: highlightMultiLangValues.slice(0)});
                highlightMultiLangValues = [];
            }

        }

        listing.highlights = convertedHighlights;
    }

    return listing;
}

const convertDateToLocal = (date:any, timezone:string) => {
    if(date && typeof(date) === 'string' && date.length > 0){
        const reformatParts = date.split("T");
        if(reformatParts.length === 2){
            return reformatParts[0] + "T" + timezone;
        }
    }
    return date;
}

export const checkDatesForTimeZone = (listing:Listing) => {
    const currentDate = new Date();
    const currentTime = currentDate.toISOString();
    const timeParts = currentTime.split("T");
    if(timeParts && timeParts.length === 2){
        const tz = timeParts[1];
        listing.availableFrom = convertDateToLocal(listing.availableFrom, tz);
        // and for each space
        if(listing.spaces && listing.spaces.length > 0){
            listing.spaces.forEach((space:Space) => {
                space.availableFrom = convertDateToLocal(space.availableFrom, tz);
            });
        }
    }
    return listing;
}