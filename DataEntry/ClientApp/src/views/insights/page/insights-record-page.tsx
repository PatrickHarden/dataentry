import React, { FC, useState } from 'react';
import styled from 'styled-components'; 
import { InsightsRecord } from '../../../types/insights/insights-record';
import InsightsPropertyOverview from './insights-property-overview';
import InsightsNavBar from './insights-navbar';
import { Grid, Row, Col } from 'react-styled-flexboxgrid';
import InsightsSidebar from './insights-sidebar';
import InsightsShareDialog from './sharing/insights-share-dialog';

export interface Props {
    navTitle: string,
    record: InsightsRecord
};

const InsightsRecordPage: FC<Props> = (props) => {

    const { navTitle, record } = props;

    const [showInsightsShareDialog, setShowInsightsShareDialog] = useState<boolean>(false);

    // note: this page is meant to be a layout page. as we build out reporting sections, we will add them here
    return (
        <Container>
            <InsightsNavBar navTitle={navTitle} showShareButton={true} showPrintButton={true} clickShareHandler={() => setShowInsightsShareDialog(true)}/>
            <Grid>
                <Row>
                    <Col sm={1} style={{ maxWidth: '150px', borderRight: '1px solid #eee' }}>
                        <InsightsSidebar listing= {record.listing} anchors={['property']}/>
                    </Col>
                    <Col sm={10}>
                        <InsightsPropertyOverview record={record}/>
                        <Spacer/>
                    </Col>
                </Row>
            </Grid>
            { showInsightsShareDialog && <InsightsShareDialog shareLink={window.location.href} closeHandler={() => setShowInsightsShareDialog(false)} />}
        </Container>
    )
};

const Container = styled.div`
    width: 100%;
`;

const Spacer = styled.div`
    width: 100%;
`;

export default InsightsRecordPage;