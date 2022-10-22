import React, { FC, useContext } from 'react'
import styled from 'styled-components';
import { useDispatch, useSelector } from 'react-redux';
import StyledButton from '../../components/styled-button/styled-button';
import { push } from 'connected-react-router';
import { Listing } from '../../types/listing/listing';
import { alertMessageSelector } from '../../redux/selectors/system/alert-message-selector';
import AlertMessage from '../common/messages/alert-message';
import { GLAnalyticsContext } from '../../components/analytics/gl-analytics-context';
import { configSelector } from '../../redux/selectors/system/config-selector';
import { currentListingSelector } from '../../redux/selectors/entry/current-listing-selector';
import { Config } from '../../types/config/config';
import NavbarStatusMessage from './navbar/navbar-status-message';
import { cancelActionSelector } from '../../redux/selectors/system/cancel-action-selector';

interface Props {
  navTitle: string,
  isDeleted: boolean,
  isDuplicatingListing: boolean,
  saveHandler?(): void,
  publishHandler?(): void,
  unpublishHandler?(): void,
  deleteHandler?(): void,
  exportListingHandler?(): void,
  duplicateListingHandler?(): void
}

const ListingNavbar: FC<Props> = (props) => {

  const dispatch = useDispatch();

  const { navTitle, saveHandler, publishHandler, unpublishHandler, deleteHandler, exportListingHandler, duplicateListingHandler, isDeleted, isDuplicatingListing } = props;

  const alertMessage = useSelector(alertMessageSelector);

  const currentListing: Listing = useSelector(currentListingSelector);
  const config: Config = useSelector(configSelector);

  const analytics = useContext(GLAnalyticsContext);

  const cancelAction = useSelector(cancelActionSelector);

  // button click handlers : callbacks passed in by parents
  const deleteListing = () => {
    if (deleteHandler) {
      deleteHandler();
    }
  }

  const cancel = () => {
    analytics.fireEvent('cancel', 'click', 'Cancel listing entry view', 'beacon');
    dispatch(push(cancelAction && cancelAction.goto && cancelAction.goto.length > 0 ? cancelAction.goto : "/"));
  }

  const save = () => {
    if (saveHandler) {
      analytics.fireEvent('save', 'click', 'Save listing', 'beacon')
      saveHandler();
    }
  }

  const unpublish = () => {
    if (unpublishHandler) {
      analytics.fireEvent('unpublish', 'click', 'Unpublish listing')
      unpublishHandler();
    }
  }

  const publish = () => {
    if (publishHandler) {
      analytics.fireEvent('publish', 'click', 'Publish listing')
      publishHandler();
    }
  }

  const exportListing = () => {
    if (exportListingHandler) {
      analytics.fireEvent('export', 'click', "Export Listing");
      exportListingHandler();
    }
  }

  const duplicateListing = () => {
    if (duplicateListingHandler) {
      analytics.fireEvent('duplicate', 'click', "Duplicate Listing");
      duplicateListingHandler();
    }
  }

  // action button renders
  const UnpublishedButtons = () => {
    return (
      <>
        <StyledButton id="lNavCancelButton" onClick={() => cancel()} primary={false}>Cancel</StyledButton>
        {config && config.exportEnabled &&
          <StyledButton id="lNavExportButton" onClick={() => exportListing()} primary={false}>Export</StyledButton>
        }
        <StyledButton id="lNavDeleteButton" onClick={() => deleteListing()} primary={false}>Delete</StyledButton>
        {config && config.duplicateEnabled && !isDuplicatingListing &&
          <StyledButton id="lNavDuplicateButton" onClick={() => duplicateListing()} primary={false}>Duplicate</StyledButton>
        }
        <StyledButton id="lNavSaveButton" onClick={() => save()} primary={false}>Save</StyledButton>
        <StyledButton id="lNavPublishButton" onClick={() => publish()} primary={true}>Publish</StyledButton>
      </>
    );
  }

  const PublishedButtons = () => {
    return (
      <>
        <StyledButton id="lNavCancelButton" onClick={() => cancel()} primary={false}>Cancel</StyledButton>
        {config && config.exportEnabled &&
          <StyledButton id="lNavExportButton" onClick={() => exportListing()} primary={false}>Export</StyledButton>
        }
        {config && config.duplicateEnabled && !isDuplicatingListing &&
          <StyledButton id="lNavDuplicateButton" onClick={() => duplicateListing()} primary={false}>Duplicate</StyledButton>
        }
        <StyledButton id="lNavSaveButton" onClick={() => save()} primary={false}>Save</StyledButton>
        <StyledButton id="lNavUnpublishButton" onClick={() => unpublish()} primary={false}>Unpublish</StyledButton>
        <StyledButton id="lNavSaveAndPublishButton" onClick={() => publish()} primary={true}>Republish</StyledButton>
      </>
    );
  }

  const PendingButtons = () => {
    return (
      <>
        <StyledButton id="lNavCancelButton" onClick={() => cancel()} primary={false}>Cancel</StyledButton>
        {config && config.exportEnabled &&
          <StyledButton id="lNavExportButton" onClick={() => exportListing()} primary={false}>Export</StyledButton>
        }
        {config && config.duplicateEnabled && !isDuplicatingListing &&
          <StyledButton id="lNavDuplicateButton" onClick={() => duplicateListing()} primary={false}>Duplicate</StyledButton>
        }
      </>
    );
  }

  return (
    <Nav>
      <BarContainer>
        <Container className="container">
          <NavbarStatusMessage navTitle={navTitle} isDeleted={isDeleted} />
          {isDeleted ?
            <Buttons>
              {config && config.duplicateEnabled && !isDuplicatingListing &&
                <StyledButton id="lNavDuplicateButton" onClick={() => duplicateListing()} primary={false}>Duplicate</StyledButton>
              }
              <StyledButton id="lNavSaveButton" onClick={() => save()} primary={false}>Save</StyledButton>
              <StyledButton id="lNavCancelButton" onClick={() => cancel()} primary={false}>Cancel</StyledButton>
            </Buttons> 
            :
            <Buttons>
              {currentListing && currentListing.hasOwnProperty("state") && !currentListing.state && <UnpublishedButtons />}
              {currentListing && currentListing.state && ((currentListing.state.toLowerCase() === "unpublished") || (currentListing.state.toLowerCase() === "publishfailed")) && <UnpublishedButtons />}
              {currentListing && currentListing.state && ((currentListing.state.toLowerCase() === "published") || (currentListing.state.toLowerCase() === "unpublishfailed")) && <PublishedButtons />}
              {currentListing && currentListing.state && ((currentListing.state.toLowerCase() === "publishing") || (currentListing.state.toLowerCase() === "unpublishing")) && <PendingButtons />}
            </Buttons>
          }
        </Container>
      </BarContainer>
      {alertMessage && alertMessage.show && <AlertMessage alertMessage={alertMessage} />}
    </Nav>
  );
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

const Buttons = styled.div`
    > button {
        margin-right:8px;
    }
    > button:last-of-type {
        margin-right:0;
    }
`;

export default ListingNavbar;