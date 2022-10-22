import React, { FC } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { configSelector } from '../../redux/selectors/system/config-selector';
import { Config } from '../../types/config/config';

const Insights404: FC = () => {

    const config:Config = useSelector(configSelector);

    return (
        <>
            { config.insightsEnabled && 
                <ErrorMessage>A specific property is required to view insights.  
                    Please go to My Listings and select View Analytics on the property you wish to view.
                </ErrorMessage>
            }
            {
                !config.insightsEnabled &&
                    <ErrorMessage>Insights are not enabled for this market currently.</ErrorMessage>
            }
        </>
    );
}

const ErrorMessage = styled.div`
    color: red;
    font-size: 20px;
`;

export default Insights404;