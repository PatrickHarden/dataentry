import { ActionPayload } from '../../../types/common/action';
import { State, MainMessageType, ListingSuffix } from '../../../types/state';
import { Listing } from '../../../types/listing/listing';
import defaultListing from '../../../api/defaults/listing';
import { setMainMessage, clearMessage } from '../system/set-main-message';
import { pullSingleListing } from '../../../api/glQueries'
import { postData } from '../../../api/glAxios'
import { refreshAllContacts } from '../contacts/refresh-all-contacts';
import { authContext } from '../../../adalConfig';
import { TeamMember } from '../../../types/team/teamMember'
import cloneDeep from 'clone-deep';
import { GLFile } from '../../../types/listing/file';
import { checkPreviewStatus, setDirtyListing } from '../../actions/listingEntry/update-navbar';
import { updateSavePending } from './update-pending-operation';
import { Config } from '../../../types/config/config';
import { setListingSuffix } from '../system/set-suffix-string';
import { Specifications } from '../../../types/listing/specifications';
import { Space } from '../../../types/listing/space';
import { saveListing, SaveType } from './save-listing';
import { updateSpaces } from './update-spaces';

// constants
export const CURRENT_LISTING_LOADED = 'CURRENT_LISTING_LOADED';

// types
export type CurrentListingLoadedAction = ActionPayload<Listing> & {
    type: typeof CURRENT_LISTING_LOADED
};

export const currentListingLoaded = (payload: Listing): CurrentListingLoadedAction => ({
    type: CURRENT_LISTING_LOADED,
    payload
});

const getData = (id: string) => {
    const query = pullSingleListing(parseInt(id, 10))
    const data = postData(query)
    return data
}

export const loadCurrentListing = (id: string, config: Config, duplicate: Boolean = false) => (dispatch: Function, getState: () => State) => {
    // tests if there's numbers in the pathname
    // const isNumeric = (n:any) => {
    //     return /\d/.test(n);
    // }

    // ensure the dirty check on the listing is false
    dispatch(setDirtyListing(false));
    // ensure that we clear out the pending save flag
    dispatch(updateSavePending(false));

    dispatch(refreshAllContacts());  // ensure that the contacts are updated
    if (id !== undefined && id.length > 0) {

        dispatch(setMainMessage({ show: true, type: MainMessageType.LOADING, message: duplicate ? "Duplicating Listing..." : "Loading Listing..." }));

        getData(id).then(result => {
            if (result.data && result.data.listing) {
                const listing: Listing = massage(result.data.listing, config);

                // Tech debt: manually set the primary to false for floorplans and brochures
                // We need to figure out why it set them as true when pass to graphql
                listing.brochures.map((p: GLFile) => { p.primary = false });
                listing.floorplans.map((p: GLFile) => { p.primary = false });
                if (listing.epcGraphs) {
                    listing.epcGraphs.map((p: GLFile) => { p.primary = false });
                }

                let defaultCultureCode: string = "";
                if (config && config.defaultCultureCode) {
                    defaultCultureCode = config.defaultCultureCode;
                }

                listing.headlineSingle = "";
                if (listing.headline && listing.headline.length > 0) {
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
                if (listing.buildingDescription && listing.buildingDescription.length > 0) {
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
                if (listing.locationDescription && listing.locationDescription.length > 0) {
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
                        if (space.name && space.name.length > 0) {
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
                        if (space.spaceDescription && space.spaceDescription.length > 0) {
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

                if (getState().miq.spaces && getState().miq.spaces.length > 0) {
                    getState().miq.spaces.forEach(space => {
                        listing.spaces.push(space)
                    })
                }

                if (result.data.listing.dataSource) {
                    const datasource = {
                        datasources: result.data.listing.dataSource.dataSources ? result.data.listing.dataSource.dataSources : [],
                        other: result.data.listing.dataSource.other ? result.data.listing.dataSource.other : ""
                    };

                    listing.datasource = datasource;
                }

                // if a country is not set and we have a defualt setup, be sure to set it here before it goes out
                if (config && config.country && config.country.show
                    && config.stateOrProvince && config.stateOrProvince.defaultCountry
                    && (!listing.country || listing.country.trim().length === 0)) {
                    listing.country = config.stateOrProvince.defaultCountry;
                }

                // find the listing suffix if it exists
                if (listing && listing.specifications) {
                    const listingSuffix: ListingSuffix = {
                        measure: listing.specifications.measure,
                        leaseTerm: listing.specifications.leaseTerm
                    }
                    dispatch(setListingSuffix(listingSuffix));
                }

                if (duplicate) {
                    // User has clicked the duplicate button, and the original has been saved. Initialize the duplicate 
                    // listing to prepare to save.

                    // Reset IDs
                    listing.id = defaultListing.id;
                    listing.externalId = null;

                    // Update name to indicate a duplicate
                    listing.propertyRecordName += " - Copy";

                    // Clear out old timestamps and ensure current user is assigned as the owner
                    initCreatedListing(listing);

                    // Initialize form in case of errors while saving
                    dispatch(currentListingLoaded(listing));

                    //
                    dispatch(updateSpaces(listing.spaces));

                    // Begin a 2nd pass through save flow to save the newly duplicated listing
                    dispatch(saveListing(listing, SaveType.SAVEDUPLICATE, config));
                } else {
                    dispatch(currentListingLoaded(listing));
                    //
                    dispatch(updateSpaces(listing.spaces));

                    // kick off the preview check so the navbar can update the preview status easily
                    checkPreviewStatus(dispatch, getState(), listing);
                    clearMessage(dispatch);
                }
            } else {
                dispatch(setMainMessage({ show: true, type: MainMessageType.ERROR, message: "Unauthorized content", home: true }));
            }
        })
    } else {

        let listingToClone: Listing;
        const miqListing: any = getState().miq.pendingAddEditRecord;
        if (miqListing && miqListing.constructor === Object && Object.keys(miqListing).length !== 0) {
            listingToClone = miqListing;
        } else {
            listingToClone = defaultListing;
        }
        // create the listing object and prepare it for the create page
        const newCreatedListing = cloneDeep(listingToClone);

        initCreatedListing(newCreatedListing);
        massage(newCreatedListing, config);
        dispatch(updateSpaces(newCreatedListing.spaces));
        dispatch(currentListingLoaded(newCreatedListing));
        clearMessage(dispatch);

        // ensure that we try to kill off any existing preview check
        checkPreviewStatus(dispatch, getState(), newCreatedListing);
    }
}

export const massage = (listing: any, config: Config) => {
    // any data that needs to be "massaged" on the front end before loading in

    if (listing.users && listing.users.length) {
        listing.users.forEach((user: any) => {
            if (user.id) {
                user.email = user.id;
            }
        });
    }

    if (listing.teams && listing.teams.length) {
        listing.teams.forEach((team: any) => {
            team.id = team.name;
        });
    }

    if (listing.microMarkets && listing.microMarkets.length > 0 && listing.microMarkets[0]) {
        listing.microMarket = listing.microMarkets[0].value;
    }

    if (listing.dateCreated) {
        listing.dateCreated = new Date(Date.parse(listing.dateCreated))
    }

    if (listing.dateUpdated) {
        listing.dateUpdated = new Date(Date.parse(listing.dateUpdated));
    }

    if (listing.datePublished) {
        const publishedDate = new Date(Date.parse(listing.datePublished));
        if (publishedDate.getFullYear() > 1999) {
            listing.datePublished = publishedDate;
        }
        else {
            listing.datePublished = null;
        }
    }
    else {
        listing.datePublished = null;
    }

    if (listing.dateListed) {
        const listedDate = new Date(Date.parse(listing.dateListed));
        if (listedDate.getFullYear() > 1999) {
            listing.dateListed = listedDate;
        }
        else {
            listing.dateListed = null;
        }
    }
    else {
        listing.dateListed = null;
    }

    if (listing.specifications) {
        verifyCurrencyCodes(listing.specifications, config);
    }
    if (listing.spaces && listing.spaces.length > 0) {
        listing.spaces.map((space: Space) => {
            if (space.specifications) {
                verifyCurrencyCodes(space.specifications, config);
            }
        })
    }

    // convert highlights to format for front end
    if (listing.highlights && listing.highlights.length > 0 && !listing.highlights[0].value) {

        // first determine language configuration to only pull the highlights in supported languages
        let supportedLangs: any[] = [];
        if (config && config.languages) {
            supportedLangs = config.languages;
        } else if (config && config.defaultCultureCode) {
            supportedLangs.push(config.defaultCultureCode);
        } else {
            supportedLangs.push("en-US"); // temp default language
        }


        const tempSortedLangHighlights: any[] = [];


        // sort only languages in config by order number
        listing.highlights.forEach((highlight: any) => {
            if (supportedLangs.includes(highlight.cultureCode)) {

                // using loop for performance/IE support 
                let i = 0;
                for (i = 0; i < tempSortedLangHighlights.length; i++) {
                    if (highlight.order.value >= tempSortedLangHighlights[i].order) {
                        break;
                    }
                }
                tempSortedLangHighlights.splice(i, 0, highlight);
            }

        });

        // convert sorted array to UI format
        const convertedHighlights: any[] = [];
        let x = 0;
        let highlightMultiLangValues: any[] = [];
        for (x = 0; x < tempSortedLangHighlights.length; x++) {

            highlightMultiLangValues.push(tempSortedLangHighlights[x]);
            if ((x === (tempSortedLangHighlights.length - 1)) || (tempSortedLangHighlights[x + 1].order > tempSortedLangHighlights[x].order)) {
                convertedHighlights.push({ order: convertedHighlights.length, value: highlightMultiLangValues.slice(0) });
                highlightMultiLangValues = [];
            }

        }

        listing.highlights = convertedHighlights;
    }

    if (listing.externalRatings && listing.externalRatings.length > 0) {
        const epcObject = listing.externalRatings.find((rating: any) => rating.ratingType === 'EPC');
        listing.epcRating = (epcObject && epcObject.ratingLevel && epcObject.ratingLevel !== null) ? epcObject.ratingLevel.replaceAll('+', 'Plus') : null;
        
        listing.wellRating = listing.externalRatings.find((rating: any) => rating.ratingType === 'WELL') ? listing.externalRatings.find((rating: any) => rating.ratingType === 'WELL').ratingLevel : null;
        listing.energyRating = listing.externalRatings.find((rating: any) => rating.ratingType === 'BREEAM') ? listing.externalRatings.find((rating: any) => rating.ratingType === 'BREEAM').ratingLevel : null;
        listing.leedRating = listing.externalRatings.find((rating: any) => rating.ratingType === 'LEED') ? listing.externalRatings.find((rating: any) => rating.ratingType === 'LEED').ratingLevel : null;

        delete listing.externalRatings;
    }
    else {
        listing.wellRating = '';
        listing.energyRating = '';
        listing.epcRating = '';
        listing.leedRating = '';
    }

    return listing;
}

export const listingFieldCheck = (listing: any) => {
    const payload: any = listing;
    for (const key in listing) {
        if (payload[key] === null) {
            payload[key] = defaultListing[key]
        }
    }
    return payload;
}

function initCreatedListing(newCreatedListing: Listing) {
    newCreatedListing.dateUpdated = undefined;
    newCreatedListing.dateCreated = undefined;
    newCreatedListing.datePublished = undefined;
    newCreatedListing.dateListed = undefined;

    if (!newCreatedListing.users) {
        newCreatedListing.users = [];
    }
    const currentUser = authContext.getCachedUser();
    if (currentUser
        && currentUser.userName
        && (
            !newCreatedListing.owner
            || currentUser.userName !== newCreatedListing.owner)) {
        newCreatedListing.owner = currentUser.userName;
        if (!newCreatedListing.users.some(u => u.email === currentUser.userName)) {
            const ownerUser: TeamMember = {
                teamMemberId: "",
                firstName: currentUser.profile.given_name,
                lastName: currentUser.profile.family_name,
                id: currentUser.userName,
                email: currentUser.userName
            };
            newCreatedListing.users.push(ownerUser);
        }
    }
}

function verifyCurrencyCodes(specifications: Specifications, config: Config) {
    if (config.currencyCode && specifications) {
        if (specifications.currencyCode && specifications.currencyCode.toUpperCase() !== config.currencyCode.toUpperCase()) {
            // Reset prices for incorrect currency codes
            specifications.minPrice = 0;
            specifications.maxPrice = 0;
        }
        // Set correct currency code if incorrect or missing
        specifications.currencyCode = config.currencyCode;
    }
}