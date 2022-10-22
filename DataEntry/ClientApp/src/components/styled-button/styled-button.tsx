import React, { FC } from 'react';
import styled, { css } from 'styled-components';

interface StyledButtonProps {
  buttonStyle?: string,
  primary?: boolean,
  styledSpan?: boolean,
  height?: string,
  backgroundColor?: string,
  borderRadius?: string,
  theme: any
}

type Props = StyledButtonProps | any;

const StyledButton: FC<Props> = (props) => {

    const {buttonStyle, primary} = props;

    const onKeyPress = (e:KeyboardEvent) => {
        const keyCode = e.charCode || e.which;
        if(keyCode === 13 || keyCode === 32){ // enter or spacebar
            e.preventDefault();
            document.getElementsByName(props.name)['0'].click();
        }
    }
    
    return (
      (props.styledSpan) ? <BaseSpan name={props.name} tabIndex={0} onKeyPress={onKeyPress} buttonStyle={buttonStyle} primary={primary} {...props} /> : <BaseButton name={props.name} tabIndex={0} onKeyPress={onKeyPress} buttonStyle={buttonStyle} primary={primary} {...props} />
    );  
};

const btnBackground1 = css` 
${props => (props.theme && props.theme.colors && props.theme.colors.primaryAccentLght ? props.theme.colors.primaryAccentLght : '#29BC9C')}; 
`
const btnBackground1primary = css` 
${props => (props.theme && props.theme.colors && props.theme.colors.secondaryAccent ? props.theme.colors.secondaryAccent : '#00B2DD')}; 
`
const btnBorderStyle2 = css` 
1px solid ${props => (props.theme.colors ? props.theme.colors.secondaryAccent : '#00B2DD')}; 
`

const style1 = css` 
background-color: ${(props: StyledButtonProps) => (props.primary ? btnBackground1primary : props.backgroundColor ? props.backgroundColor : btnBackground1)}; 
color: #ffffff; 
border:0px;
padding: 0.9em 1.7em;
`
const style2 = css` 
color: ${props => (props.theme.colors ? '#006A4D' : '#00B2DD')};  
border: ${btnBorderStyle2};
background: inherit;
padding: 0.6em 1.5em;
`
const BaseButton = styled.button`
  ${(inputProps: StyledButtonProps) => (inputProps.buttonStyle === '2') ? style2 : style1}
  height: ${(inputProps: StyledButtonProps) => inputProps.height ? inputProps.height : "auto"}
  cursor: pointer;
  border-radius: ${(inputProps: StyledButtonProps) => inputProps.borderRadius ? inputProps.borderRadius : "20px"};
  box-sizing: border-box;
  -webkit-box-sizing: border-box; 
  -moz-box-sizing: border-box; 
  display: inline-block;
  flex-grow: 1 
  position: relative;
  text-align: middle;
  text-transform: capitalize;
  min-width:8em;
  font-family: ${props => (props.theme && props.theme.font && props.theme.font.primary ? props.theme.font.bold : 'inherit')}; 
  font-size: 12px;
  text-transform:uppercase;
  &:hover {
    top:1px;
  }
  :focus {
    outline: none;
    border-color: ${props => props.theme && props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C'};
  }
`;

const BaseSpan = styled.span`
  ${(inputProps: StyledButtonProps) => (inputProps.buttonStyle === '2') ? style2 : style1}
  cursor: pointer;
  border-radius: 20px;
  box-sizing: border-box;
  -webkit-box-sizing: border-box; 
  -moz-box-sizing: border-box; 
  display: inline-block;
  flex-grow: 1
  position: relative;
  text-align: middle;
  text-transform: capitalize;
  min-width:8em;
  font-family: ${props => (props.theme && props.theme.font && props.theme.font ? props.theme.font.primary : 'inherit')}; 
  font-size: 12px;
  text-transform:uppercase;
  &:hover {
    top:1px;
    }
  :focus {
    outline: none;
    border-color: ${props => props.theme && props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C'};
  }
`;

export default StyledButton;