import React, { FC, useEffect } from 'react';
import { ConnectedRouter } from 'connected-react-router';
import Routes from './routes';
import Intercom from 'react-intercom';
import { History } from 'history';
import HeaderMain from '../views/common/header/header-main';
import Footer from '../views/common/footer/footer'
import styled, { ThemeProvider } from 'styled-components';
import { themeSelector } from '../redux/selectors/common/theme-selector';
import { Theme } from '../themes/default/theme';
import { loadConfig } from '../redux/actions/system/load-config-details';
import { ConfigDetails } from '../types/config/config';
import { configDetailsSelector } from '../redux/selectors/system/config-details-selector';
import { useSelector, useDispatch } from 'react-redux';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { authContext } from '../adalConfig';
import { checkTake } from '../redux/actions/pagedListings/set-paging-params';

interface Props {
  history: History;
}

const App: FC<Props> = (props: Props) => {
  const user = authContext.getCachedUser();
  const dispatch = useDispatch();

  const configDetails: ConfigDetails = useSelector(configDetailsSelector);
  const theme: Theme = useSelector(themeSelector);


  const configLoaded = () => {

    let siteIdCode: string = "";

    const url: string = window.location.href;

    if (url.indexOf("localhost") > -1 || url.indexOf("dev") > -1 || url.indexOf("uat") > -1) {
      // get the site id from the url (only in localhost/dev/uat) to allow for a hard switch
      if (props.history && props.history.location && props.history.location.search) {
        const queryParams = props.history.location.search;
        const paramSplit: string[] = queryParams.substr(1, queryParams.length).split("&");  // remove the ? and return an array of query params
        paramSplit.forEach((param: string) => {
          if (param.indexOf("site") > -1) {
            const valueSplit: string[] = param.split("=");
            if (valueSplit[1]) {
              siteIdCode = decodeURIComponent(valueSplit[1]);
            }
          }
        });
      }
    }

    // if we don't have a config or the siteId from the URL doesnt match the loaded config
    if (!configDetails || !configDetails.loaded || (siteIdCode.length > 0 && configDetails.config && configDetails.config.siteId !== siteIdCode)) {
      dispatch(loadConfig(siteIdCode, true));
      return false;
    }
    return true;   // config is good, proceed with render
  }


  const loaded: boolean = configLoaded();
  // Add application insights
  if (loaded && configDetails.aiKey) {
    const appInsights = new ApplicationInsights({
      config: {
        instrumentationKey: configDetails.aiKey,
        loggingLevelConsole: 1    /* critical errors only to console */
        /* ...Other Configuration Options... */
      }
    });
    appInsights.loadAppInsights();
    appInsights.trackPageView(); // Manually call trackPageView to establish the current user/session/pageview
  }


  const windowResize = () => {
    // this is added to update anything we may need based on window size. it was added initially to determine the number of records to take for paging
    const cardHeight: number = 450;
    const rows: number = Math.floor(window.innerHeight / cardHeight);
    const numberOfRecordsToTake = Math.max(rows * 4, 8); // 8 is our minimum
    dispatch(checkTake(numberOfRecordsToTake)); // the action will take care of deciding whether to set the new take
  }

  // offset hashmark changes for better UX, dispatch call for isAdministrator check
  useEffect(() => {
    windowResize();
    window.addEventListener("hashchange", () => {
      window.scrollTo(window.scrollX, window.scrollY - 60);
    });
    window.addEventListener("resize", () => {
      windowResize();
    })
  }, [])


  if (!loaded || configDetails.error) {
    if (configDetails.error) {
      return <ConfigErrorContainer>{configDetails.error}</ConfigErrorContainer>
    }
    return <></>;
  } else {
    return (
      <>
        {configDetails.config.intercomm && configDetails.config.intercommAppId &&
          <Intercom
            appID={configDetails.config.intercommAppId}
            name={`${user && user.profile && user.profile.given_name || ''} ${user && user.profile && user.profile.family_name || ''}`.trim() || 'Unknown'}
            email={(user && user.userName) || 'Unknown'} />
        }
        <ThemeProvider theme={theme}>
          <ConnectedRouter history={props.history} >
            <div>
              <HeaderMain />
              <RoutesContainer>
                <Routes />
              </RoutesContainer>
              <FooterContainer>
                <Footer />
              </FooterContainer>
            </div>
          </ConnectedRouter>
        </ThemeProvider>
      </>
    );
  }
}

const RoutesContainer = styled.div`
  min-height: 100vh;
  display: block;
  position:relative;
  #theWatermark {
    position: absolute;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    margin: auto;
    max-width: none;
  }
`;

const FooterContainer = styled.div`
  margin-top: 100px;
  bottom: 0;
  width: 100%;
`;

const ConfigErrorContainer = styled.div`
`;

export default App;