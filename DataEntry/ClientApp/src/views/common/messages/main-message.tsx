import React, { FC } from 'react'
import styled from 'styled-components';
import { MainMessageType, MainMessaging } from '../../../types/state';
import Loader from '../../../components/loader/loader';
import { Link } from 'react-router-dom';

interface Props {
    mainMessage: MainMessaging
}

const MainMessage: FC<Props> = (props) => {

    const { mainMessage } = props;

    const Loading = () => {
        return (
            <><Loader/><ProcessingText>{mainMessage.message}</ProcessingText></>
        );
    }

    const Saving = () => {
        return (
            <><Loader/><ProcessingText>{mainMessage.message}</ProcessingText></>
        );
    }

    const Error = () => {
        return (
            <>
                <ErrorText>{mainMessage.message}</ErrorText>
                {mainMessage.home && <Link style={{color: '#00B2DD', fontSize: '16px', marginTop: '-15px'}} to="/">Go back</Link>}
            </>
        );
    }

    const Notice = () => {
        return (
            <>
                <NoticeText>
                    <MessageLine>{mainMessage.message}</MessageLine>
                    <MessageLine>{mainMessage.messageLine2}</MessageLine>
                </NoticeText>
            </>
        );
    }

    return (
        <MessageContainer>
            { mainMessage.type === MainMessageType.LOADING && <Loading/>}
            { mainMessage.type === MainMessageType.SAVING && <Saving/>}
            { mainMessage.type === MainMessageType.ERROR && <Error/>}
            { mainMessage.type === MainMessageType.NOTICE && <Notice/>}
        </MessageContainer>
    );
};

const ErrorText = styled.p`
    text-align: center;
    padding: 10px;
    color: ${props => (props.theme.colors ? props.theme.colors.error : 'red')};
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    border-bottom: ${props => (props.theme.border ? props.theme.border : '#cccccc')};
    background-color: none;
`;

const ProcessingText = styled.p`
    text-align: center;
    padding: 10px;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    border-bottom: ${props => (props.theme.border ? props.theme.border : '#cccccc')};
    background-color: none;
`;

const NoticeText = styled.p`
    text-align: center;
    padding: 10px;
    color: ${props => (props.theme.colors ? props.theme.colors.notice : '#b0b0b0')};
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    border-bottom: ${props => (props.theme.border ? props.theme.border : '#cccccc')};
    background-color: none;
`;

const MessageLine = styled.p`
    margin: 0;
`;

const MessageContainer = styled.div`
    margin:150px auto 0 auto
    align-items: center;
    display: flex;
    justify-content: center;
    flex-direction: column;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
    font-size: 24px;
`;

export default MainMessage;