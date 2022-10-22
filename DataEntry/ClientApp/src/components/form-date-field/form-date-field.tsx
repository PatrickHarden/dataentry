import React, { FC, useState } from 'react';
import styled, { css } from 'styled-components';
import FormFieldLabel from '../form-field-label/form-field-label';
import DatePicker from 'react-datepicker';  // reference: https://reactdatepicker.com/
import CalendarIcon from '../../assets/images/png/calendarIcon.png';
import "../../assets/css/react-datepicker.css";

export interface FormDateFieldProps {
    name?: string,
    label?: string,
    defaultValue?: any,
    placeholder?: string,
    format?: string,
    restrictInput?: boolean,
    error?: boolean,
    theme? :any,
    appendToId?: string,
    changeHandler?(value:any) : void
}

const FormDateField:FC<FormDateFieldProps> = (props) => {
    const { name, label, placeholder, format, restrictInput, error, appendToId, changeHandler } = props;
    let { defaultValue } = props;

    if(defaultValue !== undefined && !(defaultValue instanceof Date) && defaultValue !== null){
        defaultValue = new Date(defaultValue);
    }

    let id:string | undefined = name;
    if(appendToId && appendToId.length > 0){
        id = id += "_" + appendToId;
    }

    const [currentDate, setCurrentDate] = useState(defaultValue);

    const dateFormat = format ? format : "MM/dd/yyyy";

    // handle the change event using our callback
    const handleChange = (date:any) => {
        setCurrentDate(date);
        if(changeHandler !== undefined){
          changeHandler(date);
        }
    }

    // this method simulates a click on the input if the user clicks the icon
    const clickIcon = () => {
        const picker:any = name ? document.getElementById(name) : undefined;
        if(picker !== undefined){
            picker.dispatchEvent(new MouseEvent('click',{view: window, bubbles: true, cancelable: true}));
        }
    }

    const onKeyDown = (e:React.KeyboardEvent) => {
        const keyCode = e.keyCode ? e.keyCode : e.which;
        const allowed:number[] = [8,46];  // 8 = backspace, 46 = delete

        if(keyCode && keyCode !== 9){   // 9 = tab, so let it happen naturally
          // if not tab, check for allowed key strokes
          if(keyCode && allowed.indexOf(keyCode) > -1){
              setCurrentDate(null);
              handleChange(null);
          }else if(restrictInput){
            // if input restricted, don't allow typing outside of anything we allow above
              e.preventDefault();
          }
        }
    }

    return(
      <BaseInputContainer>
        <FormFieldLabel label={label} error={error}/>
        <StyledInputDiv>
          <StyledDatePicker id={id} name={name} placeholderText={placeholder} autoComplete="off"
              dateFormat={dateFormat} selected={currentDate} onKeyDown={onKeyDown}
              onChange={handleChange}/>
          <StyledCalendarIcon src={CalendarIcon} onClick={clickIcon}/>
        </StyledInputDiv>
      </BaseInputContainer>
    );
  }

const BaseInputContainer = styled.div`
  width: 100%;
`;

const normalBorder = css`
1px solid ${props => (props.theme.colors ? props.theme.colors.border : '#cccccc')}; 
`;

const errorBorder = css` 
1px solid ${props => (props.theme.colors ? props.theme.colors.error : 'red')}; 
`;

const StyledInputDiv = styled.div`
  border: ${(props: FormDateFieldProps) => props.error ? errorBorder : normalBorder}; 
  display: flex;
  position: relative;
  :focus-within {
    outline: 1px solid ${props => props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C'};
  }
`;

const StyledDatePicker = styled(DatePicker)`
  color: #666666;
  box-sizing: border-box;
  border: none;
  -webkit-box-sizing: border-box; 
  -moz-box-sizing: border-box; 
  display: block;
  flex-grow: 1;
  outline: 0; 
  padding: 1em; 
  ::placeholder {color:#dadada;}
  text-align: left;
  text-decoration: none;
  text-transform: uppercase;
  width:100%;
  font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  font-size: ${props => (props.theme.formSize ? props.theme.formSize.input : '14px')};
  margin: 0;
  vertical-align: middle;
`;

const StyledCalendarIcon = styled.img` 
  vertical-align: middle;
  border: none;
  background-color: #FFFFFF;
  padding: .65em .75em;
  cursor: pointer;
`

  export default FormDateField;