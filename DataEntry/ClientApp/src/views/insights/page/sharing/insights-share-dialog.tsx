import React, { FC, useState } from 'react';
import styled from 'styled-components';
import shareIcon from '../../../../assets/images/png/icon-share.png';
import iconCloseX from '../../../../assets/images/png/icon-close-dialog-x.png';
import FormInput from '../../../../components/form-input/form-input';

interface Props {
    shareLink: string,
    closeHandler: () => void
}

const InsightsShareDialog: FC<Props> = (props) => {

    const { shareLink, closeHandler } = props;

    const [message, setMessage] = useState<string>("");

    const onClose = () => {
        if(closeHandler){
            closeHandler()
        }
    }

    const handleCopyLink = () => {
        const ta = document.createElement('textarea');
        ta.value = shareLink;
        document.body.appendChild(ta);
        ta.select();
        document.execCommand('copy');
        document.body.removeChild(ta);
        setMessage("The link has been copied to your clipboard!");
    }

    return (
        <ModalOverlay>
            <DialogContainer>
                <DialogHeader>
                    <Left>
                        <Icon src={shareIcon}/>
                        <Title>SHARE REPORT</Title>
                    </Left>
                    <Right> 
                        <CloseButton onClick={() => onClose()}><img src={iconCloseX}/></CloseButton>
                    </Right>
                </DialogHeader>
                <DialogBody>
                    <InputCol1>
                        <FormInput name="shareLinkTextBox" label="Report Link" disabled={true} defaultValue={shareLink} labelColor="#666666" />
                    </InputCol1>
                    <InputCol2>
                        <StyledCopyButton onClick={handleCopyLink}>COPY LINK</StyledCopyButton>
                    </InputCol2>   
                </DialogBody>
                { message && message.length > 0 && <Message>{message}</Message> }
            </DialogContainer>
        </ModalOverlay>
    );
}

const ModalOverlay = styled.div`
    position:fixed;
    top:0;
    left: 0;
    width: 100%
    height: 100%
    background: rgba(0, 0, 0, 0.5);
    right:0;
    bottom:0;
    z-index: 1000000000;
`;

const DialogContainer = styled.div`
    margin: auto;
    margin-top: 100px;
    width: 50%;
    max-width: 700px;
    text-align: center;
    background: #fff;
    z-index: 5000;
    color: #b1b1b1;
    border-spacing: 5px;
`;

const DialogHeader = styled.div`
    width: 100%;
    background: #006A4D;
    height: 60px;
    display: flex;
    color: #FFF;
    align-items: center;
`;

const Left = styled.div`
    display: inline-flex;
    margin-left: 25px;
`;

const Right = styled.div`
    display: inline-flex;
    margin-left: auto;
    margin-right: 25px;
`;

const Icon = styled.img`
    margin-right: 10px;
    display: inline-flex;   
`;

const Title = styled.div`
    font-size: 18px;
    font-weight: 400;
    text-transform: uppercase;
    display: inline-flex;
`;

const CloseButton = styled.a`
    :hover, :active, :visited {
        color: #FFF;
        text-decoration: none;
    }
    font-size: 18px;
    font-weight: 400;
    cursor: pointer;
`;

const DialogBody = styled.div`
    padding: 0 25px 25px 25px;
    display: flex;
`;

const InputCol1 = styled.div`
    display: inline-flex;
    width: 500px;
    height: 110px;
`;

const InputCol2 = styled.div`
    display: flex;
    align-items: flex-end;
    margin: 0 0 10px 15px;
`;

const StyledCopyButton = styled.a`
    background: #006A4D;
    width: 120px;
    height: 45px;
    line-height: 45px;
    color: #fff;
    text-transform: uppercase;
    border-radius: 4px;
    border: 1px solid #006A4D;
    cursor: pointer;
`;

const Message = styled.div`
    width: 675px;
    padding: 5px;
    color: #006A4D;
    font-size: 14px;
    font-weight: 400;
    display: block;
    background: #fff;
`;

export default React.memo(InsightsShareDialog);