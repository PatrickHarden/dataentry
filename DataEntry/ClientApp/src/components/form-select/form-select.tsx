import React, { FC, ReactElement } from 'react';
import styled, {withTheme} from 'styled-components';
import FormFieldLabel from '../form-field-label/form-field-label';
import Select, {components} from 'react-select';

export interface FormSelectProps {
    name: string,
    label?: string,
    options?: any,
    defaultValue?: any,
    prompt?: string,
    doSort?: boolean,
    doAlphabeticalSort?: boolean,
    error?: boolean,
    theme?: any,
    forceFocus?: boolean,
    disabled?: boolean,
    appendToId?: string,
    isClearable?: boolean,
    changeHandler?(e: any): void
}

const DropdownIndicator = (props:any) => {
    return (
      components.DropdownIndicator && (
        <components.DropdownIndicator {...props}>
            <StyledDownArrow/>
        </components.DropdownIndicator>
      )
    );
  };

const MenuList = (props:any) => {
    const newChildren:any[] =[];
    let prefix:string = "";
    
    if(props.selectProps && props.selectProps.id){
        prefix = props.selectProps.id;
    }
    
    if(props.children && Array.isArray(props.children)){
        props.children.forEach((child:ReactElement) => {
            if(child && child.props && child.props.innerProps && child.props.data && child.props.data.value){
                // add a data-test attribute for automated testing
                const innerProps = {
                    ...child.props.innerProps,
                    'data-test': prefix && prefix.length > 0 ? prefix + "-option-" + child.props.data.value : "option-" + child.props.data.value
                }
                // merge into a new props object to overcome readonly issues
                const propsObj = {
                    props: {
                        ...child.props,
                        'innerProps': innerProps
                    }
                };
                // push the new, modified child element for rendering
                newChildren.push(
                    {...child, ...propsObj }
                );
            }else{
                // fallback if we dont have the expected properties
                newChildren.push(child);
            }
        });
    }
    
    return (
        <components.MenuList {...props}>
            {newChildren}
        </components.MenuList>
    );
}


const FormSelect: FC<FormSelectProps> = (props) => {

    const { name, label, options, prompt, defaultValue, doSort, doAlphabeticalSort, error, changeHandler, forceFocus, disabled, appendToId, isClearable = true } = props;

    let id:string | undefined = name;
    if(appendToId && appendToId.length > 0){
        id = id += "_" + appendToId;
    }

    const handleChange = (selectedOption:any) => {
        if(changeHandler !== undefined){
            changeHandler(selectedOption ? selectedOption.value : null);
        } 
    }

    
    // optional sort
    if (doSort !== undefined && doSort === true) {
        options.sort((a: any, b: any) => {
            return a.order - b.order;
        });
    }

    // do alphabetical sort
    if (doAlphabeticalSort !== undefined && doAlphabeticalSort === true) {
        options.sort((a: any, b: any) => a.label.localeCompare(b.label));
    }

    // styling to work with the react-select third party component
    const focusColor = props.theme && props.theme.colors ? props.theme.colors.inputFocus : "#29BC9C";
    const highlightColor = props.theme && props.theme.colors ? props.theme.colors.tertiaryAccent : "#00B2DD";
    const errorColor = props.theme && props.theme.colors ? props.theme.colors.error : "darkred";
    const white = "#FFFFFF";
    const black = "#000000";
    
    const customStyles  = {
        menu: (base:any) => {
            return {
                ...base,
                fontFamily: "'Futura Md BT', helvetica, arial, sans-serif"
            }
        },
        control: (base: any, state: any) => {
            return {
                ...base,
                height: 45,
                minWidth:'100px',
                borderRadius: 0,
                boxShadow: state.isFocused ? 0 : 0,
                borderColor: error ? errorColor : state.isFocused ? focusColor : base.borderColor,
                '&:hover': {
                    borderColor: state.isFocused ? focusColor : base.borderColor
                }
            }
        },
        option: (base:any, state: any) => {
            return {
                ...base,
                color: state.isFocused ? white : black,
                backgroundColor: state.isFocused ? highlightColor : base.highlightColor,
                '&:active': {
                    ...base[':active'],
                    backgroundColor: highlightColor
                }
            }
        },
        indicatorSeparator: (base:any) => {
            return {
                ...base,
                display: 'none'
            }
        },
        clearIndicator : (base:any) => {
            return {
                ...base,
                color: "#666666",
                '&:hover': {
                    color: "#333333"
                }
            }
        }
  }

    return (
        <BaseInputContainer>
            <FormFieldLabel label={label} error={error} />
            <Select id={id} placeholder={prompt} isClearable={isClearable} styles={customStyles} options={options} classNamePrefix={name}
                value={options.filter((option: any) => option.value === defaultValue)}
                isDisabled={disabled ? true : false }
                onChange={handleChange} autoFocus={forceFocus ? true : false} components={{DropdownIndicator, MenuList}} />
        </BaseInputContainer>
    );
}

const BaseInputContainer = styled.div``;

const StyledDownArrow = styled.i`
    width: 0; 
    height: 0; 
    border-left: 6px solid transparent;
    border-right: 6px solid transparent;
    border-top: 6px solid #00B2DD;
    margin-right: 10px;
`;

export default withTheme(FormSelect);