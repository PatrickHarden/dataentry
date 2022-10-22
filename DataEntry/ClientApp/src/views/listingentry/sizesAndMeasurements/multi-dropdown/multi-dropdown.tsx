import React, { FC } from "react";
import styled from 'styled-components';
import FormSelect, { FormSelectProps } from '../../../../components/form-select/form-select';
import { Option, SizeTypeOption } from '../../../../types/common/option';
import FormInput, { FormInputProps } from '../../../../components/form-input/form-input'
import { Col, Row } from 'react-styled-flexboxgrid'
import DeleteIcon from '../../../../assets/images/png/deleteIcon.png';
import { convertValidationJSON } from '../../../../utils/forms/validation-adapter';
import GLField from '../../../../components/form/gl-field';
import GLForm from '../../../../components/form/gl-form';

export interface MultiDropdownProps {
    name: string,
    sizeKind: SizeTypeOption[],
    measureUnit: Option[],
    deleteRow: any,
    index: number,
    prefix?: string,
    values?: any,
    changeHandler?: any,
    defaultUnitOfMeasurement: any
    
}

const MultiDropdown: FC<MultiDropdownProps> = (props) => {

    const { sizeKind, measureUnit, deleteRow, index, prefix, values, name, changeHandler } = props;

    const validations = {};


    let updatedMeasurement: Option[] = [];
    const emptyValues = {
        sizeKind: '',
        measureUnit: '',
        amount: 0
    }


    const constructMeasurementOptions = (currentSizeType: SizeTypeOption) => {
        const sizeKindAllowed: string[] = currentSizeType.unit;
        for (const unit of measureUnit) {
            if (sizeKindAllowed.includes(unit.value)){
                updatedMeasurement.push(unit)
            }
        }
    }


    // set modifiers and terms to the default options if values being passed in are empty (a new charge has just been created)
    // if values are passed in, reference the chargeType array in the config, and extract the options from it.
    if (values === emptyValues) {
        updatedMeasurement = measureUnit;
    } else {
        for (const sizeType of sizeKind) {
            if (sizeType.value === values.sizeKind) {
                if (sizeType.unit) {
                    constructMeasurementOptions(sizeType);
                }
                
            }
        }
    }

    // The data from graphql does not match the input name and id
    // So this component needs to message the data model to property render
    const collectData = (data: any) => {
        const changedData = {
            sizeKind: data.sizeKind,
            measureUnit: data.measureUnit,
            amount: data.sizeAmount
        }
        changeHandler(changedData, index);
    }

    // transform to size kind values
    const sizeValues = {
        sizeKind: values.sizeKind,
        measureUnit: values.measureUnit,
        sizeAmount: values.amount
    }

    return (
        <GLForm initVals={sizeValues}
            validationAdapter={convertValidationJSON}
            validationJSON={validations}
            changeHandler={collectData}
            key={Date.now() + name + index}>
            <MultiDropdownContainer>
                <Row key={Date.now()}>
                    <Col xs={3}>
                        <GLField<FormSelectProps> use={FormSelect} options={sizeKind} name="sizeKind" label={(index === 0) ? 'Size Type' : undefined} />
                    </Col>
                    <Col xs={3}>
                        <GLField<FormSelectProps> use={FormSelect} options={updatedMeasurement} disabled={updatedMeasurement.length < 1 || values.sizeKind === ''} name="measureUnit" label={(index === 0) ? 'Unit of Measure' : undefined} />
                    </Col>
                   <Col xs={3}>
                        <GLField<FormInputProps> numericOnly={true} acceptDecimals={true} index={index} use={FormInput} name="sizeAmount" prefix={prefix} disabled={values.sizeKind === '' || values.measureUnit === ''} suppressFlexDisplay={true} label={(index === 0) ? 'Size' : undefined} />
                    </Col>
                    <img style={(index === 0) ? {top: '58px'} : {top: '32px'}} src={DeleteIcon} onClick={() => deleteRow(index)} />
                </Row>
            </MultiDropdownContainer>
        </GLForm>
    )
}

export default MultiDropdown;

const MultiDropdownContainer = styled.div`
    margin: 20px 0;
    > div > img {
        position:absolute;
        cursor:pointer;
        height: 32px;
        width: auto;
        right: 215px;
    }
    > div {
        position:relative;
        margin-top:-5px;
    }
`;