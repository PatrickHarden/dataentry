import React, { FC, useState } from 'react';
import { useSelector } from 'react-redux';
import { Col, Row, Grid } from 'react-styled-flexboxgrid';
import styled from 'styled-components';
import FormSelect from '../../../../components/form-select/form-select';
import GLField from '../../../../components/form/gl-field';
import GlForm from '../../../../components/form/gl-form';
import StyledButton from '../../../../components/styled-button/styled-button';
import { configDetailsSelector } from '../../../../redux/selectors/system/config-details-selector';
import { configSiteIdSelector } from '../../../../redux/selectors/system/siteid-selector';
import { Option } from '../../../../types/common/option';
import { ConfigDetails, Region } from '../../../../types/config/config';

export interface Props {
    foo: string    
};

const TestingDashboard: FC<Props> = (props) => {

    const { foo } = props;

    const initialValues = {
        environment: 'uat',
        testType: 'functional',
        region: 'none'
    }

    const [ formValues, setFormValues ] = useState(initialValues);


    const siteId:string | undefined = useSelector(configSiteIdSelector);
    const configDetails:ConfigDetails = useSelector(configDetailsSelector);

    const currentRegion:Region | undefined = configDetails.regions.find((region: Region) => {
        if(region.homeSiteID === siteId){
            return region;
        }
    })

    const changeHandler = (values:any) => {
        setFormValues(values);
    }

    const validationAdapter = () => {
        // nothin to do
    }

    const environmentOptions:Option[] = [
        { label: "UAT", value: "uat", order: 1 },
        { label: "Production", value: "prod", order: 2 } 
    ];

    const testTypes:Option[] = [
        { label: "Functional", value: "functional", order: 1 },
        { label: "Data Entry", value: "de", order: 2 },
        { label: "Smoke Tests", value: "smoke", order: 3 },
        { label: "Regression", value: "regression", order: 4 },
        { label: "Performance (load)", value: "performance", order: 5 },
    ];

    const regions:Option[] = [
        { label: "None", value: "none", order: 1},
        { label: "EMEA", value: "emea", order: 2},
        { label: "APC", value: "apc", order: 3},
        { label: "Americas", value: "americas", order: 4}
    ];

    

    return (
        <>
            <DisplayContainer>
                <BoundingBox>
                    <DisplayHeader>Automation Test Request For</DisplayHeader>
                    <GlForm 
                        initVals={formValues}
                        changeHandler={changeHandler}
                        validationAdapter={validationAdapter}
                        validationJSON={{}}
                        >
                        <Row>
                            <Col><ReadOnly><Label>Country: </Label><Value>{currentRegion && currentRegion.name || siteId}</Value></ReadOnly></Col>
                        </Row>
                        <Row>
                            <Col sm={12}><GLField name="region" label="Region" use={FormSelect} options={regions}/></Col>
                        </Row>
                        <Row>
                            <Col sm={12}><GLField name="environment" label="Environment" use={FormSelect} options={environmentOptions}/></Col>
                        </Row>
                        <Row>
                            <Col sm={12}><GLField name="testType" label="Tests Type" use={FormSelect} options={testTypes}/></Col>
                        </Row>
                        <Row>
                            <ButtonContainer><StyledButton>Run Tests</StyledButton></ButtonContainer>
                        </Row>
                    </GlForm>
                </BoundingBox>
            </DisplayContainer>
        </>
    );
}

const DisplayContainer = styled.div`
    font-family: 'Futura Md BT', helvetica, arial, sans-serif;
    padding: 25px 120px 0 120px;
`;

const BoundingBox = styled.div`
    border: 1px solid #cccccc;
    padding: 10px;
`;

const DisplayHeader = styled.div`
    font-size: 22px;
    font-weight: bold;
    color: #00a657;
    margin-bottom: 15px;
    width: 100%;
`;

const ReadOnly = styled.div`
    display: flex;
    margin: 10px 0 10px 0;
`;

const Label = styled.div`
    margin-right: 5px;
    font-size: 16px;
    color: #9EA8AB;
`;

const Value = styled.div`
    font-size: 16px;
    color: #000;
`;

const ButtonContainer = styled.div`
    margin-top: 20px;
    width: 100%;
    text-align: center;
`;

export default TestingDashboard;

