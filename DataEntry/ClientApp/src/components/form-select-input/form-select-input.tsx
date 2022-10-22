import React, { FC, ReactElement } from 'react';
import FormFieldLabel from '../form-field-label/form-field-label';
import styled, { css } from 'styled-components';
import FormSelect, { FormSelectProps } from '../form-select/form-select';
import FormInput, { FormInputProps } from '../form-input/form-input'
import { ServerDataType } from '../../types/common/max-lengths';
import GLField from '../form/gl-field';

export interface FormSelectInputProps {
    name: string,
    label?: string,
    options?: any,
    error?: boolean,
    disabled?: boolean,
    index?: number,
    value?: any,
    changeHandler?(e: any): void
}

const FormSelectInput: FC<FormSelectInputProps> = (props) => {
    const { name, label, error, options, index, value, disabled } = props;

    return (
        <>
            <FormFieldLabel label={label} error={error} />
            <AlignElements>
                <GLField<FormSelectProps> key={name + 'currencyCode-' + index} appendToId={name + 'currencyCode'+ index} use={FormSelect} name='currencyCode' isClearable={false} defaultValue={value.currencyCode} options={options} disabled={disabled} />
                <GLField<FormInputProps> key={name + 'Amount-' + index} appendToId={name + 'Amount'+ index} defaultValue={value.amount} numericOnly={true} acceptDecimals={true} serverDataType={ServerDataType.DECIMAL_CURRENCY} index={index} use={FormInput} name="amount"
                    suppressFlexDisplay={true} disabled={disabled} />
            </AlignElements>
        </>
    );
}

export default FormSelectInput;

const AlignElements = styled.div`
    display: flex;

    h5 {
        margin: 0
    }
`;