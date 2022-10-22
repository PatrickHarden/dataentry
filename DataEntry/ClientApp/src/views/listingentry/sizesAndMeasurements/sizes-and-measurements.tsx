
import React, { FC, useContext, useState } from 'react'
import { Col, Row } from 'react-styled-flexboxgrid'
import SectionHeading from "../../../components/section-heading/section-heading";
import styled from 'styled-components';
import MultiDropdown from './multi-dropdown/multi-dropdown';
import { Config, SizeTypeConfig } from '../../../types/config/config';
import StyledButton from '../../../components/styled-button/styled-button';
import { generateKey } from '../../../utils/keys';

export interface SizeTypeProps {
    name: string,
    setup: SizeTypeConfig
}

interface Props {
    config: Config,
    sizeTypeSetup: SizeTypeConfig,
    sizeTypes?: SizeType[],
    changeHandler: (sizeTypes:SizeType[]) => void
}

export interface SizeType {
    sizeKind: string,
    amount: any,
    measureUnit: string
}

const SizesAndMeasurements: FC<Props> = (props) => {

    const { config, sizeTypeSetup, sizeTypes, changeHandler } = props;

    const [data, setData] = useState<SizeType[]>(sizeTypes ? sizeTypes : []);

    const sizeTypeConfig:SizeTypeConfig = sizeTypeSetup.use && sizeTypeSetup.use.length > 0 && config[sizeTypeSetup.use] ? config[sizeTypeSetup.use] : sizeTypeSetup;

    // get the default unit of measurement from the config
    let defaultUnitOfMeasurement: string = "sqm";
    if (config && config.defaultMeasurement && config.defaultMeasurement !== '') {
        defaultUnitOfMeasurement = config.defaultMeasurement
    }

    const clearFocus = () => {
        const el:any = document.querySelector(':focus');
        if(el){
            el.blur();
        }
    }

    const addRow = () => {
        const temp:SizeType[] = data;
        temp.push({
            sizeKind: '',
            amount: 0,
            measureUnit: ''
        });
        setData([...temp]);
    }

    const deleteRow = (index: number) => {
        const temp: SizeType[] = data;
        temp.splice(index, 1);
        setData([...temp]);
    }

    const sizeChangeHandler = (values: any, index: number) => {
        const temp: SizeType[] = data;
        temp[index] = {
            sizeKind: values.sizeKind,
            measureUnit: values.measureUnit,
            amount: Number(values.amount)
        };
        const newSizeTypes:SizeType[] = [...temp];
        setData(newSizeTypes);
        changeHandler(newSizeTypes);
    }

    return (
        <SizesContainer>
            <Row>
                <Col id="Sizes" xs={12}><SectionHeading>{sizeTypeConfig.header}</SectionHeading></Col>
            </Row>
            {data.map((iteration: any, index: number) => (
                <MultiDropdown name={'sizeandm' + index} key={generateKey()}
                    index={index} deleteRow={deleteRow} changeHandler={sizeChangeHandler}
                    sizeKind={sizeTypeConfig.sizeType ? sizeTypeConfig.sizeType : []}
                    measureUnit={sizeTypeConfig.unitofmeasure ? sizeTypeConfig.unitofmeasure : []}
                    values={data[index]}
                    defaultUnitOfMeasurement={defaultUnitOfMeasurement}
                />
            ))}
            <StyledButton name={'add size'} onMouseOver={clearFocus} onClick={addRow} style={{ marginTop: '25px' }} styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp; Add Size</StyledButton>
        </SizesContainer>
    )
}

const SizesContainer = styled.div`
    #Sizes{
        h2{
            margin-top:45px;
            margin-bottom:0;
            color: #8E9A9D !important;
            font-size:18px;
        }
    }
`

export default SizesAndMeasurements;