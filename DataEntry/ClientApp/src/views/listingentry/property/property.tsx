import React, { FC, useContext, useState } from 'react';
import GLForm from '../../../components/form/gl-form';
import { convertValidationJSON } from '../../../utils/forms/validation-adapter';
import FormInput, { FormInputProps } from '../../../components/form-input/form-input';
import FormSelect, { FormSelectProps } from '../../../components/form-select/form-select';
import FormUpload, { FormUploadProps } from '../../../components/form-upload/form-upload';
import SectionHeading from "../../../components/section-heading/section-heading";
import FormRadioGroup, { FormRadioGroupProps } from '../../../components/form-radiobutton/form-radiogroup';
import FormTextArea, { FormTextAreaProps } from '../../../components/form-text-area/form-text-area';
import FormTabbedTextArea, { FormTabbedTextAreaProps } from '../../../components/form-tabbed-text-area/form-tabbed-text-area';
import FormDateField, { FormDateFieldProps } from '../../../components/form-date-field/form-date-field';
import { Listing } from '../../../types/listing/listing';
import { Col, Row } from 'react-styled-flexboxgrid'
import GLField from '../../../components/form/gl-field';
import { FormContext } from '../../../components/form/gl-form-context';
import { fieldCheckStrVal } from '../../../utils/forms/field-check';
import { AddressFields } from './partials/address-fields';
import { Address } from '../../../types/listing/address';
import { useSelector } from 'react-redux';
import { countrySelector } from '../../../redux/selectors/mapping/country-selector';
import { updateRenderTrigger } from '../render-trigger-util';
import listingTypes from '../../../api/lookups/listingTypes';
import cloneDeep from 'clone-deep';
import { configSelector } from '../../../redux/selectors/system/config-selector';
import { Config, FieldConfig, PropertyTypeFieldConfig, AvailableFromConfig, StatusConfig, EnergyRatingConfig, EpcRatingConfig, LeedRatingConfig, WellRatingConfig, SyndicationMarketConfig, EpcGraphsConfig } from '../../../types/config/config';
import { PropertyTypeOption, Option } from '../../../types/common/option';
import { WatermarkProcessStatus } from '../../../types/listing/file';
import HelpfulTextWalkthrough from '../../../components/helpful-text-walkthrough/helpful-text-walkthrough';
import styled from 'styled-components';

interface Props {
    listing: Listing,
    placesAPI: string,
    forceValidate: boolean,
    showErrors: boolean,
    validations: object,
    manualErrors: object,
    nextFocus: string,
    imageProcessingCheck: (valuesToUpdate: object) => void
}

const Property: FC<Props> = (props) => {

    const { placesAPI, listing, forceValidate, showErrors, validations, manualErrors, nextFocus, imageProcessingCheck } = props;

    const apiCountry = useSelector(countrySelector);
    const config: Config = useSelector(configSelector);
    const [currentValues, setCurrentValues] = useState(listing);    // this piece of state feeds the form it's values

    // because of how values bubble up from the form and the searchable component, we need to maintain 
    // a temporary variable for the street (line 1) if it changes without an address selection
    let tempStreet:string | null = null;    

    // value change interceptor
    const formControllerContext = useContext(FormContext);

    let updateObj: Listing = Object.assign({},listing);     // this object is keeping track of all current changes between state changes

    // error messages
    const photosKey = 'photos';
    const floorplansKey = 'floorplans';
    let photosErrorMessage:string = '';
    let floorplansErrorMessage:string = '';

    if(manualErrors && manualErrors[photosKey] && manualErrors[photosKey].error === true){
        photosErrorMessage = manualErrors[photosKey].message; 
    }

    

    if(manualErrors && manualErrors[floorplansKey] && manualErrors[floorplansKey].error === true){
        floorplansErrorMessage = manualErrors[floorplansKey].message; 
    }

    const checkAddToTeam = (propertyType: any, listingType: string) => {
        if (!config.addToTeam) {
            return;
        }

        if(currentValues.propertyType !== propertyType || currentValues.listingType !== listingType){
            updateObj.teams = [];
        }
        const team = config.addToTeam.properties.find(t => t.propertyType.toLocaleLowerCase() === propertyType.toLocaleLowerCase()
            && t.listingType.toLocaleLowerCase() === listingType.toLocaleLowerCase());
        const teamList: any[] = [];
        updateObj.teams = updateObj.teams ? updateObj.teams : [];
        if (team) {
            const teams = team.teamName.split(',');
            teams.forEach((t:string) => {
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
        updateObj.teams = teamList;
    }

    const updatePhotoPayload = (photoPayload: any) => {
        
        updateObj.photos = photoPayload;
        
        if(imageProcessingCheck){
            imageProcessingCheck(updateObj);
        }else{
            changeHandler({});
        }
    }

    const updateFloorplanPayload = (floorplanPayload: any) => {

        updateObj.floorplans = floorplanPayload;

        if(imageProcessingCheck){
            imageProcessingCheck(updateObj);
        }else{
            changeHandler({});
        }
    }

    const updateBrochurePayload = (brochurePayload: any) => {
        updateObj.brochures = brochurePayload;
        changeHandler({});
    }

    const updateEpcGraphPayload = (epcGraphPayload: any) => {
        updateObj.epcGraphs = epcGraphPayload;
        changeHandler({});
    }

    const updateHeadlinePayload = (headlinePayload: any) => {
        updateObj.headline = headlinePayload;       
        changeHandler({});
    }

    const updateBuildingDescriptionPayload = (buildingDescPayload: any) => {
        updateObj.buildingDescription = buildingDescPayload;       
        changeHandler({});
    }

    const updateLocationDescriptionPayload = (locationDescPayload: any) => {
        updateObj.locationDescription = locationDescPayload;       
        changeHandler({});
    }

    const scrubChangeObj = (values: any) => {
        const objCopy = cloneDeep(values);
        const spaces = "spaces";
        const specifications = "specifications";
        const contacts = "contacts";
        const highlights = "highlights";
        const users = "users";
        const teams = "teams";

        if (objCopy[spaces]) {
            delete objCopy[spaces];
        }
        if (objCopy[specifications]) {
            delete objCopy[specifications];
        }
        if (objCopy[contacts]) {
            delete objCopy[contacts];
        }
        if (objCopy[highlights]) {
            delete objCopy[highlights];
        }
        if (objCopy[users]) {
            delete objCopy[users];
        }
        // if (objCopy[teams]) {
        //     delete objCopy[teams];
        // }
        return objCopy;
    }

    const changeHandler = (values?: any) => {

        // for the photos, floorplans, and brochures, we want to make sure the object has the latest changes from our update object
        // if we don't do this, then the values for these files will get lost when the user makes a form change
        if (values.propertyType && values.listingType) {
            checkAddToTeam(values.propertyType, values.listingType);
        }
        values.photos = updateObj.photos;
        values.floorplans = updateObj.floorplans;
        values.brochures = updateObj.brochures;
        values.buildingDescription = updateObj.buildingDescription;
        values.locationDescription = updateObj.locationDescription;
        values.headline = updateObj.headline;
        values.epcGraphs = updateObj.epcGraphs;
        values.teams = updateObj.teams;
        
        if (values.microMarket) {
            values.microMarkets = [{ 'value': values.microMarket, 'order': 1 }];
        }
        // if the user has entered a street during this session, then we need to ensure the value overrides here
        if(tempStreet !== null){
            values.street = tempStreet;
        }

        // because we are working with the full listing here, we need to delete any properties that this portion is not responsible for 
        // before bubbling it up. we may want to re-think this approach, for now this is a bandaid.
        values = scrubChangeObj(values);
        updateObj = values;
        formControllerContext.onFormChange(values);

        if(values && values.country && values.country.length > 0 && values.country !== currentValues.country){
            setCurrentValues(updateObj);
        }
        
        if (values && values.teams && values.teams.length > 0 && values.propertyType && 
            (values.propertyType !== currentValues.propertyType || values.listingType !== currentValues.listingType)) {
            setCurrentValues(updateObj);
        }
    }

    // when the user uses an API to change the address, this is called
    // the broken up address is passed
    const remoteAddressSelected = (address: Address) => {
        let changed = {};
        Object.keys(address).forEach(key => {
            changed[key] = address[key];
        });
        changed = updateRenderTrigger(changed, "addressChange");
        changeHandler(changed);
    }

    const streetChanged = (value: string | null) => {
         tempStreet = value;
         changeHandler(updateObj);
    }

    const getPropertyType = () => {
        if (config && config.propertyType && config.propertyType.show) {
            const fieldConfig: PropertyTypeFieldConfig<PropertyTypeOption> = config.propertyType;
            return (
                <Col xs={(config.propertySubType && config.propertySubType.show) || (config.propertyUseClass && config.propertyUseClass.show) ? 6 : 12}>
                    <GLField<FormSelectProps> name="propertyType" label={fieldConfig.label} use={FormSelect} options={fieldConfig.options} doAlphabeticalSort={fieldConfig.alphabeticalSort}
                        prompt="Select Property Type..." forceFocus={nextFocus === "propertyType" ? true : false} />
                </Col>
            );
        }
        return <></>;
    }

    const currentProperty = listing.propertyType;

    const showPropertySubType = () => {
        let value = false;
        if (config && listing) {
            config.propertyType.options.map(type => {
                if (type && type.subPropertyType && type.subPropertyType === currentProperty) {
                    value = true
                }
            })
        }
        return value;
    }

    const showPropertyUseClass = () => {
        let value = false;
        if (config && listing) {
            config.propertyType.options.map(type => {
                if (type && type.useClass && type.useClass === currentProperty) {
                    value = true
                }
            })
        }
        return value;
    }

    const getPropertySubType = () => {
        if (!config || !listing || !config.propertySubType[currentProperty]) {
            return []
        } else {
            return config.propertySubType[currentProperty]
        }
    }

    const getPropertyUseClass = () => {
        if (!config || !listing || !config.propertyUseClass[currentProperty]) {
            listing.propertyUseClass = "";
            return []
        } else {
            return config.propertyUseClass[currentProperty]
        }
    }

    const getListingTypes = () => {
        if (config && config.listingTypes) 
        {
            return config.listingTypes.options;          
        }
        else
        {
            return listingTypes;
        }
    }

    const getMicroMarkets = () => {
        if (config && config.microMarkets && config.microMarkets.show) {
            return <Col xs={12}><GLField name="microMarket" label={config.microMarkets.label} use={FormSelect} options={config.microMarkets.options} prompt="Select Micro Market..." /></Col>;
        }
        return <></>
    }

    const getSyndicationMarket = () => 
    {
        if (config && config.syndication && config.syndication.show && config.syndication.markets && currentProperty) {
            const marketOptions = config.syndication.markets.find((market)=> market.propertyType === currentProperty );
            if (marketOptions && marketOptions.options)
            {
                const colSize = marketOptions && marketOptions.grid.colSize ? marketOptions.grid.colSize : 12;
                const fieldConfig: SyndicationMarketConfig  = marketOptions;
                return (
                    <Col xs={colSize} style={{ paddingLeft: '0' }}>
                        <GLField<FormSelectProps> name="syndicationMarket" use={FormSelect} {...fieldConfig} />
                    </Col>
                );
            }
            return <></>;
        }
        return <></>;
    }

    const getEneryRating = () => {
        if (config && config.energyRating && config.energyRating.show) {
            const colSize = config.energyRating.grid && config.energyRating.grid.colSize ? config.energyRating.grid.colSize : 12;
            const fieldConfig: EnergyRatingConfig = config.energyRating;
            return (
                <Col xs={colSize}>
                    <GLField<FormSelectProps> name="energyRating" use={FormSelect} {...fieldConfig} />
                </Col>
            );
        }
        return <></>;
    }

    const getEpcRating = () => {
        if (config && config.epcRating && config.epcRating.show) {
            const colSize = config.epcRating.grid && config.epcRating.grid.colSize ? config.epcRating.grid.colSize : 12;
            const fieldConfig: EpcRatingConfig = config.epcRating;
            return (
                <Col xs={colSize}>
                    <GLField<FormSelectProps> name="epcRating" use={FormSelect} {...fieldConfig} />
                </Col>
            );
        }
        return <></>;
    }

    const getLeadRating = () => {
        if (config && config.leedRating && config.leedRating.show) {
            const colSize = config.leedRating.grid && config.leedRating.grid.colSize ? config.leedRating.grid.colSize : 12;
            const fieldConfig: LeedRatingConfig = config.leedRating;
            return (
                <Col xs={colSize}>
                    <GLField<FormSelectProps> name="leedRating" use={FormSelect} {...fieldConfig} />
                </Col>
            );
        }
        return <></>;
    }

    const getWellRating = () => {
        if (config && config.wellRating && config.wellRating.show) {
            const colSize = config.wellRating.grid && config.wellRating.grid.colSize ? config.wellRating.grid.colSize : 12;
            const fieldConfig: WellRatingConfig = config.wellRating;
            return (
                <Col xs={colSize}>
                    <GLField<FormSelectProps> name="wellRating" use={FormSelect} {...fieldConfig} />
                </Col>
            );
        }
        return <></>;
    }

    const getEpcGraphs = () => {
        if (!(config && config.epcGraphs && config.epcGraphs.show)) {
            return <></>;
        }

        const fieldConfig: EpcGraphsConfig = config.epcGraphs;

        return (
            <Row style={{ width: '174%' }}>
                <Col xs={12}>
                    <GLField<FormUploadProps> 
                        name="epcGraphs" 
                        use={FormUpload}
                        files={listing.epcGraphs} 
                        updatePhotoPayload={updateEpcGraphPayload}  
                        singleUpload={true} 
                        {...fieldConfig} />
                </Col>
            </Row>
        )
    }

    const getStatus = () => {
        if (config && config.status && config.status.show) {
            const colSize = config.status.grid && config.status.grid.colSize ? config.status.grid.colSize : 12;
            const fieldConfig: StatusConfig = config.status;
            if (fieldConfig.defaultValue && (!currentValues.status || currentValues.status === "")) 
            {
                updateObj.status = fieldConfig.defaultValue;
                setCurrentValues(updateObj);
            }
            return (
                <Col xs={colSize}>
                    <GLField<FormSelectProps> name="status" use={FormSelect} {...fieldConfig} />
                </Col>
            );
        }
        return <></>;
    }

    const getAvailability = () => {
        if (config && config.availableFrom && config.availableFrom.show) {
            const colSize = config.availableFrom.grid && config.availableFrom.grid.colSize ? config.availableFrom.grid.colSize : 12;
            const fieldConfig: AvailableFromConfig = config.availableFrom;
            return (
                <Col xs={colSize} id="availableFrom">
                    <GLField<FormDateFieldProps> name="availableFrom"  use={FormDateField} {...fieldConfig} />
                </Col>
            );
        }
        return <></>;
    }

    const getTextFieldLangSettings = (placeholderName:string) => {
        let langSettings:any = [];
        if (config && config.languages) {
            let translations:any[] = [];
            if (config.translations){
                translations = Array.from(config.translations);          
            }

            const langs:string[] = Array.from(config.languages);
            langSettings = Array.from({ length: langs.length }, (v, idx) => idx).map(idx => {
                let languageName = langs[idx];
                let placeholder = "Enter Text...";
                if (translations.length>0){
                    const itemData = translations.filter((d: any) => {
                        return d.cultureCode === langs[idx];
                      });
                      if (itemData.length > 0) {
                        languageName = itemData[0].languageName;
                        placeholder = itemData[0][placeholderName];
                      }
                }

                const settings: any = {
                    cultureCode: `${langs[idx]}`,
                    lang: `${languageName}`,
                    placeholder: `${placeholder}`
                };
                return settings;
            });
        }
        return langSettings;
    }

    const headlineUI = () => {
        if (config && config.languages) {
            return (
                <GLField<FormTabbedTextAreaProps> name="headline" data={listing.headline} tabsettings={getTextFieldLangSettings("plHeadline")} placeholder="Headline" label="Headline" updateTabData={updateHeadlinePayload} use={FormTabbedTextArea} />
            );
        }
        return (
            <GLField<FormInputProps> name="headlineSingle" placeholder="e.g. 40,000 SF Office for Lease in Downtown Annapolis" 
                label={(config && config.headlineSingle && config.headlineSingle.label) ? config.headlineSingle.label : "Headline"} 
                use={FormInput} forceFocus={nextFocus === "headlineSingle" ? true : false} />
            );
        }

    const buildDescriptionUI = () => {
        if (config && config.languages) {
            return (
                <GLField<FormTabbedTextAreaProps> name="buildingDescription" data={listing.buildingDescription} tabsettings={getTextFieldLangSettings("plBuildingDescription")} placeholder="Property Description" label="Property Description" updateTabData={updateBuildingDescriptionPayload} use={FormTabbedTextArea} />
            );
        }
        return <GLField<FormTextAreaProps> name="buildingDescriptionSingle" placeholder="Building Description" label="Building Description" use={FormTextArea} />;
    }

    const locationDescriptionUI = () => {
        if (config && config.languages) {
            return (
                <GLField<FormTabbedTextAreaProps> name="locationDescription" data={listing.locationDescription} tabsettings={getTextFieldLangSettings("plLocationDescription")} placeholder="Location Description" label="Location Description" updateTabData={updateLocationDescriptionPayload} use={FormTabbedTextArea} />
            );
        }
        return <GLField<FormTextAreaProps> name="locationDescriptionSingle" placeholder="Location Description" label="Location Description" use={FormTextArea} />;
    }

    // temporary
    const watemarkProcessStatusOptions:Option[] = [
        {label: "No Watermark", value: WatermarkProcessStatus.NO_WATERMARK, order: 1},
        {label: "Normal Watermark", value: WatermarkProcessStatus.WATERMARK, order: 2},
        {label: "CRE Watermark", value: WatermarkProcessStatus.CRE_WATERMARK, order: 3},
        {label: "Error", value: WatermarkProcessStatus.WATERMARK_ERROR, order: 4},
    ];
    const [photoStatus, setPhotoStatus] = useState<WatermarkProcessStatus>(WatermarkProcessStatus.NO_WATERMARK);
    const [floorplansStatus, setFloorplansStatus] = useState<WatermarkProcessStatus>(WatermarkProcessStatus.NO_WATERMARK);

    const changePhotoStatus = (newStatus:WatermarkProcessStatus) => {
        setPhotoStatus(newStatus);
    }

    const changeFloorplanStatus = (newStatus:WatermarkProcessStatus) => {
        setFloorplansStatus(newStatus);
    } 

    return (       
        <GLForm initVals={currentValues}
            validationAdapter={convertValidationJSON}
            validationJSON={validations}
            changeHandler={changeHandler}
            forceValidate={forceValidate}
            showErrors={showErrors}>
            <Row id="property">
                <Col xs={12} style={{marginTop: '8px', marginBottom: '-18px'}}><SectionHeading>Property</SectionHeading></Col>
            </Row>
            <Row>
                <Col xs={12}>
                    <Row between="sm">
                        {getPropertyType()}
                        {config && config.propertySubType && config.propertySubType.show &&
                            <Col xs={6}>
                                <GLField<FormSelectProps> name="propertySubType" label="Property Subtype" use={FormSelect} options={getPropertySubType()}
                                    prompt="Select Property Subtype..." forceFocus={nextFocus === "propertyType" ? true : false} disabled={!showPropertySubType()} />
                            </Col>
                        }
                        {config && config.propertyUseClass && config.propertyUseClass.show && 
                            <Col xs={6}>
                                <GLField<FormSelectProps> name="propertyUseClass" label={config.propertyUseClass.label} use={FormSelect} options={getPropertyUseClass()}
                                    prompt="Select Use Class..." forceFocus={nextFocus === "propertyType" ? true : false} disabled={!showPropertyUseClass()} />
                            </Col>
                        }
                    </Row>
                    <Row>
                        <Col xs={12}>
                            <GLField<FormRadioGroupProps> name="listingType" label="Listing Type*" options={getListingTypes()} use={FormRadioGroup}
                                forceFocus={nextFocus === "listingType" ? true : false} />
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={12}><GLField<FormInputProps> name="propertyRecordName" subText="For internal records only, will not display on cbre.com" label="Property Record Name*" placeholder="Property Record Name"  use={FormInput} />
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={12}>
                            <GLField<FormInputProps> name="propertyName" toolTip="This will display with your address.We suggest using when there is a building name associated with the address or listing/availability." placeholder="e.g. San Jacinto Tower" label="Building Display Name" use={FormInput} />
                        </Col>
                    </Row>
                    <AddressFields changeHandler={remoteAddressSelected} streetChanged={streetChanged} placesAPI={placesAPI} apiCountry={apiCountry} currentCountryCode={currentValues.country}/>
                    <Row>
                        <Col xs={12}>{getSyndicationMarket()}</Col>
                    </Row>
                    <Row>
                        <Col xs={12}>{headlineUI()}</Col>
                        {/* <Col xs={12}><GLField<FormTabbedTextAreaProps> name="buildingDescription" tabdata={listing.photos} tabsettings={listing.buildingDescription} placeholder="Property Description" label="Property Description" use={FormTabbedTextArea} /></Col> */}
                        <Col xs={12}>{buildDescriptionUI()}</Col>
                        <Col xs={12}>{locationDescriptionUI()}</Col>
                        {getStatus()}
                        {getAvailability()}
                        {getEpcRating()}
                        {getLeadRating()}
                        {getEneryRating()}
                        {getWellRating()}
                        <Col xs={12}><GLField<FormInputProps> name="website" toolTip="Please leave field blank if you do not have a dedicated property listing URL." placeholder="e.g. www.yourpropertybuildingsite.com" label="Property Website" use={FormInput} /></Col>
                        {getMicroMarkets()}
                    </Row>
                    {fieldCheckStrVal(listing.propertyType, ['flex']) &&
                        <Row between="sm">
                            <Col xs={12}><GLField<FormInputProps> name="operator" placeholder="Operator" label="Operator" use={FormInput} /></Col>
                        </Row>
                    }
                </Col>
            </Row>
            <Row style={{ width: '174%' }}>
                <Col xs={12}>
                    <GLField<FormUploadProps> showPrimary={true} name="photos" title="Photos*" label="Add Photo"
                        manualError={photosErrorMessage && photosErrorMessage.length > 0 ? true : false} manualErrorMessage={photosErrorMessage}
                        accepted=".jpg, .png, .jpeg" description="Recommended size 1600 x 1200 px (JPEG or PNGs only)" watermark={config.watermark}
                        files={listing.photos} allowWatermarkingDetect={true} useRestb={true} defaultWaterMarkTrue={config.featureFlags.defaultWaterMarkTrue}
                        updatePhotoPayload={updatePhotoPayload} use={FormUpload} />
                </Col>
            </Row>
            <Row style={{ width: '174%' }}>
                <Col xs={12}>
                    <GLField<FormUploadProps> showPrimary={false} name="floorplans" title="Floorplan" label="Add Floorplan"
                        manualError={floorplansErrorMessage && floorplansErrorMessage.length > 0 ? true : false} manualErrorMessage={floorplansErrorMessage}
                        accepted=".jpg, .png, .jpeg, .pdf" description="JPEG, PNG or PDF" watermark={config.watermark}
                        files={listing.floorplans} allowWatermarkingDetect={false} useRestb={true} defaultWaterMarkTrue={config.featureFlags.defaultWaterMarkTrue}
                        updatePhotoPayload={updateFloorplanPayload} use={FormUpload} />
                </Col>
            </Row>
            <Row style={{ width: '174%' }}>
                <Col xs={12}>
                    <GLField<FormUploadProps> showPrimary={false} name="brochures" title="Brochure" label="Add Brochure"
                        accepted=".pdf" description="PDF only"
                        files={listing.brochures} allowWatermarkingDetect={false} updatePhotoPayload={updateBrochurePayload} use={FormUpload} />
                </Col>
            </Row>
            {getEpcGraphs()}
            <Row>
                <Col xs={12}><GLField<FormInputProps> name="video" label="Video URL" placeholder="e.g. https://cbre.qumucloud.com/view/your-availbility"  use={FormInput} /></Col>
            </Row>
            <Row style={{ width: '174%' }}>
                <Col xs={7}>
                    <GLField<FormInputProps> name="walkThrough" label="3D/Virtual Tour URL" placeholder="e.g. https://my.matterport.com/show/?m=your-availbility"  use={FormInput} />
                </Col>
                <Col xs={5} style={{ marginTop: "3.75em" }}>
                    <OnDesktopHelpfulTextWalkthrough>
                        <HelpfulTextWalkthrough/>
                    </OnDesktopHelpfulTextWalkthrough>
                </Col>
            </Row>
            <OnTabletHelpfulTextWalkthrough>
                <Row style={{ width: '174%', marginTop: "2em" }}>
                    <Col xs={12}>
                        <HelpfulTextWalkthrough/>
                    </Col>
                </Row>
            </OnTabletHelpfulTextWalkthrough>
            {config && config.floorsField && config.floorsField.show &&
                <Col xs={12} style={{ paddingLeft: '0' }}>
                    <GLField<FormInputProps> name="floors" title="Number of floors" mask="9[9]" numericOnly={true} placeholder={'0'} use={FormInput} />
                </Col>
            }
            {config && config.yearField && config.yearField.show &&
                <Col xs={12} style={{ paddingLeft: '0' }}>
                    <GLField<FormInputProps> name="yearBuilt" title="Year Built" mask="9[9][9][9]" numericOnly={true} placeholder={'Year Built'} use={FormInput} />
                </Col>
            }
        </GLForm>
    );
};

const OnTabletHelpfulTextWalkthrough = styled.div`
   @media (max-width: 1025px) { display: inline; }
   @media (min-width: 1025px) { display: none; }
`;

const OnDesktopHelpfulTextWalkthrough = styled.div`
   @media (max-width: 1025px) { display: none; }
   @media (min-width: 1025px) { display: inline; }
`;

export default React.memo(Property, (prevProps, nextProps) => nextProps.listing !== prevProps.listing);