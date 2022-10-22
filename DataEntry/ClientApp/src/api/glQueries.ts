import { Listing } from '../types/listing/listing';
import { Space } from '../types/listing/space'
import moment from 'moment';
import { Team } from '../types/team/team';
import { UserSearchFilter } from '../types/team/userSearchFilter';
import { Config } from '../types/config/config';
import { SpecsListingTypeFields, SpecsFieldSetup } from '../types/config/specs/specs';
import { findSpecificationFields } from '../utils/config/specifications-fields';
import { SpacesListingTypeFields, SpacesFieldSetup } from '../types/config/spaces/spaces';
import { findSpacesFields } from '../utils/config/spaces-fields';
import { GLFile } from '../types/listing/file';
import { Contact } from '../types/listing/contact';
import { sortGLFiles } from '../utils/sort-files';
import { Filter } from '../types/listing/filters';
import { MultiLangString } from '../types/listing/multi-lang-string';
import { isArray } from 'lodash';
import { gql } from 'graphql-tag'
import { print } from 'graphql/language/printer';
import { DocumentNode } from 'graphql/language/ast';

export const defaultRegionID = "00000000-0000-0000-0000-000000000001";

export const loadAppSettings = () => {
  return buildRequestData (
    gql`query {
      configs {
        homeSiteId
        aiKey
        previewFeatureFlag
        watermarkDetectionFeatureFlag
        miqImportFeatureFlag
        miqLimitSearchToCountryCodeFeatureFlag
        googleMapsKey,
        googleMapsChannel
      }
    }`
  );
}

export const pullRegions = () => {
  return buildRequestData(
    gql`query {
      regions {
        iD, 
        name, 
        homeSiteID, 
        previewSiteID, 
        listingPrefix, 
        previewPrefix, 
        cultureCode, 
        countryCode
      }
    }`
  );
}

export const getAllContacts = () => {
  return buildRequestData(
    gql`query {
      contacts {
        contactId
        firstName
        lastName
        email
        phone
        location
        avatar
        additionalFields {
          license
        }
      }
    }`
  );
}

export const isAdmin = (regionID: string|null) => {
  return buildRequestData(
    gql`query ($regionID: String){
      isAdmin (regionID: $regionID) 
    }`,
    {
      regionID: regionID
    }
  );
}

export const countAssigned = () => {
  return buildRequestData(
    gql`query {
      count(
        filterOptions: [
          {type:"MiqOnly", value:"true"}
          {type:"AssignmentStatus", value:"true"}
        ]
      )
    }`);
}

export const pullListingsPaged = (skip: number, take:number, filters:Filter[], region?: string | null) => {
  return buildRequestData(
    gql`query (
      $skip: Int = null
      $take: Int = null
      $filterOptions: [FilterInput] = null
      $regionID: String = null
    ) {
      listings(
        skip: $skip
        take: $take
        filterOptions: $filterOptions
        regionID: $regionID
      ) {
        id
        isDeleted 
        propertyRecordName
        street
        city
        stateOrProvince
        postalCode
        propertyType
        listingType
        state
        published
        externalPublishUrl
        photos {
          primary
          url
        }
        specifications {
          contactBrokerForPrice
          totalSpace
          measure
          salePrice
          minPrice
          maxPrice
        }
        contacts {
          firstName
          lastName
          avatar
        }
        listingAssignment {
          assignedBy
          assignmentFlag
          assignedDate
        }
      }
    }`,
    {
      skip,
      take,
      filterOptions: filters,
      regionID: region || defaultRegionID
    });
};

export const findMiQProperties = (term: string, countryCode: string) => {
  return buildRequestData(
    gql`query (
      $term: String!
      $countryCode: String = null
    ) {
      searchEdpProperties(
        keyword:$term
        country:$countryCode
      ) {
        id
        name
        street1
        street2
        city
        stateProvince
        postalCode
        propertyType
      }
    }`,
    {
      term,
      countryCode
    }
  );
}

export const getAllTeamsForUser = () => {
  return buildRequestData(
    gql`query {
      teams {
        name
        users {
          id
          firstName
          lastName     
        }
      } 
    }`
  );
}

export const pullSingleListing = (id: number) => {
  return buildRequestData(
    gql`query(
      $id: Int!
    ) {
      listing(id: $id) {
        ...listingEditPageFields
      }
    }
    ${listingEditPageFields}`,
    {
      id
    });
}

export const updateListingQuery = (listingData: Listing, config:Config, regionID?: any) => {
  return buildRequestData(
    gql`mutation(
      $listing: ListingInput!
    ) {
      updateListing(listing: $listing) {
        ...listingEditPageFields
      }
    }
    ${listingEditPageFields}`,
    {
      listing: getListingDTO(listingData, config, regionID)
    }
  );
}

export const createNewListingQuery = (listingData: Listing, config: Config, regionId?: string) => {
  const listing = getListingDTO(listingData, config, regionId);

  // Do not pass the listing id on create
  delete listing.id;

  return buildRequestData(
    gql`mutation(
      $listing: ListingInput!
    ) {
      createListing(
        listing: $listing
      )
      {
      ...listingEditPageFields
      }
    }
    ${listingEditPageFields}`,
    {
      listing
    }
  );
}

export const createNewTeamQuery = (teamData: Team) => {
  const userList:string[] = teamData.users.map((member:any) => member.email);

  return buildRequestData(
    gql`mutation(
      $name: String!
      $users: [String!] = null
    ) {
      createTeam(
        name: $name,
        users: $users
      )
    }`,
    {
      name: teamData.name,
      users: Array.isArray(userList) ? userList : []
    });
}

export const updateTeamQuery = (teamData: Team) => {
  const userList:string[] = teamData.users.map((member:any) => member.email);

  return buildRequestData(
    gql`
      mutation(
        $name: String!
        $newName: String = null
        $users: [String!] = null
      ) {
        updateTeam(
          name: $name,
          newName: $newName,
          users: $users
        )
      }
    `,
    {
      name: teamData.id,
      newName: teamData.name,
      users: Array.isArray(userList) ? userList : []
    });
}

export const deleteTeamQuery = (id: string) => {
  return buildRequestData(
    gql`mutation(
      $name: String!
    ) {
      deleteTeam(
        name: $name
      )
    }`,
    {
      name: id
    });
}

export const getUsers = (userData: UserSearchFilter) => {
  return buildRequestData(
    gql`query(
      $term: String = null
      $blacklist: [String] = null
      $skip: Int = null
      $take: Int = null
    ) {
      users(
        term: $term
        blacklist: $blacklist
        skip: $skip
        take: $take
      )
      {
        id
        fullName
        firstName
        lastName
      }
    }`,
    {
      term: userData.term || ``,
      blacklist: Array.isArray(userData.blacklist) ? userData.blacklist : null,
      skip: userData.skip || null,
      take: userData.take || null 
    });
}

export const getUsersOrTeams = (userData: UserSearchFilter) => {
  return buildRequestData(
    gql`query(
      $term: String = null
      $blacklist: [String] = null
      $skip: Int = null
      $take: Int = null
    ) {
      claimants(
        term: $term
        blacklist: $blacklist
        skip: $skip
        take: $take
      )
      {
        name
        fullName
        firstName
        lastName
        isTeam
      }
    }`,
    {
      term: userData.term || ``,
      blacklist: Array.isArray(userData.blacklist) ? userData.blacklist : null,
      skip: userData.skip || null,
      take: userData.take || null 
    });
}

export const deleteListingQuery = (id: number) => {
  return buildRequestData(
    gql`mutation(
      $id: Int!
    ) {
      deleteListing(
        id: $id
      )
    }`,
    {
      id
    });
}

export const checkForNewSpaces = (id: number) => {
  return buildRequestData(
    gql`query (
      $id: Int!
    ) {
      spaces(id: $id) { 
        id        
        miqId        
        availableFrom        
        name {          
          cultureCode          
          text        
        }        
        spaceDescription {          
          cultureCode          
          text        
        }        
        spaceType        
        status        
        specifications {          
          contactBrokerForPrice          
          currencyCode          
          leaseTerm          
          leaseType          
          maxPrice          
          maxSpace          
          measure          
          minPrice          
          minSpace          
          totalSpace          
          salePrice        
        }        
        photos {          
          id          
          active          
          displayText          
          primary          
          order          
          url          
          watermark          
          watermarkProcessStatus          
          userOverride        
        }        
        floorplans {          
          id          
          active          
          displayText          
          primary          
          order          
          url          
          watermark          
          watermarkProcessStatus          
          userOverride        
        }        
        brochures {          
          active          
          displayText          
          primary          
          url        
        }        
        video        
        walkThrough        
        spaceSizes {          
          sizeKind          
          amount          
          measureUnit
        }
      }
    }`,
    {
      id
    }
  );
}

export const publish = (id: number) => {
  return buildRequestData(
    gql`mutation(
      $id: Int!
    ) {
      publishListing(
        id: $id
      ) 
      {
        id
        state
      }
    }`,
    {
      id
    });
}

export const unpublish = (id: number) => {
  return buildRequestData(
    gql`mutation(
      $id: Int!
    ) {
      unpublishListing(
        id: $id
      )
      {
        id
        state
      }
    }`,
    {
      id
    });
}

export const saveContact = (contact: Contact) => {
  return buildRequestData(
    gql`mutation(
      $contactId: Int
      $firstName: String
      $lastName: String
      $location: String
      $avatar: String
      $phone: String
      $email: String
      $license: String
    ) {
      saveContact(
        broker:{
          contactId: $contactId
          firstName: $firstName
          lastName: $lastName
          location: $location
          phone: $phone
          email: $email
          avatar: $avatar
          additionalFields:{
              license: $license
          }
        }
      )
      {
          contactId,
          firstName
          lastName
          location
          phone
          email
          avatar
          additionalFields
          {
              license
          }
      }
    }`,
    {
      contactId: contact.contactId || null,
      firstName: contact.firstName || ``,
      lastName: contact.lastName || ``,
      location: contact.location || ``,
      phone: contact.phone || ``,
      email: contact.email || ``,
      avatar: contact.avatar || ``,
      license: contact.additionalFields && contact.additionalFields.license || ``
    });
}

export const checkImagesProcessingStatus = (listingId:number | null, imageIds:number[] | null) => {
  return buildRequestData(
    gql`query(
      $listingId: Int = null
      $imageIds: [Int] = null
    ) {
      images(
        listingId: $listingId
        imageIds: $imageIds
      ) {
          id
          url
          watermarkProcessStatus
      }
    }`,
    {
      listingId,
      imageIds
    });
}

export const importEDPProperties = (miqID: number, regionID: string) => {
  return buildRequestData(
    gql`query(
      $id: Int!
      $regionID: String = null
    ) {    
      getEdpImportProperty(
        id: $id
        regionID: $regionID
      ) {
        ...listingEditPageFields    
        floorplans {
          base64String      
        }      
        brochures {
          base64String      
        }          
        epcGraphs {
          base64String      
        }     
        photos {
          base64String      
        }     
        spaces {
          photos {
            base64String       
          }        
          floorplans {
            base64String        
          }   
          brochures {
            base64String        
          }
        }   
      }  
    }
    ${listingEditPageFields}`,
    {
      id: miqID,
      regionID: regionID || defaultRegionID
    });
}

function buildRequestData(doc: DocumentNode, variables?:any) {
  return {
    "query": print(doc),
    "variables": variables
  }
};

const listingEditPageFields = gql`fragment listingEditPageFields on Listing {
  id
  miqId
  regionID
  externalId
  state
  propertyName
  propertyRecordName
  configId
  street
  street2
  city
  stateOrProvince
  postalCode
  country
  operator
  lat
  lng
  alternatePostalAddresses {
    street
    street2
    city
    stateOrProvince
    postalCode
    country
    lat
    lng
  }
  energyRating
  externalRatings {
    ratingType
    ratingLevel
  }
  buildingDescription {
    cultureCode
    text
  }
  locationDescription {
    cultureCode
    text
  }
  listingType
  propertyType
  propertySubType
  propertyUseClass
  status
  availableFrom
  syndicationFlag
  syndicationMarket
  website
  headline {
    cultureCode
    text
  }
  video
  walkThrough
  importedData
  owner
  floors
  yearBuilt
  aspects
  previewSearchApiEndPoint
  externalPreviewUrl
  externalPublishUrl
  dateCreated
  dateUpdated
  datePublished
  dateListed
  isDeleted
  chargesAndModifiers {
    chargeType
    chargeModifier
    term
    amount
    perUnitType
    year
    currencyCode
  }
  propertySizes{
    sizeKind
    measureUnit
    amount
  }
  parkings {
    ratio 
    ratioPer 
    ratioPerUnit 
    parkingDetails {
      parkingType 
      parkingSpace 
      amount 
      interval
      currencyCode
    }
  }
  pointsOfInterests{
    interestKind 
    places{
      name
      type
      distances
      distanceUnits
      duration
      travelMode
      order
    }
  }
  transportationTypes{
    type
    places{
      name
      duration
      distanceUnits
      distances
      travelMode
      order
    }
  }
  users {
    id
    firstName
    lastName
    fullName
  }
  teams {
    name
    users {
      id
      firstName
      lastName
      fullName
    }
  }
  floorplans {
    id
    active
    displayText
    primary
    url
    watermark
    watermarkProcessStatus
    userOverride
    order
  }
  brochures {
    active
    displayText
    primary
    url
  }
  epcGraphs {
    active
    displayText
    primary
    url
  }
  highlights {
    order
    cultureCode
    text    
    miqId
  }

  microMarkets {
    order
    value
  }
  dataSource {
    dataSources
    other
  }
  photos {
    id
    active
    displayText
    primary
    url
    watermark
    watermarkProcessStatus
    userOverride
    order
  }
  specifications {
    contactBrokerForPrice
    showPriceWithUoM
    currencyCode
    leaseTerm
    leaseType
    leaseRateType
    maxPrice
    maxSpace
    salePrice
    measure
    minPrice
    minSpace
    totalSpace
    taxModifer
    bedrooms
    autoCalculateMinSpace
    autoCalculateTotalSpace
    autoCalculateMinPrice
    autoCalculateMaxPrice
    autoCalculateTotalPrice
  }
  microMarkets {
    order
    value
  }
  listingAssignment {
    assignedBy
    assignmentFlag
    assignedDate
  }
  spaces {
    id
    miqId
    availableFrom
    name {
      cultureCode
      text
    }
    spaceDescription {
      cultureCode
      text
    }
    spaceType
    status
    specifications {
      contactBrokerForPrice
      currencyCode
      leaseTerm
      leaseType
      maxPrice
      maxSpace
      measure
      minPrice
      minSpace
      totalSpace
      salePrice
    }
    photos {
      id
      active
      displayText
      primary
      order
      url
      watermark
      watermarkProcessStatus
      userOverride
    }
    floorplans {
      id
      active
      displayText
      primary
      order
      url
      watermark
      watermarkProcessStatus
      userOverride
    }
    brochures {
      active
      displayText
      primary
      url
    }
    video
    walkThrough
    spaceSizes {
      sizeKind
      amount
      measureUnit
    }
  }
  contacts {
    contactId
    firstName
    lastName
    location
    phone
    email
    avatar
    additionalFields {
      license
    }
  }
}`

function getListingDTO(listingData: Listing, config: Config, regionID?: string)
{
  const data: Listing = prepareListingData(listingData, config);
  const useLeaseRateType:boolean = config && config.featureFlags.leaseRateTypeEnabled;
  const saveDataSource:boolean = config && config.dataSource && config.dataSource.show;

  console.log(data.specifications)

  return {
    id: data.id,
    miqId: data.miqId,
    regionID: regionID ? regionID : "00000000-0000-0000-0000-000000000001",
    externalId: data.externalId ? data.externalId : null,
    published: data.published || null,
    propertyName: data.propertyName,
    propertyRecordName: data.propertyRecordName,
    configId: config && config.siteId || ``,
    propertyType: data.propertyType,
    propertySubType: data.propertySubType || ``,
    propertyUseClass: data.propertyUseClass || ``,
    listingType: data.listingType,
    street: data.street || ``,
    street2: data.street2 || ``,
    website: data.website || ``,
    operator: data.operator || ``,
    headline: Array.isArray(data.headline) ? data.headline : [],
    video: data.video || ``,
    walkThrough: data.walkThrough || ``,
    city: data.city || ``,
    stateOrProvince: data.stateOrProvince || ``,
    postalCode: data.postalCode ? String(data.postalCode) : ``,
    country: data.country ? String(data.country) : ``,
    lat: data.lat || null,
    lng: data.lng || null,
    floors: data.floors || null,
    yearBuilt: data.yearBuilt || null,
    energyRating: data.energyRating || ``,
    externalRatings: data.externalRatings || [],
    buildingDescription: Array.isArray(data.buildingDescription) ? data.buildingDescription : [],
    locationDescription: Array.isArray(data.locationDescription) ? data.locationDescription : [],
    status: data.status || ``,
    syndicationFlag: data.syndicationFlag || null,
    syndicationMarket: data.syndicationMarket || ``,
    availableFrom: data.availableFrom && data.availableFrom !== '' ? data.availableFrom : null,
    photos: data.photos,
    brochures: data.brochures,
    floorplans: data.floorplans,
    highlights: data.highlights,
    epcGraphs: data.epcGraphs,
    propertySizes: data.propertySizes,
    chargesAndModifiers: data.chargesAndModifiers,
    specifications: {
      contactBrokerForPrice: data.specifications.contactBrokerForPrice || false,
      showPriceWithUoM: data.specifications.showPriceWithUoM || false,
      leaseTerm: data.specifications.leaseTerm || ``,
      leaseType: data.specifications.leaseType || ``,
      leaseRateType: useLeaseRateType && data.specifications.leaseRateType || ``,
      minPrice: data.specifications.minPrice || null,
      maxPrice: data.specifications.maxPrice || null,
      minSpace: data.specifications.minSpace || null,
      maxSpace: data.specifications.maxSpace || null,
      totalSpace: data.specifications.totalSpace || null,
      salePrice: data.specifications.salePrice || null,
      measure: data.specifications.measure || ``,
      taxModifer: data.specifications.taxModifer || `None`,
      bedrooms: data.specifications.bedrooms ? parseInt(data.specifications.bedrooms, 10) : null,
      currencyCode: data.specifications.currencyCode || config && config.currencyCode || `USD`,
      autoCalculateMinSpace: data.specifications.autoCalculateMinSpace || false,
      autoCalculateTotalSpace: data.specifications.autoCalculateTotalSpace || false,
      autoCalculateMinPrice: data.specifications.autoCalculateMinPrice || false,
      autoCalculateMaxPrice: data.specifications.autoCalculateMaxPrice || false,
      autoCalculateTotalPrice: data.specifications.autoCalculateTotalPrice || false
    },
    microMarkets: Array.isArray(data.microMarkets) ? data.microMarkets : [],
    spaces: Array.isArray(data.spaces) ? data.spaces : [],
    contacts: Array.isArray(data.contacts) ? data.contacts : [],
    userNames: Array.isArray(data.userList) ? data.userList : [],
    parkings: data.parkings || null,
    pointsOfInterests: Array.isArray(data.pointsOfInterests) ? data.pointsOfInterests : [],
    transportationTypes: Array.isArray(data.transportationTypes) ? data.transportationTypes : [],
    teamNames: Array.isArray(data.teamList) ? data.teamList : [],
    aspects: Array.isArray(data.aspects) ? data.aspects : [],
    dataSource: {
      dataSources: saveDataSource && data.datasource.datasources || [],
      other: saveDataSource && data.datasource.other || ``
    },
    listingAssignment: data.listingAssignment || null
  }
}

// Format dates for save, stripping time and timezone. Datepicker tends to
// produce a datetime at local midnight converted to UTC, sometimes resulting
// in the wrong date. This function reverts back to local time to extract the
// calendar date, then passes the date as a UTC date at 11AM. 11AM is the
// same date in all timezones.
const prepareDate = (value: string) => {
  if (value) {
    return moment(value).local().format("YYYY-MM-DDT11:00:00");
  }
  return value;
}

const scrubMultiLangString = (value: MultiLangString[], config:Config):MultiLangString[] => {
  return Array.isArray(value) && value
    .filter(v => v.text && v.text !== '')
    .map((v, i):MultiLangString => {
      return {
        text: v.text || "",
        cultureCode: v.cultureCode || (config && config.defaultCultureCode) || ''
      }
    }) || [];
};

// prepare data that we receive before we send to the server
// this function is basically a final check on data and ensuring it is in a certain form to limit save errors, etc.
const prepareListingData = (listing:Listing, config:Config) => {

    let cultureCodeStr:string = "en-US";
    if (config && config.defaultCultureCode){
      cultureCodeStr = config.defaultCultureCode;
    }

    if (!listing.datasource){
      listing.datasource = {};
    }
    // TODO: Need to seprate with another method and get this logic implemented over there.
    if (config && config.addToTeam) {
      const teamList: Team[] = [];
      listing.teams = listing.teams ? listing.teams : [];
      const teamDetails = config.addToTeam.properties.find(t => t.propertyType.toLocaleLowerCase() === listing.propertyType.toLocaleLowerCase()
        && t.listingType.toLocaleLowerCase() === listing.listingType.toLocaleLowerCase());
      if (teamDetails) {
        const teams = teamDetails.teamName.split(',');
        teams.forEach((t: any) => {
          const isTeamExist = teamList.filter(tl => tl.name === t);
          if (isTeamExist.length === 0) {
            const newTeam: any = {
              name: t,
              id: "",
              users: []
            };
            teamList.push(newTeam);
          }
        });
      }
      listing.teams = teamList;
    }

    if (!(config && config.languages)){
      // convert any single string values to cultureCode array
      
      

      listing.headline = [];
      if (listing.headlineSingle){    
        listing.headline.push({ cultureCode: cultureCodeStr, text: listing.headlineSingle})
      }

      listing.buildingDescription = [];
      if (listing.buildingDescriptionSingle){    
        listing.buildingDescription.push({ cultureCode: cultureCodeStr, text: listing.buildingDescriptionSingle})
      }

      listing.locationDescription = [];
      if (listing.locationDescriptionSingle){    
        listing.locationDescription.push({ cultureCode: cultureCodeStr, text: listing.locationDescriptionSingle})
      }

      if(listing.spaces){
        listing.spaces.forEach((space:Space) => {
          space.name = [];
          if (space.nameSingle){    
            space.name.push({ cultureCode: cultureCodeStr, text: space.nameSingle})
          }

          space.spaceDescription = [];
          if (space.spaceDescriptionSingle){    
            space.spaceDescription.push({ cultureCode: cultureCodeStr, text: space.spaceDescriptionSingle})
          }
          
        });
      }

    }
    else
    {
      listing.headline = scrubMultiLangString(listing.headline, config);
      listing.buildingDescription = scrubMultiLangString(listing.buildingDescription, config);
      listing.locationDescription = scrubMultiLangString(listing.locationDescription, config);
      if(listing.spaces){
        listing.spaces.forEach((space:Space) => {
          if (space.nameSingle && listing.propertyType === 'flexOffice') {    
            space.name = [{ cultureCode: cultureCodeStr, text: space.nameSingle}];
          }
          else {
            space.name = scrubMultiLangString(space.name, config);
          }
          space.spaceDescription = scrubMultiLangString(space.spaceDescription, config);
        });
      }
    }

    // ensure files are sorted with primary always being 1 with an order of 1
    if(listing.photos){
      listing.photos = sortGLFiles(listing.photos);
    }
    if(listing.floorplans){
      listing.floorplans = sortGLFiles(listing.floorplans);
    }
    if(listing.spaces){
      listing.spaces.forEach((space:Space) => {
        if(space.photos){
          space.photos = sortGLFiles(space.photos);
        }
        if(space.floorplans){
          space.floorplans = sortGLFiles(space.floorplans);
        }

      });
    }


    // delete remnant object properties that breaks api
    const imageArrays: string[] = ['photos', 'floorplans', 'brochures', 'epcGraphs'];
  
    // specifications level 
    imageArrays.map(object => {
      if (listing[object] && listing[object].length > 0){
        listing[object].map((file: any) => {
          let fileExtension = '';
          if(file && file.name){
            fileExtension = file.name.substr(file.name.lastIndexOf('.'), file.name.length);
          }
          if (file.hasOwnProperty("errorDisplay")){
            delete file.errorDisplay;
          }
          if (file.hasOwnProperty("loadingMsg")){
            delete file.loadingMsg;
          }
          // special one off case for PDF
          if (fileExtension === ".pdf" && file.hasOwnProperty("userOverride")){
            delete file.userOverride;
          }
          if (object === "brochures" || object === "epcGraphs"){
            if(file.hasOwnProperty("userOverride")){
              delete file.userOverride;
            }
            if(file.hasOwnProperty("id")){
              delete file.id;
            }
          }
        });
      }
    });

    // highlight check (should be no blank highlights)
    if(listing.highlights){
       const scrubbedHighlights:any[] = [];
       listing.highlights.forEach((highlight:any) => {
          if(highlight && highlight.value && highlight.value.length > 0){
            let highlightOrder:number | undefined = highlight.order
            if(!highlight.order && highlight.id && typeof highlight.id === "string"){
              highlightOrder = Number(highlight.id);
              delete highlight.id;
            }
            if(highlightOrder && typeof highlightOrder === "string"){
              highlightOrder = Number(highlight.order);
            }
            highlight.value.forEach((multiLangEntry:MultiLangString) => {
              if(multiLangEntry.text && multiLangEntry.text.trim().length > 0){
                scrubbedHighlights.push({
                  order: highlightOrder, 
                  text: multiLangEntry.text, 
                  cultureCode: multiLangEntry.cultureCode,
                  miqId: multiLangEntry.miqId
                });         
              }     
            });
          }  
       });
       listing.highlights = [...scrubbedHighlights];
    }

    // clear out empty string distances
    if (listing.pointsOfInterests && isArray(listing.pointsOfInterests)){
      listing.pointsOfInterests.forEach((poi: any, index: number) => {
        if (poi.places){
          poi.places.forEach((place: any, i: number) => {
            if (place.distances == null || place.distances.length === 0){  // tslint:disable-line
              listing.pointsOfInterests[index].places[i].distances = null;  // tslint:disable-line
            }
            if (place.duration == null || place.duration.length === 0){  // tslint:disable-line
              listing.pointsOfInterests[index].places[i].duration = null;  // tslint:disable-line
            }
          })
        }
      })
    }
    if (listing.transportationTypes && isArray(listing.transportationTypes)){
      listing.transportationTypes.forEach((poi: any, index: number) => {
        if (poi.places){
          poi.places.forEach((place: any, i: number) => {
            if (place.distances == null || place.distances.length === 0){  // tslint:disable-line
              listing.transportationTypes[index].places[i].distances = null;  // tslint:disable-line
            }
            if (place.duration == null || place.duration.length === 0){  // tslint:disable-line
              listing.transportationTypes[index].places[i].duration = null;  // tslint:disable-line
            }
          })
        }
      })
    }

    // contacts : handle cleaning out temp ids for creates
    for (const contact of listing.contacts) {
      if(contact.tempId && contact.tempId === true){
        delete contact.tempId;
        delete contact.contactId;
      }
    }
    if (!listing.chargesAndModifiers){
      const type = "chargesAndModifiers"
      listing[type] = []
    }

    

    // grab the field config given the property type and listing type
    const spaceFieldSetups:SpacesListingTypeFields | undefined = findSpacesFields(config, listing.propertyType, listing.listingType);

    if(spaceFieldSetups){
        // resolve spaces data
      const spaces: Space[] = [];
      if (listing.spaces && listing.spaces.length > 0) {
        listing.spaces.map((space: any) => {
          // first, strip out any values that don't belong to this property/type listing type combo to kill stale values that might be sticking around
          const spaceLevelAllowedFields:string[] = ["id","miqId","specifications","photos","floorplans","brochures", "video", "walkThrough"]; // initialize with any "hidden" fields not a part of the config
          const specsAllowedFields:string[] = ["currencyCode"];

          // go through the field setups and find only the fields that we should include (NON specifications level)
          Object.keys(spaceFieldSetups).forEach((fieldSetupName:string) => {
              const fieldSetup:SpacesFieldSetup<any> = spaceFieldSetups[fieldSetupName];
              if(fieldSetup && fieldSetup.show && fieldSetup.properties && fieldSetup.properties.hasOwnProperty("name")){
                
                  if(fieldSetup.properties.name.indexOf("specifications") === -1){
                      // space level because the name doesnt include specifications
                      if (fieldSetup.properties.hasOwnProperty("dbName")){
                        spaceLevelAllowedFields.push(fieldSetup.properties.dbName);
                      }
                      else {
                        spaceLevelAllowedFields.push(fieldSetup.properties.name);
                      }                   
                  }else if(fieldSetup.properties.name.indexOf("specifications") > -1){
                      // specifications level
                      const specsName = fieldSetup.properties.name.replace("specifications.","");
                      specsAllowedFields.push(specsName)
                  }
              }
          });
          // now strip out any fields that aren't allowed
          Object.keys(space).forEach((propertyName:string) => { 
            if(spaceLevelAllowedFields.indexOf(propertyName) === -1){
              space[propertyName] = null;
            }
          });
          // now we have to do the same thing at the specifications level for the space
          Object.keys(space.specifications).forEach((propertyName:string) => { 
            if(specsAllowedFields.indexOf(propertyName) === -1){
              space.specifications[propertyName] = null;
            }
          });

          // ensure space data is setup properly
          const scrubbedSpace: any = {
            id: space.id ? space.id : null,
            miqId: space.miqId ? space.miqId : null,
            availableFrom: space.availableFrom && space.availableFrom !== '' ? prepareDate(space.availableFrom) : null,
            name: space.name ? space.name : [],
            spaceDescription: space.spaceDescription ? space.spaceDescription : [],
            spaceType: space.spaceType ? space.spaceType : "",
            status: space.status ? space.status : "",
            specifications: {
              leaseTerm: space.specifications.leaseTerm ? space.specifications.leaseTerm : "",
              leaseType: space.specifications.leaseType ? space.specifications.leaseType : "",
              measure: space.specifications.measure ? space.specifications.measure : "",
              minPrice: space.specifications.minPrice ? space.specifications.minPrice : null,
              maxPrice: space.specifications.maxPrice ? space.specifications.maxPrice : null,
              minSpace: space.specifications.minSpace ? space.specifications.minSpace : null,
              maxSpace: space.specifications.maxSpace ? space.specifications.maxSpace : null,
              totalSpace: space.specifications.totalSpace ? space.specifications.totalSpace : null,
              salePrice: space.specifications.salePrice ? space.specifications.salePrice : null,
              contactBrokerForPrice: space.specifications.contactBrokerForPrice ? space.specifications.contactBrokerForPrice : null,
              currencyCode: (listing.specifications && listing.specifications.currencyCode) ? listing.specifications.currencyCode : config && config.currencyCode ? config.currencyCode : "USD"
            },
            photos: space.photos ? space.photos : [],
            floorplans: space.floorplans ? space.floorplans : [],
            brochures: space.brochures ? space.brochures : [],
            video: space.video ? space.video : "",
            walkThrough: space.walkThrough ? space.walkThrough : "",
            spaceSizes: space.spaceSizes ? space.spaceSizes : []
          }

          // resolve image issues
          imageArrays.map(imageArr => {
            if (scrubbedSpace[imageArr] && scrubbedSpace[imageArr].length > 0){
              scrubbedSpace[imageArr].map((spaceFile: any) => {
                if (spaceFile.hasOwnProperty("errorDisplay")){
                  delete spaceFile.errorDisplay;
                }
                if (spaceFile.hasOwnProperty("loadingMsg")){
                  delete spaceFile.loadingMsg;
                }
                if(imageArr === "brochures" && spaceFile.hasOwnProperty("id")){
                  delete spaceFile.id;
                }
                if(imageArr === "brochures" && spaceFile.hasOwnProperty("userOverride")){
                  delete spaceFile.userOverride;
                }
              });
            }
          });
          spaces.push(scrubbedSpace);
        });
      }
      listing.spaces = spaces;
    }else{
      listing.spaces = [];
    }

    // user list
    if(listing.users){
      const userList:string[] = [];
      listing.users.forEach((user:any) => {
        userList.push(user.email);
      });   
      listing.userList = userList;
    }

    // team list
    if(listing.teams){
      const teamList:string[] = [];
      listing.teams.forEach((team:any) => {
        teamList.push(team.name);
      });
      listing.teamList = teamList;
    }

    // this is a TEMPORARY solution to avoid saving issues.  REMOVE this when the validations have been properly added to address this
    if(listing.photos && listing.photos.length > 0){
        const photos:GLFile[] = [];
        listing.photos.forEach((file:GLFile) => {
          if(file && file.id && file.id > 0){
            photos.push(file);
          }
        });
        listing.photos = photos;
    }

    if(listing.floorplans && listing.floorplans.length > 0){
        const floorplans:GLFile[] = [];
        listing.floorplans.forEach((file:GLFile) => {
          if(file && file.id && file.id > 0){
            floorplans.push(file);
          }
        });
        listing.floorplans = floorplans;
    }

    if(listing.spaces && listing.spaces.length > 0){

        const spaces:Space[] = [];
        listing.spaces.forEach((space:Space) => {
            const spacePhotos:GLFile[] = [];
            space.photos.forEach((file:GLFile) => {
              if(file && file.id && file.id > 0){
                spacePhotos.push(file);
              }
            });
            space.photos = spacePhotos;

            const spaceFloorplans:GLFile[] = [];
            space.floorplans.forEach((file:GLFile) => {
              if(file && file.id && file.id > 0){
                spaceFloorplans.push(file);
              }
            });
            space.floorplans = spaceFloorplans;
            spaces.push(space);
        });
        listing.spaces = spaces;
    } 

    // END TEMPORARY SOLUTION TO REMOVE

    // specifications : ensure that we are only passing thed data we should be based on property type/listing type
    const fieldSetups:SpecsListingTypeFields | undefined = findSpecificationFields(config,listing.propertyType,listing.listingType);

    if(fieldSetups){
        const allowedFields:string[] = ["currencyCode","taxModifer"]; // initialize with any "hidden" fields not a part of the config
        // now we have the field setups, but we need to go through the list and create a hash to know which properties we have available
        Object.keys(fieldSetups).forEach((fieldSetupName:string) => {
            const fieldSetup:SpecsFieldSetup<any> = fieldSetups[fieldSetupName];
            if(fieldSetup && fieldSetup.show && fieldSetup.properties && fieldSetup.properties.hasOwnProperty("name")){
                allowedFields.push(fieldSetup.properties.name);
            }
        });

        // now strip out any fields that aren't allowed
        Object.keys(listing.specifications).forEach((propertyName:string) => { 
          if(allowedFields.indexOf(propertyName) === -1){
              listing.specifications[propertyName] = null;
          }
        });
    }

    if(!listing.externalRatings || listing.externalRatings){
      listing.externalRatings = [];
    }

    /*Convering the energy rating fields to an array of objects*/
    if (listing.wellRating || listing.energyRating || listing.leedRating || listing.epcRating) {
      listing.externalRatings.push({ ratingType: "WELL", ratingLevel: listing.wellRating})
      listing.externalRatings.push({ ratingType: "LEED", ratingLevel: listing.leedRating})
      listing.externalRatings.push({ ratingType: "BREEAM", ratingLevel: listing.energyRating})
      listing.externalRatings.push({ ratingType: "EPC", ratingLevel: listing.epcRating})
      
      delete listing.wellRating;
      delete listing.leedRating;
      delete listing.epcRating;
    }
    return listing;
}
