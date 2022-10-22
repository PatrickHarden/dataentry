import React, { FC, useEffect, useRef, useState } from 'react';
import StyledButton from '../styled-button/styled-button';
import styled, { css } from 'styled-components';
import { Col, Row, Grid } from 'react-styled-flexboxgrid';
import { ConfirmDialogParams, MIQ } from '../../types/state';
import { Listing, AlternatePostalAddresses } from '../../types/listing/listing';
import { useDispatch, useSelector } from 'react-redux';
import { miqCurrentRecordsSelector } from '../../redux/selectors/miq/miq-current-record-selector';
import Iconclose from '../../assets/images/png/icon-close-dialog-x.png'
import { clearMiqAddressPopup } from '../../redux/actions/system/set-miq-address';
import { prepareRecordForAddEdit } from '../../redux/actions/miq/prepare-record-for-add-edit';
import { configSiteIdSelector } from '../../redux/selectors/system/siteid-selector';
import { setCancelAction } from '../../redux/actions/system/set-cancel-action';
import { any } from 'ramda';
import IconInfo from '../../assets/images/png/icon-info.png';
import { Config } from '../../types/config/config';
import { configSelector } from '../../redux/selectors/system/config-selector';


interface Props {
    miqPopupAddressParams: MIQ
}

const MiqPopupAddress: FC<Props> = (props) => {

    const currentRecords = useSelector(miqCurrentRecordsSelector);

    const dispatch = useDispatch();

    const currentRecordsData = currentRecords;
    const alternateAddressData = currentRecordsData && currentRecordsData[0].alternatePostalAddresses ? currentRecordsData[0].alternatePostalAddresses : [];
    

    const siteId = useSelector(configSiteIdSelector);

    const config: Config = useSelector(configSelector);


    const [selectedAddress, setSelectedAddress] = useState<AlternatePostalAddresses>({})

    const runCancel = () => {
        clearMiqAddressPopup(dispatch);
    }

    const runConfirm = () => {
        dispatch(setCancelAction({ goto: "/miq/import" })); // ensure any cancel action in a subsequent page returns to this page)

        if (currentRecords && selectedAddress && Object.keys(selectedAddress).length > 0) {
            const updatedCurrentRecordsData: Listing = currentRecords[0];
            updatedCurrentRecordsData.street = selectedAddress.street;
            updatedCurrentRecordsData.street2 = selectedAddress.street2;
            updatedCurrentRecordsData.city = selectedAddress.city;
            updatedCurrentRecordsData.country = selectedAddress.country;
            updatedCurrentRecordsData.lat = selectedAddress.lat;
            updatedCurrentRecordsData.lng = selectedAddress.lng;
            updatedCurrentRecordsData.postalCode = selectedAddress.postalCode;
            updatedCurrentRecordsData.stateOrProvince = selectedAddress.stateOrProvince;

            dispatch(prepareRecordForAddEdit(updatedCurrentRecordsData, siteId));
            runCancel();
        } else {
            runCancel();
        }
    }

    const changeInput = (value: any) => {
        setSelectedAddress(value);
    }

    const msgIcon: any = IconInfo;

    return (
        <ModalOverlay>
            <DialogContainer>
                <StyledHeader>
                    <Row style={{ margin: '8px' }}>
                        <Col xs={11} style={{ marginTop: '8px' }}>
                            <PopupHeading>CREATE LISTING</PopupHeading>
                        </Col>
                        <Col xs={1} style={{ marginTop: '8px', paddingLeft: '40px' }}>
                            <CloseIcon onClick={() => runCancel()}><img src={Iconclose} /></CloseIcon>
                        </Col>
                    </Row>

                </StyledHeader>
                <Row>
                    <Col xs={12} style={{ padding: '30px 30px 0px 30px' }}>
                        <StyledMessageContainer style={{ "color": '#555555', 'padding': '20px 30px 40px' }}>
                            <StyledMessageContent style={{ float: 'left' }}>
                                <StyledMessageIcon src={msgIcon} />
                                Please select the correct location to import into your listing.
                            </StyledMessageContent>
                        </StyledMessageContainer>
                    </Col>
                </Row>
                <AlertMessage>
                    <RowHeading>
                        <Row style={{ justifyContent: 'space-around' }}>
                            <Col xs={1}>
                                <h4>Select</h4>
                            </Col>
                            <Col xs={3}>
                                <h4>Address</h4>
                            </Col>
                            <Col xs={2}>
                                <h4>City</h4>
                            </Col>
                            {config && !config.featureFlags.hideStateOrProvince && <Col xs={2}>
                                <h4>State</h4>
                            </Col>}
                            <Col xs={2}>
                                <h4>Postal Code</h4>
                            </Col>
                        </Row>
                    </RowHeading>

                    {alternateAddressData.map((option: any, key: number) => {
                        return <RowContent key={key}>
                            <Row style={{ justifyContent: 'space-around' }}>
                                <Col xs={1} style={{ paddingTop: '8px' }}>
                                    <Root>
                                        <Input
                                            type="radio" onClick={(e) => { changeInput(option) }} name={option.street} value={option.postalCode} defaultChecked={false} aria-checked={true} />
                                        <Fill />
                                    </Root>
                                </Col>
                                <Col xs={3}>
                                    <p>{option.street} {option.street2 ? ',' + option.street2 : ''} </p>
                                </Col>
                                <Col xs={2}>
                                    <p>{option.city}</p>
                                </Col>
                                {config && !config.featureFlags.hideStateOrProvince && <Col xs={2}>
                                    <p>{option.stateOrProvince}</p>
                                </Col>}
                                <Col xs={2}>
                                    <p>{option.postalCode}</p>
                                </Col>
                            </Row>
                        </RowContent>

                    })}
                    <Row>
                        <Col xs={11} style={{ paddingTop: '10px' }}>
                            <ActionButton disabled={Object.keys(selectedAddress).length === 0} onClick={() => { runConfirm() }} >Continue</ActionButton>
                        </Col>
                        <Col xs={1} style={{ paddingTop: '10px' }}>
                            <CancelButton onClick={() => { runCancel() }} >Cancel</CancelButton>
                        </Col>
                    </Row>
                </AlertMessage>

            </DialogContainer>
        </ModalOverlay>

    );
}

export default MiqPopupAddress;


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
    width: 100%;
    max-width: 800px;
    background: #fff;
    z-index: 5000;
    color: #1A1A1A
    border-spacing: 5px;
`;

const CloseIcon = styled.div`
    margin: auto;
    color: #FFFFFF;
    font-size: 1.5em;
    cursor: pointer;
`;

const PopupHeading = styled.div`
    background: rgb(0,106,77);
  float: left;
  font-family: Futura Md BT Bold;	
  letter-spacing: .01rem;
  text-transform: uppercase;
  font-size: 1.5em;
  padding-left: 20px;
`;

const ActionButton = styled(StyledButton)`
${(props) => props.disabled ? `
    background-color: gray;
`: 'background-color: #006A4D;'}

    float: left;
    border-radius: 4px;
    padding-left: 8px;
    padding-right: 8px;
`;

const CancelButton = styled(StyledButton)`
    background-color: #ffffff;
    color: #006A4D;
    float: right;
    border-radius: 4px;
    padding-left: 8px;
    padding-right: 8px;
`;

const Fill = styled.div`
    background: #00B2DD;
    width: 0;
    height: 0;
    border-radius: 100%;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    pointer-events: none;
    z-index: 1;

    &::before {
        content: "";
        opacity: 0;
        width: calc(20px - 4px);
        position: absolute;
        height: calc(20px - 4px);
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        border: 2px solid #00B2DD;
        border-radius: 100%;
    }
`;

const Input = styled.input`
    opacity: 0;
    z-index: 2;
    top: 0;
    width: 100%;
    height: 100%;
    margin: 0;
    cursor: pointer;

    &:focus {
        outline: none;
    }

    &:checked {
        & ~ ${Fill} {
        width: calc(100% - 8px);
        height: calc(100% - 8px);

        &::before {
            opacity: 1;
        }
        }
    }
`;

const Root = styled.div`
  margin: 5px;
  cursor: pointer;
  width: 18px;
  height: 18px;
  position: relative;
  label {
    margin-left: 25px;
  }
  &::before {
    content: "";
    border-radius: 100%;
    border: 1px solid;
    background:  "#FAFAFA";
    width: 100%;
    height: 100%;
    position: absolute;
    top: 0;
    box-sizing: border-box;
    pointer-events: none;
    z-index: 0;
  }
`;

const AlertMessage = styled.div`
padding: 20px 23px 12px 23px;
overflow-y: auto; 
max-height: 300px;

`;


const StyledHeader = styled.div`
padding: 2px 16px;
background: #006A4D;
color: white;
`

const StyledMessageContent = styled.div`
float: left;
`
const StyledMessageContainer = styled.div`
    text-align: center;
    padding: 24px;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    border-bottom: ${props => (props.theme.border ? props.theme.border : '#cccccc')};
    background-color: #f5f3ed;
`;

const StyledMessageIcon = styled.img`
    margin-right: 10px;
    margin-bottom: -5px;
`;

const RowHeading = styled.div`
    background: rgba(26, 26, 26, 0.04); 
    border-width: 1px 0px;
    border-style: solid;
    border-color: rgba(0, 63, 45, 0.15);
    `;

const RowContent = styled.div`
    border-width: 1px 0px;
    border-style: solid;
    border-color: rgba(0, 63, 45, 0.15);
    display: grid;
    `;
