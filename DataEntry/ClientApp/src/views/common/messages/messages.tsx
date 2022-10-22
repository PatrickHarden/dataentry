import React, { FC } from 'react';
import { useDispatch } from 'react-redux';
import styled from 'styled-components';
import { AlertMessaging, AlertMessagingType } from '../../../types/state';
import { setAlertMessage } from '../../../redux/actions/system/set-alert-message';
import IconError from '../../../assets/images/png/icon-error.png';
import IconInfo from '../../../assets/images/png/icon-info.png';
import IconSuccess from '../../../assets/images/png/icon-success.png';
import IconWarning from '../../../assets/images/png/icon-warning.png';

interface Props {
    alertMessage: AlertMessaging
}

const Messages: FC<Props> = (props) => {
    const dispatch = useDispatch();
    
    const closeMessage = () => {

        dispatch(setAlertMessage({show: false}));
    }

    const { alertMessage } = props;

    let fontColor:string = "#000000";
    let msgIcon:any =  IconInfo;

    if(alertMessage.type === AlertMessagingType.ERROR){
        fontColor = "#e86d6d";
        msgIcon = IconError;
    }else if(alertMessage.type === AlertMessagingType.WARNING){
        fontColor = "#d9a300";
        msgIcon = IconWarning;
    }else if(alertMessage.type === AlertMessagingType.SUCCESS){
        fontColor = "#00a384";
        msgIcon = IconSuccess;
    }else if(alertMessage.type === AlertMessagingType.NOTICE){
        fontColor = "#000000";
    }

    return (
        <StyledMessageContainer style={{"color": fontColor}}>
            <div>
            <StyledMessageIcon src={msgIcon} />
            { alertMessage.message }
            { alertMessage.allowClose && <StyledMessageCloseButton onClick={closeMessage}>X</StyledMessageCloseButton>}
            </div>
        </StyledMessageContainer>
    );
};

const StyledMessageIcon = styled.img`
    margin-right: 10px;
    margin-bottom: -5px;
`
const StyledMessageCloseButton = styled.div`
    text-align: center;
    cursor: pointer;
    display: inline;
    margin-left: 15px;
    font-size: 16px;
`;

const StyledMessageContainer = styled.div`
    text-align: center;
    padding: 10px;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    border-bottom: ${props => (props.theme.border ? props.theme.border : '#cccccc')};
    background-color: #f5f3ed;
`;

export default Messages;