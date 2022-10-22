import React from 'react';
import { useSelector } from 'react-redux';
import { Switch, Route } from 'react-router-dom';
import { RoutePaths } from './routePaths';
import ListingsContainer from '../views/listings/listings-container';
import ListingEntryContainer from '../views/listingentry/listing-entry-container';
import TeamsContainer from '../views/teams/teams-container';
import MIQImportContainer from '../views/miq/miq-import-container';
import MIQExportContainer from '../views/miq/miq-export-container';
import { Config } from '../types/config/config';
import { configSelector } from '../redux/selectors/system/config-selector';
import styled from 'styled-components';
import AdminContainer from '../views/admin/admin-container';
import Insights from '../views/insights/insights-main';
import Insights404 from '../views/insights/insights-404';


const Routes: React.FC = () => {
  const config: Config = useSelector(configSelector);

  return (
    <Switch>
      <Route
        exact={true}
        path={RoutePaths.listings}
        component={ListingsContainer} />
      <Route
        exact={true}
        path={RoutePaths.listingEntry}
        component={ListingEntryContainer} />
      <Route
        exact={true}
        path={RoutePaths.createListing}
        component={ListingEntryContainer} />
      <Route
        exact={true}
        path={RoutePaths.admin}
        component={AdminContainer} />
      {config && config.insightsEnabled && 
        <Route
        exact={true}
        path={RoutePaths.insights}
        component={Insights} />
      }
      <Route
          exact={true}
          path={RoutePaths.insightsRoot}
          component={Insights404}
      />
      {config && config.teamsEnabled && 
        <Route
          exact={true}
          path={RoutePaths.teams}
          component={TeamsContainer} />
      }
      <Route
        exact={true}
        path={RoutePaths.miqImport}
        component={MIQImportContainer} />
      <Route
        exact={true}
        path={RoutePaths.miqExport}
        component={MIQExportContainer} 
        />
      <Route
        path="*"
        render={() => (
          <Container404>
            <p>404 page not found</p>
            <GoHomeButton onClick={() => {window.location.href = "/"}}>Go Home</GoHomeButton>
          </Container404>
        )}
        key="errorRoute"
      />
    </Switch>
  );
};

const Container404 = styled.div`
  font-family: 'Futura Md BT Bold',helvetica,arial,sans-serif;
  text-align:center;
  margin-top:75px;
  p {
    color: #9EA8AB;
    font-size: 16px;
  }
`;

const GoHomeButton = styled.a`
  color:#00B2DD;
  cursor: pointer;
  font-size:14px;
  transition:.2s ease all;
  text-decoration:underline;
  &:hover {
    filter:brightness(0.85);
  }
`;

export default Routes;