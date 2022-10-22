import React, { FC } from 'react';
import styled from 'styled-components';

const AdminHeader: FC = (props) => {
    
    return (
        <StyledListingsHeader>
            <StyledInnerContainer>
                <StyledTitle style={{ float: "left", 'marginRight': '15px', 'marginTop': '8px', 'marginBottom': '-8px' }}>Admin Dashboard</StyledTitle>
            </StyledInnerContainer>
        </StyledListingsHeader>
    );
}

const StyledListingsHeader = styled.div`
    background: #006A4D;
    overflow:auto;
    padding:12px 0;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    position:sticky;
    top:0;
    z-index:100;
    height: 37px;
    box-shadow: 0 4px 6px rgba(204,204,204,0.12), 0 2px 4px rgba(204,204,204,0.24);
`;

const StyledInnerContainer = styled.div`
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
`;

const StyledTitle = styled.h1`
    display: inline-block;
    color:white;
    float:left;
    text-transform:uppercase;
    font-size: 1.3em;
    font-family: ${props => (props.theme.font ? props.theme.font.bold : 'sans-serif')};
    padding:0px;
`;

export default AdminHeader;

