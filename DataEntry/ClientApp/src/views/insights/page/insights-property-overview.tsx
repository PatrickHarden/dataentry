import React, { FC, useState } from 'react';
import { Col, Grid, Row } from 'react-styled-flexboxgrid';
import styled from 'styled-components';
import { InsightsRecord } from '../../../types/insights/insights-record';
import { Listing } from '../../../types/listing/listing';
import DefaultPropertyImage from '../../../assets/images/png/property-default.png';
import DiagnolArrowIcon from '../../../assets/images/png/diagnol-arrow-icon.png';
import { formatExternalPublishedURL } from '../../../utils/listing/format-published-url';
import { Contact } from '../../../types/listing/contact';
import ImageAvatar, { ImageAvatarSize, PlaceholderSize } from '../../../components/image-avatar/image-avatar';
import { generateInitials } from '../../../components/contact-card/util/generate-initials';

export interface Props {
    record: InsightsRecord
};

const InsightsPropertyOverview: FC<Props> = (props) => {
    
    const { record } = props;
    const listing:Listing = record.listing;

    const [showInsightsDialog, setShowInsightsDialog] = useState<boolean>(false);

    const generateCoverImage = () => {
        let src:any = DefaultPropertyImage;
        if(listing && listing.photos){
            const photoArr = [...listing.photos];
            photoArr.sort((a: any, b: any) => (a.primary ? a : b));
            if(photoArr.length > 0){
                src = photoArr[0].url
            }
        }
        return (
            <CoverImage src={src} alt="Cover Image"/>
        );
    }

    const generateAddressLine2 = ():string => {
        let addressLine2:string = "";
        if(listing.city && listing.city.length > 0){ addressLine2 += listing.city; }
        if(listing.stateOrProvince && listing.stateOrProvince.length > 0){ 
            if(addressLine2.length > 0){ 
                addressLine2 += ", ";
            }
        }
        if(listing.postalCode && listing.postalCode.length > 0){ 
            if(addressLine2.length > 0){
                addressLine2 += " ";
            }
            addressLine2 += listing.postalCode; 
        }


        return addressLine2;
    }

    const getDaysOnMarket = ():string => {
        let val:string = "---";
        if(listing.state && listing.state.toLowerCase() === "published"){
            if (listing.dateListed)
            {
                const timeDiff = Math.abs(new Date().getTime() - listing.dateListed.getTime());
                val = Math.ceil(timeDiff/ (1000*60*60*24)).toString();
            }
            else if (listing.datePublished)
            {
                const timeDiff = Math.abs(new Date().getTime() - listing.datePublished.getTime());
                val = Math.ceil(timeDiff/ (1000*60*60*24)).toString();
            }
        }
        return val;
    }

    const getDateListed = ():string => {
        let val:string = "---";
        if(listing.state && listing.state.toLowerCase() === "published"){
            if (listing.dateListed)
            {
                val = listing.dateListed.toLocaleDateString();
            }
            else if (listing.datePublished)
            {
                val = listing.datePublished.toLocaleDateString();
            }
        }
        return val;
    }

    const getLastUpdated = ():string => {
        let val:string = "---";
        if(listing.dateUpdated){
            val = listing.dateUpdated.toLocaleDateString();
        }
        return val;
    }

    const getPropertyId = ():string => {
        return listing.externalId || "";
    }

    return (
        <Container>
            { listing &&
                <Grid>
                    <Row>
                        { /* image and view listing link column */ }
                        <Col sm={4}>
                            <Row>
                                { /* property cover image */ }
                                <Col sm={6}>
                                    <CoverImageContainer>{ generateCoverImage() }</CoverImageContainer>
                                    { listing && listing.state && listing.state.toLowerCase() === "published" && listing.externalPublishUrl && listing.externalPublishUrl.length > 0 &&
                                        <ViewPropertyLink><a target="_blank" href={formatExternalPublishedURL(listing,listing.externalPublishUrl) }>
                                            View Listing <img src={DiagnolArrowIcon} alt="View Property"/></a>
                                        </ViewPropertyLink>
                                    }
                                </Col>
                            </Row>
                        </Col>
                        { /* property information column */ }
                        <Col sm={5} style={{marginLeft: '10px'}}>
                                { /* property name header */}
                                <Row>    
                                    <Col sm={12}>
                                        <PropertyHeader>{listing.propertyName}</PropertyHeader>
                                    </Col>
                                </Row>
                                { /* address lines */}
                                <Row>
                                    <Col sm={12}>
                                        <AddressLine>{listing.street}</AddressLine>
                                        <AddressLine>{generateAddressLine2()}</AddressLine>
                                    </Col>
                                </Row>
                                { /* property information */}
                                <InfoRow>
                                    <Col sm={4}>
                                        <InfoHeader>Days on Market</InfoHeader>
                                        <InfoValue>{getDaysOnMarket()}</InfoValue>
                                    </Col>
                                    <Col sm={4}>
                                        <InfoHeader>Date Listed</InfoHeader>
                                        <InfoValue>{getDateListed()}</InfoValue>
                                    </Col>
                                    <Col sm={4}>
                                        <InfoHeader>Last Updated</InfoHeader>
                                        <InfoValue>{getLastUpdated()}</InfoValue>
                                    </Col>
                                </InfoRow>
                                { /* property information */}
                                <InfoRow>
                                    <Col sm={12}>
                                        <InfoHeader>Property Id</InfoHeader>
                                        <InfoValue>{getPropertyId()}</InfoValue>
                                    </Col>
                                </InfoRow>
                        </Col>
                        { /* brokers */ }
                        <Col sm={2}>
                            { listing.contacts && 
                                <BrokerSection>
                                    {
                                        listing.contacts.map((broker:Contact) => {
                                            return <BrokerRow key={"broker-row"+broker.contactId}>
                                                <Col>
                                                    <BrokerContainer>
                                                        <ImageAvatar key={"ia_"+broker.contactId} 
                                                            placeholderSize={PlaceholderSize.SMALL}
                                                            placeholder={generateInitials(broker)}  
                                                            avatarSize={ImageAvatarSize.SMALL}
                                                            imageURL={broker.avatar}
                                                            allowUpload={false}/>
                                                        <BrokerName>{broker.firstName + " " + broker.lastName}</BrokerName> 
                                                    </BrokerContainer>
                                                </Col>
                                            </BrokerRow>;
                                        })
                                    }
                                </BrokerSection>
                            }
                        </Col>
                    </Row>
                </Grid>
            }
        </Container>
    );
};

const Container = styled.div`
    width: 100%;
    padding: 30px 0 30px 0;
`;

const CoverImageContainer = styled.div``;

const CoverImage = styled.img`
    width: 350px;
`;

const ViewPropertyLink = styled.div`
    margin-top: 20px;
    width: 120px;
    height: 30px;
    line-height: 30px;
    background: #006A4D;
    color: #FFFFFF;
    border-radius: 2px;
    text-align: center;
    cursor: pointer;
    a, a:visited {
        color: #FFFFFF;
        text-decoration: none;
    }
`;

const PropertyHeader = styled.div`
    font-size: 36px;
    font-weight: 400;
    color: #003F2D;
`;

const AddressLine = styled.div`
    font-size: 18px;
    font-weight: 600;
    color: #003F2D;
`;

const InfoRow = styled(Row as any)`
    margin-top: 30px;
`;

const InfoHeader = styled.div`
    font-size: 14px;
    font-weight: 400;
`;

const InfoValue = styled.div`
    font-size: 21px;
    font-weight: 700;
`;

const BrokerSection = styled.div`
    border-left: 1px solid #ccc;
    height: 100%;
`;

const BrokerRow = styled(Row as any)``;

const BrokerContainer = styled.div`
    display: flex;
    align-items: center;
    border-bottom: 1px solid #cccccc;
    width: 250px;
    padding: 15px;
    margin-left: 10px;
`;

const BrokerName = styled.div`
    color: #003F2D;
    font-size: 18px;
    font-weight: 600;
    margin-left: 10px;
`;

export default InsightsPropertyOverview;