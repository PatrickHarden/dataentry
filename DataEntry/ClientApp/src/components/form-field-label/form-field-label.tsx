import React, { FC } from 'react';
import styled, { css } from 'styled-components';

interface Props {
   label?: string,
   error?: boolean,
   title?: string,
   styles?: FormFieldLabelStyles
}

export interface FormFieldLabelStyles {
   fontSize?: string,
   fontWeight?: string,
   color?: string
}

const FormFieldLabel: FC<Props> = (props) => {

   const { error, label, title } = props;

   return (
      <BaseFieldHeading error={error} {...props}>{title && <Title>{title}</Title>}{label}</BaseFieldHeading>
   );
}

const BaseFieldHeading = styled.h5`
  color: ${(inputProps: Props) => inputProps.error ? css` 
   ${props => (props.theme.colors ? props.theme.colors.error : 'red')}; 
  ` : inputProps.styles && inputProps.styles.color ? inputProps.styles.color : '#9EA8AB'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  font-size: ${props => props.styles && props.styles.fontSize ? props.styles.fontSize : '16px' };
  margin-block-end: 0.5em;
  text-transform: capitalize;
  font-weight: ${props => props.styles && props.styles.fontWeight ? props.styles.fontWeight : '300' };
`;

const Title = styled.span`
   font-family: ${props => (props.theme.font ? props.theme.font.bold : 'inherit')};
   color: #8E9A9D;
   margin-right:10px;
`;

export default FormFieldLabel;