import React, { FC } from 'react';
import styled from 'styled-components';
import { PreviewStatusMessage } from '../../../types/state';
import { navbarPreviewStatusSelector } from '../../../redux/selectors/entry/navbar-preview-status-selector';
import { useSelector } from 'react-redux';
import { Listing } from '../../../types/listing/listing';
import { currentListingSelector } from '../../../redux/selectors/entry/current-listing-selector';
import IconPending from '../../../assets/images/png/icon-pending.png';
import IconPublishError from '../../../assets/images/png/icon-publisherror.png';
import StyledButton from '../../../components/styled-button/styled-button';

interface Props {
    navTitle: string,
    isDeleted: boolean
}

const NavbarStatusMessage: FC<Props> = (props) => {

    const { navTitle, isDeleted } = props;

    const previewStatus:PreviewStatusMessage = useSelector(navbarPreviewStatusSelector);
    const currentListing:Listing = useSelector(currentListingSelector);

    const renderSpecialStatusMessage = () => {
        if(currentListing && currentListing.state){
            if (currentListing.state.toLowerCase() === "publishing") {
                return <PublishingFlag><img style={{ 'marginBottom': '-3px' }} src={IconPending} /> Pending Publish</PublishingFlag>;
            } else if (currentListing.state.toLowerCase() === "unpublishing") {
                return <PublishingFlag><img style={{ 'marginBottom': '-3px' }} src={IconPending} /> Pending Unpublish</PublishingFlag>;
            } else if (currentListing.state.toLowerCase() === "publishfailed") {
                return <FailFlag><img style={{ 'marginBottom': '-3px' }} src={IconPublishError} /> Publish Failed!</FailFlag>;
            } else if (currentListing.state.toLowerCase() === "unpublishfailed") {
                return <FailFlag><img style={{ 'marginBottom': '-3px' }} src={IconPublishError} /> Unpublish Failed!</FailFlag>;
            }       
        }
        return <></>;
    }

  // messages and preview button renders (left hand sidebar)
  const renderPreviewStatusMessage = () => {

    if (isDeleted){
      return <PreviewInfoMsg />;
    } else {
      if(previewStatus === PreviewStatusMessage.LOADING){
        return <PreviewInfoMsg> Checking for Preview...</PreviewInfoMsg>;
      }else if(previewStatus === PreviewStatusMessage.AVAILABLE){
        return <PreviewLink>
          <StyledButton primary={true} onClick={() => window.open(currentListing.externalPreviewUrl, '_blank')}> PREVIEW AVAILABLE </StyledButton>
        </PreviewLink>;
      }else if(previewStatus === PreviewStatusMessage.UNAVAILABLE){
        return <PreviewInfoMsg> Generating Preview...</PreviewInfoMsg>;
      }else if(previewStatus === PreviewStatusMessage.FAILED){
        return <PreviewInfoMsg> Preview not Available</PreviewInfoMsg>;
      }else if(previewStatus === PreviewStatusMessage.OUTDATED){
        return <PreviewLink>
          <StyledButton primary={true} onClick={() => window.open(currentListing.externalPreviewUrl, '_blank')}> OUTDATED PREVIEW AVAILABLE </StyledButton>
        </PreviewLink>;
      }else if(previewStatus === PreviewStatusMessage.SAVE_TO_GENERATE){
        return <PreviewInfoMsg> Save to Generate Preview </PreviewInfoMsg>;
      }
    }
      return <></>; 
  }

  return (
    <NavTitle>
        {navTitle}
        {renderSpecialStatusMessage()}
        {renderPreviewStatusMessage()}
    </NavTitle>
  );
};

const NavTitle = styled.div`
    padding-top:8px;
    /* width: 174px;	 */
    color: #FFFFFF;
    font-family: ${props => (props.theme.font ? props.theme.font.bold : 'sans-serif')};
    font-size: 18px;
    line-height: 21px;
`;

const PublishingFlag = styled.div`
    z-index:10;
    display:inline-block;
    position:relative;
    background-color: #FFDD00;
    border-radius: 20px;
    height:24px;
    margin-right:10px;
    margin-left:20px;
    top: -2px;
    line-height:24px;
    text-align:center;
    color: #333333;
    font-size: 12px;
    padding-left:8px;
    padding-right:8px;
`;

const FailFlag = styled.div`
    z-index:10;
    display:inline-block;
    position:relative;
    background-color: #E86C6C;
    border-radius: 20px;
    height:24px;
    margin-right:10px;
    margin-left:20px;
    top:-2px;
    line-height:24px;
    text-align:center;
    color: white;
    font-size: 12px;
    padding-left:8px;
    padding-right:8px;
`;

const PreviewInfoMsg = styled.div`
    color: #dedede;
    float: right;
    margin-left: 20px;
    font-family: "Futura Md BT";
`;

const PreviewLink = styled.div`
    float: right;
    margin-left: 20px;
    margin-top: -8px;    
`;

export default NavbarStatusMessage;