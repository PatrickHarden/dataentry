import React, {FC, useContext } from 'react';
import styled, {css } from 'styled-components';
import MapPlaceholderImage from '../../../assets/images/png/map-placeholder.png';
import { GeoCoordinates, MapComponent } from '../../../types/map/maps';
import { checkCoordinates } from '../../../utils/map';
import GLGoogleMap from '../../../components/map/google/google-map';
import Mapbox from '../../../components/map/mapbox/mapbox';
import GLForm from '../../../components/form/gl-form';
import { convertValidationJSON } from '../../../utils/forms/validation-adapter';
import FormInput, {FormInputProps} from '../../../components/form-input/form-input';
import GLField from '../../../components/form/gl-field';
import { Col, Row } from 'react-styled-flexboxgrid';
import { updateRenderTrigger } from '../render-trigger-util';
import { FormContext } from '../../../components/form/gl-form-context';
import { createAddressFromGoogleResultUsingLatLng } from '../../../api/places/google/google';

interface Props {
    coordinates?: GeoCoordinates,
    api: string
}

interface ErrorProps {
    error: boolean
}

type MapProps = Props & ErrorProps;

const Map:FC<MapProps> = (props) => {

    const { coordinates, api} = props;
    let { error } = props;
    const component = (api === "mapbox" ? MapComponent.MAPBOX : MapComponent.GOOGLE);   // the map component we'll use
    
    // value change interceptor
    const formControllerContext = useContext(FormContext);

    // script is loaded from head, so we need to just check and make sure it's available
    const googleLoaded = window.google ? true : false;

    // check coordinates, if we dont have them or they are invalid display different state
    const coordinateCheck:boolean = checkCoordinates(coordinates);  

    let topMessage:string = coordinateCheck ? "" : "Please enter an address to see the map*";
    if(component === MapComponent.GOOGLE && window.google === false){
        topMessage = "There was an error loading Google Maps";
        error = true;   // force error to true
    }

    const MapDisplay = () => {
        if(coordinateCheck && coordinates){
            if(component === MapComponent.GOOGLE && googleLoaded){
                return <GLGoogleMap center={coordinates} zoom={12}/>
            }else if(component === MapComponent.MAPBOX){
                return <Mapbox center={coordinates} zoom={12}/>;
            }
        }
        return <StyledPlaceholderImage src={MapPlaceholderImage}/>;    // fallback
    }

    const latLngChangehandler = (values:any) => {
        if(values){
            values = updateRenderTrigger(values, "latLngChange");
            formControllerContext.onFormChange(values);
        }
    }
  
    return (
        <MapContainer>
            <TopMessage error={error}>{topMessage}</TopMessage>
            <MapWrapper error={error}>
                <MapDisplay/>
            </MapWrapper>   
            <CoordinatesWrapper>
              <GLForm initVals={coordinates || {}}
                    validationAdapter={convertValidationJSON}
                    validationJSON={()=> (null)}
                    changeHandler={latLngChangehandler}>
                    <Row>
                        <Col xs={6}><GLField<FormInputProps> disableTab={true} name="lat" numericOnly={true} allowNegatives={true} acceptDecimals={true} placeholder="Latitude" label="Latitude" use={FormInput} /></Col>
                        <Col xs={6}><GLField<FormInputProps> disableTab={true} name="lng" numericOnly={true} allowNegatives={true} acceptDecimals={true} placeholder="Longitude" label="Longitude" use={FormInput} /></Col>
                    </Row>
                </GLForm>
            </CoordinatesWrapper>
        </MapContainer>
    );
};

const normalColor = css`
    ${props => (props.theme.colors ? props.theme.colors.notice : '#b0b0b0')}
`;

const errorColor = css`
    ${props => (props.theme.colors ? props.theme.colors.error : 'darkred')}
`;

const normalBorder = css`
    1px solid ${props => (props.theme.colors ? props.theme.colors.border : '#cccccc')};
`;

const errorBorder = css` 
    1px solid ${props => (props.theme.colors ? props.theme.colors.error : 'darkred')}; 
`;

const MapContainer = styled.div`
    position: relative;
    text-align: center;
`;

const TopMessage = styled.div`
    margin-bottom: 10px;
    min-height: 20px;
    padding-left: 50px;
    font-size: ${props => (props.theme.font ? props.theme.font.formSize.label : '14px')}; 
    color: ${(props: ErrorProps) => props.error ? errorColor : normalColor};
`;

const MapWrapper = styled.div`
    padding: 20px;
    background: #FFFFFF;
    width: 100%;
    height: 450px;
    border: ${(props: ErrorProps) => props.error ? errorBorder : normalBorder};
`;

const StyledPlaceholderImage = styled.img`
    width: 100%;
    height: 100%;
`;

const CoordinatesWrapper = styled.div`
    text-align:center;
    margin-top: 5px;
    width: 100%;
`;

export default React.memo(Map);