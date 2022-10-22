import React, { FC, useEffect, useContext } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { GLAnalyticsContext } from '../../../components/analytics/gl-analytics-context';
import Select from 'react-select';
import { loadConfig } from '../../../redux/actions/system/load-config-details';
import { setCountry } from '../../../redux/actions/mapping/set-country';
import { setCountryCode } from '../../../redux/actions/mapping/set-country-code';
import styled from 'styled-components';
import { configDetailsSelector } from '../../../redux/selectors/system/config-details-selector';
import { resetPagedListings } from '../../../redux/actions/pagedListings/load-listings-paged';
import { RoutePaths } from '../../../app/routePaths';
import { matchPath } from 'react-router';
import { routeSelector } from '../../../redux/selectors/router/route-selector';
//
import UK from '../../../assets/images/png/GB.png';
import NL from '../../../assets/images/png/Netherlands.png';
import US from '../../../assets/images/png/US.png';
import MX from '../../../assets/images/png/Mexico.png';
import IN from '../../../assets/images/png/India.png';
import SG from '../../../assets/images/png/Singapore.png';
import IT from '../../../assets/images/png/IT.png';
import FI from '../../../assets/images/png/FI.png';
import NO from '../../../assets/images/png/NO.png';
import PL from '../../../assets/images/png/PL.png';
import DK from '../../../assets/images/png/DK.png';

interface Props {
    config: any
}

const RegionSelect: FC<Props> = (props) => {

    const dispatch = useDispatch();
    const { config } = props;
    const configDetails: any = useSelector(configDetailsSelector);
    const route: string = useSelector(routeSelector);
    const analytics = useContext(GLAnalyticsContext);

    const getCountryFlag = (siteId: string) => {
        switch (siteId) {
            case 'uk-comm':
                return UK;
            case 'us-comm':
                return US;
            case 'sg-comm':
                return SG;
            case 'in-comm':
                return IN;
            case 'mx-comm':
                return MX;
            case 'nl-comm':
                return NL;
            case 'it-comm':
                return IT;            
            case 'fi-comm':
                return FI; 
            case 'no-comm':
                return NO;
            case 'pl-comm':
                return PL;
            case 'dk-comm':
                return DK;
            default:
                return US;
        }
    }

    const options = configDetails.regions.map((region: any, index: number) => {
        const temp = {
            value: region.homeSiteID,
            label: <div className="regionValue"><img src={getCountryFlag(region.homeSiteID)} height="20px" width="28px" />{region.name}</div>,
            order: index
        }
        return temp;
    })

    const selected = (value: any) => {
        const siteId = value.value;
        analytics.fireEvent('home', 'click', 'Country selected: ' + siteId)
        
        // Set the siteId selected in local storage
        // So the user's lastest country choice stays selected
        if (typeof(Storage) !== "undefined") {
            localStorage.setItem("siteId", siteId);
        }

        refreshPLP(siteId);
    }

    const initialValue = options.map((option: any) => {
        if (option.value === config.siteId) {
            return option;
        }
    })

    const refreshPLP = (siteId: any) => {
        const regionsFiltered = configDetails.regions.filter((r:any) => r.homeSiteID === siteId);
        if(regionsFiltered && regionsFiltered.length > 0){
            const regionId = regionsFiltered[0].iD;
            dispatch(setCountry(regionId));
            dispatch(loadConfig(siteId, true));
            resetPagedListings(dispatch, true);
        }else{
            alert("There is an issue with your region selection. Please clear your local cache and reload.")
        }
    };


    const noEntryRoutes = ['admin', 'listingEntry'];
    let noEntry = false;
    for (const [key, value] of Object.entries(RoutePaths)) {
        const match = matchPath(route, value);
        if (match && match.isExact) { // if matchPath confirms same route
            if (noEntryRoutes.includes(key)) {
                noEntry = true;
            } 
        }
    }

    useEffect(() => {
        refreshPLP(config.siteId);
        dispatch(setCountryCode(config.siteId.substring(0,2)))
    }, [config])


    if (noEntry || !configDetails.regions || (configDetails.regions.length < 2)){
        return (<span />);
    } else {
        return (
            <RegionSelectContainer>
                <Select id="regionSelect" onChange={selected} options={options} value={initialValue} />
            </RegionSelectContainer>
        );
    }

};

export default RegionSelect;


const RegionSelectContainer = styled.div`
    width:600px;
    z-index: 5;
    position:relative;
    #regionSelect {
        > div {
            border: none;
        }
        width:200px;
        margin-top:5px;
        color: #666;
        .regionValue {
            display:flex;
            align-items:center;
            font-family: "Futura Md BT";
            img {
                margin-right: 15px;
            }
        }
    }
`;