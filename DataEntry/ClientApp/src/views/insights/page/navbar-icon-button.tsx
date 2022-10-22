import React, { FC } from 'react';
import styled from 'styled-components';

export interface Props {
    icon: any,
    label: string,
    clickHandler: () => void
};

const NavbarIconButton: FC<Props> = (props) => {
    
    const { icon, label, clickHandler } = props;

    const onClick = () => {
        if(clickHandler){
            clickHandler();
        }
    }

    return (
        <ButtonContainer onClick={onClick}>
            <Icon src={icon}/>
            <Label>{label}</Label>
        </ButtonContainer>
    )
};

const ButtonContainer = styled.div`
    display: flex;
    background: none;
    border: none;
    cursor: pointer;
    line-height: 36px;
    align-items: center;
`

const Icon = styled.img`
    display: flex-inline;
    margin-right: 10px;
    height: 17px;
`;

const Label = styled.span`
    display: flex-inline;
    font-size: 14px;
    color: #fff;
    font-weight: 400px;
    text-transform: uppercase;
`;

export default NavbarIconButton;