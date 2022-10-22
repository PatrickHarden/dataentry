import React, { FC } from 'react';
import styled from 'styled-components';
import { Listing } from '../../types/listing/listing';
import ListingSingle from '../../views/listings/listing-single';    /* todo: move one or the other */
import { Col, Row } from 'react-styled-flexboxgrid';

export interface Props {
    header: string,
    listings: Listing[]
}

const ListingSection: FC<Props> = (props) => {

    const { header, listings } = props;

    return (
        <>
            <Row><StyledHeader>{header}</StyledHeader></Row>
            <Row>
                {listings.map((listing:Listing, index: number) => (
                    <Col xs={12} sm={6} md={4} lg={3} key={'listing-card-' + listing.id} className="listing-card-col">
                        <ListingSingle listing={listing} key={index} />
                    </Col>
                ))}
            </Row>
        </>
    );
};

const StyledHeader = styled.span`
    font-family: "Futura Md BT";
    font-weight: 700;
    font-size: 21px;
    color: #006A4D;
    text-transform: uppercase;
    padding: 20px 0 20px 10px;
`;

export default ListingSection;