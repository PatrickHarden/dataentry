
import React, { FC, useContext, useEffect, useState } from 'react'
import GLForm from '../../../components/form/gl-form';
import { convertValidationJSON } from '../../../utils/forms/validation-adapter';
import { Col, Row } from 'react-styled-flexboxgrid'
import SectionHeading from "../../../components/section-heading/section-heading";
import { FormContext } from '../../../components/form/gl-form-context';
import { Listing } from '../../../types/listing/listing';
import styled from 'styled-components';
// field partials
import { Specifications } from '../../../types/listing/specifications';
import { Config } from '../../../types/config/config';
import { useSelector, useDispatch } from 'react-redux';
import GLField from '../../../components/form/gl-field';
import FormSelect, { FormSelectProps } from '../../../components/form-select/form-select';
import FormInput, { FormInputProps } from '../../../components/form-input/form-input';
import { taxTypes } from '../../../api/lookups/tax-types';
import FormCheckbox, { FormCheckboxProps } from '../../../components/form-checkbox/form-checkbox';
import FormRadioGroup, { FormRadioGroupProps } from '../../../components/form-radiobutton/form-radiogroup';
import { SpecsListingTypeFields, SpecsFieldSetup, SpecsPropertyTypeFields } from '../../../types/config/specs/specs';
import { configSelector } from '../../../redux/selectors/system/config-selector';
import { setListingSuffix } from '../../../redux/actions/system/set-suffix-string';
import { suffixSelector } from '../../../redux/selectors/system/suffix-string-selector';
import { currentSpacesSelector } from '../../../redux/selectors/entry/current-spaces-selector';
import { ListingSuffix } from '../../../types/state';
import { findSpecificationFields } from '../../../utils/config/specifications-fields';
import { calculateSpecifications } from '../../../utils/listing/calculate-specifications';
import { routeSelector } from '../../../redux/selectors/router/route-selector';
import { RoutePaths } from '../../../app/routePaths';
import { matchPath } from 'react-router';
// to get around 'Switch' cannot be used as a JSX component. Its instance type 'ReactSwitch' is not a valid JSX element. error
import WrongSwitch from 'react-switch';
import { isArray } from 'lodash';
const Switch = WrongSwitch as any;


interface Props {
    listing: Listing,
    autoCalculate: boolean
}

const SpecificationsView: FC<Props> = (props) => {

    const { listing, autoCalculate } = props;

    const validations = {};

    const dispatch = useDispatch();
    const formControllerContext = useContext(FormContext);
    const config: Config = useSelector(configSelector);
    const reduxSpaces = useSelector(currentSpacesSelector);
    const route: string = useSelector(routeSelector);
    let spaces = listing ? listing.spaces : [];
    if (reduxSpaces && reduxSpaces.length > 0) {
        spaces = reduxSpaces;
    }

    const noEntryRoutes = ['createListing'];
    let toggleSwitches = false;
    for (const [key, value] of Object.entries(RoutePaths)) {
        const match = matchPath(route, value);
        if (match && match.isExact) { // if matchPath confirms same route
            if (noEntryRoutes.includes(key) && config.featureFlags && config.featureFlags.autoCalculateSizeAndPrice ) {
                toggleSwitches = true;
            }
        }
    }

    const contactBrokerField: string = "contactBrokerForPrice";
    const showPriceWithUoMField: string = "showPriceWithUoM";
    const measureField: string = "measure";
    const leaseTermField: string = "leaseTerm";
    const taxModifierField: string = "taxModifer";

    // toggle switches state - need to move up a level
    const [minSpace, setMinSpace] = useState<boolean | undefined>(listing.specifications.autoCalculateMinSpace ? listing.specifications.autoCalculateMinSpace : toggleSwitches);
    const [totalSpace, setTotalSpace] = useState<boolean | undefined>(listing.specifications.autoCalculateTotalSpace ? listing.specifications.autoCalculateTotalSpace : toggleSwitches);
    const [minLeasePrice, setMinLeasePrice] = useState<boolean | undefined>(listing.specifications.autoCalculateMinPrice ? listing.specifications.autoCalculateMinPrice : toggleSwitches);
    const [maxLeasePrice, setMaxLeasePrice] = useState<boolean | undefined>(listing.specifications.autoCalculateMaxPrice ? listing.specifications.autoCalculateMaxPrice : toggleSwitches);
    const [maxPrice, setMaxPrice] = useState<boolean | undefined>(listing.specifications.autoCalculateTotalPrice ? listing.specifications.autoCalculateTotalPrice : toggleSwitches);

    const [priceDisabled, setPriceDisabled] = useState<boolean>(listing.specifications.contactBrokerForPrice);
    const [showPriceUoM, setShowPriceUoM] = useState<boolean>(listing.specifications.showPriceWithUoM);
    // possibly move this into the changeHandler function
    const [specs, setSpecs] = useState<Specifications>(listing.specifications);
    const [measure, setMeasure] = useState<string>(listing.specifications && listing.specifications.measure ? listing.specifications.measure : "");
    const [taxModifer, setTaxModifer] = useState<string>(listing.specifications && listing.specifications.taxModifer ? listing.specifications.taxModifer : "");
    const [leaseTerm, setLeaseTerm] = useState<string>(listing.specifications && listing.specifications.leaseTerm ? listing.specifications.leaseTerm : "");
    const [lastFocus, setLastFocus] = useState<string>("");
    const [localListingSuffix, setLocalListingSuffix] = useState<ListingSuffix>(useSelector(suffixSelector));


    const changeHandler = (values: any, isMinSpace?: boolean, isTotalSpace?: boolean, isMinLeasePrice?: boolean, isMaxLeasePrice?: boolean, maxSalePrice?: boolean) => {
        const temp: any = values;

        if (checkIfNull(temp.plusSalesTax) && checkIfNull(specs.plusSalesTax) && checkIfNull(temp.includingSalesTax) && checkIfNull(specs.includingSalesTax)) {
            if (temp.plusSalesTax && specs.plusSalesTax) {
                if (temp.includingSalesTax) {
                    temp.plusSalesTax = false;
                }
            }
            if (temp.includingSalesTax && specs.includingSalesTax) {
                if (temp.plusSalesTax) {
                    temp.includingSalesTax = false;
                }
            }
        }

        const valueObj = {
            'specifications': {
                ...temp
            }
        }

        valueObj.specifications.autoCalculateMinSpace = isMinSpace;
        valueObj.specifications.autoCalculateTotalSpace = isTotalSpace;
        valueObj.specifications.autoCalculateMinPrice = isMinLeasePrice;
        valueObj.specifications.autoCalculateMaxPrice = isMaxLeasePrice;
        valueObj.specifications.autoCalculateTotalPrice = maxSalePrice;

        let resetSpecs: boolean = false;

        // disable logic for pricing
        if (temp && temp[contactBrokerField] !== undefined && temp[contactBrokerField] !== priceDisabled) {
            // ensure local data state is set too 
            resetSpecs = false;
            setPriceDisabled(temp[contactBrokerField]);
            setLastFocus("contactBroker");
        }

        if (temp && temp[showPriceWithUoMField] !== undefined && temp[showPriceWithUoMField] !== showPriceUoM) {
            resetSpecs = true;
            setShowPriceUoM(temp[showPriceWithUoMField]);
            setLastFocus("showPriceWithUoM");
        }
        if (temp && temp[measureField] !== measure) {
            resetSpecs = true;
            setMeasure(temp[measureField]);
            setLastFocus("measure");
        }
        if (temp && temp[leaseTermField] !== leaseTerm) {
            resetSpecs = true;
            setLeaseTerm(temp[leaseTermField]);
            setLastFocus("leaseTerm");
        }
        if (temp && temp[taxModifierField] && temp[taxModifierField] !== taxModifer) {
            resetSpecs = true;
            setTaxModifer(temp[taxModifierField]);
        }

        if (resetSpecs) {
            setSpecs(temp); // performing this update re-renders and causes focus issues, so only do situationally and not every time
        }
        let listingSuffixChanged: boolean = false;
        // check measure
        if (temp.measure && temp.measure.length > 0 && temp.measure !== localListingSuffix.measure) {
            listingSuffixChanged = true;
        }
        // check lease term
        if (temp.leaseTerm && temp.leaseTerm.length > 0 && temp.leaseTerm !== localListingSuffix.leaseTerm) {
            listingSuffixChanged = true;
        }
        // if changed, dispatch
        if (listingSuffixChanged) {
            const newSuffix: ListingSuffix = {
                measure: temp.measure,
                leaseTerm: temp.leaseTerm
            };
            dispatch(setListingSuffix(newSuffix));
            setLocalListingSuffix(newSuffix);
        }

        if (isMinSpace || isTotalSpace || isMinLeasePrice || isMaxLeasePrice || maxSalePrice) {
            valueObj.specifications = calculateSpecifications(valueObj.specifications, spaces, isMinSpace, isTotalSpace, isMinLeasePrice, isMaxLeasePrice, maxSalePrice);
            setSpecs(valueObj.specifications);
        }

        formControllerContext.onFormChange(valueObj);
        setSpecs(valueObj.specifications);
    }



    // make sure our data exists before operating on  it - this is incase someone disables it in the config
    const checkIfNull = (dataPoint: any) => {
        if (dataPoint === undefined || dataPoint === null) {
            return false
        } else {
            return true
        }
    }

    // we pull in some of the differing configurations for our specifications page from the config
    const fields: SpecsListingTypeFields | undefined = findSpecificationFields(config, listing.propertyType, listing.listingType);

    let currencyIcon: string = "$";
    if (config && config.currencySymbol) {
        currencyIcon = config.currencySymbol;
    }

    const getSpaceLabel = (baseLabel: string | undefined) => {
        // dynamic label for flex
        if (listing.propertyType === "flex" && measure) {
            return measure + " " + baseLabel;
        }
        return baseLabel;
    }

    const getSpaceSuffix = () => {
        if (listing.propertyType !== "flex" && measure) {
            return measure;
        }
        return "";
    }

    const getPriceSuffix = () => {
        let suffix: string = "";
        if (listing.listingType === "sale") {
            return "";
        }
        if (showPriceUoM !== false && measure) {
            suffix += "/" + measure;
        }
        if (leaseTerm) {
            suffix += "/" + leaseTerm;
        }
        return suffix;
    }

    const getColSize = <T extends {}>(fieldSetup: SpecsFieldSetup<T>): number => {
        if (fieldSetup && fieldSetup.show && fieldSetup.grid && fieldSetup.grid.colSize) {
            return fieldSetup.grid.colSize;
        }
        return 6;  // 6 is our current default for col size if not specified
    }

    const setDefaultLeaseTypeValue = (value: string) => {
        if (!specs.leaseType || specs.leaseType === '') {
            const temp: Specifications = specs;
            const key: string = "leaseType"
            temp[key] = value;
            setSpecs(temp);
        }
    }

    const checkForceFocus = (fieldName: string): boolean => {
        if (lastFocus === fieldName) {
            return true;
        }
        return false;
    }

    const setDefaultContactBrokerForPrice = () => {
        // Map "Contact Broker For Pricing" checkbox to config, but only for brand new listings
        if (fields && fields.contactBrokerForPrice && fields.contactBrokerForPrice.properties && fields.contactBrokerForPrice.properties.defaultValue) {
            if (!listing.id || listing.id === 0) {
                listing.specifications.contactBrokerForPrice = fields.contactBrokerForPrice.properties.defaultValue;
            }
        }
    }

    const setDefaultShowPriceWithUoM = () => {
        if (fields && fields.showPriceWithUoM && fields.showPriceWithUoM.properties) {
            if (!listing.id || listing.id === 0 || showPriceUoM === undefined || showPriceUoM === null) {
                listing.specifications.showPriceWithUoM =
                    fields.showPriceWithUoM.properties.defaultValue ? fields.showPriceWithUoM.properties.defaultValue : true;
            }
        }
    }

    const handleMinSpaceChecked = (checked: boolean) => {
        changeHandler(specs, checked, totalSpace, minLeasePrice, maxLeasePrice, maxPrice);
        setMinSpace(checked);
    }
    const handleTotalSpaceChecked = (checked: boolean) => {
        changeHandler(specs, minSpace, checked, minLeasePrice, maxLeasePrice, maxPrice);
        setTotalSpace(checked);
    }
    const handleMinLeaseChecked = (checked: boolean) => {
        changeHandler(specs, minSpace, totalSpace, checked, maxLeasePrice, maxPrice);
        setMinLeasePrice(checked);
    }
    const handleMaxLeaseChecked = (checked: boolean) => {
        changeHandler(specs, minSpace, totalSpace, minLeasePrice, checked, maxPrice);
        setMaxLeasePrice(checked)
    }
    const maxPriceChecked = (checked: boolean) => {
        changeHandler(specs, minSpace, totalSpace, minLeasePrice, maxLeasePrice, checked);
        setMaxPrice(checked)
    }

    useEffect(() => {
        let send = false;
        if (listing && listing.spaces && listing.spaces.length > 0){
            listing.spaces.forEach((space: any, index: number) => {
                if (reduxSpaces[index] && JSON.stringify(space.specifications) !== JSON.stringify(reduxSpaces[index].specifications)){
                    send = true;
                }
            })
        }
        if (send){
            changeHandler(specs, minSpace, totalSpace, minLeasePrice, maxLeasePrice, maxPrice)
        }
    }, [reduxSpaces]);

    if (fields) {
        return (
            <SpecificationsContainer>
                <Row>
                    <Col id="specifications" xs={12}><SectionHeading>{(config && config.specifications && config.specifications.label) ? config.specifications.label : "Specifications"}</SectionHeading></Col>
                </Row>
                <GLForm initVals={specs}
                    validationAdapter={convertValidationJSON}
                    validationJSON={validations}
                    changeHandler={changeHandler}
                    key={Date.now()}>
                    <Row>
                        {fields.leaseType && fields.leaseType.show &&
                            <Col xs={getColSize(fields.leaseType)}>
                                {fields.leaseType.properties.options[0] && fields.leaseType.properties.defaultValue && setDefaultLeaseTypeValue(fields.leaseType.properties.defaultValue)}
                                <GLField<FormSelectProps> {...fields.leaseType.properties} use={FormSelect} />
                            </Col>
                        }
                        {fields.leaseRateType && fields.leaseRateType.show &&
                            <Col xs={getColSize(fields.leaseRateType)}>
                                <GLField<FormSelectProps> {...fields.leaseRateType.properties} use={FormSelect} />
                            </Col>
                        }
                        {fields.leaseTerm && fields.leaseTerm.show &&
                            <Col xs={getColSize(fields.leaseTerm)}>
                                <GLField<FormSelectProps> {...fields.leaseTerm.properties} use={FormSelect} forceFocus={checkForceFocus("leaseTerm")} />
                            </Col>
                        }
                        {fields.measure && fields.measure.show &&
                            <Col xs={getColSize(fields.measure)}>
                                <GLField<FormSelectProps> {...fields.measure.properties} use={FormSelect} forceFocus={checkForceFocus("measure")} />
                            </Col>
                        }
                        {fields.minSpace && fields.minSpace.show &&
                            <Col xs={getColSize(fields.minSpace)}>
                                <GLField<FormInputProps> {...fields.minSpace.properties} suffix={getSpaceSuffix()} disabled={minSpace}
                                    label={getSpaceLabel(fields.minSpace.properties.label)} use={FormInput}
                                />
                                {autoCalculate &&
                                    <SwitchContainer>
                                        <Switch
                                            onChange={handleMinSpaceChecked}
                                            checked={minSpace}
                                            handleDiameter={23}
                                            uncheckedIcon={false}
                                            checkedIcon={false}
                                            key={'autoCalculateMinSpace'}
                                            name={'autoCalculateMinSpace'}
                                            boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
                                            activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
                                            height={18}
                                            width={45}
                                        />
                                        Use minimum size from available spaces
                                    </SwitchContainer>
                                }
                            </Col>
                        }
                        {fields.maxSpace && fields.maxSpace.show &&
                            <Col xs={getColSize(fields.maxSpace)}>
                                <GLField<FormInputProps> {...fields.maxSpace.properties} suffix={getSpaceSuffix()} label={getSpaceLabel(fields.maxSpace.properties.label)} use={FormInput} />
                            </Col>
                        }
                        {fields.totalSpace && fields.totalSpace.show &&
                            <Col xs={getColSize(fields.totalSpace)}>
                                <GLField<FormInputProps> {...fields.totalSpace.properties} suffix={getSpaceSuffix()} disabled={totalSpace} label={getSpaceLabel(fields.totalSpace.properties.label)} use={FormInput} />
                                {autoCalculate &&
                                    <SwitchContainer>
                                        <Switch
                                            onChange={handleTotalSpaceChecked}
                                            checked={totalSpace}
                                            handleDiameter={23}
                                            key={'specificationsTotalSpace'}
                                            name={'specificationsTotalSpace'}
                                            uncheckedIcon={false}
                                            checkedIcon={false}
                                            boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
                                            activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
                                            height={18}
                                            width={45}
                                        />
                                        Sum total from available spaces
                                    </SwitchContainer>
                                }
                            </Col>
                        }
                        {fields.spacePlaceholder && fields.spacePlaceholder.show &&
                            <Col xs={fields.spacePlaceholder.colSize} />
                        }
                        {fields.minPrice && fields.minPrice.show &&
                            <Col xs={getColSize(fields.minPrice)}>
                                <GLField<FormInputProps> {...fields.minPrice.properties} prefix={currencyIcon} suffix={getPriceSuffix()} disabled={(priceDisabled || minLeasePrice)} use={FormInput} />
                                {autoCalculate &&
                                    <SwitchContainer>
                                        <Switch
                                            onChange={handleMinLeaseChecked}
                                            checked={minLeasePrice}
                                            handleDiameter={23}
                                            uncheckedIcon={false}
                                            checkedIcon={false}
                                            name={'autoCalculateMinPrice'}
                                            key={'autoCalculateMinPrice'}
                                            boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
                                            activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
                                            height={18}
                                            width={45}
                                        />
                                        Use minimum lease price from available spaces
                                    </SwitchContainer>
                                }
                            </Col>
                        }
                        {fields.maxPrice && fields.maxPrice.show &&
                            <Col xs={getColSize(fields.maxPrice)}>
                                <GLField<FormInputProps> {...fields.maxPrice.properties} prefix={currencyIcon} suffix={getPriceSuffix()} disabled={(priceDisabled || maxLeasePrice)} use={FormInput} />
                                {autoCalculate &&
                                    <SwitchContainer>
                                        <Switch
                                            onChange={handleMaxLeaseChecked}
                                            checked={maxLeasePrice}
                                            handleDiameter={23}
                                            uncheckedIcon={false}
                                            name={'autoCalculateMaxPrice'}
                                            key={'autoCalculateMaxPrice'}
                                            checkedIcon={false}
                                            boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
                                            activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
                                            height={18}
                                            width={45}
                                        />
                                        Use maximum lease price from available spaces
                                    </SwitchContainer>
                                }
                            </Col>
                        }
                        {fields.salePrice && fields.salePrice.show &&
                            <Col xs={getColSize(fields.salePrice)}>
                                <GLField<FormInputProps> {...fields.salePrice.properties} prefix={currencyIcon} disabled={priceDisabled || maxPrice} use={FormInput} />
                                {autoCalculate &&
                                    <SwitchContainer>
                                        <Switch
                                            onChange={maxPriceChecked}
                                            checked={maxPrice}
                                            handleDiameter={23}
                                            uncheckedIcon={false}
                                            name={'autoCalculateTotalPrice'}
                                            key={'autoCalculateTotalPrice'}
                                            checkedIcon={false}
                                            boxShadow="0px 1px 5px rgba(0, 0, 0, 0.6)"
                                            activeBoxShadow="0px 0px 1px 10px rgba(0, 0, 0, 0.2)"
                                            height={18}
                                            width={45}
                                        />
                                        Use the cumulative sale price from available spaces
                                    </SwitchContainer>
                                }
                            </Col>
                        }
                        {fields.includingSalesTax && fields.includingSalesTax.show && fields.plusSalesTax && fields.plusSalesTax.show &&
                            <Col xs={12} id="salesTax">
                                <GLField<FormRadioGroupProps> name="taxModifer" label="Sales Tax" options={taxTypes} use={FormRadioGroup} />
                            </Col>
                        }
                        {fields.bedrooms && fields.bedrooms.show &&
                            <Col xs={getColSize(fields.maxPrice)}>
                                <GLField<FormInputProps> {...fields.bedrooms.properties} name="bedrooms" mask="9[9]" numericOnly={true} placeholder={'0'} use={FormInput} />
                            </Col>
                        }
                        {fields.contactBrokerForPrice && fields.contactBrokerForPrice.show &&
                            <Col xs={getColSize(fields.contactBrokerForPrice)}>
                                <>
                                    {setDefaultContactBrokerForPrice()}
                                    <GLField<FormCheckboxProps> {...fields.contactBrokerForPrice.properties} use={FormCheckbox} name="contactBrokerForPrice" forceFocus={checkForceFocus("contactBroker")} />
                                </>
                            </Col>
                        }
                        {fields.showPriceWithUoM && fields.showPriceWithUoM.show &&
                            <Col xs={4}>
                                <>
                                    {setDefaultShowPriceWithUoM()}
                                    <GLField<FormCheckboxProps> {...fields.showPriceWithUoM.properties} use={FormCheckbox} forceFocus={checkForceFocus("showPriceWithUoM")} />
                                </>
                            </Col>
                        }
                    </Row>
                </GLForm>
            </SpecificationsContainer>
        )
    } else {
        return <></>;
    }
}

const SpecificationsContainer = styled.div`
    margin-top: 20px;
`;

const SwitchContainer = styled.div`
    display: flex;
    align-items: center;
    margin-top:15px;
    color: rgba(0,0,0,0.68);
    > div {
        margin-right: 20px;
    }
`;


export default SpecificationsView;
