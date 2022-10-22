import React, { FC, useState, useEffect } from 'react';
import styled from 'styled-components';
import FormFieldLabel from '../form-field-label/form-field-label';
import FormRadioButton from './form-radiobutton';
import { Option } from '../../types/common/option';

export interface FormRadioGroupProps {
    name: string,
    label?: string,
    options? : any,
    defaultValue?: any,
    error?: boolean,
    forceFocus? : boolean,
    changeHandler?(e: any): void
}

const FormRadioGroup: FC<FormRadioGroupProps> = (props) => {

    const { name, label, options, defaultValue, forceFocus, changeHandler } = props;
    const error = props.error ? true : false;
    const [ focusValue, setFocusValue ] = useState<string>("");
    const [ selectedValue, setSelectedValue ] = useState<string>(defaultValue);
    const [ lostFocus, setLostFocus ] = useState<boolean>(false);

    const onKeyDown = (e:React.KeyboardEvent) => {
        // if 37 or 39, user is using arrow keys to navigate
        const keyPressed = e.charCode || e.which;
        if(keyPressed === 37){  // left arrow
            e.preventDefault();
            const prev:number = findIdx(-1);
            if(prev > -1){
                setFocusValue(options[prev].value);
            }
        }else if(keyPressed === 39){    // right arrow
            e.preventDefault();
            const next:number = findIdx(1);
            if(next < options.length){
                setFocusValue(options[next].value);
            }
        }else if(keyPressed === 13){
            // if user presses enter, assume they want to select the current option
            if(focusValue !== ""){
                setSelectedValue(focusValue);
                if(changeHandler){
                    changeHandler(focusValue);  // since we use the option value to set the focus, we can just fire a change upwards
                }  
            }
        }else if(keyPressed === 9){    // tab 
            setFocusValue("");
            setLostFocus(true);
        }
    }   

    const findIdx = (next:number) => {
        let currentIdx = 0;
        options.forEach((option:any) => {
            if(option.value === focusValue){
                currentIdx = options.indexOf(option);
            }
        });
        return currentIdx + next;
    }

    const onFocus = () => {
        // on focus, ensure that the correct option value is focused on
        if(!defaultValue || focusValue !== defaultValue){
            setLostFocus(false);
            if(!defaultValue){
                setFocusValue(options[0].value);    // if no default value, set focus on first item in list
            }else{
                setFocusValue(defaultValue);
            }
        }
    }

    const handleRBClick = (value:string) => {
        if(changeHandler){
            changeHandler(value);
            setFocusValue("");
        }
    }

    const onMouseLeave = () => {
        setLostFocus(true);
    }

    useEffect(() => {
        // force focus is passed in externally from a parent, telling the component to focus on itself
        if(forceFocus && !lostFocus){
            const ele = document.getElementById("RG"+name);
            if(ele){
                ele.focus();
            }
        }
    });

    return (
        <RadioGroup id={"RG"+name} onFocus={onFocus} onKeyDown={onKeyDown} tabIndex={0} onMouseLeave={onMouseLeave}>
            <FormFieldLabel label={label} error={error} />
            { options.map((option:Option) => {
                return <FormRadioButton key={"rbkey"+option.value} name={name} 
                    label={option.label} optionVal={option.value} error={error} defaultValue={selectedValue}
                    forceFocus={option.value === focusValue} changeHandler={handleRBClick} />
            })}
        </RadioGroup>
    );
}

const RadioGroup = styled.div`
  width: 100%;
  : focus {
      outline: none;
  }
`;

export default FormRadioGroup;