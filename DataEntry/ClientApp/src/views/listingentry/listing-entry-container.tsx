import React, { FC, useEffect, useMemo } from 'react'
import { loadCurrentListing } from '../../redux/actions/listingEntry/load-current-listing';
import { currentListingSelector } from '../../redux/selectors/entry/current-listing-selector';
import { useDispatch, useSelector } from 'react-redux';
import ListingEntry from './listing-entry';
import { RouteMatch } from '../../types/common/routes';
import { mainMessageSelector } from '../../redux/selectors/system/main-message-selector';
import { MainMessaging } from '../../types/state';
import { Listing } from '../../types/listing/listing';
import MainMessage from '../common/messages/main-message';
import LoadingContainer from '../../components/loader/loading-container'
import { reloadListingIdSelector } from '../../redux/selectors/system/reload-listing-id-selector';
import { setReloadListingId } from '../../redux/actions/system/set-reload-listing-id';
import { Config } from '../../types/config/config';
import { configSelector } from '../../redux/selectors/system/config-selector';
import { isDuplicatingListingSelector } from '../../redux/selectors/system/is-duplicating-listing-selector';
import { setIsDuplicatingListing } from '../../redux/actions/system/set-is-duplicating-listing';

interface Props {
    match: RouteMatch
}

const ListingEntryContainer: FC<Props> = (props) => {

    const dispatch = useDispatch();

    let navTitle: string = "CREATE A LISTING";
    
    const mainMessage: MainMessaging = useSelector(mainMessageSelector);
    const reloadListingId: number = useSelector(reloadListingIdSelector);
    const config: Config = useSelector(configSelector);
    const isDuplicatingListing: boolean = useSelector(isDuplicatingListingSelector);

    const listing: Listing = useSelector(currentListingSelector);
    // this code determines if it has an id (create or edit fork)
    let id: string = "";
    const { match } = props;
    if (match && match.params && match.params.id) {
        id = match.params.id;
        navTitle = "EDIT LISTING";
    }

    const anchors: string[] = ['team', 'property', 'highlights', 'specifications', 'spaces', 'contacts'];

    // always run once
    useEffect(() => {
        dispatch(loadCurrentListing(id, config));
        dispatch(setIsDuplicatingListing(false));
    },[]);

    // run this only when the reload listing id has been changed and is > 0
    useEffect(() => {
        if(reloadListingId > 0){
            id = reloadListingId.toString();
            dispatch(loadCurrentListing(id, config, isDuplicatingListing));
            dispatch(setReloadListingId(-1)); // clear out the flag so it can be tripped again on next save (and this doesnt enter an infinite loop)
        }
    },[reloadListingId]);

    // Remove navTitle if the listing was deleted
    if (listing.isDeleted){
        navTitle = '';
    }

    const ListingEntryMemo = useMemo(() => <ListingEntry key={"listing" + new Date().getTime()} listing={listing} anchors={anchors} navTitle={navTitle} />, [listing, isDuplicatingListing]);


    return (
        <LoadingContainer isLoading={mainMessage.show ? false : true} psuedoEditNav={true} navTitle={navTitle}>
            {mainMessage.show ?
                <MainMessage mainMessage={mainMessage} /> :
                <>{ListingEntryMemo}</>
            }
        </LoadingContainer>
    );

};

export default ListingEntryContainer;