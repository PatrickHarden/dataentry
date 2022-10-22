import React, { FC, useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import FormController from '../../components/form/gl-form-controller';
import ListingTeam from './team/listing-team';
import Amenities from './amenities/amenities';
import Property from './property/property';
import ChargesAndModifiers from './chargesAndModifiers/charges-and-modifiers'
import Highlights from './highlights/highlights'
import styled from 'styled-components';
import { Col, Row } from 'react-styled-flexboxgrid';
import ListingNavbar from './listing-navbar';
import ListingSidebar from './listing-sidebar';
import Specifications from './specifications/specifications';
import SizesAndMeasurements, { SizeType } from './sizesAndMeasurements/sizes-and-measurements';
import Spaces from './spaces/spaces'
import { Listing } from '../../types/listing/listing';
import Map from './map/map';
import APISelect from './apiselect/api-select';
import Contacts from './contacts/contacts';
import { saveListing, SaveType } from '../../redux/actions/listingEntry/save-listing';
import { deleteListing } from '../../redux/actions/listingEntry/delete-listing';
import { setAlertMessage } from '../../redux/actions/system/set-alert-message';
import { AlertMessagingType, FeatureFlags } from '../../types/state';
import { checkTrigger } from './render-trigger-util';
import { createCoordinates } from '../../utils/map';
import { extraPublishedValidationFunc, PublishedExtraInfo, ManualError } from './validations/published';
import { unpublishListing, UnpublishType } from '../../redux/actions/listingEntry/unpublish-listing';
import SectionHeading from '../../components/section-heading/section-heading';
import cloneDeep from 'clone-deep';
import { standardSpecifications } from '../../api/defaults/specs-standard';
import { flexSpecifications } from '../../api/defaults/specs-flex';
import { Config } from '../../types/config/config';
import { configSelector } from '../../redux/selectors/system/config-selector';
import { setConfirmDialog } from '../../redux/actions/system/set-confirm-dialog';
import { setDatasourcePopup, clearDatasourcePopup } from '../../redux/actions/system/set-datasource-popup';
import { setEntryPageScrollPosition } from '../../redux/actions/system/entry-page-scroll-position';
import { entryPageScrollPositionSelector } from '../../redux/selectors/system/entry-page-scroll-position-selector';
import { setDirtyListing } from '../../redux/actions/listingEntry/update-navbar';
import { featureFlagSelector } from '../../redux/selectors/featureFlags/feature-flag-selector';
import PointsOfInterest from './pointsOfInterest/points-of-interest';
import TransportationsType from './transportationType/transportation-type';
import Parking from './parkings/parkings'
import InsightsIcon from '../../assets/images/png/insights-icon.png';
import { isDuplicatingListingSelector } from '../../redux/selectors/system/is-duplicating-listing-selector';
import { checkForNewSpaces } from '../../api/glQueries';
import { postData } from '../../api/glAxios';
import SpacesImportModal from '../../components/spaces-import-modal/spaces-import-model';
import { updateSpaces } from '../../redux/actions/listingEntry/update-spaces';
// to get around 'Switch' cannot be used as a JSX component. Its instance type 'ReactSwitch' is not a valid JSX element. error
import WrongSwitch from 'react-switch';
const Switch = WrongSwitch as any;

interface Props {
    anchors: string[],
    navTitle: string,
    listing: Listing
}

enum PendingAction {
    NONE = "NONE",
    SAVE = "SAVE",
    PUBLISH = "PUBLISH",
    EXPORT = "EXPORT",
    DUPLICATE = "DUPLICATE"
}

const ListingEntry: FC<Props> = (props) => {

    const { anchors, navTitle, listing } = props;

    const dispatch = useDispatch();
    const config: Config = useSelector(configSelector);
    const featureFlags: FeatureFlags = useSelector(featureFlagSelector);
    const entryPageScrollPosition: number = useSelector(entryPageScrollPositionSelector);
    const isDuplicatingListing = useSelector(isDuplicatingListingSelector);

    const [currentData, setCurrentData] = useState<Listing>(Object.assign({}, listing));
    const [coordinates, setCoordinates] = useState(createCoordinates(listing));
    const [api, setAPI] = useState("google");
    const [forceValidate, setForceValidate] = useState<boolean>(false);
    const [showErrors, setShowErrors] = useState<boolean>(false);
    const [pendingAction, setPendingAction] = useState<PendingAction>(PendingAction.NONE);
    const [propertyValidations, setPropertyValidations] = useState<object>(listing.published ? config.validations.published : config.validations.unpublished);
    const [publishedExtra, setPublishedExtra] = useState<boolean>(listing.published ? true : false);
    const [manualErrors, setManualErrors] = useState<object>({});
    const [mapError, setMapError] = useState<boolean>(false);
    const [contactsError, setContactsError] = useState<ManualError>({ error: false, message: "" });
    const [nextFocus, setNextFocus] = useState<string>("default");
    const [miqSpaces, setMiqSpaces] = useState<any>([]);
    const [spacesModal, setSpacesModal] = useState<boolean>(false);

    let propertyType = currentData.propertyType;
    let listingType = currentData.listingType;

    let updateObj: Listing = Object.assign({}, currentData);
    
    const handleSwitchChange = (checked: any) => { 
        updateObj.syndicationFlag = checked;
        setCurrentData(updateObj);
     }

    const handleFormChanges = (values: any) => {

        if (config.featureFlags.autoCalculateSizeAndPrice && values.spaces){
            dispatch(updateSpaces(values.spaces))
        }

        Object.keys(values).forEach(key => {
            updateObj[key] = values[key];
        });

        // if a key piece of data changes, we'll need to re-render
        let rerender: boolean = false;

        // if the listing type has changed, always clear the specification fields
        if (updateObj.listingType !== listingType) {
            updateObj = clearSpecificationFields(updateObj);
        }

        // property type (industrial, retail, etc.)
        if (updateObj.propertyType !== propertyType) {
            // if either the previous property type (propertyType) or current propertyType (updateObj.propertyType) 
            // is found in the config values for "clearSpacesOn", then we need to clear the spaces out
            // for example, flex spaces in SG and IN will hit this logic
            if (config.propertyType && config.propertyType.clearSpacesOn) {
                const changeSpacesOn: string[] = config.propertyType.clearSpacesOn;
                if (changeSpacesOn.includes(propertyType) || changeSpacesOn.includes(updateObj.propertyType)) {
                    // clear out specifications
                    updateObj = clearSpecificationFields(updateObj);
                    // clear out spaces
                    updateObj.spaces = [];
                }
            }
            propertyType = updateObj.propertyType;
            setNextFocus("propertyType");
            rerender = true;
        }

        // listing type (sale, lease, etc.)
        if (updateObj.listingType !== listingType) {
            listingType = updateObj.listingType;
            setNextFocus("listingType");
            rerender = true;
        }

        // render triggers
        if (updateObj.triggers) {
            // individiual checks
            if (checkTrigger(currentData, updateObj, "addressChange")) {
                rerender = true;
                setNextFocus("headline");   // todo: this is only true if the next field is always headline after address (config?)
                setMapError(false);
                setCoordinates(createCoordinates(updateObj));
            }
            if (checkTrigger(currentData, updateObj, "latLngChange")) {
                rerender = true;
                setCoordinates(createCoordinates(updateObj));
            }
        }

        // trim property record name to ensure we don't have empty spaces
        // note: the yup vaidation should take care of trimming, we should figure out how to do it there instead when time permits.
        if (updateObj && updateObj.propertyRecordName && typeof updateObj.propertyRecordName === "string") {
            updateObj.propertyRecordName = updateObj.propertyRecordName.trim();
        }

        // Check to see if the specifications' checkbox's changed
        if (updateObj.specifications && currentData.specifications){
            if ((updateObj.specifications.contactBrokerForPrice !== currentData.specifications.contactBrokerForPrice) || (updateObj.specifications.showPriceWithUoM !== currentData.specifications.showPriceWithUoM)){
                rerender = true;
            }
        }

        if (rerender === true) {
            setCurrentData(Object.assign({}, updateObj));
        }
    
        // ensure we have marked the listing as dirty (if it's dirty) [note: if we can do a true comparison we should find a way]
        dispatch(setDirtyListing(true));
    }

    const changeSizesAndMeasurements = (sizeTypes: SizeType[]) => {
        updateObj.propertySizes = sizeTypes;
    }

    // clears specification fields in spec & spaces view to the default values on property switch
    const clearSpecificationFields = (theListing: Listing) => {
        const payload: Listing = theListing;
        if (payload.propertyType === 'flex') {
            payload.specifications = cloneDeep(flexSpecifications);
        } else {
            payload.specifications = cloneDeep(standardSpecifications);
        }
        return payload;
    }

    const validate = () => {
        dispatch(setAlertMessage({ show: false, message: "", type: AlertMessagingType.NOTICE, allowClose: true }));
        setForceValidate(true);
        setShowErrors(true);
        setCurrentData(updateObj);
    }

    const checkManualErrors = (manual: object) => {
        // check the manual errors and set so the UI can display, if they exist
        setManualErrors(manual);

        // a check for the map coordinates error, which is set outside of our normal form stuff
        const coordsName: string = "coords";
        if (manual[coordsName]) {
            setMapError(true);
        } else {
            setMapError(false);
        }
        // check for a contacts error
        const contactsName: string = "contacts";
        if (manual[contactsName]) {
            setContactsError(manual[contactsName]);
        } else {
            setContactsError(manual[contactsName]);
        }
    }

    const validateComplete = (errors: string[]) => {

        setForceValidate(false);
        if (publishedExtra) {
            const errorInfo: PublishedExtraInfo = extraPublishedValidationFunc(updateObj, featureFlags, errors);
            errors = errorInfo.errors;
            checkManualErrors(errorInfo.manual);
        }
        if (errors.length === 0) {
            appendCurrencyCode();
            if (pendingAction === PendingAction.SAVE) {
                finishSave();
            } else if (pendingAction === PendingAction.PUBLISH) {
                finishPublish();
            } else if (pendingAction === PendingAction.EXPORT) {
                finishExport();
            } else if (pendingAction === PendingAction.DUPLICATE) {
                finishDuplicate();
            }
        } else {
            // kill the scroll position in redux on error so it goes to the top
            dispatch(setEntryPageScrollPosition(0));
            // ensure user is at the top of the screen to review errors
            window.scrollTo(0, 0);

            let errorMessage: string = "There are error(s) that must be fixed before saving. ";
            if (pendingAction === PendingAction.PUBLISH) {
                errorMessage = "There are error(s) that must be fixed before publishing. ";
            } else if (pendingAction === PendingAction.EXPORT) {
                errorMessage = "There are error(s) that must be fixed before exporting. ";
            }
            dispatch(setAlertMessage({ show: true, message: errorMessage, type: AlertMessagingType.ERROR, allowClose: true }));
        }
        setPendingAction(PendingAction.NONE);
    }

    const finishSave = () => {
        dispatch(saveListing(updateObj, SaveType.SAVE, config));
    }

    const appendCurrencyCode = () => {
        const type: string = "currencyCode";
        if (updateObj.specifications && config.currencyCode) {
            updateObj.specifications[type] = config.currencyCode;
        }
        if ((!config.currencies || config.currencies.options.length === 0) && updateObj.chargesAndModifiers && updateObj.chargesAndModifiers.length > 0) {
            for (const charge of updateObj.chargesAndModifiers) {
                charge[type] = config.currencyCode;
            }
        }
    }

    const finishPublish = () => {
        // for now, we'll just save the listing until we have the integration with back end working properly
        dispatch(saveListing(updateObj, SaveType.PUBLISH, config));
    }

    const submitPublish = (popupData: any) => {
        if (config && config.dataSource && config.dataSource.show) {
            updateObj.datasource.datasources = popupData.dataSources;
            updateObj.datasource.other = popupData.other;
        }
        updateValidation(PendingAction.PUBLISH);
    }

    const finishExport = () => {
        dispatch(saveListing(updateObj, SaveType.EXPORT, config));
    }

    const finishDuplicate = () => {
        dispatch(saveListing(updateObj, SaveType.DUPLICATE, config));
    }

    const saveHandler = () => {
        updateValidation(PendingAction.SAVE);
    }

    const duplicateListingHandler = () => {
        updateValidation(PendingAction.DUPLICATE);
    }

    const publishHandler = () => {
        const action = (updateObj.externalPreviewUrl !== "") ? 'update' : 'create';

        if (config && config.dataSource && config.dataSource.show) {
            if (updateObj.datasource === undefined) {
                const data = {
                    datasources: [],
                    other: ""
                }
                updateObj.datasource = data;
            }
            dispatch(setDatasourcePopup({ show: true, action, datasource: updateObj.datasource, confirmFunc: submitPublish }));
        }
        else {
            updateValidation(PendingAction.PUBLISH);
        }
    }

    const exportListingHandler = () => {
        updateValidation(PendingAction.EXPORT);
    }

    const updateValidation = (action: PendingAction) => {
        if (action === PendingAction.SAVE || action === PendingAction.DUPLICATE) {
            // if save, record scroll position 
            let scrollPosition: number = document.documentElement.scrollTop || document.body.scrollTop;
            scrollPosition -= 19.2; // 19.2 is the height of the message
            dispatch(setEntryPageScrollPosition(scrollPosition));
        }
        // the validations we apply depend on the "state" in the listing
        setPendingAction(action);
        if (action === PendingAction.PUBLISH || currentData.state === "SUBMITTED" || currentData.state === "PUBLISHED") {
            setPropertyValidations(config.validations.published);
            setPublishedExtra(true);
            if (config && config.dataSource && config.dataSource.show) {
                clearDatasourcePopup(dispatch);
            }
        } else if (!currentData.state || currentData.state === "") {
            setContactsError({ error: false, message: "" });
            setMapError(false);
            setPropertyValidations(config.validations.unpublished);
            setManualErrors({});
            setPublishedExtra(false);
        }
        validate();
    }

    const deleteHandler = () => {
        dispatch(deleteListing(updateObj))
    }

    const deleteThisListing = () => {
        dispatch(setConfirmDialog({ show: true, title: "Confirm Delete Listing", message: "Are you sure you want to Delete this Listing?", confirmTxt: "Delete", confirmFunc: deleteHandler }));
    }

    const unpublishHandler = () => {
        dispatch(saveListing(updateObj, SaveType.SAVE, config));
        dispatch(unpublishListing(updateObj, UnpublishType.LISTINGENTRY));
    }

    const propertyTypeExistsInSpacesConfig = () => {
        let exists = false;
        if (currentData.propertyType && currentData.propertyType.length > 0 && currentData.listingType && currentData.listingType.length > 0) {
            if (config) {
                const validPropertyTypes: any = [];
                for (const setup of config.spaces.accordionSetups) {
                    for (const validPropertyType of setup.propertyTypes) {
                        validPropertyTypes.push(validPropertyType)
                    }
                }
                if (currentData.listingType && validPropertyTypes.includes(currentData.propertyType)) {
                    exists = true
                }
            }
        }
        return exists;
    }

    const isValidPropertyType = () => {
        let isValid = false;
        const values: any = [];
        if (config) {
            for (const option of config.propertyType.options) {
                values.push(option.value)
            }
        }
        if (values.includes(currentData.propertyType) || !currentData.propertyType) {
            isValid = true;
        }
        if (!isValid && config) {
            dispatch(setAlertMessage({ show: true, message: "This listing appears to be created by another config. Please reselect from property type dropdown.", type: AlertMessagingType.NOTICE, allowClose: true }));
        }
        return isValid;
    }

    if (!config.teamsEnabled && anchors.indexOf('team') > -1) {
        anchors.splice(anchors.indexOf('team'), 1);
    }

    const checkImagesForProcessing = (valuesToUpdate: object) => {
        if (valuesToUpdate) {
            handleFormChanges(valuesToUpdate);
        }
    }

    const formatDateString = (date: any) => {
        try{
            if(typeof date === 'string' && date.length > 0){
                return date.slice(0, 10);
            }else if(date && date instanceof Date) {
                return date.toISOString().slice(0, 10);
            }
        }catch(e){
            return "";
        }
    }

    const showSpacesModal = () => {
        setSpacesModal(!spacesModal)
    }

    const addSpaces = (data: any) => {
        const temp: any = currentData;
        temp.spaces = data;
        console.log(temp.spaces)
        setCurrentData(Object.assign({}, temp))
    }

    useEffect(() => {
        // on render, check the scroll position to see if we need to scroll (non-zero)
        if (entryPageScrollPosition && entryPageScrollPosition > 0) {
            window.scrollTo(0, entryPageScrollPosition);
            dispatch(setEntryPageScrollPosition(0));
        }
    }, []);

    useEffect(() => {
        if (listing && listing.id){
            const query = checkForNewSpaces(listing.id);
            postData(query).then((response:any) => {
                if (response.data && response.data.spaces && response.data.spaces.length > 0){
                    let temp = false;
                    response.data.spaces.forEach((space: any) => {
                        if (space.id === 0){
                            temp = true;
                        }
                    })
                    if (temp){
                        setMiqSpaces(response.data.spaces)
                    }
                }
            }).catch((error: any) => {
                console.log(error);
            })
        }
    }, [listing])

    return (
        <ListingEntryContainer>
            {miqSpaces && miqSpaces.length > 0 && spacesModal && <SpacesImportModal listing={currentData} spaces={miqSpaces} showSpacesModal={showSpacesModal} addSpaces={addSpaces} />}
            <ListingNavbar navTitle={navTitle} saveHandler={saveHandler} publishHandler={publishHandler} unpublishHandler={unpublishHandler} deleteHandler={deleteThisListing} exportListingHandler={exportListingHandler} duplicateListingHandler={duplicateListingHandler} isDeleted={currentData.isDeleted} isDuplicatingListing={isDuplicatingListing} />
            {
                (currentData.state && (currentData.state.toLowerCase() === "publishing" || currentData.state.toLowerCase() === "unpublishing")) && <DisableOverlay />
            }
            { config && config.insightsEnabled &&
                <InsightsSection>
                    <img src={InsightsIcon}/>
                    <StyledInsightLink href={"/insights/" + listing.id}>View Analytics</StyledInsightLink>
                </InsightsSection>
            }
            <FormController changeHandler={handleFormChanges} validateComplete={validateComplete}>
                <GridContainer>
                    <Row between="sm">
                        <Col sm={2} style={{ maxWidth: '150px', borderRight: '1px solid #eee' }}>
                            <ListingSidebar anchors={anchors} listing={currentData} overrideSpecification={config && config.specifications && config.specifications.label ? config.specifications.label : ""} />
                        </Col>
                        <FormContainerCol sm={10}>
                            <Row>
                                <Col sm={4}>
                                    {config && config.syndication && config.syndication.show && 
                                    <SwitchContainer>
                                        <Switch
                                            onChange={handleSwitchChange}
                                            checked={(currentData.syndicationFlag !== null) ? currentData.syndicationFlag! : false}
                                            handleDiameter={23}
                                            uncheckedIcon={false}
                                            checkedIcon={false}
                                            boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
                                            activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
                                            height={18}
                                            width={45}
                                        /> 
                                        <SwitchLabel>
                                            {(config && config.syndication && config.syndication.label) ? config.syndication.label : "Send to Third Party"}
                                        </SwitchLabel>
                                    </SwitchContainer>
                                    }
                                </Col>
                                <Col sm={8} style={{ display: 'flex', marginTop: '10px', justifyContent: 'flex-end' }}>
                                    {config.displayMIQId && currentData.miqId && currentData.miqId !== '' && currentData.miqId !== null && <CustomDate><Label>MIQ ID:</Label> {currentData.miqId}</CustomDate>}
                                    {currentData.dateCreated && <CustomDate><Label> Date Created: </Label> {formatDateString(currentData.dateCreated)}</CustomDate>}
                                    {currentData.dateUpdated && <CustomDate><Label> Date Updated: </Label> {formatDateString(currentData.dateUpdated)}</CustomDate>}
                                    {currentData.datePublished && <CustomDate><Label> Published Date: </Label> {formatDateString(currentData.datePublished)}</CustomDate>}
                                </Col>
                            </Row>
                            <Disclaimer>Legal Disclosure: All data and images entered into Global Listings must originate from CBRE or the property owner. Data and images from third-party listing platforms or websites cannot be entered into Global Listings under any circumstances.</Disclaimer>
                            <InitialFocus autoFocus={nextFocus === "default" ? true : false} />
                            {config.teamsEnabled && <Row>
                                <Col key={"teamData" + new Date().getTime()} sm={12}>
                                    <ListingTeam listing={currentData} />
                                </Col>
                            </Row>}
                            <Row>
                                <Col key={"propData" + new Date().getTime()} sm={7}>
                                    <TopFormContainer>
                                        <Property listing={currentData} placesAPI={api} forceValidate={forceValidate}
                                            showErrors={showErrors} manualErrors={manualErrors} nextFocus={nextFocus}
                                            validations={propertyValidations} imageProcessingCheck={checkImagesForProcessing} />

                                    </TopFormContainer>
                                </Col>
                                <Col sm={5}>
                                    <Row><Col sm={12}><SectionHeading>&nbsp;</SectionHeading></Col></Row>
                                    <Row>
                                        <Col sm={12}>
                                            <MapContainer>
                                                <Map coordinates={coordinates} error={mapError} api={api} />
                                            </MapContainer>
                                        </Col>
                                    </Row>
                                    <APISelectContainer style={{ display: 'none' }} >
                                        <APISelect api={api} changeHandler={setAPI} />
                                    </APISelectContainer>
                                </Col>
                            </Row>
                            {config && config.parkings && config.parkings.show 
                                    && listing && listing.parkings &&
                                <Row style={{ marginTop: '30px' }}>
                                    <Col sm={12} key={'parkings'}>
                                        <Parking
                                            config={config}
                                            parkingsConfig={config.parkings}
                                            parkings={listing.parkings}
                                        />
                                    </Col>
                                </Row>
                            }
                            {config && config.amenities && config.amenities.show &&
                                <Row>
                                    <Col sm={12} key={"amenitiesData" + new Date().getTime()}>
                                        <Amenities listing={currentData} />
                                    </Col>
                                </Row>
                            }
                            <Row>
                                <Col sm={12} key={"highlightsData" + new Date().getTime()} >
                                    <Highlights listing={currentData} label={(config && config.highlight && config.highlight.label) ? config.highlight.label : "Highlights"} />
                                    {isValidPropertyType() && currentData.listingType &&
                                        <>
                                            <Specifications listing={currentData} autoCalculate={config.featureFlags.autoCalculateSizeAndPrice ? true : false} />
                                            {config && config.sizesandmeasurements && config.sizesandmeasurements.show &&
                                                <SizesAndMeasurements
                                                    config={config}
                                                    sizeTypeSetup={config.sizesandmeasurements}
                                                    sizeTypes={listing.propertySizes}
                                                    changeHandler={changeSizesAndMeasurements}
                                                />}
                                            <ChargesAndModifiers listing={currentData} />
                                        </>
                                    }
                                </Col>
                            </Row>
                            {config && config.transportationTypes && config.transportationTypes.show &&
                                <Row style={{ marginTop: '30px' }}>
                                    <Col sm={12} key={'transportationsType'}>
                                        <TransportationsType
                                            tt={currentData.transportationTypes}
                                            types={config.transportationTypes.types}
                                            distanceUnits={config.transportationTypes.distanceUnits}
                                            travelMode={config.transportationTypes.travelMode}
                                            sectionHeading={config.transportationTypes.label}
                                        />
                                    </Col>
                                </Row>
                            }
                            {config && config.pointsOfInterests && config.pointsOfInterests.show &&
                                <Row style={{ marginTop: '30px' }}>
                                    <Col sm={12} key={'pointsOfInterest'}>
                                        <PointsOfInterest
                                            poi={currentData.pointsOfInterests}
                                            interestKinds={config.pointsOfInterests.interestKinds}
                                            distanceUnits={config.pointsOfInterests.distanceUnits}
                                            travelMode={config.pointsOfInterests.travelMode}
                                            sectionHeading={config.pointsOfInterests.label}
                                        />
                                    </Col>
                                </Row>
                            }
                            {propertyTypeExistsInSpacesConfig() &&
                                <Row>
                                    <Col sm={12} key={"spaceData" + new Date().getTime()}>
                                        <Spaces listing={currentData} miqSpaces={miqSpaces} showSpacesModal={showSpacesModal} imageProcessingCheck={checkImagesForProcessing} manualErrors={manualErrors} />
                                    </Col>
                                </Row>
                            }
                            <Row>
                                <Col sm={12} key={"contactData" + new Date().getTime()}>
                                    <Contacts listing={currentData} error={contactsError} validateHandler={validate} />
                                </Col>
                            </Row>
                        </FormContainerCol>
                    </Row>
                </GridContainer>
            </FormController>
        </ListingEntryContainer>
    );
};

const ListingEntryContainer = styled.div`
    background: #fdfdfd;
    margin-bottom:300px;
    width:100%;
    #availableFrom {
        > div {
            > div {
                display:flex;
                justify-content: space-between;
            }
        }
    }
`;

const CustomDate = styled.span`
    color: black;
    margin-right: 1em;
    font-weight:bold;
    letter-spacing:1px;
`;

const Label = styled.span`
    color: #9EA8AB;
    margin-left: 1em;
`;

const SwitchLabel = styled.span`
    font-weight:bold;
    letter-spacing:1px;
    margin-left: 1em;
    color: black;
    line-height: 28px; 
    vertical-align: text-bottom;
`;

const SwitchContainer = styled.div`
    margin-top: 5px;
    padding: 0px;
    height:20px;
`;

const GridContainer = styled.div`
    > div {
        max-width:100%;
    }
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
`;

const TopFormContainer = styled.div`
    margin-right: 2em;
`;

// note: margin-top on map container matches the label margin for alignment purposes
const MapContainer = styled.div`
    margin-top: 1.67em;           
`;

const FormContainerCol = styled(Col as any)`
    background:#fdfdfd;
`;

const APISelectContainer = styled.div`
    padding: 25px 15px;
`;

const DisableOverlay = styled.div`
    width:100%;
    height:100%;
    position: absolute;
    background-color: #000;
    opacity: .25;
    z-index: 5;
`;

const InitialFocus = styled.input`   
    height: 0px;
    width: 0px;
    padding: 0;
    border: none;
    outline: none;
`;

const Disclaimer = styled.h5`
    font-size: 14px;
    font-weight: 400;
    color: #666;
    font-style: italic;
`;

const InsightsSection = styled.div`
    display: flex-inline;
    width: 100%;
    font-family: 'Futura Md BT', helvetica, arial, sans-serif;
    padding: 10px 20px 0 10px;
`;

const StyledInsightLink = styled.a`
    color: #006A4D;
    font-size: 14px;
    font-weight: 400;
    height: 18px;
    text-decoration: none;
    margin-left: 10px;
`;

export default React.memo(ListingEntry);