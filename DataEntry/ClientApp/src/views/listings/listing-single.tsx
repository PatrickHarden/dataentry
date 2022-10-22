import React, { FC, useContext } from 'react';
import { Link } from 'react-router-dom';
import styled, { css } from 'styled-components';
import CardBrokerView from '../../components/card-broker-view/card-broker-view';
import { Listing } from '../../types/listing/listing';
import IconViewListingUrl from '../../assets/images/png/icon-view-listing.png';
import IconEditListing from '../../assets/images/png/icon-edit-listing.png';
import IconViewListing from '../../assets/images/png/icon-view.png';
import IconPending from '../../assets/images/png/icon-pending.png';
import IconPublishError from '../../assets/images/png/icon-publisherror.png';
import InsightsIcon from '../../assets/images/png/insights-icon.png';
import { Contact } from '../../types/listing/contact';
import { formatNumberWithCommas } from '../../utils/forms/format-number-commas';
import { Config, ConfigDetails } from '../../types/config/config';
import { useSelector } from 'react-redux';
import { configDetailsSelector } from '../../redux/selectors/system/config-details-selector';
import { formatDecimals } from '../../utils/forms/format-decimals';
import { uneditableStates, nonDraftStates } from '../../constants/listing-filter';
import LazyLoad from 'react-lazyload';
import { GLAnalyticsContext } from '../../components/analytics/gl-analytics-context';
import { formatExternalPublishedURL } from '../../utils/listing/format-published-url';
import { Option } from '../../types/common/option';
import listingTypes from '../../api/lookups/listingTypes';

interface Props {
    listing: Listing
}

const ListingSingle: FC<Props> = (props) => {

    const { listing } = props;


    const configDetails: ConfigDetails = useSelector(configDetailsSelector);
    const config: Config = configDetails.config;

    const analytics = useContext(GLAnalyticsContext);

    // find the cover image
    let coverImageUrl;
    if (props.listing.photos !== undefined && props.listing.photos.length > 0) {
        // sort the photos based on primary then choose the first one (this ensures an image is always shown)
        props.listing.photos.sort((a: any, b: any) => (a.primary ? a : b));
        coverImageUrl = props.listing.photos[0].url;
    }

    let currencySymbol = "$";
    if (configDetails && configDetails.config) {
        currencySymbol = configDetails.config.currencySymbol;
    }

    /* flag used to show certain bits of info for dev */
    let showDevInfo: boolean = false;
    const url: string = window.location.href;
    if (url.indexOf("localhost") > -1 || url.indexOf("dev") > -1 || url.indexOf("uat") > -1) {
        showDevInfo = true;
    }

    const viewListingsButton = () => {
        if ((listing.state && listing.state.toLowerCase() === "published") && (listing.externalPublishUrl != null)) { // TODO: also only display if URL is available
            return <a id={"preview" + listing.id} target="_blank" href={formatExternalPublishedURL(listing, listing.externalPublishUrl)}><CardActionBtn>View Listing <img src={IconViewListingUrl} /></CardActionBtn></a>;
        }
        return <></>;
    }

    const viewPendingStatus = () => {
        if (listing.state && listing.state.toLowerCase() === "publishing") {
            return <PublishingFlag><img style={{ 'marginBottom': '-3px' }} src={IconPending} /> Pending Publish</PublishingFlag>;
        } else if (listing.state && listing.state.toLowerCase() === "unpublishing") {
            return <PublishingFlag><img style={{ 'marginBottom': '-3px' }} src={IconPending} /> Pending Unpublish</PublishingFlag>;
        }
        return <></>;
    }

    const viewErrorStatus = () => {
        if (listing.state && listing.state.toLowerCase() === "publishfailed") {
            return <FailFlag><img style={{ 'marginBottom': '-3px' }} src={IconPublishError} /> Publish Failed!</FailFlag>;
        } else if (listing.state && listing.state.toLowerCase() === "unpublishfailed") {
            return <FailFlag><img style={{ 'marginBottom': '-3px' }} src={IconPublishError} /> Unpublish Failed!</FailFlag>;
        }
        return <></>;
    }



    const addressLine1 = (): string => {
        if (listing.street) {
            return listing.street;
        }
        return "";
    }

    const addressLine2 = (): string => {
        let returnStr: string = "";
        if (listing.city) {
            returnStr += listing.city;
        }
        if (listing.stateOrProvince) {
            returnStr += ", " + listing.stateOrProvince;
        }
        if (listing.postalCode) {
            returnStr += " " + listing.postalCode;
        }
        return returnStr;
    }

    const size = (): string => {
        let returnStr = "";
        if (listing.specifications) {
            if (listing.specifications.totalSpace) {
                returnStr += " " + listing.specifications.totalSpace;
            }
            if (returnStr.length > 0 && listing.specifications.measure) {
                returnStr += " " + listing.specifications.measure.toUpperCase()
            }
        }
        if (returnStr.length === 0) {
            returnStr = "-";
        }
        returnStr = formatNumberWithCommas(returnStr);
        return returnStr;
    }

    const price = (): string => {
        let returnStr = "";
        if (listing.specifications) {
            if (listing.specifications.contactBrokerForPrice) {
                returnStr = "Contact Broker";
            } else if ((listing.listingType === "sale" || listing.listingType === "saleLease" || listing.listingType === "investment") && listing.specifications.salePrice) {
                returnStr += formatPrice(listing.specifications.salePrice);
            }
            else if (listing.listingType === "lease" || listing.listingType === "saleLease") {
                if (listing.specifications.minPrice) {
                    returnStr += formatPrice(listing.specifications.minPrice);
                }
                if (listing.specifications.maxPrice) {
                    if (returnStr.length > 0) {
                        returnStr += " - ";
                    }
                    returnStr += formatPrice(listing.specifications.maxPrice);
                }
                if (returnStr.length > 0 && listing.specifications.measure && listing.specifications.measure.length > 0) {
                    returnStr += " / " + listing.specifications.measure.toUpperCase();
                }
            }
        }
        if (returnStr.length === 0) {
            returnStr = "-";
        }

        return returnStr;
    }

    const formatPrice = (value: number): string => {
        let result = "" + value;
        result = formatDecimals(result, 2);
        result = formatNumberWithCommas(result);
        result = currencySymbol + result;
        return result;
    }

    const status = (): string => {
        return listing.published ? 'Active' : 'Inactive';
    }

    const getListingTypeLabel = (value: string) => {
        let types: Option[] = [];
        types = configDetails && configDetails.config.listingTypes ? configDetails.config.listingTypes.options : listingTypes;
        let temp: string = "";

        if (types && types.length > 0) {
            types.forEach((x: Option) => {
                if (x.value === value) {
                    temp = x.label
                }
            });
        }

        return temp;
    }

    const listingType = (): string => {
        const lTypeText: string = getListingTypeLabel(listing.listingType);
        return ((lTypeText && lTypeText.length > 0) ? lTypeText : '-');
    }

    const displayBrokerList: any[] = [];



    for (let brokerCounter = 0; brokerCounter < 3; brokerCounter++) {
        if (listing.contacts && listing.contacts[brokerCounter]) {
            displayBrokerList.push(listing.contacts[brokerCounter]);
        }
    }

    const getBadgeColor = (propertyType: string) => {
        let color: string | undefined = "#dddddd";
        if (configDetails && configDetails.propertyTypeColors && configDetails.propertyTypeColors.get(propertyType)) {
            color = configDetails.propertyTypeColors.get(propertyType);
        }
        else if (configDetails && configDetails.propertyTypeColors && configDetails.propertyTypeColors.get(propertyType.toLowerCase())) {
            color = configDetails.propertyTypeColors.get(propertyType.toLowerCase());
        }
        return color;
    }

    const getBadgeLabel = (propertyType: string) => {
        let label: string | undefined = propertyType;
        if (configDetails && configDetails.propertyTypeLabels && configDetails.propertyTypeLabels.get(propertyType)) {
            label = configDetails.propertyTypeLabels.get(propertyType);
        }
        else if (configDetails && configDetails.propertyTypeLabels && configDetails.propertyTypeLabels.get(propertyType.toLowerCase())) {
            label = configDetails.propertyTypeLabels.get(propertyType.toLowerCase());
        }

        return label;
    }

    return (
        <BaseListingCard>
            <BaseListingCardHeader>
                <HeaderImage>
                    {coverImageUrl && coverImageUrl.length > 0 &&
                        <LazyLoad height={200} offset={100}>
                            <StyledImage src={coverImageUrl} />
                        </LazyLoad>
                    }
                    <StyledImageOverlay />
                </HeaderImage>
                <HeaderContent>
                    {listing.isDeleted ?
                        <DraftFlag show={true} style={{ color: '#fff', backgroundColor: 'rgb(255, 100, 85)' }}>DELETED</DraftFlag>
                        :
                        <DraftFlag show={!(listing.state && nonDraftStates.includes(listing.state.toLowerCase()))}>DRAFT</DraftFlag>
                    }
                    {listing.propertyType &&
                        <TypeFlag color={getBadgeColor(listing.propertyType)}
                            flagtype={listing.propertyType}>{getBadgeLabel(listing.propertyType)}</TypeFlag>}
                    <BaseNameDiv>
                        <TitleText>
                            {listing.propertyRecordName}
                            {showDevInfo && <PropertyIdText className="dispPropId">ID: {listing.id}</PropertyIdText>}
                        </TitleText>

                    </BaseNameDiv>
                    <BaseAddressDiv>
                        <AddressText>{addressLine1()}</AddressText>
                        <AddressText>{addressLine2()}</AddressText>
                    </BaseAddressDiv>
                    <ButtonArea>

                        <Link to={"/le/" + listing.id} onClick={() => { analytics.fireEvent('listing', 'click', 'edit listing ' + listing.id, 'beacon'); }}>
                            <CardActionBtn
                                style={{ 'float': 'left' }}>
                                <img style={{ 'marginBottom': '-2px' }}
                                    src={(listing.state && uneditableStates.includes(listing.state.toLowerCase())) ? IconViewListing : IconEditListing} />
                                {(listing.state && uneditableStates.includes(listing.state.toLowerCase())) ? "View" : "Edit"} </CardActionBtn></Link>
                        {viewListingsButton()}
                        {viewPendingStatus()}
                        {viewErrorStatus()}
                    </ButtonArea>
                </HeaderContent>
            </BaseListingCardHeader>
            {configDetails && configDetails.config && configDetails.config.insightsEnabled &&
                <InsightsSection>
                    <img src={InsightsIcon} />
                    <StyledInsightLink href={"/insights/" + listing.id}>View Analytics</StyledInsightLink>
                </InsightsSection>
            }
            <InfoSection>
                <Row>
                    <InfoLabel>Total Space Available</InfoLabel>
                    <InfoField>{size()}</InfoField>
                </Row>
                <Row>
                    <InfoLabel>Price Range</InfoLabel>
                    <InfoField title={price()}>{price()}</InfoField>
                </Row>
                <Row>
                    <InfoLabel>Listing Type</InfoLabel>
                    <InfoField>{listingType()}</InfoField>
                </Row>
            </InfoSection>

            <BrokerSection>
                {listing.contacts && displayBrokerList.map((contact: Contact, index: number) => (
                    <CardBrokerView contact={contact} key={index} />
                ))}
                {listing.contacts && (listing.contacts.length > 3) &&
                    <div style={{ "flex": "1" }}>  <AdditionalBrokers>+{listing.contacts.length - 3}</AdditionalBrokers> </div>

                }

            </BrokerSection>

        </BaseListingCard>

        /*
        add back in when listing type is fixed....
        <br style={{'lineHeight': '200%'}}/>
    <InfoLabel>Listing Type:</InfoLabel>
    <InfoField>{listingType()}</InfoField>
    */

    );
};

export default React.memo(ListingSingle);


const BaseListingCard = styled.div`
    /* height: 450px; */
    width: auto;
    margin: auto;
    border: 1px solid #e8e8e8;
    box-shadow: 0 0 10px #e8e8e8;
    overflow:hidden;
    position:relative;
    margin-bottom: 30px;
`;

/*
const imgUrlCss = css` 
    background-image: 
    linear-gradient(to bottom, rgba(255,255,255,0) 0%,rgba(255,255,255,0) 35%,rgba(0,0,0,.9) 100%),
    url( ${(props: ImgCardProps) => (props.photo)}); 
`;
*/

const BaseListingCardHeader = styled.div`
    width: 100%;
    height: 200px;
    background-position:center center;
    background-size: cover;
    background-repeat: no-repeat;
    background-color: #006A4D;
    position:relative;
`;

const ButtonArea = styled.div`
    /* background-color: #ccddcc; */
    /* background-color: #29BC9C; */
    position: relative;
    height: 30px;
    text-align: right;
`;

/*
interface ImgCardProps {
    photo: string;
}
*/

const Row = styled.div`
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    width: 100%;
`;

const HeaderImage = styled.div`
    width: 100%;
    height: 200px;
`;

const StyledImage = styled.img`
    height: 200px;
    width: 100%;
    position: absolute;
    top: 0;
    z-index: 0;
`;

const StyledImageOverlay = styled.div`
    background-image: linear-gradient(to bottom, rgba(255,255,255,0) 0%,rgba(255,255,255,0) 35%,rgba(0,0,0,.9) 100%);
    width: 100%;
    height: 200px;
    position: absolute;
    top: 0;
    z-index: 1;
`;

const HeaderContent = styled.div`
    width: 100%;
    height: 200px;
    position: absolute;
    top: 0;
    z-index: 2;
`;

const BaseNameDiv = styled.div`
    position: relative;
    color:white;
    width: 320px;
    text-align:left;
    margin:auto;
    height:112px;
`;
const TitleText = styled.h2`
    position:absolute;
    bottom: 0;
    color: white;
    margin:0px;
    left:10px;
    font-size:18px;
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
`;

const PropertyIdText = styled.div`
    font-size:11px; 
`;

const BaseAddressDiv = styled.div`
    position: relative;
    color:white;
    width: auto;
    text-align:left;
    margin:auto;
    height:40px;
    margin-top:8px;
    margin-bottom:8px;
    padding-left:10px;
    font-size:12px;
`
const AddressText = styled.div`
    color: white;
    margin:0px;
    font-weight:normal;
    font-size: 14px;
`;

const DraftFlag = styled.div`
    z-index:10;
    background-color: white;
    border-radius: 2px;
    height:24px;
    /* width:90px; */
    padding-left:8px;
    padding-right:8px;
    position:absolute;
    top: 10px;
    left: 10px;
    line-height:25px;
    text-align:center;
    color: #999999;
    font-size: 11px;
    ${(props: PublishedFlagProps) => props.show ? "" : "display:none"}
`;

interface PublishedFlagProps {
    show: boolean;
}


const TypeFlag = styled.div`
    z-index:10;
    border-radius: 2px;
    height:24px;
    min-width:50px;  /* 30px; */
    padding-left:10px;
    padding-right:10px;
    position:absolute;
    top: 10px;
    right: 10px;
    line-height:24px;
    text-align:center;
    color: #fff;
    font-size: 11px;
    background-color: ${(props: TypeFlagProps) => props.color ? props.color : "#dddddd"};
    text-transform: uppercase;
`;

interface TypeFlagProps {
    color: string | undefined;
    flagtype: string;
}


const PublishingFlag = styled.div`
    z-index:10;
    display:inline-block;
    position:relative;
    background-color: #FFDD00;
    border-radius: 20px;
    height:24px;
    margin-right:10px;
    margin-left:10px;
    margin-top:-3px;
    top: 5px;
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
    margin-left:10px;
    margin-top:-3px;
    top: 5px;
    line-height:24px;
    text-align:center;
    color: white;
    font-size: 12px;
    padding-left:8px;
    padding-right:8px;
`;

const CardActionBtn = styled.div`
    cursor: pointer;
    display:inline-block;
    position:relative;
    top:5px;
    vertical-align:center;
    border-radius: 20px;
    height:24px;
    line-height:24px;
    font-size:12px;
    min-width:50px; 
    text-align:center;
    background-color:rgba(255,255,255,0.2);
    padding-left:12px;
    padding-right:12px;
    color:white;
    text-decoration:none;
    margin-right:10px;
    margin-left:10px;
    margin-top:-3px;
    &:hover {
        background-color: #00B2DD;
    }

`;


const InfoSection = styled.div`
    padding:5px 15px;
    margin-top:10px;
    height: 120px;
    line-height: 28px;
`;

const InfoLabel = styled.div`
    width: 50%;
    color: #666666;
    display: inline;
`;

const InfoField = styled.div`
    width: 50%;
    height: 30px;
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
    color: #666666;
    display: inline-block;
    overflow:hidden;
    text-overflow: ellipsis;
    text-indent: 10px;
`;

const BrokerSection = styled.div`
    padding:0 30px;
    height: 80px;
    background-color: #F8F8F8;
    text-align:center;
    justify-content:center;

    display:flex;
`;

const AdditionalBrokers = styled.div`
    /* line-height: 50px;
    vertical-align: top;
    color: #cccccc;
    display: inline-block;
    margin-left: 10px;
    font-size: 30px; */
    background:#cecece;
    border-radius:50%;
    height:40px;
    width:40px;
    font-size:14px;
    color:#ffffff;/* #006A4D; */
    text-align:center;   
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
    line-height:40px;
    /* margin-bottom:4px; */
    display: inline-block;
    vertical-align: top;
    margin-left: 5px;
    margin-top: 5px;
    
`

const InsightsSection = styled.div`
    display: flex-inline;
    width: 100%;
    padding: 10px 0 0 10px;
`;

const StyledInsightLink = styled.a`
    color: #006A4D;
    font-size: 14px;
    font-weight: 400;
    height: 18px;
    text-decoration: none;
    margin-left: 10px;
`;