import React, { FC, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import ListingsHeader from './listings-header';
import ListingsFilter from './filters/listings-filter-bar';
import ListingsMain from './listings-main';
import styled from 'styled-components';
import { Listing } from '../../types/listing/listing';
import { MainMessaging, AlertMessaging } from '../../types/state';
import { mainMessageSelector } from '../../redux/selectors/system/main-message-selector';
import { alertMessageSelector } from '../../redux/selectors/system/alert-message-selector';
import MainMessage from '../common/messages/main-message';
import AlertMessage from '../common/messages/alert-message';
import { clearAlertMessage } from '../../redux/actions/system/set-alert-message';
import { getListingsPaged } from '../../redux/actions/pagedListings/load-listings-paged';
import { getAssignedListingsCount } from '../../redux/actions/miq/get-assigned-listings-count';
import { pagedListingsSelector } from '../../redux/selectors/pagedListings/paged-listings-selector';
import { skipSelector } from '../../redux/selectors/pagedListings/skip-selector';
import { takeSelector } from '../../redux/selectors/pagedListings/take-selector';
import { moreRecordsSelector } from '../../redux/selectors/pagedListings/more-records-selector';
import { FilterSetup, Filter } from '../../types/listing/filters';
import { filterSetupsSelector } from '../../redux/selectors/pagedListings/filter-setups-selector';
import { extractFilters } from '../../utils/extract-filters';
import { searchTextSelector } from '../../redux/selectors/pagedListings/search-text-selector';
import { filtersChanged } from '../../redux/actions/pagedListings/change-filters';
import { clearMIQData } from '../../redux/actions/miq/clear-miq-data';
import { setCancelAction } from '../../redux/actions/system/set-cancel-action';
import { assignedListingsSelector } from '../../redux/selectors/pagedListings/assigned-listings-selector';
import { setSaveAction } from '../../redux/actions/system/set-save-action';
import { setMIQAddEditRecord } from '../../redux/actions/miq/select-miq-record-add-edit'

const ListingsContainer: FC = () => {

    const dispatch = useDispatch();

    const mainMessage: MainMessaging = useSelector(mainMessageSelector);
    const alertMessage: AlertMessaging = useSelector(alertMessageSelector);

    const filterSetups: FilterSetup[] = useSelector(filterSetupsSelector);
    const searchText: string = useSelector(searchTextSelector);
    const listings: Listing[] = useSelector(pagedListingsSelector);
    const assignedListings: Listing[] = useSelector(assignedListingsSelector);
    const skip: number = useSelector(skipSelector);
    const take: number = useSelector(takeSelector);
    const moreRecordsAvailable: boolean = useSelector(moreRecordsSelector);

    const filterChangeHandler = (changedFilters: FilterSetup[]) => {
        dispatch(filtersChanged(changedFilters));
    }

    useEffect(() => {
        clearAlertMessage(dispatch);
        dispatch(clearMIQData()); // clear out any lingering MIQ data when we hit the listings page
        dispatch(setCancelAction({goto: "/"})); // ensure any cancel action in a subsequent page returns to this page
        dispatch(setSaveAction({}));
        dispatch(getAssignedListingsCount());
        getNextListings();
        dispatch(setMIQAddEditRecord(null)) // clear out imported miq record
    }, []);

    const getAssignedCount = () => {
        dispatch(getAssignedListingsCount());
    }

    const getNextListings = () => {
        const filters: Filter[] = extractFilters(filterSetups, searchText);
        dispatch(getListingsPaged(skip, take, filters));
    }

    return (
        <>
            <ListingsHeader showSearch={true} />
            {alertMessage && alertMessage.show && <AlertMessage alertMessage={alertMessage} />}
            <ListingsMainContainer style={{ marginTop: '20px', marginBottom: '260px' }}>
                {
                    <ListingsFilter setups={filterSetups} changeFilters={filterChangeHandler} />
                }
                {
                    mainMessage.show && <MainMessage mainMessage={mainMessage} />
                }
                {
                    !mainMessage.show && <ListingsMain normalListings={listings} assignedListings={assignedListings} 
                                                loadListingsHandler={getNextListings} moreRecordsAvailable={moreRecordsAvailable} />
                }
            </ListingsMainContainer>
        </>
    );

};

const ListingsMainContainer = styled.div`
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
`;

export default ListingsContainer;