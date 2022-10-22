import React, { FC } from 'react';
import { GeoCoordinates } from '../../../types/map/maps';
import GoogleMapComponent from './google-map-component';
import styled from 'styled-components';

interface GoogleMapProps {
    center?: GeoCoordinates,
    zoom?: number
}

const GLGoogleMap:FC<GoogleMapProps> = (props) =>{
    return <GoogleMapComponent {...props} 
                containerElement={<Container/>}
                mapElement={<MapElement/>}/>
}


const Container = styled.div`
  height: 100%;
  width: 100%;
  overflow: hidden;
  :focus-within {
    outlined: 1px solid red;
  }
`;

const MapElement = styled.div`
  height: 100%;
  :focus-within {
    outlined: 1px solid yellow;
  }
`;

export default GLGoogleMap;
