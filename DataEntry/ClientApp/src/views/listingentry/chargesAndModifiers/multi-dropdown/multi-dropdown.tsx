import React, { useState, FC } from "react";
import styled from 'styled-components';
import FormSelect, { FormSelectProps } from '../../../../components/form-select/form-select';
import { Option, ChargeTypeOption } from '../../../../types/common/option';
import FormInput, { FormInputProps } from '../../../../components/form-input/form-input'
import { Col, Row } from 'react-styled-flexboxgrid'
import DeleteIcon from '../../../../assets/images/png/deleteIcon.png';
import { convertValidationJSON } from '../../../../utils/forms/validation-adapter';
import GLField from '../../../../components/form/gl-field';
import GLForm from '../../../../components/form/gl-form';
import { ServerDataType } from '../../../../types/common/max-lengths';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import FormFieldLabel from '../../../../components/form-field-label/form-field-label';
import FormSelectInput, { FormSelectInputProps } from '../../../../components/form-select-input/form-select-input'


export interface MultiDropdownProps {
    name: string,
    enablePerUnit: boolean,
    enableYear: boolean,
    chargeTypes: ChargeTypeOption[],
    chargeModifiers: Option[],
    terms: Option[],
    perUnit: Option[] | null,
    deleteRow: any,
    index: number,
    prefix?: string,
    values?: any,
    changeHandler?: any,
    defaultUnitOfMeasurement: any,
    currencies?: Option[]
}

const MultiDropdown: FC<MultiDropdownProps> = (props) => {

    const { enablePerUnit, enableYear, chargeTypes, chargeModifiers, defaultUnitOfMeasurement, terms, perUnit, deleteRow, index, prefix, values, name,
        changeHandler, currencies } = props;

    const validations = {};


    let updatedModifiers: Option[] = [];
    let updatedTerms: Option[] = [];
    let disableAmount: boolean = false;
    const showCurrencyDropdown: boolean = (!!currencies && Array.isArray(currencies) && currencies.length > 0);
    const emptyValues = {
        chargeType: '',
        chargeModifier: '',
        term: '',
        amount: 0,
        perUnitType: '',
        year: 2011
    }


    const constructModifierOptions = (currentChargeType: ChargeTypeOption) => {
        const chargeModifiersAllowed: string[] = currentChargeType.modifiers;
        for (const modifier of chargeModifiers) {
            if (chargeModifiersAllowed.includes(modifier.value)) {
                updatedModifiers.push(modifier)
            }
        }
    }


    const constructTermOptions = (currentChargeType: ChargeTypeOption) => {
        const termsAllowed: string[] = currentChargeType.terms;
        for (const term of terms) {
            if (termsAllowed.includes(term.value)) {
                updatedTerms.push(term)
            }
        }
    }


    // set modifiers and terms to the default options if values being passed in are empty (a new charge has just been created)
    // if values are passed in, reference the chargeType array in the config, and extract the options from it.
    if (values === emptyValues) {
        updatedModifiers = chargeModifiers;
        updatedTerms = terms;
    } else {
        for (const chargeType of chargeTypes) {
            if (chargeType.value === values.chargeType) {
                if (chargeType.modifiers) {
                    constructModifierOptions(chargeType);
                }
                if (chargeType.terms) {
                    constructTermOptions(chargeType);
                }
                if (chargeType.amount === false) {
                    disableAmount = true;
                }
            }
        }
    }

    const collectData = (data: any) => {
        changeHandler(data, index)
    }

    const handleSwitchChange = (selected: Date | null) => {
        const temp = values;
        temp.year = selected && selected.getFullYear();
        changeHandler(temp, index);
    }


    return (

        <GLForm initVals={values}
            validationAdapter={convertValidationJSON}
            validationJSON={validations}
            changeHandler={collectData}
            key={Date.now() + name + index}>
            <MultiDropdownContainer data-testid="charges-and-modifiers-row">
                <Row key={Date.now()}>
                    <Col xs={enablePerUnit ? 2.5 : 3}>
                        <GLField<FormSelectProps> use={FormSelect} options={chargeTypes} name="chargeType" label={(index === 0) ? 'Charge Type' : undefined} />
                    </Col>
                    <Col xs={showCurrencyDropdown ? 1.5 : 2.5}>
                        <GLField<FormSelectProps> use={FormSelect} options={updatedModifiers} disabled={updatedModifiers.length < 1 || values.chargeType === ''} name="chargeModifier" label={(index === 0) ? 'Charge Modifier' : undefined} />
                    </Col>
                    <Col xs={showCurrencyDropdown && enablePerUnit ? 3.0 : 2.0}>
                        {showCurrencyDropdown ?
                            <GLField<FormSelectInputProps> key={'chargeAmount-' + index} value={values} use={FormSelectInput} options={currencies} name='charges'
                                label={index === 0 ? 'Amount' : undefined} index={index} disabled={disableAmount || values.chargeType === '' || values.chargeModifier === 'OnApplication'} /> :
                            <GLField<FormInputProps> appendToId={ name } numericOnly={true} acceptDecimals={true} serverDataType={ServerDataType.DECIMAL_CURRENCY} index={index} use={FormInput}
                                suppressFlexDisplay={true} name="amount" prefix={prefix} disabled={disableAmount || values.chargeType === '' || values.chargeModifier === 'OnApplication'} label={(index === 0) ? 'Amount' : undefined} />
                        }
                    </Col>
                    {enablePerUnit &&
                        <Col xs={2.5}>
                            <GLField<FormSelectProps> use={FormSelect} options={perUnit} disabled={updatedModifiers.length < 1 || values.chargeType === ''} name="perUnitType" label={(index === 0) ? 'Unit of Measure' : undefined} />
                        </Col>}
                    <Col xs={enablePerUnit ? 1.5 : 2}>
                        <GLField<FormSelectProps> use={FormSelect} options={updatedTerms} disabled={updatedTerms.length < 1 || values.chargeType === ''} name="term" label={(index === 0) ? 'Term' : undefined} />
                    </Col>
                    {enableYear &&
                        <Col xs={1.2}>
                            <FormFieldLabel label={(index === 0 ? "Year Assessed" : undefined)} />
                            <StyledDatePicker
                                id={"YearPicker" + index}
                                name="year"
                                selected={values.year ? new Date(values.year + '-06-01T01:00:00Z') : null}
                                onChange={handleSwitchChange}
                                dateFormat="yyyy"
                                showYearPicker={true}
                                yearItemNumber={9}
                                autoComplete="off"
                                minDate={new Date('2018-01-02T01:00:00Z')}
                                maxDate={new Date('2035-01-02T01:00:00Z')}
                            />
                        </Col>}
                    <img style={(index === 0) ? { top: '58px' } : { top: '32px' }} src={DeleteIcon} data-testid="deleteDropdownRow" onClick={() => deleteRow(index)} />
                </Row>
            </MultiDropdownContainer>
        </GLForm>
    )
}

export default MultiDropdown;

const MultiDropdownContainer = styled.div`
    margin: 20px 0; 
    width: 1000px;
    > div > img {
        position:absolute;
        cursor:pointer;
        height: 32px;
        width: auto;
        right: -31px;
    }
    > div {
        position:relative;
        margin-top:-5px;
    }
`;

const StyledDatePicker = styled(DatePicker)`
color: #666666;
box-sizing: border-box;
border: 1px solid #cccccc;
-webkit-box-sizing: border-box; 
-moz-box-sizing: border-box; 
display: block;
flex-grow: 1;
outline: 0; 
padding: 0.93em; 
::placeholder {color:#dadada;}
text-align: left;
text-decoration: none;
text-transform: uppercase;
width:150px;
font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
font-size: ${props => (props.theme.formSize ? props.theme.formSize.input : '14px')};
margin: 0;
vertical-align: middle;
`;