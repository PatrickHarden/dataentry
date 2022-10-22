import React, { FC } from "react";
import styled from 'styled-components';
import FormSelect, { FormSelectProps } from '../../../../components/form-select/form-select';
import { Option } from '../../../../types/common/option';
import FormInput, { FormInputProps } from '../../../../components/form-input/form-input'
import FormSelectInput, { FormSelectInputProps } from '../../../../components/form-select-input/form-select-input'
import { Col, Row } from 'react-styled-flexboxgrid'
import DeleteIcon from '../../../../assets/images/png/deleteIcon.png';
import { convertValidationJSON } from '../../../../utils/forms/validation-adapter';
import GLField from '../../../../components/form/gl-field';
import GLForm from '../../../../components/form/gl-form';
import { ServerDataType } from '../../../../types/common/max-lengths';
import { generateKey } from "../../../../utils/keys";

export interface MultiDropdownProps extends Option {
    name: string,
    parkingType: Option[],
    interval: Option[],
    currencies: Option[],
    deleteRow: any,
    index: number,
    prefix?: string,
    changeHandler?: (data: any, index: number) => void
}

const MultiDropdown: FC<MultiDropdownProps> = (props) => {

    const { parkingType, interval, deleteRow, index, prefix, value, name, changeHandler, currencies } = props;

    const validations = {};

    const collectData = (data: any) => {
        if (changeHandler) {
            changeHandler(data, index)
        }
    }

    return (
        <GLForm initVals={value}
            validationAdapter={convertValidationJSON}
            validationJSON={validations}
            changeHandler={collectData}
            key={generateKey()}>
            <MultiDropdownContainer data-testid="parkings-row">
                <Row key={generateKey()}>
                    <Col xs={3}>
                        <GLField<FormSelectProps> key={'parkingType-' + index} use={FormSelect} defaultValue={value.parkingType} options={parkingType} disabled={false} name="parkingType" label={(index === 0) ? 'Parking Type' : ' '} />
                    </Col>
                    <Col xs={3}>
                        <GLField<FormInputProps> key={'parkingSpace-' + index} defaultValue={value.parkingSpace} numericOnly={true}
                            acceptDecimals={true} serverDataType={ServerDataType.DECIMAL_CURRENCY} index={index} use={FormInput} name="parkingSpace" prefix={prefix}
                            suppressFlexDisplay={true} disabled={false} label={index === 0 ? 'No of Spaces' : undefined} />
                    </Col>
                    <Col xs={3}>
                        {(currencies && Array.isArray(currencies) && currencies.length > 0) ?
                            <GLField<FormSelectInputProps> key={'parkingAmount-' + index} value={value} use={FormSelectInput} options={currencies} name='parking'
                                label={index === 0 ? 'Price' : undefined} index={index} /> :
                            <GLField<FormInputProps> appendToId={name} key={'parkingAmount-' + index} defaultValue={value.amount} numericOnly={true} acceptDecimals={true} serverDataType={ServerDataType.DECIMAL_CURRENCY} index={index} use={FormInput} name="amount" prefix={prefix}
                                suppressFlexDisplay={true} disabled={false} label={index === 0 ? 'Price' : undefined} />
                        }</Col>
                    <Col xs={3}>
                        <GLField<FormSelectProps> key={'parkingInterval-' + index} use={FormSelect} defaultValue={value.interval} options={interval} disabled={false} name="interval" label={(index === 0) ? 'Interval' : ' '} />
                    </Col>
                    <img style={(index === 0) ? { top: '58px' } : { top: '32px' }} src={DeleteIcon} data-testid="deleteDropdownRow" onClick={() => deleteRow(index)} />
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
        right: -31px;
    }
    > div {
        position:relative;
        margin-top:-5px;
    }
`;