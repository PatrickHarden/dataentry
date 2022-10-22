import React, { FC, useContext } from 'react';
import { Link } from 'react-router-dom';
import StyledButton from '../../components/styled-button/styled-button';
import styled from 'styled-components';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import BulkUploadButton from '../../components/bulk-upload-button/bulk-upload-button';
import { useSelector, useDispatch } from 'react-redux';
import { searchTextSelector } from '../../redux/selectors/pagedListings/search-text-selector'
import { searchChanged } from '../../redux/actions/pagedListings/update-search-text';
import { GLAnalyticsContext } from '../../components/analytics/gl-analytics-context';
import debounce from 'debounce';
import { featureFlagSelector } from '../../redux/selectors/featureFlags/feature-flag-selector';
import { FeatureFlags, MainMessageType, State } from '../../types/state';
import { axiosDownloadReport } from '../../api/glAxios';
import { defaultRegionID } from '../../api/glQueries';
import { regionIdSelector } from '../../redux/selectors/system/config-region-id-selector'
import { clearMessage, setMainMessage } from '../../redux/actions/system/set-main-message';

interface Props{
    showSearch:boolean;
}

const ListingHeader: FC<Props> = (props) => {

    const { showSearch } = props;

    const dispatch: any = useDispatch();
    const analytics = useContext(GLAnalyticsContext);
    const searchText:string = useSelector(searchTextSelector);
    const regionId: string|undefined = useSelector(regionIdSelector);

    // feature flags
    const featureFlags:FeatureFlags = useSelector(featureFlagSelector);
    const miqImport:boolean = featureFlags.miqImportFeatureFlag;
    
    let value = ''
    const handleInput = (e: any) => {
        value = e.target.value;
        dispatchSearch();
    };

    const dispatchSearch =  debounce(() => setTimeout(() => dispatch(searchChanged(value)), 400), 400);

    const downloadListingsReport = () => {
        dispatch(setMainMessage({ show: true, type: MainMessageType.LOADING, message: "Generating Report..." }));
        axiosDownloadReport(`/api/report/download/${regionId || defaultRegionID}`)
         .then((response : any)=>{
            clearMessage(dispatch);
            const url = window.URL.createObjectURL(new Blob([response]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'DEReport.xlsx');
            document.body.appendChild(link);
            link.click();
        });

    }

    return (
        <StyledListingsHeader>
            <StyledInnerContainer>
                <StyledTitle style={{ float: "left", 'marginRight': '15px', 'marginTop': '8px', 'marginBottom': '-8px' }}>My Listings</StyledTitle>
                <StyledSearchBox><StyledSearchInput onClick={() => analytics.fireEvent('searchClick', 'click', 'Property searchbar click')} placeholder="Search by Name, Address, State, or PostalCode..." defaultValue={searchText} onChange={showSearch ? handleInput : () => (null)} /><StyledSearchIconBtn><FontAwesomeIcon style={{ 'marginTop': '3px' }} icon={faSearch} /></StyledSearchIconBtn></StyledSearchBox>
                <Link id="createListingButton" style={{ float: "right" }} onClick={() => analytics.fireEvent('createListingClick', 'click', 'Create listing button click', 'beacon')} to="/le"><StyledButton>Create Listing</StyledButton></Link>
                {miqImport && <Link id="importFromMIQButton" style={{ float: "right", marginRight: "8px" }} onClick={() => analytics.fireEvent('importFromMIQClick', 'click', 'Import From MIQ button click', 'beacon')} to="/miq/import"><StyledButton>Import from MIQ</StyledButton></Link>}
                <BulkUploadButton sendAnalytics={() => analytics.fireEvent('bulkUploadClick', 'click', 'Bulk upload button click')} />
                <Link id="download" style={{ float: "right", marginRight: "8px" }}  onClick={downloadListingsReport} to=""><StyledButton>Listings Report</StyledButton></Link>
            </StyledInnerContainer>
        </StyledListingsHeader>
    );
};


export default ListingHeader;


const StyledListingsHeader = styled.div`
    background: #006A4D;
    overflow:auto;
    padding:12px 0;
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

const StyledSearchInput = styled.input`
  float:left;
  border: 0;
  padding: 0;
  color: #666666; 
  min-width: 330px;
  font-weight: ${props => props.theme.font ? props.theme.font.weight.normal : 'normal'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  ::placeholder {color:#cccccc;}
  font-size: 14px;
  text-align: left;
  text-decoration: none;
  vertical-align: text-bottom;
  display:table-cell;
  width:auto;
  &:focus {
        outline:none;
    }
`;

const StyledSearchBox = styled.div`
    background-color: #ffffff !important;
    padding:10px 12px 10px 16px;
    display:table;
    border-radius: 20px;
    float: left;
    input:disabled {
        background: #ffffff !important;
    }
`;

const StyledSearchIconBtn = styled.div`
    color: #1AA083;
    cursor:pointer;
    display:inline-block;
    margin-top:-5px;
`;