import React, { FC, useEffect } from 'react';
import mapboxgl from 'mapbox-gl';
import { GeoCoordinates } from '../../../types/map/maps';

interface MapboxProps {
    center?: GeoCoordinates,
    zoom?: number
}

const Mapbox: FC<MapboxProps> = (props) => {
    let { zoom, center } = props;

    useEffect(() => {
        mapboxgl.accessToken = 'pk.eyJ1IjoibGlnbGRldnMiLCJhIjoiY2p3amthb2R4MGR6cjQ5cW41MWsxdHB3ciJ9.cmINTRsP78I-obiEXYoNqw';
        center = center ? center : {lat: 0, lng: 0};
        zoom = zoom ? zoom : 9;

        if (center) {
            const map = new mapboxgl.Map({
                'container': 'map',
                'style': 'mapbox://styles/mapbox/streets-v11',
                'center': [center.lng, center.lat],
                'zoom': zoom
            });
    
            // Add pin drop
            new mapboxgl.Marker()
                    .setLngLat([center.lng, center.lat])
                    .addTo(map);

            // Add zoom in/out buttons
            map.addControl(new mapboxgl.NavigationControl());
        }
        else {
            const map = new mapboxgl.Map({
                'container': 'map',
                'style': 'mapbox://styles/mapbox/streets-v11',
                'zoom': zoom
            });
        }
    });

    return (
        <>
            <link href='https://api.mapbox.com/mapbox-gl-js/v1.0.0/mapbox-gl.css' rel='stylesheet' />
            <div id="map" style={{height:'100%', width:'100%'}} />
        </>
    );
} 

export default Mapbox;