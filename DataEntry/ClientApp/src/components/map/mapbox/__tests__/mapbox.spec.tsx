import Mapbox from "../mapbox";
import React from 'react';
import { shallow } from 'enzyme';
import { GeoCoordinates } from '../../../../types/map/maps';

jest.mock('mapbox-gl/dist/mapbox-gl', () => ({ Map: () => ({}) }));

describe('components', () => {
  describe('mapbox', () => {

    const center:GeoCoordinates = {lat: 32.7767, lng: -96.7970}

    // snapshot render
    it('renders according to snapshot', () => {
        const wrapper = shallow(<Mapbox center={center} zoom={12} />);
        expect(wrapper).toMatchSnapshot();
    });

  });
});