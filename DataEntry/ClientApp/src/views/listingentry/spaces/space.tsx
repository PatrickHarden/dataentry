import React, { FC, useState, useEffect } from 'react';
import { Col, Row } from 'react-styled-flexboxgrid';
import { Space } from '../../../types/listing/space';
import { Config } from '../../../types/config/config';
import { useSelector } from 'react-redux';
import { configSelector } from '../../../redux/selectors/system/config-selector';
import { SpacesListingTypeFields, SpacesPropertyTypeFields } from '../../../types/config/spaces/spaces';
import { suffixSelector } from '../../../redux/selectors/system/suffix-string-selector';
import SpaceFormView from './space-form';
import { findSpacesFields } from '../../../utils/config/spaces-fields';

interface Props {
    idx: number,
    dataParams: any,
    data: Space,
    handleChange: (values: any, idx: number) => void    
}

export interface ViewProperties {
    spaceIndex: number,
    currencyIcon: string,
    disablePrice: boolean,
    measure: string,
    leaseTerm: string,
    priceSuffix: string,
    autoFocusField: string,
    disableContactBrokerForPricingCheckbox: boolean
}

const SpaceView: FC<Props> = (props) => {

    /* space view is responsible for setting up and handling data for a space */
    const { idx, dataParams, data, handleChange } = props;

    /* find the field configuration based on the property type and listing type the user has selected */
    const config: Config = useSelector(configSelector);

    // first, extract the property type and listing type
    const listingTypeField:string = "listingType";
    const propertyTypeField:string = "propertyType";
    let propertyType:string = "";
    let listingType:string = "";
    if(dataParams && dataParams[propertyTypeField]){
        propertyType = dataParams[propertyTypeField];
    }
    if(dataParams && dataParams[listingTypeField]){
        listingType = dataParams[listingTypeField];
    }

    // grab the field config using a utility function
    const fields:SpacesListingTypeFields | undefined = findSpacesFields(config, propertyType, listingType);

    /* if we have lease type, we need to ensure we set the value if it's not set in our data */
    if(fields && fields.leaseType && fields.leaseType.show && (!data.specifications.leaseType || data.specifications.leaseType.trim().length === 0)){
        if(fields.leaseType.properties && fields.leaseType.properties.defaultValue){
            data.specifications.leaseType = fields.leaseType.properties.defaultValue;
        }
    }

    // Use Unit of Measure DefaultValue as a fall back
    if (fields && fields.spaceUnitOfMeasure && fields.spaceUnitOfMeasure.properties && fields.spaceUnitOfMeasure.properties.defaultValue) {
        if (data && data.specifications && (!data.specifications.measure || data.specifications.measure.trim().length === 0)) {
            data.specifications.measure = fields.spaceUnitOfMeasure.properties.defaultValue
        }
    }

    // Map "Contact Broker For Pricing" checkbox to config, but only if undefined.
    if (fields && fields.contactBrokerForPrice && fields.contactBrokerForPrice.properties && fields.contactBrokerForPrice.properties.defaultValue) {
        if (data.specifications.contactBrokerForPrice === undefined) {
            data.specifications.contactBrokerForPrice = fields.contactBrokerForPrice.properties.defaultValue;
        }

        // If the checkbox is disabled in the config, force it to the default value from config
        if(fields.contactBrokerForPrice.properties.disabled) {
            data.specifications.contactBrokerForPrice = fields.contactBrokerForPrice.properties.defaultValue;
        }
    }

    /* setup our initial space state */
    const [space, setSpace] = useState<Space>(data);

    /* setup our initial view properties object.  this contains items the form relies on to display info to the user (like suffixes) */
    const specsField: string = "specifications";

    /* figure out which suffix to display on the price */
    const listingSuffix = useSelector(suffixSelector);

    const findSuffix = () => {
        let measure:string = "";
        let leaseTerm:string = "";
        let suffix:string = "";

        if(space && space.specifications && space.specifications.measure && space.specifications.measure.trim().length > 0){
            measure = space.specifications.measure;
        }else if(listingSuffix && listingSuffix.measure && listingSuffix.measure.trim().length > 0){
            measure = listingSuffix.measure;
        }

        if(space && space.specifications && space.specifications.leaseTerm && space.specifications.leaseTerm.trim().length > 0){
            leaseTerm = space.specifications.leaseTerm;
        }else if(listingSuffix && listingSuffix.leaseTerm && listingSuffix.leaseTerm.length > 0){
            leaseTerm = listingSuffix.leaseTerm;
        }

        if(measure && measure.length > 0){
            suffix += "/" + measure;
        }
        if(leaseTerm && leaseTerm.length > 0){
            suffix += "/" + leaseTerm;
        }

        return suffix;
    }

    const [viewProperties, setViewProperties] = useState<ViewProperties>({
        spaceIndex: idx,
        currencyIcon: config && config.currencySymbol ? config.currencySymbol : "$",
        disablePrice: space.specifications.contactBrokerForPrice,
        measure: space.specifications.measure,
        leaseTerm: space.specifications.leaseTerm,
        priceSuffix: findSuffix(),
        autoFocusField: "",
        disableContactBrokerForPricingCheckbox: fields && fields.contactBrokerForPrice && fields.contactBrokerForPrice.properties && fields.contactBrokerForPrice.properties.disabled ? true : false
    });

    // our change handler is responsible for setting our update object (user changes) and bubbling up the changes
    const changeHandler = (values: any) => {
 
        const contactBrokerField: string = "contactBrokerForPrice";
        const measureField:string = "measure";
        const leaseTermField: string = "leaseTerm";

        const updateObj:Space = Object.assign(space,{});

        Object.keys(values).forEach(key => {
            updateObj[key] = values[key];
        });

        let updateState:boolean = false;
        const viewProps:ViewProperties = Object.assign({},viewProperties);

        viewProps.autoFocusField = "";

        // contact broker checkbox
        if (values[specsField] && values[specsField][contactBrokerField] !== undefined && values[specsField][contactBrokerField] !== viewProperties.disablePrice) {
            updateState = true;
            viewProps.autoFocusField = "specifications.contactBrokerForPrice";
            viewProps.disablePrice = values[specsField][contactBrokerField];
        }
        // measure
        if(values[specsField] && values[specsField][measureField] !== undefined && values[specsField][measureField] !== viewProperties.measure){
            updateState = true;
            viewProps.autoFocusField = "specifications.measure";
            viewProps.measure = values[specsField][measureField];
        }
        // lease term
        if(values[specsField] && values[specsField][leaseTermField] !== undefined && values[specsField][leaseTermField] !== viewProperties.leaseTerm){
            updateState = true;
            viewProps.autoFocusField = "specifications.leaseTerm";
            viewProps.leaseTerm = values[specsField][leaseTermField];
        }

        if(updateState){
            setSpace(updateObj);
            setViewProperties(viewProps);
        }
        handleChange(updateObj, props.idx);
    }

    const getKey = ():string => {
        if(space && space.id){
            return "space-form-" + space.id;
        }
        return "space-form-create-" + idx;
    }

    useEffect(() => {
        // ensure the suffix is always updated properly
        const suffixCheck:string = findSuffix();
        if(viewProperties && suffixCheck !== viewProperties.priceSuffix){
            const updateVP = Object.assign({},viewProperties);
            updateVP.priceSuffix = suffixCheck;
            setViewProperties(updateVP);
        }
    });

    if(!fields){
        return (
            <Row>
                <Col xs={12}>
                    Error: Field configuration not properly setup.
                </Col>
            </Row>
        );
    }else{
        return <SpaceFormView key={getKey()} idx={idx} space={space} viewProperties={viewProperties} config={config} fields={fields} handleChange={changeHandler} />;
    }
}

export default SpaceView;