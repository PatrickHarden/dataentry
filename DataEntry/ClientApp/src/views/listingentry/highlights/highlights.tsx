
import React, { FC, useContext } from 'react'
import GLForm from '../../../components/form/gl-form';
import { convertValidationJSON } from '../../../utils/forms/validation-adapter';
import GLField from '../../../components/form/gl-field';
import { Col, Row } from 'react-styled-flexboxgrid'
import DraggableMultiLangList from '../../../components/draggable-list/draggable-multilang-list'
import SectionHeading from "../../../components/section-heading/section-heading";
import { FormContext } from '../../../components/form/gl-form-context';
import { Listing } from '../../../types/listing/listing';

interface Props {
    listing: Listing, 
    label: string
}

const Highlights: FC<Props> = (props) => {

    const { listing, label } = props;
    
    const init:object = {};
    const validations = {};
    
    // value change interceptor
    const formControllerContext = useContext(FormContext);

    const changeHandler = (values:any) => {
        formControllerContext.onFormChange(values);
    }

    return (
        <GLForm initVals={init}
            validationAdapter={convertValidationJSON}
            validationJSON={validations}
            changeHandler={changeHandler}>
            <Row>
                <Col xs={12} id="highlights" style={{ marginTop: '25px' }} ><SectionHeading>{label}</SectionHeading></Col>
                <Col xs={12}>
                    <GLField name="highlights" items={listing.highlights} use={DraggableMultiLangList}/>
                </Col>
            </Row>
        </GLForm>
    )
}

export default Highlights