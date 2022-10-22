import React, { FC, useState, useContext } from 'react';
import styled from 'styled-components';
import { DataSourcePopupParams } from '../../types/state';
import { useDispatch } from 'react-redux';
import { Col, Row } from 'react-styled-flexboxgrid';
import GLField from '../form/gl-field';
import GLForm from '../form/gl-form';
import { useSelector } from 'react-redux';
import { ConfigDetails } from '../../types/config/config';
import { configDetailsSelector } from '../../redux/selectors/system/config-details-selector';
import FormRadioButton from '../form-radiobutton/form-radiobutton';
import FormInput, { FormInputProps } from '../form-input/form-input';
import FormFieldLabel from '../form-field-label/form-field-label';
import Iconclose from '../../assets/images/png/icon-close.png'
import { convertValidationJSON } from '../../utils/forms/validation-adapter';
import { clearDatasourcePopup } from '../../redux/actions/system/set-datasource-popup';
import { GLAnalyticsContext } from '../../components/analytics/gl-analytics-context';


interface Props {
    datasourcePopupDialogParams: DataSourcePopupParams
}


interface PopupData {
    dataSources: string[],
    other: string
}

const DataSourcePopupDialog: FC<Props> = (props) => {

    const { confirmFunc, action, datasource } = props.datasourcePopupDialogParams;
    const configDetails: ConfigDetails = useSelector(configDetailsSelector);
    const dataSourceOptions = (configDetails.config.dataSource && configDetails.config.dataSource.options) ? configDetails.config.dataSource.options : [];

    const other = (datasource !== undefined && datasource.other) ? datasource.other : "";
    const analytics = useContext(GLAnalyticsContext);
    let dataSources = (datasource !== undefined && datasource.datasources) ? datasource.datasources : [];

    // This code removes any values set on a listing that are no longer part of options available in case 
    // the options change in the configuration
    const optionValues = dataSourceOptions.map((x: any) => {
        return x.value;
    })
    dataSources = dataSources.filter(e => optionValues.indexOf(e) !== -1);


    // setup state
    const [error, setError] = useState<boolean>(false);
    const [errorMsg, setErrorMsg] = useState<string>("");
    const [popupData, setPopupData] = useState<PopupData>({ dataSources, other });
    const dispatch = useDispatch();


    const runCancel = () => {
        clearDatasourcePopup(dispatch);
    }


    const runConfirm = () => {
        setError(false);
        if (!validate()) {
            setError(true);
        } else if (confirmFunc !== undefined) {
            const data = {
                dataSources: popupData.dataSources,
                other: popupData.other
            }
            analytics.fireEvent('data-source', 'click', popupData.dataSources + ' ' + popupData.other)
            confirmFunc(data);
        }
    };


    const validate = () => {
        if (popupData.other.length > 0 && popupData.other.length < 5) {
            setErrorMsg("Please enter 5 or more letters in the 'Other' Field");
        }
        if ((popupData.dataSources.length === 0) && popupData.other.length === 0) {
            setErrorMsg("Please choose an option for your data source");
        }

        return (((popupData.dataSources.length > 0) && ((popupData.other.length === 0) || (popupData.other.length >= 5)) || popupData.other.length >= 5));
    };


    const formChangeHandler = (values: any) => {
        Object.keys(values).forEach(key => {
            popupData[key] = values[key];
        });
    };


    const dataSourceChangeHandler = (option: any) => {
        // option.selected = !option.selected;
        const idx = popupData.dataSources.findIndex((u: string) => u === option.value);
        if (idx > -1) {
            popupData.dataSources.splice(idx, 1);
        }
        else {
            popupData.dataSources.push(option.value);
        }
        setError(false);
        setPopupData({ ...popupData });
    };


    return (
        <ModalOverlay>
            <DialogContainer>
                <GLForm initVals={popupData}
                    validationAdapter={convertValidationJSON}
                    validationJSON={{}}
                    changeHandler={formChangeHandler}
                    forceValidate={true}
                    showErrors={error}>
                    {error &&
                        <Row style={{ justifyContent: 'center' }}>
                            <Col style={{ color: 'darkred' }}>
                                {errorMsg}
                            </Col>
                        </Row>
                    }
                    <Row>
                        <Col xs={11} style={{ marginTop: '8px' }}>
                            <PopupHeading>Data Source</PopupHeading>
                            {action === 'create' && <Disclaimer>Before we publish your availability and list it online, please let us know where you sourced your data. </Disclaimer>}
                            {action === 'update' && <Disclaimer>Before we update your listing online, please let us know where you sourced your data. </Disclaimer>}
                        </Col>
                        <Col xs={1} style={{ marginTop: '8px' }}>
                            <CloseIcon onClick={() => runCancel()}><img src={Iconclose} /></CloseIcon>
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={12}>
                            <CustomFormFieldLabel title={configDetails.config.dataSource.label} error={(configDetails.config.dataSource.required && popupData.other.length === 0) && error} />
                            <DataSourceTags>
                                {dataSourceOptions.map((option: any) => {
                                    return <FormRadioButton key={"rbkey" + option.value} name={option.label}
                                        label={option.label} optionVal={option.value} defaultValue={(popupData.dataSources.indexOf(option.value) > -1) ? option.value : ''}
                                        forceFocus={false} changeHandler={() => { dataSourceChangeHandler(option) }} selectedColor='#006A4D' error={(configDetails.config.dataSource.required && popupData.other.length === 0) && error} />

                                })}
                            </DataSourceTags>
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={12}><GLField<FormInputProps> name="other" title="Other" placeholder="Enter the name of source" use={FormInput} />
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={12} style={{ padding: '.5rem' }}>
                            <StyledButton onClick={() => { runConfirm() }} >Submit</StyledButton>
                        </Col>
                    </Row>
                </GLForm>
            </DialogContainer>
        </ModalOverlay>
    );
}

export default DataSourcePopupDialog;

const DataSourceTags = styled.div`
   > div {
        margin-bottom: 1.0em;
        margin-right: 5px;
   }
`;

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
    padding: 30px;
    width: 50%;
    max-width: 600px;
    text-align: center;
    background: #fff;
    border: #444444 solid 1px;
    z-index: 5000;
    color: #b1b1b1;
    border-spacing: 5px;
`;

const CustomFormFieldLabel = styled(FormFieldLabel)`
   && { 
       span {
         color: inherit;
       }
      >h5 
       height: 15px;
        width: 601px;	
        color: 'red';
        font-family: "futura PT Book italic";
        font-size: 16px;
        font-weight:300;
        font-family: ${props => props.theme.font ? props.theme.font : 'italic'};	
        line-height: 15px;
        margin-bottom:15px;
    }
`;

const Disclaimer = styled.h5`
    font-size: 14px;
    font-weight: 300;
`;

const CloseIcon = styled.div`
    margin: auto;
    color: #b1b1b1;
    color: #006A4D;
    font-size: 1.5em;
    cursor: pointer;
`;

const PopupHeading = styled.div`
  color: #006A4D;
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')};
  font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
  letter-spacing: .01rem;
  text-transform: uppercase;
  font-size: 1.5em;
`;

const StyledButton = styled.button`
    background-color: #006A4D
    color: #ffffff; 
    height: 50px;
    letter-spacing:4px;
    border:0px;
    width: 100%;
    padding: 0.9em 1.7em;
    cursor: pointer;
    margin-top: 2em;
    box-sizing: border-box;
    -webkit-box-sizing: border-box; 
    -moz-box-sizing: border-box; 
    display: inline-block;
    flex-grow: 1 
    position: relative;
    text-align: middle;
    text-transform: capitalize;
    min-width:8em;
    font-family: ${props => (props.theme.font.primary ? props.theme.font.bold : 'inherit')}; 
    font-size: 14px;
    text-transform:uppercase;
    &:hover {
    top:1px;
    }
    :focus {
    outline: none;
    border-color: ${props => props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C'};
    }
`;



