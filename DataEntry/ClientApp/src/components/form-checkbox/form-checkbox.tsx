import React, { FC, useState } from 'react';
import styled, { css } from 'styled-components';
import FormFieldLabel, {FormFieldLabelStyles} from '../form-field-label/form-field-label';

export interface FormCheckboxProps {
  name?: string,
  label?: string,
  defaultValue?: boolean,
  error?: boolean,
  value?: boolean,
  disabled?: boolean,
  appendToId?: string,
  styles?: FormCheckboxStyles,
  theme?: any,
  testId?: string,
  forceFocus?: boolean,
  changeHandler?(e: any): void,
}

export interface FormCheckboxStyles {
  padding?: string, 
  marginTop?: string,
  labelStyles?: FormFieldLabelStyles,
  backgroundColor?: string,
  checkColor?: string,
  hoverColor?: string,
  error?: boolean,
  outline?: string
}

const FormCheckbox: FC<FormCheckboxProps> = (props) => {

  const { name, label, changeHandler, forceFocus, value, disabled, appendToId, testId } = props;

  let { defaultValue } = props;
  if (!defaultValue) {
    defaultValue = false;
  }
  const [checked, setChecked] = useState<boolean>(defaultValue);
  const [focused, setFocused] = useState<boolean>(false);

  let id:string | undefined = name;
  if(appendToId && appendToId.length > 0){
      id = id += "_" + appendToId;
  }

  // click handler
  const handleClick = (e: React.FormEvent<HTMLInputElement>) => {
    if (changeHandler) {
      if (typeof value === "boolean") {
        setChecked(value)
      } else {
        setChecked(e.currentTarget.checked);
      }
      changeHandler(e.currentTarget.checked);
    }
  }

  const onKeyDown = (e: React.KeyboardEvent) => {
    
    const keyPressed = e.charCode || e.which;
    if (keyPressed === 32) {  // spacebar
      e.preventDefault();
      const newChecked: boolean = !checked;
      setChecked(newChecked);
      if (changeHandler) {
        changeHandler(newChecked);
      }
    }
  }

  const getOutline = () => {
    if(focused){
      return '1px solid ' + (props.theme && props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C');
    }else {
      return '0';
    }
  }

  const onFocus = () => {
      setFocused(true);
  }

  const onBlur = () => {
      setFocused(false);
  }

  return (
    // @ts-ignore
    <BaseInputContainer {...props}>
      <BaseFormInput id={id} name={name} type="checkbox" checked={(typeof value === "boolean") ? value : checked} 
          onChange={handleClick} autoFocus={forceFocus} tabIndex={0} data-testid={testId ? testId : "form-checkbox"} onFocus={onFocus} onBlur={onBlur}/>
      <CheckMark {...props.styles} {...props.error } outline={getOutline()} className={disabled ? 'checkmark disabled' : 'checkmark'} onKeyDown={onKeyDown} />
      <FormFieldLabel label={label} styles={props.styles && props.styles.labelStyles ? props.styles.labelStyles : {}} />
    </BaseInputContainer>
  );
}

const normalBorder = css`
1px solid ${props => (props.theme.colors ? props.theme.colors.border : '#cccccc')}; 
`;

const errorBorder = css` 
1px solid ${props => (props.theme.colors ? props.theme.colors.error : 'red')}; 
`;

const BaseInputContainer = styled.div`
  width:100%;
  display:inline-flex;
  position:relative;
  padding: ${(props: FormCheckboxProps) => props.styles && props.styles.padding ? props.styles.padding : "10px"};
  margin-top: ${(props: FormCheckboxProps) => props.styles && props.styles.marginTop ? props.styles.marginTop : "15px"};
  padding-left:0;
  > h5 {
      font-size: ${(props: FormCheckboxProps) => props.styles && props.styles.labelStyles && props.styles.labelStyles.fontSize ? props.styles.labelStyles.fontSize : "12px"};
      margin:0;
      margin-top:4px;
      color:rgba(0,0,0,0.68);
      margin-left:45px;
  }
  input[type="checkbox"]:checked {
      background: blue;
  } 
  > input {
    position: absolute;
    opacity: 0;
    cursor: pointer;
    height: 100%;
    width: 20px;
    z-index:1;
    background:transparent !important;
    pointer-events: ${(props: FormCheckboxProps) => props.disabled ? "none" : "inherit"};
    user-select: ${(props: FormCheckboxProps) => props.disabled ? "none" : "inherit"};
  }
  input:hover ~ .checkmark {
    background-color: ${(props: FormCheckboxProps) => props.styles && props.styles.hoverColor ? props.styles.hoverColor : '#ccc'};
  }
  .checkmark:after {
    content: "";
    position: absolute;
    display: none;
  }
  input:checked ~ .checkmark:after {
    display: block;
  }
  .checkmark:after {
    left: 9px;
    top: 5px;
    width: 5px;
    height: 10px;
    border: solid ${(props: FormCheckboxProps) => props.styles && props.styles.checkColor ? props.styles.checkColor : props.theme && props.theme.colors ? props.theme.colors.primaryAccent : '#00ff00'};
    border-width: 0 3px 3px 0;
    -webkit-transform: rotate(45deg);
    -ms-transform: rotate(45deg);
    transform: rotate(45deg);
  }
  .checkmark.disabled {
    background-color: #ccc;
  }
`;

const CheckMark = styled.span`
  position: absolute;
  top: 5px;
  left: 0;
  height: 25px;
  width: 25px;
  color: white;
  background: ${(props: FormCheckboxStyles) => props && props.backgroundColor ? props.backgroundColor : '#FFF'};
  border: ${(props: FormCheckboxStyles) => props.error ? errorBorder : normalBorder};
  outline: ${(props: FormCheckboxStyles) => props.outline ? props.outline : '0'}
`;

const BaseFormInput = styled.input`
  color: #436f43;
  box-sizing: border-box;
  -webkit-box-sizing: border-box; 
  -moz-box-sizing: border-box; 
  display: flex;
  outline: 0; 
  padding: 1em; 
  ::placeholder {color:#dadada;}
  text-align: left;
  text-decoration: none;
  text-transform: capitalize;
  width:20px;
  margin-top:0;
  margin-bottom:0;
  height:20px;
  margin-right:15px;
  background:#fff;
`;

export default FormCheckbox;