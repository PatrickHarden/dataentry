import React, {FC} from 'react'
import { Col, Row } from 'react-styled-flexboxgrid'
import GLField from '../../../../components/form/gl-field';
import FormInput, {FormInputProps} from '../../../../components/form-input/form-input';
import { AutoCompleteResult, AutoCompleteRequest } from '../../../../types/common/auto-complete';
import SearchableInput, { SearchableInputProps, SearchableInputNoDataProps } from '../../../../components/searchable-input/searchable-input';
import { getMapboxPlaces, createAddressFromMapboxResult } from '../../../../api/places/mapbox/mapbox';
import { getPitneyPlaces, createAddressFromPitneyBowesResult } from '../../../../api/places/pitney-bowes/pitney-bowes';
import { getGooglePlaces, createAddressFromGoogleResult } from '../../../../api/places/google/google';
import { useSelector } from 'react-redux';
import { configSelector } from '../../../../redux/selectors/system/config-selector';
import FormSelect from '../../../../components/form-select/form-select';
import { FieldConfig, Config, StateFieldConfig } from '../../../../types/config/config';
import { Option, StateOrProvinceOption } from '../../../../types/common/option';
import { countryCodeSelector } from '../../../../redux/selectors/mapping/country-code-selector';

interface Props {
    placesAPI : string,
    apiCountry: string,
    currentCountryCode: string,
    changeHandler(values:any): void,
    streetChanged(value:string | null): void
}

export const AddressFields: FC<Props> = (props) => {

    const { placesAPI, changeHandler, streetChanged, apiCountry, currentCountryCode } = props;

    const config = useSelector(configSelector);
    const countryCode = useSelector(countryCodeSelector);

    const addressHandler = (suggestion:AutoCompleteResult) => {
        let address;
        const addressComponentsConfig = config.map.addressComponents;
        streetChanged(null);  // since we are selecting a result, ensure the temp street gets zeroed out
        if(placesAPI === "pitney"){
            address = createAddressFromPitneyBowesResult(suggestion);
            changeHandler(address);
        }else if(placesAPI === "mapbox"){
            address = createAddressFromMapboxResult(suggestion);
            changeHandler(address);
        }else{
            createAddressFromGoogleResult(suggestion, addressComponentsConfig).then(addressResult => {
                address = addressResult;
                changeHandler(address);
            });
        }
    }
    
    const findRemoteDataProvider = (request:AutoCompleteRequest) : Promise<AutoCompleteResult[]> => {  

        request.countryCodes = config.map.places.limitCountryCodes;

        
        if(placesAPI === "pitney"){
            return new Promise((resolve,reject) => {
                resolve(getPitneyPlaces(request));
            });
        }else if(placesAPI === "mapbox"){
            return new Promise((resolve,reject) => {
                resolve(getMapboxPlaces(request));
            });
        }else{
            return new Promise((resolve,reject) => {
                resolve(getGooglePlaces(request));
            });
        }
    }

    const noDataProps:SearchableInputNoDataProps = {
        showNoData: true,
        noDataMessage: 'No addresses found...',
        showNoDataButton: false
    }

    const getColSize = (fieldName:string):number => {
        if(config && config[fieldName] && config[fieldName].size){
            return config[fieldName].size;
        }
        return 12;
    }

    const getCountry = () => {
        if(config && config.country && config.country.show){
            const fieldConfig:FieldConfig<Option> = config.country;
            return <Col lg={getColSize("country")} md={6} sm={6}><GLField name="country" label={fieldConfig.label} use={FormSelect} options={fieldConfig.options}/></Col>
        }
        return <></>;
    }
    
    const getStateOrProvince = () => {
        if(config && config.stateOrProvince && config.stateOrProvince.show){
            const fieldConfig:StateFieldConfig = config.stateOrProvince;
            let tempCountryCode:string = "";
            if(currentCountryCode && currentCountryCode.length > 0){
                tempCountryCode = currentCountryCode;
            }else if(fieldConfig.defaultCountry){
                tempCountryCode = fieldConfig.defaultCountry;
            }
            let stateOptions:Option[] = [];
            if(fieldConfig.countryStates){
                fieldConfig.countryStates.forEach((countryState:StateOrProvinceOption) => {
                    if(countryState.countryCode === tempCountryCode){
                        stateOptions = countryState.options;
                    }
                });
            }
            return <Col lg={getColSize("stateOrProvince")} md={6} sm={6}><GLField name="stateOrProvince" label={fieldConfig.label} use={FormSelect} options={stateOptions}/></Col>
        }
        return <></>;
    }

    return (
        <>
            <Row between="sm">
                <Col xs={12}><GLField<SearchableInputProps> name="street" placeholder="Search for..." countryCode={countryCode}
                                label="Street Address*" use={SearchableInput} extraData={apiCountry} remoteDataProvider={findRemoteDataProvider} 
                                autoCompleteFinish={addressHandler} showSearchIcon={false} noDataProps={noDataProps} captureValue={streetChanged}/></Col>
                <Col xs={12}><GLField<FormInputProps> name="street2" placeholder="Address 2" label="Address 2" use={FormInput}/></Col> 
                <Col lg={getColSize("city")} md={12} sm={12} xs={12}><GLField<FormInputProps> name="city" placeholder="City" label="City*" use={FormInput}/></Col>
                { getCountry() }
                { getStateOrProvince() }                        
                <Col lg={getColSize("postalCode")} md={6} sm={6}><GLField<FormInputProps> name="postalCode" placeholder="Postal Code" label="Postal Code*" use={FormInput}/></Col>
            </Row>
        </>
    );
}

                        