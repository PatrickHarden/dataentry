import React, { FC } from 'react';
import { Listing } from '../../types/listing/listing';
import styled from 'styled-components';
import InfiniteScroll from 'react-infinite-scroll-component';
import ListingSection from '../../components/listing-section/listing-section';

export interface Props {
    readonly normalListings: Listing[],
    assignedListings: Listing[],
    readonly moreRecordsAvailable: boolean,
    readonly loadListingsHandler: () => void
}

const ListingsMain: FC<Props> = (props) => {

    const { normalListings, assignedListings, moreRecordsAvailable, loadListingsHandler } = props;

    const totalLength = normalListings.length + assignedListings.length;

    return (
        <ListingsContainer>
            { totalLength > 0 && 
                <StyledInfiniteScroll
                    dataLength={totalLength} 
                    next={loadListingsHandler}
                    hasMore={moreRecordsAvailable}
                    loader={<LoadingDiv>Loading more listings, one moment please...</LoadingDiv>}>
                        { assignedListings.length > 0 && <ListingSection header="Listings Assigned from MIQ" listings={assignedListings} /> }
                        { normalListings.length > 0 && <ListingSection header="My Listings" listings={normalListings} /> }
                </StyledInfiniteScroll>
            }
        </ListingsContainer>
    )
};

const ListingsContainer = styled.div`
    .listing-card-col {
        > div {
            box-shadow: 0 4px 6px rgba(204,204,204,0.12), 0 2px 4px rgba(204,204,204,0.24);
            transition: all 0.3s cubic-bezier(.25,.8,.25,1);
            &:hover {
                box-shadow: 0 7px 14px rgba(204,204,204,0.20), 0 5px 5px rgba(204,204,204,0.17);
            }
        }
    }
`;

const StyledInfiniteScroll = styled(InfiniteScroll)` 
    overflow: hidden !important;
`;

const LoadingDiv = styled.div`
    width: 100%;
    text-align: center;
    margin-top: 20px;
    color: #006A4D;
    font-size: 22px;
    font-weight: bold;
`;

export default ListingsMain;