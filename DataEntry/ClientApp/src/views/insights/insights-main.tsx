import React, { FC, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import styled from 'styled-components';
import { RouteMatch } from '../../types/common/routes';
import { loadCurrentInsightsRecord } from '../../redux/actions/insights/load-insights-action';
import { insightsLoadingSelector } from '../../redux/selectors/insights/insights-loading-selector';
import { InsightsRecord } from '../../types/insights/insights-record';
import { currentInsightsRecordSelector } from '../../redux/selectors/insights/current-insights-record-selector';
import InsightsRecordPage from './page/insights-record-page';
import LoadingContainer from '../../components/loader/loading-container';
import MainMessage from '../common/messages/main-message';
import { MainMessaging } from '../../types/state';

export interface Props {
    match: RouteMatch
};

const Insights: FC<Props> = (props) => {

    // this file is basically our insights "root".  it is responsible for loading the insight and communicating
    // to the user the result of the load.  all layout and sub views should go within the InsightsRecordPage view
    const [error, setError] = useState<string | undefined>(undefined);

    const dispatch = useDispatch();

    const navTitle:string = "Property Analytics Report";

    const mainMessaging:MainMessaging = useSelector(insightsLoadingSelector);
    const currentInsightRecord:InsightsRecord = useSelector(currentInsightsRecordSelector);

    let id: string = "";
    const { match } = props;
    if (match && match.params && match.params.id) {
        // the id should be in the url.  this will be the listing id.
        id = match.params.id;
        const listingId = parseInt(id, 10);

        const alreadyLoadingId:boolean = currentInsightRecord && currentInsightRecord.listing && listingId === currentInsightRecord.listing.id;
        if(!mainMessaging.show && !alreadyLoadingId){
            // if we have an ID, we need to load the insights record
            dispatch(loadCurrentInsightsRecord(listingId));
        }
    }else{
        // error state
        setError("Invalid listing ID specified");
    }   

    return (
        <ReportContainer>
            <LoadingContainer isLoading={mainMessaging.show ? false : true} psuedoEditNav={true} forceNavUp={mainMessaging.show} navTitle={navTitle}>
                {mainMessaging.show ?
                    <MainMessage mainMessage={mainMessaging}/> :
                    <InsightsRecordPage navTitle={navTitle} record={currentInsightRecord}/>
                }
            </LoadingContainer>
        </ReportContainer>
    )
};

const ReportContainer = styled.div`
    width: 100%;
    font-family: 'Futura Md BT', helvetica, arial, sans-serif;
`;

const ErrorMessage = styled.div`
    color: red;
`;

export default Insights;