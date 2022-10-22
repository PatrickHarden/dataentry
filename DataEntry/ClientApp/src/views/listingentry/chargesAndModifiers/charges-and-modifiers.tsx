
import React, { FC, useContext, useState } from 'react'
import { Col, Row } from 'react-styled-flexboxgrid'
import SectionHeading from "../../../components/section-heading/section-heading";
import { FormContext } from '../../../components/form/gl-form-context';
import { Listing } from '../../../types/listing/listing';
import styled from 'styled-components';
import MultiDropdown from './multi-dropdown/multi-dropdown';
import { Config } from '../../../types/config/config';
import { useSelector } from 'react-redux';
import { configSelector } from '../../../redux/selectors/system/config-selector';
import StyledButton from '../../../components/styled-button/styled-button';


export interface Props {
    listing: Listing
}

export interface ChargesType {
    chargeType: string,
    chargeModifier: string,
    term: string,
    amount: any,
    perUnitType: string,
    year: any,
    currencyCode: string
}

const ChargesAndModifiers: FC<Props> = (props) => {

    const { listing } = props;

    // set up initial data
    let chargesData: ChargesType[];
    if (listing.chargesAndModifiers) {
        chargesData = listing.chargesAndModifiers;
    } else {
        chargesData = [];
    }


    const [data, setData] = useState<ChargesType[]>(chargesData);
    const config: Config = useSelector(configSelector);
    const formControllerContext = useContext(FormContext);


    // get currency prefix
    let currencyIcon: string = "$";
    if (config && config.currencySymbol) {
        currencyIcon = config.currencySymbol;
    }
    let defaultUnitOfMeasurement:string = "sqm";
    if (config && config.defaultMeasurement && config.defaultMeasurement !== ''){
        defaultUnitOfMeasurement = config.defaultMeasurement
    }


    const getInitialValues = () => {
        // fallback values incase config.chargeTypes aren't present
        const initialValues = ["SalePrice", "From", "Once"];
        if (config.chargesAndModifiers && config.chargesAndModifiers.chargesType[0]){
            const firstChargeType: any = config.chargesAndModifiers.chargesType[0];
            if (firstChargeType.label && firstChargeType.modifiers[0] && firstChargeType.terms[0]){
                initialValues[0] = firstChargeType.value;
                initialValues[1] = firstChargeType.modifiers[0];
                initialValues[2] = firstChargeType.terms[0];
            }
        }
        return initialValues;
    }


    const addRow = () => {
        const temp: ChargesType[] = data;
        const defaultValues: string[] = getInitialValues();
        temp.push({
            chargeType: '',
            chargeModifier: '',
            term: '',
            amount: 0,
            perUnitType: '',
            year: null,
            currencyCode: config.currencies ? config.currencies.defaultValue : ''
        });
        setData([...temp]);
        formControllerContext.onFormChange({ chargesAndModifiers: temp });
    }


    const deleteRow = (index: number) => {
        const temp: ChargesType[] = data;
        temp.splice(index, 1);
        setData([...temp]);
        formControllerContext.onFormChange({ chargesAndModifiers: temp });
    }


    const changeHandler = (values: any, index: number) => {
        const temp: ChargesType[] = data;
        temp[index] = {
            chargeType: values.chargeType,
            chargeModifier: values.chargeModifier,
            term: values.term,
            amount: Number(values.amount),
            perUnitType: values.perUnitType,
            year: values.year,
            currencyCode: values.currencyCode
        };
        setData([...temp]);
        formControllerContext.onFormChange({ chargesAndModifiers: temp });
    }


    if (checkConfigStatus(config)) {
        return (
            <ChargesContainer>
                <Row>
                    <Col id="charges" xs={14}><SectionHeading>Charges</SectionHeading></Col>
                </Row>
                {data.map((iteration: any, index: number) => (
                    <MultiDropdown name={'chargesDropdown' + index} key={index} prefix={currencyIcon}
                        index={index} deleteRow={deleteRow} changeHandler={changeHandler}
                        chargeTypes={config.chargesAndModifiers.chargesType}
                        chargeModifiers={config.chargesAndModifiers.chargeModifier}
                        terms={config.chargesAndModifiers.term}
                        currencies={config.currencies ? config.currencies.options : []}
                        values={iteration}
                        defaultUnitOfMeasurement={defaultUnitOfMeasurement}
                        enablePerUnit={config && config.chargesAndModifiers && config.chargesAndModifiers.enablePerUnit ? config.chargesAndModifiers.enablePerUnit : false}
                        enableYear = {config && config.chargesAndModifiers && config.chargesAndModifiers.enableYear ? config.chargesAndModifiers.enableYear : false}
                        perUnit={config && config.chargesAndModifiers && config.chargesAndModifiers.enablePerUnit && config.chargesAndModifiers.perUnit ? config.chargesAndModifiers.perUnit : null }
                    />
                ))}
                <StyledButton name={'add charge'} onClick={addRow} style={{ marginTop: '25px' }} data-testid="add-charges-and-modifiers" styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp; Add Charge</StyledButton>
            </ChargesContainer>
        )
    } else {
        return <></>
    }
}

export default ChargesAndModifiers;

const ChargesContainer = styled(Col as any)`
    position:relative !important;
    max-width:900px;
    padding-left:0;
    #charges {
        h2 {
            margin-top:45px;
            margin-bottom:0;
            color: #8E9A9D;
            font-size:18px;
        }
    }
`;

const checkConfigStatus = (config: Config) => {
    if (config && config.chargesAndModifiers && config.chargesAndModifiers.show && config.chargesAndModifiers.chargeModifier && config.chargesAndModifiers.chargesType && config.chargesAndModifiers.term){
        return true;
    } else {
        return false
    }
}