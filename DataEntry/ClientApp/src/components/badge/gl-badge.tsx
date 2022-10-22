import React, { FC } from 'react';
import styled from 'styled-components';

export interface Props {
    label: string | undefined,
    styles?: BadgeStyles
}

export interface BadgeStyles {
    background?: string,
    color?: string,
    border?: string,
    borderRadius?: string,
    lineHeight?:string,
    height?: string,
    minWidth?: string,
    padding?: string,
    textAlign?: string,
    textTransform?: string,
    fontSize?: string
}

const GLBadge: FC<Props> = (props) => {
    
    const { label, styles } = props;

    return <Badge {...styles} data-testid="gl-badge">{label ? label : ""}</Badge>;
}

const Badge = styled.div`
    background: ${(props: BadgeStyles) => props.background ? props.background : '#ffffff'};
    color: ${(props: BadgeStyles) => props.color ? props.color : '#006a4d'};
    border: ${(props: BadgeStyles) => props.border ? props.border : '1px solid #006a4d'};
    border-radius: ${(props: BadgeStyles) => props.borderRadius ? props.borderRadius : '0'};
    line-height: ${(props: BadgeStyles) => props.lineHeight ? props.lineHeight : '24px'};
    height: ${(props: BadgeStyles) => props.height ? props.height : '24px'};
    min-width: ${(props: BadgeStyles) => props.minWidth ? props.minWidth : '50px'};
    padding: ${(props: BadgeStyles) => props.padding ? props.padding : '0 10px 0 10px'};
    text-align: ${(props: BadgeStyles) => props.textAlign ? props.textAlign : 'center'};
    font-size: ${(props: BadgeStyles) => props.fontSize ? props.fontSize : '11px'};
    text-transform: ${(props: BadgeStyles) => props.textTransform ? props.textTransform : 'uppercase'};
`;
  
export default GLBadge;