import React, { FC, useEffect } from 'react';
import styled, { css } from 'styled-components';
import FormFieldLabel from '../form-field-label/form-field-label';
import InputMask from 'inputmask';
import { ServerDataType, getRestriction } from '../../types/common/max-lengths';
import { formatNumberWithCommas } from '../../utils/forms/format-number-commas';
import { formatDecimals } from '../../utils/forms/format-decimals';
import ReactTooltip from 'react-tooltip';
import TooltipOff from '../../assets/images/png/tooltip-off.png'
import TooltipOn from '../../assets/images/png/tooltip-on.png'

export interface FormInputProps {
  name: string,
  label?: string,
  subText?: string,
  defaultValue?: any,
  placeholder?: string,
  prefix?: string;
  suffix?: string;
  error?: boolean,
  title?: string,
  theme?: any,
  quoteID?: number,
  index?: number,
  appendToId?: string,
  numericOnly?: boolean,
  allowNegatives?: boolean,
  acceptDecimals?: boolean,
  maxDecimalPlaces?: number,
  useOnChange?: boolean,
  forceFocus?: boolean,
  disabled?: boolean,
  disableTab?: boolean,
  mask?: string,
  toolTip?: string,
  serverDataType?: ServerDataType,
  inputvalue?: any,
  suppressFlexDisplay?: boolean,
  labelColor?: string,
  blurHandler?(values: any): void,
  indexedBlurHandler?(index: any, value: any): void
}

const FormInput: FC<FormInputProps> = (props) => {
  const { name, label, subText, defaultValue, placeholder, error, index, appendToId, blurHandler,
    indexedBlurHandler, prefix, suffix, numericOnly, allowNegatives, useOnChange, title,
    acceptDecimals, maxDecimalPlaces, forceFocus, disabled, mask, serverDataType, inputvalue, toolTip, disableTab, suppressFlexDisplay, labelColor } = props;

  let ctrlDown: boolean = false; // tracking of control key 
  let shiftDown: boolean = false; // tracking of shift key
  let selected: boolean = false; // text selection

  // decimal place restrictions can be passed in directly, but for any not set, this will handle the price fields (2 decimal places)
  let decimalRestriction = maxDecimalPlaces;
  if (!maxDecimalPlaces && acceptDecimals && serverDataType && serverDataType === ServerDataType.DECIMAL_CURRENCY) {
    decimalRestriction = 2;
  }

  // we use an id to handle grabbing our focus objects, so we need to assign it here
  let id: string = name;

  // note: appendToId overrides an index, so only use this option if you know that index isn't being injected to affect the id
  if (appendToId && appendToId.length > 0) {
    id = id + "_" + appendToId;   // used in automated testing when we need to append an id.  For example, with spaces, it needs a unique id for each field.
  } else if (index && index > -1) {
    id = id + "_" + index;  // if we have an index, this is part of a set so make sure the id is unique otherwise bugs will happen
  }

  // build the container style
  let containerStyle = {};
  if (!suppressFlexDisplay) {
    containerStyle = { display: "flex" }
  }
  // ensure numeric values are formatted with commas
  const formatValue = (value: any) => {
    if (!value) {
      return value;
    }
    if (numericOnly) {
      if (acceptDecimals && decimalRestriction) {
        value = formatDecimals(value, decimalRestriction);
      }
      value = formatNumberWithCommas(value);
    }
    return value;
  }

  const formattedValue: any = formatValue(defaultValue);   // this is what gets set in our default value

  // handle the blur event and use the callback function to set our value upwards
  const handleBlur = (e: React.FormEvent<HTMLInputElement>) => {
    bubbleChange(e.currentTarget.value);
  }

  const handleChange = (e: React.FormEvent<HTMLInputElement>) => {
    if (useOnChange) {
      bubbleChange(e.currentTarget.value);
    }
  }

  const bubbleChange = (value: any) => {
    // if this is a numeric only field, prepare the value to be a number
    if (numericOnly) {
      if (value && typeof value.replace === "function") {
        // remove any commas
        value = value.replace(/,/g, '');
        // remove leading 0s
        value = value.replace(/^0+/, '');
        // ensure value is a number
        value = Number(value);
      }
    }

    if (blurHandler !== undefined) {
      blurHandler(value);
      if (numericOnly && id) {
        const ele: any = document.getElementById(id);
        ele.value = formatValue(value);
      }
    }
    if (indexedBlurHandler !== undefined) {
      indexedBlurHandler(index, value);
    }
  }

  const onKeyDown = (e: React.KeyboardEvent) => {

    // the key code for this key press
    const keyCode = e.keyCode ? e.keyCode : e.which;

    if (numericOnly) {

      if (!ctrlDown) {
        ctrlDown = e.ctrlKey || e.metaKey;
      }

      if (!shiftDown) {
        shiftDown = e.shiftKey;
      }

      const numbers: number[] = [48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105];
      const decimals: number[] = [110, 190];
      const tab: number[] = [9];
      const shift: number[] = [16];
      const backspace: number[] = [8, 46];  // backspace and delete
      const enter: number[] = [13];
      const arrowKeys: number[] = [37, 39];
      const ctrlFunctions: number[] = [67, 86, 88];  // c, v, x
      const commas: number[] = [188]; // commas
      const negatives: number[] = [109, 189]; // - (keypad), -

      let validKeyCodes: number[] = [...numbers, ...tab, ...backspace, ...enter, ...arrowKeys, ...shift, ...commas];

      if (acceptDecimals) {
        validKeyCodes = [...validKeyCodes, ...decimals];
      }
      if (ctrlDown) {
        validKeyCodes = [...validKeyCodes, ...ctrlFunctions];
      }
      if (shiftDown) {
        validKeyCodes = [];
      }
      if (allowNegatives) {
        validKeyCodes = [...validKeyCodes, ...negatives];
      }

      // if ctrl key is down and key is not a ctrl function, check ctrl key again
      if (ctrlDown && ctrlFunctions.indexOf(keyCode) === -1) {
        ctrlDown = e.ctrlKey;
      }
      if (shiftDown) {  // check the shift key again (otherwise it wont be false)
        shiftDown = e.shiftKey;
      }

      let allowInput: boolean = true;
      const ele: any = document.getElementById(id);
      if (ele && ele.value) {
        const val = ele.value;

        // if the user has entered decimals, check to see if they already have one (and don't allow)
        if (decimals.indexOf(keyCode) > -1 && acceptDecimals && val.indexOf(".") > -1) {
          allowInput = false;
        }
        // if the user has entered a numeric character, check to see if the length after the decimal is maxDecimalPlaces (and dont allow)
        if (decimalRestriction && numbers.indexOf(keyCode) > -1 && acceptDecimals && val.substr(val.indexOf("."), val.length).length > decimalRestriction && !selected) {
          allowInput = false;
        }
        // if user has the ctrl key held down and the key is not a valid ctrl function, then don't allow
        if (ctrlDown && ctrlFunctions.indexOf(keyCode) === -1) {
          allowInput = false;
        }
        // max number size check
        if (serverDataType && numbers.indexOf(keyCode) > -1) {
          const proposedValue: string = val + String.fromCharCode(keyCode);
          if (Number(proposedValue) > getRestriction(serverDataType)) {
            allowInput = false;
          }
        }
      }

      selected = false; // ensure if selected and a key input happens, it's no longer selected (for decimal #s)

      if (!allowInput || validKeyCodes.indexOf(keyCode) === -1) {
        e.preventDefault();
      }
    }
  }

  const onPaste = (e: React.ClipboardEvent) => {

    const ele: any = document.getElementById(id);
    if (ele && numericOnly) { // only worry about pasting on numeric only fields
      e.stopPropagation();
      e.preventDefault();
      // get the clipboard paste data
      const clipboardData = e.clipboardData;
      if (clipboardData) {
        let pasted = clipboardData.getData('Text');
        if (pasted && pasted.length > 0) {
          let regex;
          if (acceptDecimals) {
            if (allowNegatives) {
              regex = /[^0-9.-]/g;
            } else {
              regex = /[^0-9.]/g;
            }

            // remove anything after first decimal (+ 2 characters)
            if (decimalRestriction && pasted.indexOf(".") > -1) {
              let decLength = pasted.substr(pasted.indexOf("."), pasted.length).length;
              if (decLength <= 1) {
                regex = /[^0-9]/g;  // if we don't have anything after the decimal, then we will strip the decimal too
              } else if (decLength > decimalRestriction + 1) {
                decLength = decimalRestriction + 1;
              }
              pasted = pasted.substr(0, pasted.indexOf(".") + decLength);
            }
          } else {
            // remove anything after first decimal, if there is 
            if (pasted.indexOf(".") > -1) {
              pasted = pasted.substr(0, pasted.indexOf("."));
            }
            if (allowNegatives) {
              regex = /[^0-9.-]/g;
            } else {
              regex = /[^0-9]/g;
            }
          }
          // remove any non-numeric from remaining string using regex
          pasted = pasted.replace(regex, "").replace(/ +/, " ");
          // final check is on size, if set
          if (serverDataType) {
            const dataTypeVal: Number = getRestriction(serverDataType);
            if (Number(pasted) > dataTypeVal) {
              pasted = pasted.substr(0, dataTypeVal.toString().length);
              if (Number(pasted) > dataTypeVal) { // one more check to see if the value is > numerically
                pasted = pasted.substr(0, pasted.length - 1);  // if so, do the final cut down
              }
            }
          }
          ele.value = formatValue(pasted);
        }
      }
    }
  }

  const onContainerFocus = () => {
    const ele: any = document.getElementById(id);
    if (ele) {
      selected = true;
      ele.select();
    }
  }

  useEffect(() => {
    if (mask && mask.length > 0) {
      const ele = document.getElementById(id);
      if (ele) {
        const inputMask = new InputMask(mask);
        inputMask.mask(ele);
      }
    }
  }, []);

  let inputbox = null;
  if (prefix || suffix) {

    let pre = null;
    if (prefix) {
      pre = <Prefix>{prefix}</Prefix>;
    }

    let suf = null;
    if (suffix) {
      suf = <Suffix>{suffix}</Suffix>;
    }

    inputbox = <ExtrasInputContainer name={name} error={error} tabIndex={disableTab ? -1 : 0} onFocus={onContainerFocus}>{pre}<ExtrasFormInput id={id} placeholder={placeholder} defaultValue={formattedValue} value={inputvalue} onBlur={handleBlur} onChange={handleChange} onKeyDown={onKeyDown} onPaste={onPaste} autoFocus={forceFocus} disabled={disabled} />{suf}</ExtrasInputContainer>
  } else {
    inputbox = <BaseFormInput id={id} name={name} placeholder={placeholder} error={error} tabIndex={disableTab ? -1 : 0} defaultValue={formattedValue} value={inputvalue} onBlur={handleBlur} onChange={handleChange} onKeyDown={onKeyDown} onPaste={onPaste} autoFocus={forceFocus} disabled={disabled} />;
  }

  const labelStyles = labelColor ? { color: labelColor } : {}

  return (
    <BaseInputContainer>
      <div style={containerStyle}>
        <FormFieldLabel label={label} error={error} title={title} styles={labelStyles} />
        {subText &&
          <StyledSubtext>{subText}</StyledSubtext>
        }
        {toolTip && <>
          <IconTag data-tip="true" data-for={toolTip}><img src={TooltipOff} onMouseOver={e => (e.currentTarget.src = TooltipOn)} onMouseOut={e => (e.currentTarget.src = TooltipOff)} /></IconTag>
          <StyledTooltip id={toolTip} type='light' border={false} place='right' multiline={true}>
            <span>{toolTip}</span>
          </StyledTooltip>
        </>}
      </div>
      {inputbox}
    </BaseInputContainer>
  );
}

const IconTag = styled.a`
  margin-block-start: 2em;
  margin-left: .5em; 
`;
const StyledSubtext = styled.div`
  color: #9EA8AB;
  margin-block-start: 2.0em;
  margin-left: 1.5em;
  font-family: "futura PT Book italic";
  font-size: 14px;
`;

const StyledTooltip = styled(ReactTooltip)`
  box-shadow: 0 0 4px 2px grey;
  width: 250px;
  font-family: "Futura Md BT", helvetica, arial, sans-serif;
  font-size: 12px;
  font-weight: 500;
  text-transform: none;
`;
const BaseInputContainer = styled.div`
  width:100%;
`;

const normalBorder = css`
1px solid ${(props: FormInputProps) => (props.theme.colors ? props.theme.colors.border : '#cccccc')}; 
`;

const errorBorder = css` 
1px solid ${(props: FormInputProps) => (props.theme.colors ? props.theme.colors.error : 'red')}; 
`;

const BaseFormInput = styled.input`
  color: #666666; 
  border: ${(props: FormInputProps) => props.error ? errorBorder : normalBorder}; 
  box-sizing: border-box;
  -webkit-box-sizing: border-box; 
  -moz-box-sizing: border-box; 
  display: block;
  flex-grow: 1
  outline: 0; 
  padding: 0.94em; 
  ::placeholder {color:#cccccc;}
  text-align: left;
  text-decoration: none;
  width:100%;
  font-weight: ${(props: FormInputProps) => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${(props: FormInputProps) => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  :focus {
    border-color: ${(props: FormInputProps) => props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C'};
  }
  font-size: ${(props: FormInputProps) => (props.theme.formSize ? props.theme.formSize.input : '14px')}; 
`;

// styles below are for the "extras" input with a prefix/suffix

const ExtrasInputContainer = styled.div`
  border: ${(props: FormInputProps) => props.error ? errorBorder : normalBorder};  
  background-color: #fff;
  box-sizing: border-box;
  -webkit-box-sizing: border-box; 
  -moz-box-sizing: border-box; 
  display: flex;
  flex-direction: row;
  outline: 0;
  width:100%;
  :focus-within {
    outline: 1px solid ${(props: FormInputProps) => props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C'};
  }
`;

const ExtrasFormInput = styled.input`
  border: 0;
  padding: 1em;
  outline: 0;
  color: #666666; 
  font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  font-size: ${props => (props.theme.formSize ? props.theme.formSize.input : '14px')};
  ::placeholder {color:#cccccc;}
  text-align: left;
  text-decoration: none;
  vertical-align: text-bottom;
  width: 100%;
`;

const Prefix = styled.div`
  color: #8E9A9D;
  text-align: center;
  font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  font-size: 12px;
  margin-top:2px;
  cursor: pointer;
  padding: 11px 5px 5px 5px;
  min-width: 35px;
`;

const Suffix = styled.div`
  color: #8E9A9D;
  text-align:right;
  font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  font-size: 12px;
  margin-top:2px;
  cursor: pointer;
  padding: 11px 5px 5px 5px;
`;

export default FormInput;