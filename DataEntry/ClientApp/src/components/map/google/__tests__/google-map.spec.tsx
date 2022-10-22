import GoogleMap from "../google-map";
import React from 'react';
import { shallow } from 'enzyme';
import { GeoCoordinates } from '../../../../types/map/maps';

describe('components', () => {
  describe('google-map', () => {

    const center:GeoCoordinates = {lat: 32.7767, lng: -96.7970}

    // snapshot render
    it('renders according to snapshot', () => {
        const wrapper = shallow(<GoogleMap center={center} zoom={12} />);
        expect(wrapper).toMatchSnapshot();
    });
  });
});