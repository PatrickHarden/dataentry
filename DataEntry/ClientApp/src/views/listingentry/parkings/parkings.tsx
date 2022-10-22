
import React, { FC, useContext, useState, useCallback } from 'react'
import { Col, Row } from 'react-styled-flexboxgrid'
import FormSelect, { FormSelectProps } from '../../../components/form-select/form-select';
import FormInput, { FormInputProps } from '../../../components/form-input/form-input'
import SectionHeading from "../../../components/section-heading/section-heading";
import styled from 'styled-components';
import MultiDropdown from './multi-dropdown/multi-dropdown';
import { Config, ParkingsConfig } from '../../../types/config/config';
import StyledButton from '../../../components/styled-button/styled-button';
import { generateKey } from '../../../utils/keys';
import { Listing } from '../../../types/listing/listing';
import { Parkings, ParkingDetails } from '../../../types/listing/parkings';
import { Option } from '../../../types/common/option';
import GLField from '../../../components/form/gl-field';
import { ServerDataType } from '../../../types/common/max-lengths';
import GLForm from '../../../components/form/gl-form';
import { convertValidationJSON } from '../../../utils/forms/validation-adapter';
import { FormContext } from '../../../components/form/gl-form-context';


interface Props {
    config: Config,
    parkingsConfig: ParkingsConfig,
    parkings: Parkings,
}

const Parking: FC<Props> = (props) => {

    const { parkings, parkingsConfig, config } = props; 
    const [data, setData] = useState<Parkings>(parkings);

    const validations = {};
    const [, updateState] = useState(); 
    const forceUpdate = useCallback(() => updateState({}), []); // @ts-ignore
    const formControllerContext = useContext(FormContext);

    const clearFocus = () => {
        const el:any = document.querySelector(':focus');
        if(el){
            el.blur();
        }
    }

    const addRow = (index: number) => {
        const temp:Parkings = data;
        temp.parkingDetails.push({
            parkingType: '',
            parkingSpace: 0,
            amount: 0,
            interval: '',
            currencyCode: config.currencies ? config.currencies.defaultValue : ''
        });
        setData(temp);
        forceUpdate();
    }

    const deleteRow = (index: number) => {
        const temp: Parkings = data;
        temp.parkingDetails.splice(index, 1);
        setData(temp);
        forceUpdate();
    }

    const parkingChangeHandler = (values: any, index: number) => {
        const temp: Parkings = data;
        temp.parkingDetails[index] = {
                parkingType: values.parkingType,
                parkingSpace: Number(values.parkingSpace),
                amount: Number(values.amount),
                interval: values.interval,
                currencyCode: values.currencyCode ? values.currencyCode : config.currencyCode ? config.currencyCode  : ''
        };
        formControllerContext.onFormChange({ parkings: temp });
    }

    const changeHandler = (values: any) => {
        const temp: Parkings = data;
        temp.ratio = values.ratio;
        temp.ratioPer = values.ratioPer;
        temp.ratioPerUnit = values.ratioPerUnit;
        formControllerContext.onFormChange({ parkings: temp });
    }

    return (
        <ParkingsContainer>
            <GLForm initVals={parkings}
                    validationAdapter={convertValidationJSON}
                    validationJSON={validations}
                    changeHandler={changeHandler}
                    key={generateKey()}>
                <Row>
                    <Col id="Parkings" xs={12}><SectionHeading>{parkingsConfig.label}</SectionHeading></Col>
                </Row>
                {data.parkingDetails && Array.isArray(data.parkingDetails) && data.parkingDetails.map((parkingData: ParkingDetails, index: number) => (
                    <MultiDropdown name={'parking' + index} key={generateKey()}
                        index={index} deleteRow={deleteRow}
                        parkingType={parkingsConfig.parkingType ? parkingsConfig.parkingType : []}
                        interval={parkingsConfig.interval ? parkingsConfig.interval : []}
                        value={parkingData} changeHandler={parkingChangeHandler} order={index}
                        label={parkingsConfig.label ? parkingsConfig.label : ' '}
                        currencies={config.currencies ? config.currencies.options : []}
                    />
                ))}
                <StyledButton name={'add parking'} onMouseOver={clearFocus} onClick={addRow} style={{ marginTop: '25px' }} styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp; Add Parking</StyledButton>     
                <Row>
                    <Col id="ParkingRatio" xs={12}><SectionHeading>PARKING RATIO</SectionHeading></Col>
                </Row>
                <Row>
                    <Col xs={3}>
                        <GLField<FormInputProps> defaultValue={parkings.ratio} numericOnly={true} acceptDecimals={true} serverDataType={ServerDataType.DECIMAL_CURRENCY} use={FormInput} name="ratio" 
                            disabled={false} label='Ratio' />
                    </Col>
                    <Col xs={3}>
                        <GLField<FormInputProps> defaultValue={parkings.ratioPer} numericOnly={true} acceptDecimals={true} serverDataType={ServerDataType.DECIMAL_CURRENCY} use={FormInput} name="ratioPer"
                            disabled={false} label='Per' />
                    </Col>
                    <Col xs={3}>
                        <GLField<FormSelectProps> defaultValue={parkings.ratioPerUnit} use={FormSelect} options={parkingsConfig.ratioPerUnit} disabled={false} name="ratioPerUnit" label='Ratio Per Unit' />
                    </Col>
                </Row>
            </GLForm>
        </ParkingsContainer>
    )
}

const ParkingsContainer = styled.div`
    #Parkings, #ParkingRatio{
        h2{
            margin-top:45px;
            margin-bottom:0;
            color: #8E9A9D !important;
            font-size:18px;
        }
    }
`
export default Parking;