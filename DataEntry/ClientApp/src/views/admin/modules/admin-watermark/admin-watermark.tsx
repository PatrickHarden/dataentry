import React, { FC, useEffect, useState } from 'react';
import styled from 'styled-components';
import { watermarkDetectionProcessStatus, watermarkDetectionProcessSetStatus } from '../../../../api/glAxios';
import AdminWatermarkDisplay from './admin-watermark-display';

export interface WatermarkProcessData {
    processRunning: string,
    unprocessedImages: number,
    runOptionCodes: string[],
    currentRunCode: number
}

const AdminWatermark: FC = (props) => {

    const [ loading, setLoading ] = useState<boolean>(false);
    const [ processData, setProcessData ] = useState<WatermarkProcessData | null>(null);


    const refreshWatermarkProcessData = () => {
        // directy call the api 
        setLoading(true);
        const processStatusCall:Promise<WatermarkProcessData> = watermarkDetectionProcessStatus();
        processStatusCall.then(result => {
            setProcessData(result);
            setLoading(false);
        });
    }

    async function setProcessStatus(status: string) {
        // Example calling re-try check image
        setLoading(true);
        await watermarkDetectionProcessSetStatus(status, (data: any) => {
            setProcessData(data);
            setLoading(false);
        });
    }

    useEffect(() => {
        if(processData === null){
            refreshWatermarkProcessData();
        }
    },[]);
    
    return (
        <AdminWatermarkContainer>
            <AdminWatermarkDisplay loading={loading} processData={processData} startStopHandler={setProcessStatus}/>
        </AdminWatermarkContainer>
    );
}

const AdminWatermarkContainer = styled.div``;

export default AdminWatermark;

