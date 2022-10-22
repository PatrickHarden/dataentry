import React, { FC, useState } from 'react';
import { Col, Row, Grid } from 'react-styled-flexboxgrid';
import styled from 'styled-components';
import { WatermarkProcessData } from './admin-watermark';
import StyledButton from '../../../../components/styled-button/styled-button';

export interface Props {
    processData: WatermarkProcessData | null,
    loading: boolean,
    startStopHandler?: (status:string) => void;
}

const AdminWatermarkDisplay: FC<Props> = (props) => {

    const {processData, loading, startStopHandler} = props;

    const unknownProcessStatus:boolean = processData && processData.processRunning === "True" ? false : processData && processData.processRunning === "False" ? false : true;

    const findDefaultRunOptionCode = () => {        
        if(processData && processData.runOptionCodes && processData.currentRunCode > -1){
            return processData.runOptionCodes[processData.currentRunCode];
        }
        return "";
    }

    const defaultCode = findDefaultRunOptionCode();

    const [runOptionCode, setRunOptionCode] = useState<string>(defaultCode);

    const execute = () => {
        if(startStopHandler){
            startStopHandler(runOptionCode);
        }
    }

    const changeRunOptionCode = (event:any) => {
        event.preventDefault();
        setRunOptionCode(event.currentTarget.value);
    }
    
    return (
        <>
            <DisplayContainer>
                <BoundingBox>
                    <DisplayHeader>Watemark Process Status</DisplayHeader>
                    {!loading && 
                        <>
                            <Row>
                                <Col xs={3}><Indicator>Batch Process Running?</Indicator></Col>
                                <Col xs={2}>
                                    <Value>
                                    { unknownProcessStatus ? "Unknown" : processData && processData.processRunning === "True" ? "Yes" : "No"}
                                    </Value>
                                </Col>
                            </Row>
                            <Row style={{marginBottom: '15px'}}>
                                <Col xs={5}><HelperMessage>The indicator will only be "Yes" if an admin has kicked off a batch process using the button below.  This does not indicate that watermarking is turned off or on.</HelperMessage></Col>
                            </Row>
                            <Row style={{marginBottom: '15px'}}>
                                <Col xs={3}><Indicator># "Not Processed"</Indicator></Col>
                                <Col xs={2}>
                                    <Value>
                                        { processData ? processData.unprocessedImages : "Unknown"}
                                    </Value>
                                </Col>
                            </Row>
                            {defaultCode && 
                                <Row style={{marginBottom: '15px'}}>
                                    <Col xs={3}><Indicator>Currently Running</Indicator></Col>
                                    <Col xs={2}>
                                        <Value>
                                            { defaultCode }
                                        </Value>
                                    </Col>
                                </Row>
                            }
                            {processData && processData.runOptionCodes && 
                                <Row style={{marginBottom: '15px'}}>
                                    <Col xs={3}><Indicator>Code</Indicator></Col>
                                    <Col xs={2}>
                                        <select onChange={changeRunOptionCode} value={runOptionCode}>
                                            {
                                                processData.runOptionCodes.map((roCode:string, index:number) => {
                                                    if(processData && processData.currentRunCode && index === processData.currentRunCode){
                                                        return <option key={"k_"+roCode} value={roCode} selected={true}>{roCode}</option>;
                                                    }else{
                                                        return <option key={"k_"+roCode} value={roCode}>{roCode}</option>;
                                                    }
                                                })
                                            }
                                        </select>
                                    </Col>
                                </Row>
                            }
                            <Row>
                                <Col xs={5}>
                                    {processData && <StyledButton onClick={execute}>Run Batch Process</StyledButton>}
                                </Col>
                            </Row>  
                        </>
                    }
                </BoundingBox>
            </DisplayContainer>
        </>
    );
}

const HelperMessage = styled.span`
    font-style: italic;
    color: #A9A9A9;
`;

const DisplayContainer = styled.div`
    font-family: 'Futura Md BT', helvetica, arial, sans-serif;
    padding: 25px 120px 0 120px;
`;

const LoadingBox = styled.div`
    text-align: center;
    width: 300px;
    font-size: 16px;
    font-weight: bold;
`;

const BoundingBox = styled.div`
    border: 1px solid #cccccc;
    padding: 10px;
`;

const DisplayHeader = styled.div`
    font-size: 22px;
    font-weight: bold;
    color: #00a657;
    margin-bottom: 15px;
    border-bottom: 1px solid #000;
    width: 275px;
`;

const Indicator = styled.span`
    font-size: 18px;
    font-weight: bold;
`;

const Value = styled.span`
    font-size: 16px;
`;

export default AdminWatermarkDisplay;

