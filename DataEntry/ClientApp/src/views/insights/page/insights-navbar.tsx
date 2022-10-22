import React, { FC, useState } from 'react';
import NavbarIconButton from './navbar-icon-button';
import shareIcon from '../../../assets/images/png/icon-share.png';
import styled from 'styled-components';

export interface Props {
    navTitle: string,
    showShareButton: boolean,
    showPrintButton: boolean,
    clickShareHandler?: () => void;
};

const InsightsNavBar: FC<Props> = (props) => {
    
    const { showShareButton, showPrintButton, clickShareHandler } = props;

    const shareReport = () => {
        if(clickShareHandler){
            clickShareHandler();
        }
    }

    return (
        <>
            <Nav>
                <BarContainer>
                    <Container className="container">
                        <NavTitle>
                            {props.navTitle && <>{props.navTitle}</>}
                        </NavTitle>
                        <Buttons>
                            <NavbarIconButton icon={shareIcon} label="Share" clickHandler={shareReport}/>
                        </Buttons>
                    </Container>
                </BarContainer>
            </Nav>
        </>
    )
};

const Nav = styled.div`
    position:sticky;
    top:0;
    z-index:10;
    box-shadow: 0 4px 6px rgba(204,204,204,0.12), 0 2px 4px rgba(204,204,204,0.24);
`;

const BarContainer = styled.div`
    background: #006A4D;
    padding:12px 0;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
`;

const Container = styled.div`
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
    display:flex;
    justify-content:space-between;
`;

const NavTitle = styled.div`
    padding-top:8px;
    width: 300px;	
    color: #FFFFFF;
    font-family: ${props => (props.theme.font ? props.theme.font.bold : 'sans-serif')};
    font-size: 18px;
    line-height: 21px;
`;

const Buttons = styled.div`
    > button {
        margin-right:8px;
    }
    > button:last-of-type {
        margin-right:0;
    }
    height: 36px;
`;

export default InsightsNavBar;