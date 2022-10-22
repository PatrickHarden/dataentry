import React, { FC } from 'react';
import styled, { css } from 'styled-components';

export interface SectionHeadingProps {
  error?: boolean;
}

const SectionHeading: FC<SectionHeadingProps> = (props) => {
    const { error } = props;
    return <BaseHeading error={error}>{props.children}</BaseHeading>
}
  
const BaseHeading = styled.h2`
  color: ${(inputProps: SectionHeadingProps) => inputProps.error ? css` 
    ${props => (props.theme.colors ? props.theme.colors.error : 'red')}; 
    ` : '#006A4D'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')};
  font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
  letter-spacing: .01rem;
  text-transform: uppercase;
`;

export default SectionHeading;