import React, { FC } from 'react';
import FormCheckbox, { FormCheckboxProps } from './form-checkbox';

export interface Props {
    data: object,
    selected: boolean,
    disabled: boolean,
    checkboxProps: FormCheckboxProps,
    changeHandler: (data:object, selected: boolean) => void
}

const FormCheckboxWrapper : FC<Props> = (props) => {

  const { data, selected, disabled, checkboxProps, changeHandler } = props;

  const checkboxChanged = (value:boolean) => {
      changeHandler(data,value);
  }

  return (
    <>
        <FormCheckbox {...checkboxProps} disabled={disabled} changeHandler={checkboxChanged} defaultValue={selected} />
    </>
  );
}

export default FormCheckboxWrapper;