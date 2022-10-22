import React, { FC, useEffect } from 'react';
import styled from 'styled-components';
import PageHeader from '../common/header/page-header';
import { Col, Row } from 'react-styled-flexboxgrid';
import MIQSidebar from './miq-sidebar';
import { AutoCompleteResult } from '../../types/common/auto-complete';
import { selectMIQSearchResult } from '../../redux/actions/miq/set-miq-search-result';
import { useDispatch, useSelector } from 'react-redux';
import { miqCurrentRecordsSelector } from '../../redux/selectors/miq/miq-current-record-selector';
import { generateKey } from '../../utils/keys';
import RecordsDisplayView from './record-display/records-display-view';
import SelectRecordView from './select-record/select-record-view';
import { Listing } from '../../types/listing/listing';
import { miqStatusSelector } from '../../redux/selectors/miq/miq-status-selector';
import { MoonLoader } from 'react-spinners';
import { setCancelAction } from '../../redux/actions/system/set-cancel-action';
import { prepareRecordForAddEdit } from '../../redux/actions/miq/prepare-record-for-add-edit';
import { configSiteIdSelector } from '../../redux/selectors/system/siteid-selector';
import { configSelector } from '../../redux/selectors/system/config-selector';
import { setMiqSpaces } from '../../redux/actions/miq/set-miq-spaces';
import { Config } from '../../types/config/config';
import { setMiqAddress } from '../../redux/actions/system/set-miq-address';
import { setDatasourcePopup, clearDatasourcePopup } from '../../redux/actions/system/set-datasource-popup';

const MIQImportContainer: FC = () => {

    const anchors: string[] = ['search'];
    const dispatch = useDispatch();

    // selectors
    const currentRecords = useSelector(miqCurrentRecordsSelector);  // the loaded records (disabled in table(s))

    const status = useSelector(miqStatusSelector);  // loading indicator
    const siteId = useSelector(configSiteIdSelector); // config's siteId
    const config: Config = useSelector(configSelector);

    // event handlers

    // user selects a property from the typeahead search
    const handleSelectProperty = (selection: AutoCompleteResult) => {
        dispatch(selectMIQSearchResult(selection));
    }


    // user presses the create or edit button
    const handleUserAddEditAction = (record: Listing) => {
        if (record && record.alternatePostalAddresses && record.alternatePostalAddresses.length > 0) {
            dispatch(setMiqAddress({ show: true }));
        } else {
            dispatch(setCancelAction({ goto: "/miq/import" }));
            dispatch(prepareRecordForAddEdit(record, siteId));
        }
    }

    // ensure the cancel action from this screen takes the user back to the home page
    useEffect(() => {
        dispatch(setMiqSpaces([]));
        dispatch(setCancelAction({ goto: "/" }));
    }, []);

    return (
        <PageContainer>
            <PageHeader pageName={"Import from MIQ"} showSearch={false} />
            <GrayDivider />
            <GridContainer>
                <Row between="sm">
                    <Col sm={2} style={{ maxWidth: '150px', borderRight: '1px solid #eee' }}>
                        <MIQSidebar anchors={anchors} />
                    </Col>
                    <Col sm={10}>
                        <DisclaimerContainer>
                            <Disclaimer>Legal Disclosure: All data and images entered into Global Listings must originate from CBRE or the property owner. Data and images from third-party listing platforms or websites cannot be entered into Global Listings under any circumstances.</Disclaimer>
                        </DisclaimerContainer>
                        {!status.processingFiles &&
                            <SelectionContainer>
                                <SelectRecordView selectionHandler={handleSelectProperty} />
                            </SelectionContainer>
                        }
                        {status.loading && <LoadingContainer><MoonLoader color="#00B2DD" /></LoadingContainer>}
                        {!status.loading && currentRecords && <RecordsContainer>
                            <RecordsDisplayView
                                key={generateKey()}
                                records={currentRecords}
                                allowAssign={false}
                                assignMode={false}
                                actionHandler={handleUserAddEditAction}
                                allowUnpublish={false} />
                        </RecordsContainer>
                        }
                    </Col>
                </Row>
            </GridContainer>
        </PageContainer>
    );
};

const PageContainer = styled.div`
    background: #fdfdfd;
    width:100%;
    min-height: 100vh;
`;

const GrayDivider = styled.div`
    background: #F2F2F2;
    width: 100%;
    height:40px;
`;

const DisclaimerContainer = styled.div`
    width: 100%
    border-bottom:solid 1px #E2E2E2;
    margin:0 auto;
`;

const Disclaimer = styled.h5`
    font-size: 14px;
    font-weight: 400;
    color: #666;
    font-style: italic;
`;

const SelectionContainer = styled.div``;

const RecordsContainer = styled.div`
    margin-top: 90px;
`;

const LoadingContainer = styled.div`
    display: flex;
    width: 100%;
    height: 200px;
    align-items: center;
    justify-content: center;
`;

const GridContainer = styled.div`
    > div {
        max-width:100%;
    }
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
`;

export default MIQImportContainer;