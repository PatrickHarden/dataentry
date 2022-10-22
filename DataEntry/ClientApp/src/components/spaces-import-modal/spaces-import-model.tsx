import React, { FC, useState } from 'react';
import styled from 'styled-components';
import StyledButton from '../styled-button/styled-button';
import PropertyResultView from '../../views/miq/record-display/property-result-view';
import RecordsDisplayView from '../../views/miq/record-display/records-display-view';
import { generateKey } from '../../utils/keys';

export interface Props {
    spaces?: any,
    show?: boolean,
    listing?: any,
    showSpacesModal: any,
    addSpaces: (data: any) => void
}



const SpacesImportModal: FC<Props> = (props) => {

    const { spaces, listing, addSpaces, showSpacesModal } = props;

    const [tempSpaces, setTempSpaces] = useState(listing.spaces)

    const temp = [{ ...listing }];
    temp[0].spaces = [].concat(spaces);

    const importSpaces = () => {
        addSpaces(tempSpaces);
        showSpacesModal();
    }

    const updateTempSpaces = (space: any, selected: boolean) => {
        console.log(space, selected);
        if (selected){
            const result: any = tempSpaces;
            result.push(space);
            setTempSpaces(result);
        }
    }

    return (
        <ModalOverlay>
            <DialogContainer>
                <h1>Import from Market IQ </h1>
                <StyledModalDismiss style={{ marginRight: "30px" }} onClick={() => showSpacesModal()}>X</StyledModalDismiss>
                <MessageBlockContainer>
                    <RecordsDisplayView
                        key={generateKey()}
                        records={temp}
                        allowAssign={false}
                        assignMode={false}
                        updateSpaces={updateTempSpaces}
                        actionHandler={(e) => {console.log(e)}}
                        allowUnpublish={false} spacesImport={true} />
                    <StyledButton style={{ marginRight: "10px", float: "right" }} onClick={() => importSpaces()} >Import</StyledButton>
                    <StyledButton style={{ marginRight: "20px", backgroundColor: "#333", float: "right" }} onClick={() => showSpacesModal()} >Cancel</StyledButton>
                </MessageBlockContainer>
            </DialogContainer>
        </ModalOverlay>
    )
}

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
    height: 640px;
    text-align: center;
    background: #fff;
    border: #444444 solid 1px;
    z-index: 5000;
    color: #b1b1b1;

    > h1 {
            background: #006A4D;
            color: #fff;
            margin-top: 0;
            padding: 5px 0;
            margin-bottom: -25px;
            text-align: left;
            padding-left: 30px;
            text-transform: uppercase;
            font-family: 'Calibre-R';
            font-style: normal;
            font-weight: 700;
            font-size: 24px;
    }

    .gl-table-styled-grid {
        .gl-table-row:nth-of-type(1), .gl-table-row-header{
            display:none;
        }
    }
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

const StyledModalDismiss = styled.div`
    width: 20px;
    height: 20px;
    float: right;
    color: #FFFFFF;
    cursor: pointer;
    font-weight: bolder;
`;

export default SpacesImportModal