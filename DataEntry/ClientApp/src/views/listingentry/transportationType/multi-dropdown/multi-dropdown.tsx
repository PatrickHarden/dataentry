import React, { FC } from "react";
import styled from 'styled-components';
import FormSelect, { FormSelectProps } from '../../../../components/form-select/form-select';
import FormInput, { FormInputProps } from '../../../../components/form-input/form-input'
import { Col, Row } from 'react-styled-flexboxgrid'
import DeleteIcon from '../../../../assets/images/png/greenDelete.png';
import DragIcon from '../../../../assets/images/png/greenDrag.png'
import { convertValidationJSON } from '../../../../utils/forms/validation-adapter';
import GLField from '../../../../components/form/gl-field';
import GLForm from '../../../../components/form/gl-form';
import { ServerDataType } from '../../../../types/common/max-lengths';
import { TransportationPlace } from '../../../../types/listing/transportationType';

export interface Option {
    label: string,
    value: any,
    order: number
}

export interface MultiDropdownProps {
    name: string,
    travelMode: Option[],
    distanceUnits: Option[],
    deleteRow: any,
    index: number,
    prefix?: string,
    values?: TransportationPlace,
    changeHandler?: any,
    defaultUnitOfMeasurement: any
}

const MultiDropdown: FC<any> = (props) => {

    const { kind, travelMode, distanceUnits, deleteRow, index, prefix, values, name, changeHandler, transportationIndex } = props;

    const validations = {};

    const collectData = (data: any) => {
        changeHandler(data, kind, index)
    }



    return (
        <GLForm initVals={values}
            validationAdapter={convertValidationJSON}
            validationJSON={validations}
            changeHandler={collectData}
            key={Date.now() + name + index}>
            <MultiDropdownContainer data-testid="transportation-type-row" index={index}>
                <Row key={Date.now()}>
                    <Col xs={4}>
                        <GLField<FormInputProps> use={FormInput} defaultValue={values.name} name="name" label={index === 0 ? 'Name' : undefined} />
                    </Col>
                    <Col xs={2}>
                        <GLField<FormInputProps> appendToId={name + 'distances'} defaultValue={values.distances} numericOnly={true} acceptDecimals={true} serverDataType={ServerDataType.DECIMAL_CURRENCY} index={index} use={FormInput} name={'distances'} prefix={prefix}
                            disabled={false} label={index === 0 ? 'Distance' : undefined} />
                    </Col>
                    <Col xs={2}>
                        <GLField<FormSelectProps> use={FormSelect} defaultValue={values.distanceUnits} options={distanceUnits} disabled={false} name="distanceUnits" label={index === 0 ? 'Measurement' : ' '} />
                    </Col>
                    <Col xs={2}>
                        <GLField<FormInputProps> appendToId={name + 'duration'} defaultValue={values.duration} numericOnly={true} acceptDecimals={true} serverDataType={ServerDataType.DECIMAL_CURRENCY} index={index} use={FormInput} name="duration" prefix={prefix}
                            disabled={false} label={index === 0 ? 'Time (minutes)' : undefined} />
                    </Col>
                    <Col xs={2}>
                        <div style={index === 0 ? { marginTop: '53px' } : {}}>
                            <GLField<FormSelectProps> defaultValue={values.travelMode} use={FormSelect} options={travelMode} disabled={false} name="travelMode" label={undefined} />
                        </div>
                    </Col>
                    <img style={(index === 0) ? { top: '58px' } : { top: '42px' }} src={DeleteIcon} data-testid="deleteDropdownRow" onClick={() => deleteRow(index, kind)} />
                    <img style={(index === 0) ? { top: '58px', right: '-70px' } : { top: '42px', right: '-70px' }} src={DragIcon} data-testid="drag" />
                </Row>
            </MultiDropdownContainer>
        </GLForm>
    )
}

export default MultiDropdown;

const MultiDropdownContainer = styled.div<any>`
    width: 100%;
    display: flex;
    > div > img {
        position:absolute;
        cursor:pointer;
        height: 23px;
        width: auto;
        right: -31px;
        margin-top:5px;
    }
    > div {
        position:relative;
        margin-top:-5px;
        width:90%;
        > div div > h5 {
          min-height: ${({ index }) => (index === 0) ? `auto` : `1px`};
        }
    }
`;