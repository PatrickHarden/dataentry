import React, { FC, useContext } from 'react';
import { Link } from 'react-router-dom';
import StyledButton from '../../components/styled-button/styled-button';

import BulkUploadButton from '../../components/bulk-upload-button/bulk-upload-button';
import { useSelector, useDispatch } from 'react-redux';
import { searchTextSelector } from '../../redux/selectors/pagedListings/search-text-selector'
import { changeSearch } from '../../redux/actions/pagedListings/update-search-text';
import { GLAnalyticsContext } from '../../components/analytics/gl-analytics-context';
import styled from 'styled-components';

interface Props{
    showSearch:boolean;
}


const TeamsHeader: FC<Props> = (props) => {

    const dispatch = useDispatch();
    const analytics = useContext(GLAnalyticsContext);

    return (
        <StyledTeamsHeader>
            <StyledInnerContainer>
                <StyledTitle style={{ float: "left", 'marginRight': '15px', 'marginTop': '8px' }}>Data Entry Teams</StyledTitle>
                <Link id="createListingButton" style={{ float: "right" }} onClick={() => analytics.fireEvent('createListingClick', 'click', 'Create listing button click', 'beacon')} to="/le"><StyledButton>Create Listing</StyledButton></Link>
                <BulkUploadButton sendAnalytics={() => analytics.fireEvent('bulkUploadClick', 'click', 'Bulk upload button click')} />
            </StyledInnerContainer>
        </StyledTeamsHeader>
    );
};


export default TeamsHeader;


const StyledTeamsHeader = styled.div`
    background: rgb(0, 106, 77);
    overflow:auto;
    padding:12px 0 7px;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    position:sticky;
    top:0;
    z-index:100;
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
