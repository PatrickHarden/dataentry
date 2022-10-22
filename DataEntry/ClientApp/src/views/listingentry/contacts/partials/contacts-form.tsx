import React, { FC } from 'react'
import GLField from '../../../../components/form/gl-field';
import FormInput, { FormInputProps } from '../../../../components/form-input/form-input';
import { Col, Row } from 'react-styled-flexboxgrid';
import { convertValidationJSON } from '../../../../utils/forms/validation-adapter';
import GLForm from '../../../../components/form/gl-form';
import styled from 'styled-components';
// images
import BlueEmailIcon from '../../../../assets/images/png/icon-email-blue.png';
import BlueLicenseIcon from '../../../../assets/images/png/icon-license-blue.png';
import BlueLocationIcon from '../../../../assets/images/png/icon-location-blue.png';
import BluePhoneIcon from '../../../../assets/images/png/icon-phone-blue.png';
import BlueMobileIcon from '../../../../assets/images/png/icon-mobile-blue.png';
import { Config, ConfigFieldType } from '../../../../types/config/config';
import { useSelector } from 'react-redux';
import { configSelector } from '../../../../redux/selectors/system/config-selector';
import FormSelect, { FormSelectProps } from '../../../../components/form-select/form-select';

interface Props {
    validations: object,
    changeHandler: any,
    values: any
}

const ContactsForm: FC<Props> = (props) => {
    const { values, validations, changeHandler } = props;

    const config:Config = useSelector(configSelector);

    let phoneMask = "(999) 999-9999";
    if(config && config.contacts && config.contacts.phoneMask){
        phoneMask = config.contacts.phoneMask;
    }
    let homeOfficeFieldType:ConfigFieldType | undefined;
    if(config && config.contacts && config.contacts.homeOffice && config.contacts.homeOffice.fieldType){
        homeOfficeFieldType = config.contacts.homeOffice.fieldType;
    }

    const changeFields = (changeValues:object) => {
        const firstNameField:string = "firstName";
        const lastNameField:string = "lastName";
        const emailField:string = "email";

        // trim spaces for first name / last name / email before they bubble up
        if(changeValues && changeValues[firstNameField]){
            changeValues[firstNameField] = changeValues[firstNameField].trim();
        }
        if(changeValues && changeValues[lastNameField]){
            changeValues[lastNameField] = changeValues[lastNameField].trim();
        }
        if(changeValues && changeValues[lastNameField]){
            changeValues[emailField] = changeValues[emailField].trim();
        }

        if(changeHandler){
            changeHandler(changeValues);
        }
    }

    return (
        <ContactFormContainer>
            <GLForm initVals={values}
                validationAdapter={convertValidationJSON}
                validationJSON={validations}
                changeHandler={changeFields}>
                <Row>
                    <Col sm={6}>
                        <GLField<FormInputProps> name="firstName" placeholder="First" label="First Name" use={FormInput} />
                    </Col>
                    <Col sm={6}>
                        <GLField<FormInputProps> name="lastName" placeholder="Last" label="Last Name" use={FormInput} />
                    </Col>
                    <Col sm={1}>
                        <img src={BlueLocationIcon} />
                    </Col>
                    <Col sm={11}>
                        { homeOfficeFieldType && homeOfficeFieldType === ConfigFieldType.FORM_INPUT && <GLField<FormInputProps> name="location" 
                            placeholder={config.contacts.homeOffice.label} label={config.contacts.homeOffice.label} use={FormInput} /> }
                        { homeOfficeFieldType && homeOfficeFieldType === ConfigFieldType.FORM_SELECT && 
                            <FormSelectWrapper>
                                <GLField<FormSelectProps> name="location" 
                                    label={config.contacts.homeOffice.label} prompt="Select Home Office..." 
                                    use={FormSelect} options={config.contacts.homeOffice.options} />
                            </FormSelectWrapper>       
                        }
                        
                    </Col>
                    <Col sm={1}>
                        <img src={BluePhoneIcon} />
                    </Col>
                    <Col sm={11}>
                        <GLField<FormInputProps> name="phone" placeholder="Phone #" label="Phone Number" use={FormInput} mask={phoneMask} />
                    </Col>
                    <Col sm={1}>
                        <img src={BlueEmailIcon} />
                    </Col>
                    <Col sm={11}>
                        <GLField<FormInputProps> name="email" subText="CBRE email addresses only" placeholder="e.g. john.doe@cbre.com" label="Email Address"  use={FormInput} />
                    </Col>
                    <Col sm={1}>
                        <img src={BlueLicenseIcon} />
                    </Col>
                    <Col sm={11}>
                        <GLField<FormInputProps> name="additionalFields.license" subText="Only enter if required in your market" placeholder="e.g. 12345678" label="License" use={FormInput} />
                    </Col>
                </Row>
            </GLForm>
        </ContactFormContainer>
    )
}

const ContactFormContainer = styled.div`
    input {
        background:transparent;
        border:none;
        border-bottom: solid 1px #7F7F7F;
        color:#333;
        padding-left:0;
    }
    img {
        position:relative; 
        top:58px;
    }
    > div {
        background:transparent;
    }
    h5 {
        margin-bottom:-4px;
    }
    padding-bottom:30px;
    padding-top:28px
`;

const FormSelectWrapper = styled.div`
    h5 {
        margin-bottom: 5px;
    }
`;

export default React.memo(ContactsForm, (prevProps, nextProps) => nextProps.values !== prevProps.values);