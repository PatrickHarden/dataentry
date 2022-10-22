import React, { FC, useState, useEffect } from 'react';
import styled, { css } from 'styled-components';
import FormFieldLabel from '../form-field-label/form-field-label';
import { triggerFocusChange } from '../../utils/forms/trigger-focus-change';

export interface FormRadioButtonProps {
    name: string,
    label?: string,
    defaultValue?: any,
    error: boolean,
    optionVal?: any,
    theme?: any,
    forceFocus: boolean,
    selectedColor? : string,
    changeHandler?(e: any): void
}

interface StyleProps {
    theme?: any,
    selectedColor? : string,
    error: boolean,
    focused: boolean
}

const FormRadioButton: FC<FormRadioButtonProps> = (props) => {

    const { name, label, defaultValue, optionVal, error, forceFocus, selectedColor, changeHandler } = props;

    // handle user mouse down to select the item
    const mouseDown = (e: any) => {

        triggerFocusChange();
        
        if (changeHandler !== undefined) {
            changeHandler(optionVal);
        }
    }

    return (
        <BaseInputContainer error={error} focused={forceFocus} selectedColor={selectedColor} onMouseDown={mouseDown}>
            <StyledLabel>
                <BaseFormInput id={name + "_" + optionVal} className="radio-btn" name={name} checked={optionVal === defaultValue} value={optionVal} type="radio" readOnly={true}/>
                <FormFieldLabel label={label} error={error} />
            </StyledLabel>
        </BaseInputContainer>
    );
}

const normalBorder = css`
    1px solid ${props => (props.theme.colors ? props.theme.colors.border : '#cccccc')}; 
`;

const errorBorder = css` 
    1px solid ${props => (props.theme.colors ? props.theme.colors.error : 'darkred')}; 
`;

const focusBorder = css`
    1px solid ${props => (props.theme.colors ? props.theme.colors.primaryAccentLght : '#29BC9C')};
`;

const selectedFill = css`
${props => (props.theme.colors ? props.theme.colors.secondaryAccent : '#A9A9A9')}; 
`;

const BaseInputContainer = styled.div`
  margin: 0 0 0 0;
  padding: 0 0 0 0;
  float:left;

  > label {
        margin: 0 5px 0 0;
        padding: 0 0 0 0;
        float:left;
        background-color: ${(props: StyleProps) => props.theme.colors ? props.theme.colors.primaryBackground : "#fbfbfb"}                     
        border-radius:20px;
        border: ${(props: StyleProps) => props.error ? errorBorder : props.focused ? focusBorder : normalBorder};
        overflow:auto;
        cursor:pointer;
  }

  > label h5 {
        text-align:center;
        text-transform: capitalize;
        font-size: 1em;
        color: #AEAEAE;
        padding: 0.75em 1.5em 0.75em 1.5em;
        margin: 0 0 0 0;
        display:block;
  }

  > label input {
        position:absolute;
        top:-20px;
        visibility:hidden;
  }

  > label input:checked + h5 {
        background-color: ${props => (props.selectedColor ? props.selectedColor : (props.theme.colors ? props.theme.colors.tertiaryAccent : 'deepskyblue'))};
        color:#FFFFFF;
    }
`;

const StyledLabel = styled.label``;

const BaseFormInput = styled.input`
`;

export default FormRadioButton;