import React, { FC } from 'react';
import { withGoogleMap, GoogleMap } from "react-google-maps";
import { Marker } from 'react-google-maps';
import { GeoCoordinates } from '../../../types/map/maps';

interface Props {
    center?: GeoCoordinates,
    zoom?: number
}

const GLGoogleMap:FC<Props> = (props) =>{
    
    let { center, zoom } = props;

    // if not passed, setup some defaults to display something 
    center = center ? center : {lat: 0, lng: 0};
    zoom = zoom ? zoom : 9;

    // map options, static for now
    const options:google.maps.MapOptions = {
        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.SMALL,
        },
        mapTypeControl: true,
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.DEFAULT
        },
        streetViewControl: false,
        rotateControl: false,
        fullscreenControl: true
    }

    /*
    const onTilesLoaded = () => {    

        const ele = document.getElementsByClassName("gm-style")['0'];
        if(ele){
            disableFocus(ele.children);
        }

        const iframes = document.querySelectorAll('iframe');
        iframes.forEach((iframe:any) => {
            if(iframe.contentDocument){
                console.log(iframe.contentDocument);
            }
        });
       
    }
    */

    /*
    const disableFocus = (children:[]) => {
        if(children){
            let child:any;
            for(child of children){
                if(child && child.hasAttribute("tabindex")){
                    child.removeAttribute("tabindex");
                }
                if(child && child.children){
                    disableFocus(child.children);
                }
            }
        }
    }
    */
    
    return (
        <GoogleMap
          defaultZoom={zoom}
          center={center}
          options={options}>
              <Marker 
                    position={center} 
                    icon="data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHhtbG5zOnhsaW5rPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5L3hsaW5rIiB3aWR0aD0iMjkiIGhlaWdodD0iMzUiPjxpbWFnZSB3aWR0aD0iMjkiIGhlaWdodD0iMzUiIHhsaW5rOmhyZWY9ImRhdGE6aW1hZ2Uvc3ZnK3htbDtiYXNlNjQsUEQ5NGJXd2dkbVZ5YzJsdmJqMGlNUzR3SWlCbGJtTnZaR2x1WnowaVZWUkdMVGdpUHo0OGMzWm5JSGRwWkhSb1BTSTBOSEI0SWlCb1pXbG5hSFE5SWpVemNIZ2lJSFpwWlhkQ2IzZzlJakFnTUNBME5DQTFNeUlnZG1WeWMybHZiajBpTVM0eElpQjRiV3h1Y3owaWFIUjBjRG92TDNkM2R5NTNNeTV2Y21jdk1qQXdNQzl6ZG1jaUlIaHRiRzV6T25oc2FXNXJQU0pvZEhSd09pOHZkM2QzTG5jekxtOXlaeTh4T1RrNUwzaHNhVzVySWo0Z0lDQWdQR2NnYVdROUlsTjViV0p2YkhNaUlITjBjbTlyWlQwaWJtOXVaU0lnYzNSeWIydGxMWGRwWkhSb1BTSXhJaUJtYVd4c1BTSnViMjVsSWlCbWFXeHNMWEoxYkdVOUltVjJaVzV2WkdRaVBpQWdJQ0FnSUNBZ1BHY2dhV1E5SWsxcGMyTXZUV0Z3TDFCcGJpSWdkSEpoYm5ObWIzSnRQU0owY21GdWMyeGhkR1VvTFRFd0xqQXdNREF3TUN3Z0xUWXVNREF3TURBd0tTSWdabWxzYkQwaUl6QXdOa0UwUkNJK0lDQWdJQ0FnSUNBZ0lDQWdQSEJoZEdnZ1pEMGlUVE15TERVNUlFTXhOeTR6TXpNek16TXpMRFEyTGpRek16VXdPVGNnTVRBc016WXVNVEF3TVRjMk15QXhNQ3d5T0NCRE1UQXNNVFV1T0RRNU56TTFOU0F4T1M0NE5EazNNelUxTERZZ016SXNOaUJETkRRdU1UVXdNalkwTlN3MklEVTBMREUxTGpnME9UY3pOVFVnTlRRc01qZ2dRelUwTERNMkxqRXdNREUzTmpNZ05EWXVOalkyTmpZMk55dzBOaTQwTXpNMU1EazNJRE15TERVNUlGb2dUVE15TERNNUlFTXpPQzR3TnpVeE16SXlMRE01SURRekxETTBMakEzTlRFek1qSWdORE1zTWpnZ1F6UXpMREl4TGpreU5EZzJOemdnTXpndU1EYzFNVE15TWl3eE55QXpNaXd4TnlCRE1qVXVPVEkwT0RZM09Dd3hOeUF5TVN3eU1TNDVNalE0TmpjNElESXhMREk0SUVNeU1Td3pOQzR3TnpVeE16SXlJREkxTGpreU5EZzJOemdzTXprZ016SXNNemtnV2lJZ2FXUTlJbTFoY0Mxd2FXNGlQand2Y0dGMGFENGdJQ0FnSUNBZ0lEd3ZaejRnSUNBZ1BDOW5Qand2YzNablBnPT0iLz48L3N2Zz4="/>
        </GoogleMap>
      );
}

export default withGoogleMap(GLGoogleMap);