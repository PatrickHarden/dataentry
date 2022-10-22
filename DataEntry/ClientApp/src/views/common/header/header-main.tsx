import React, { FC, useContext } from 'react'
import styled from 'styled-components';
import { authContext } from '../../../adalConfig';
import { Link } from 'react-router-dom';
import Logo from '../../../assets/images/png/GL-Logo.png';
import { useSelector, useDispatch } from 'react-redux';
import { Config } from '../../../types/config/config';
import { configSelector } from '../../../redux/selectors/system/config-selector';
import ConfirmationDialog from '../../../components/confirmation-dialog/confirmation-dialog';
import DataSourcePopupDialog from '../../../components/dataSource-popup-dialog/datasource-popup-dialog';
import MiqPopupAddress from '../../../components/miq-popup-address/miq-popup-address';
import { confirmDialogSelector } from '../../../redux/selectors/system/confirm-dialog-selector';
import { dataSourcePopupSelector } from '../../../redux/selectors/system/datasource-popup-selector';
import { ConfirmDialogParams, DataSourcePopupParams, User, MIQ } from '../../../types/state';
import { GLAnalyticsContext } from '../../../components/analytics/gl-analytics-context';
import { userSelector } from '../../../redux/selectors/system/user-selector';
import { configDetailsSelector } from '../../../redux/selectors/system/config-details-selector';
import { checkEnvironment } from '../../../utils/helpers/check-environment';
import { setMiqAddressPopup } from '../../../redux/selectors/system/set-miq-address-popup';
import { loadConfig } from '../../../redux/actions/system/load-config-details';
import { setCountry } from '../../../redux/actions/mapping/set-country';
import RegionSelect from './region-select';

const HeaderMain: FC = () => {

    const dispatch = useDispatch();
    const userName = authContext.getCachedUser();
    const confirmDialog: ConfirmDialogParams = useSelector(confirmDialogSelector);
    const dataSourcePopup: DataSourcePopupParams = useSelector(dataSourcePopupSelector);
    const config: Config = useSelector(configSelector);
    const user: User = useSelector(userSelector);
    const configDetails: any = useSelector(configDetailsSelector);
    const miqPopupAddress: MIQ = useSelector(setMiqAddressPopup);

    const analytics = useContext(GLAnalyticsContext);

    const displayConfig = () => {
        // include the site id if we are in localhost, dev, or uat.
        const environmentCheck:boolean = checkEnvironment(config, {localhost: true, dev: true, uat: true, prod: false});
        if(environmentCheck){
            return (
                <ConfigDisplay>[{config.siteId}]</ConfigDisplay>
            );
        }
        return <></>;
    }

    let displayMIQ = false;
    const windowUrl: string = window.location.href;
    if (windowUrl.indexOf("miq/create") > -1) {
        displayMIQ = true;
    } 



    return (
        <Nav>
            {dataSourcePopup && dataSourcePopup.show && <DataSourcePopupDialog datasourcePopupDialogParams={dataSourcePopup} />}
            {confirmDialog && confirmDialog.show && <ConfirmationDialog confirmationDialogParams={confirmDialog} />}
            {miqPopupAddress && miqPopupAddress.show && <MiqPopupAddress  miqPopupAddressParams={miqPopupAddress}/>}
            <Container>
            {displayMIQ && <Miq>Market IQ
                </Miq>}
            { !displayMIQ && <Links>
                    <Link to="/" onClick={() => { analytics.fireEvent('home', 'click', 'home page', 'beacon') }}>
                        <img src={Logo} alt="Logo" />
                    </Link>
                    <Link to="/" onClick={() => { analytics.fireEvent('home', 'click', 'home page', 'beacon') }}>
                        <p style={(location.pathname === "/") ? { fontFamily: 'Futura Md BT Bold' } : { fontFamily: 'Futura Md BT' }}>
                            My Listings
                        </p>
                    </Link>
                    {(config && config.teamsEnabled) &&
                        <Link to="/myteams" onClick={() => { analytics.fireEvent('myTeams', 'click', 'My Teams page click', 'beacon') }}>
                            <p style={(location.pathname === "/myteams") ? { fontFamily: 'Futura Md BT Bold' } : { fontFamily: 'Futura Md BT' }}>
                                Data Entry Teams
                          </p>
                        </Link>
                    }
                    {user && user.isAdmin &&
                        <Link to="/admin" onClick={() => { analytics.fireEvent('admin', 'click', 'Admin page click', 'beacon') }}>
                            <p style={(location.pathname === "/admin") ? { fontFamily: 'Futura Md BT Bold' } : { fontFamily: 'Futura Md BT' }}>
                                Admin Dashboard
                            </p>
                        </Link>
                    }
                </Links>
                }
                <RegionSelect config={config} />
                <UserContainer>
                    <UserAvatar onClick={() => { authContext.logOut(); analytics.fireEvent('logout', 'click', 'User initials circle, header click'); }}>
                        {(userName.profile.given_name ? userName.profile.given_name.substr(0, 1) : '') + ' ' + (userName.profile.family_name ? userName.profile.family_name.substr(0, 1) : '')}
                    </UserAvatar>
                    <span>{(userName.profile.given_name ? userName.profile.given_name : '') + ' ' + (userName.profile.family_name ? userName.profile.family_name : '')}</span>
                </UserContainer>
            </Container>
        </Nav>
    );
};

const Nav = styled.nav`
    position:relative;
    padding:10px 0;
    background:#fff;
    z-index: 101;
    font-family: ${props => props.theme.font ? props.theme.font.primary : 'helvetica'};	
`;

const Container = styled.div`
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
    display:flex;
`;

const Miq = styled.div`
    display:inline-flex;
    min-width:70%;
    position:relative;
    top:20px;
    line-height: 18px;
    text-transform:uppercase;
    text-align:left;
    color:#00A384;
    font-family: 'Futura Md BT Bold';
    font-size:24px;
`;

const Links = styled.div`
    display:inline-flex;
    min-width:70%;
    position:relative;
    top:3px;
    p {
        margin-left:62px;
        display:inline-flex;
        color: #999999;
        font-size: 14px;
        font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
        line-height: 18px;
        text-transform:uppercase;
    }
    img {
        position:relative;
        top:13px;
    }
    a {
        color:inherit;
        text-decoration:none;
    }
`;

const UserContainer = styled.div`
    display:inline-flex;
    width:100%;
    justify-content:flex-end;
    > span {
        color: #9EA8AB;
        font-size: 12px;
        font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
        line-height: 15px;	
        text-align: right;
        line-height:48px;
    }
`;

const UserAvatar = styled.div`
    cursor:pointer;
    background:#eee;
    border-radius:50%;
    color:#8E9A9D;
    text-align:center;
    height:50px;
    width:50px;
    line-height:50px;
    margin-right:13px;
    letter-spacing:-1px;
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
`;

const ConfigDisplay = styled.div`
    margin: 18px 0 0 10px;
    font-size: 10px;
    width: 220px;
    color: rgb(158, 168, 171);
    font-family: "Futura Md BT Bold", helvetica, arial, sans-serif;
`;

const ComingSoon = styled.small`
    padding: 2px;
    color: red;
    position: relative;
    right: -64px;
    top: -17px;
    font-size: 10px;
    opacity: .7;
    font-family: ${props => props.theme.font ? props.theme.font.primary : 'helvetica'};
`;

export default HeaderMain;