import React, { FC } from 'react';
import styled, { css } from 'styled-components';
import FormFieldLabel from '../form-field-label/form-field-label'

export interface FormTextAreaProps {
  name?: string,
  label?: string,
  placeholder?: string,
  defaultValue?: string,
  error?: boolean,
  theme?: any,
  appendToId?: string,
  blurHandler?(e: any): void
}

export interface StyleErrorProps {
  error?: boolean
}

const FormTextArea: FC<FormTextAreaProps> = (props) => {

  const { name, label, placeholder, defaultValue, error, appendToId, blurHandler } = props;

  let id:string | undefined = name;
  if(appendToId && appendToId.length > 0){
    id = id += "_" + appendToId;
  }
  // handle the blur event and use the callback function to set our value upwards
  const handleBlur = (e: React.FormEvent<HTMLTextAreaElement>) => {
    if (blurHandler !== undefined) {
      blurHandler(e.currentTarget.value);
    }
  }

  return (
    <BaseInputContainer>
      <FormFieldLabel label={label} error={error} />
      <BaseFormTextArea id={id} error={error} placeholder={placeholder} name={name} defaultValue={defaultValue} onBlur={handleBlur} />
    </BaseInputContainer>
  );
};

const BaseInputContainer = styled.div`
  width:100%;
`;

const normalBorder = css`
1px solid ${props => (props.theme.colors ? props.theme.colors.border : '#cccccc')}; 
`;

const errorBorder = css` 
1px solid ${props => (props.theme.colors ? props.theme.colors.error : 'red')}; 
`;

const BaseFormTextArea = styled.textarea`
  color: #666666; 
  border: ${(props: StyleErrorProps) => props.error ? errorBorder : normalBorder}; 
  box-sizing: border-box;
  -webkit-box-sizing: border-box; 
  -moz-box-sizing: border-box; 
  display: block;
  flex-grow: 1
  outline: 0; 
  padding: 1em; 
  ::placeholder {color:#dadada;}
  resize: vertical;
  text-align: left;
  text-decoration: none;
  width:100%;
  font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')};
  :focus {
    border-color: ${props => props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C'};
  }
  font-size: ${props => (props.theme.formSize ? props.theme.formSize.input : '14px')}; 
`;

export default FormTextArea;