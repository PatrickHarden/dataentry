import React, { FC, useRef } from 'react';
import StyledButton from '../styled-button/styled-button';
import styled, { css }   from 'styled-components';
import { ConfirmDialogParams } from '../../types/state';
import { useDispatch } from 'react-redux';
import { setConfirmDialog, clearConfirmDialog } from '../../redux/actions/system/set-confirm-dialog';


interface Props {
    confirmationDialogParams: ConfirmDialogParams
}

const ConfirmationDialog: FC<Props> = (props) => {

    const { title, message, cancelTxt, confirmTxt, copyTxt, cancelFunc, confirmFunc, showConfirmButton, showCopyButton, scrollable } = props.confirmationDialogParams;
    const messageBlockRef = useRef(null);

    const dispatch = useDispatch();

    const runCancel = () => {
        if (cancelFunc !== undefined) {
            cancelFunc();
        
            dispatch(setConfirmDialog({show: false}));
          }
          else {
            dispatch(setConfirmDialog({show: false}));
          }      
    }

    const runConfirm = () => {
        if (confirmFunc !== undefined) {
            confirmFunc();
            dispatch(setConfirmDialog({show: false}));
          }
          else {
            dispatch(setConfirmDialog({show: false}));
          }      
    }

    const runCopy = () => {
        if (messageBlockRef && messageBlockRef.current) {
            (messageBlockRef as any).current.select();
            document.execCommand('copy');
        }
    }

  //  dispatch(clearConfirmDialog(dispatch));
   return (
       <ModalOverlay>
           <DialogContainer>
               <div style={{ margin: "auto" }}>
                   <div style={{ width: "100%", height: "50px", "backgroundColor": "#ccc" }}>
                       {
                           title ? <h1 style={{ color: "#444", margin: "0px", lineHeight: "50px" }}>{title} </h1> : <h1 style={{ color: "#444", margin: "0px", lineHeight: "50px" }}> Are You Sure? </h1>
                       }
                   </div>
                   <MessageBlockContainer>
                       {(showCopyButton || scrollable) &&
                           <MessageBlock
                               as={"textarea"}
                               ref={messageBlockRef}
                               style={{ height: "300px" }}
                               defaultValue={message ? message : "Do you want to proceed?"} />
                           ||
                           <MessageBlock>
                               {message ? message : "Do you want to proceed?"}
                           </MessageBlock>
                       }
                   </MessageBlockContainer>
               </div>
               <StyledButton style={{ marginRight: "10px", backgroundColor: "#333" }} onClick={() => runCancel()} >{cancelTxt ? cancelTxt : "Cancel"}</StyledButton>
               {(showConfirmButton === undefined || showConfirmButton) &&
                   <StyledButton onClick={() => runConfirm()}>{confirmTxt ? confirmTxt : "Confirm"}</StyledButton>
               }
               {showCopyButton &&
                   <StyledButton onClick={() => runCopy()}>{copyTxt ? copyTxt : "Copy To Clipboard"}</StyledButton>
               }
           </DialogContainer>
       </ModalOverlay>
   );
}
   
export default ConfirmationDialog;


const ModalOverlay = styled.div`
    position:fixed;
    top:0;
    left: 0;
    border: purple 3px solid;
    width: 100%
    height: 100%
    z-index: 5000;
    background: rgba(0, 0, 0, 0.5);
    right:0;
    bottom:0;
    z-index: 1000000000;
`;

const DialogContainer = styled.div`
    margin: auto;
    margin-top: 30px;
    padding-bottom: 30px;
    width: 80%;
    text-align: center;
    background: #fff;
    border: #444444 solid 1px;
    z-index: 5000;
    color: #b1b1b1;
`;

const MessageBlockContainer = styled.div`
    margin: 50px 30px;
`

const MessageBlock = styled.h2`
    width: 100%;
    resize: vertical;
    box-sizing: border-box;
    font-family: "Futura Md BT";
`;