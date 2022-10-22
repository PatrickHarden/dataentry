import React, { FC, useContext } from 'react';
import StyledButton from '../../../components/styled-button/styled-button';
import { useSelector, useDispatch } from 'react-redux';
import { GLAnalyticsContext } from '../../../components/analytics/gl-analytics-context';
import styled from 'styled-components';
import { cancelActionSelector } from '../../../redux/selectors/system/cancel-action-selector';
import { push } from 'connected-react-router';

interface Props{
    pageName:string,
    showSearch:boolean
}


const PageHeader: FC<Props> = (props) => {

    const dispatch = useDispatch();
    const analytics = useContext(GLAnalyticsContext);
    const { pageName } = props;
    const cancelAction = useSelector(cancelActionSelector);

    return (
        <StyledPageHeader>
            <StyledInnerContainer>
                <StyledTitle style={{ float: "left", 'marginRight': '15px', 'marginTop': '8px' }}>{pageName}</StyledTitle>
                <StyledButton id="cancelButton" style={{ float: "right" }} onClick={() => {analytics.fireEvent('cancelButton', 'click', 'Cancel button click', 'beacon'); dispatch(push(cancelAction && cancelAction.goto && cancelAction.goto.length > 0 ? cancelAction.goto : "/"))}}>Cancel</StyledButton>
            </StyledInnerContainer>
        </StyledPageHeader>
    );
};


export default PageHeader;


const StyledPageHeader = styled.div`
    background: rgb(0, 106, 77);
    overflow:auto;
    padding:12px 0 7px;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    position:sticky;
    top:0;
    z-index:100;
    box-shadow: 0 4px 6px rgba(204,204,204,0.12), 0 2px 4px rgba(204,204,204,0.24);
`;

const StyledInnerContainer = styled.div`
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
`;

const StyledTitle = styled.h1`
    display: inline-block;
    color:white;
    float:left;
    text-transform:uppercase;
    font-size: 1.3em;
    font-family: ${props => (props.theme.font ? props.theme.font.bold : 'sans-serif')};
    padding:0px;
`;
