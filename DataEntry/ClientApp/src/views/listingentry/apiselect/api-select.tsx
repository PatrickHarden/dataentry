import React, { FC } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Col, Row } from 'react-styled-flexboxgrid';
import FormRadioButton from '../../../components/form-radiobutton/form-radiobutton';
import FormSelect from '../../../components/form-select/form-select';
import { Option } from '../../../types/common/option';
import { countrySelector } from '../../../redux/selectors/mapping/country-selector';
import { selectCountry } from '../../../redux/actions/mapping/select-country';

interface Props {
    api: string,
    changeHandler: (api: string) => void;
}

const APISelect: FC<Props> = (props) => {

    const { api, changeHandler } = props;

    const country = useSelector(countrySelector);
    const dispatch = useDispatch();

    const changeAPI = (value:string) => {
        changeHandler(value);
    }

    const pbCountries:Option[] = [
        {label: "United States", value: "USA", order: 1},
        {label: "Singapore", value: "SGP", order: 2},
        {label: "India", value: "IND", order: 3},
        {label: "China", value: "CHN", order: 4},
        {label: "United Kingdom", value: "GBR", order: 5},
        {label: "Canada", value: "CAN", order: 6},
        {label: "Australia", value: "AUS", order: 7},
        {label: "New Zealand", value: "NZL", order: 8},
    ]

    const changeCountry = (value:string) => {
        dispatch(selectCountry(value));
    }

    const getDropdown = () => {
        if(api === "pitney"){
            return (
                <FormSelect name="pbCountry" label="Pitney Bowes - Country" defaultValue={country} options={pbCountries} changeHandler={changeCountry} />
            );
        }else{
            return <></>;
        }
    }

    return (
        <Row between="sm">
            <Col xs={12}>
                <FormRadioButton name="googleRB" label="Google" optionVal="google" defaultValue={api} changeHandler={changeAPI} error={false} forceFocus={false}/>
                <FormRadioButton name="mapboxRB" label="Mapbox" optionVal="mapbox" defaultValue={api} changeHandler={changeAPI} error={false} forceFocus={false}/>
                <FormRadioButton name="pbRB" label="Pitney Bowes" optionVal="pitney" defaultValue={api} changeHandler={changeAPI} error={false} forceFocus={false}/>
            </Col>
            <Col xs={12}>
                { getDropdown() }
            </Col>
        </Row>
    );
};

export default React.memo(APISelect);


