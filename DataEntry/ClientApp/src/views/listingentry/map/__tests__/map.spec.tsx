import React from 'react';
import { mount, shallow } from 'enzyme';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';

import Map from '../map';

describe('components', () => {
  describe('MapView', () => {
    let props: any;
    let wrapper: any;
    let store: any;

    const initialState = {
      system: {
        configDetails: {
          loaded: true,
          config: {
          }
        }
      },
      mapping: {
        country: {
        }
      }
    };
    const mockStore = configureStore();
  
    beforeEach(() => {
      props = {
        coordinates: {
          lat:0,
          lng:0
        },
        validations: {
          'field1': {required: true}
        }
      }
      store = mockStore(initialState);
    });

    it("check the existence of lat tag inside the map component", () => {
      wrapper = mount(<Provider store={store}><Map {...props} /></Provider>);
      expect(wrapper.exists('[name="lat"]')).toBeTruthy()
    });

    it("check the existence of lng tag inside the map component", () => {
      wrapper = mount(<Provider store={store}><Map {...props} /></Provider>);
      expect(wrapper.exists('[name="lng"]')).toBeTruthy()
    });

    it("given a prop the value is set", () => {
      const lat = "10";
      const lng = "20";
      props.coordinates.lat = lat;
      props.coordinates.lng = lng;
      wrapper = mount(<Provider store={store}><Map {...props} /></Provider>);
      expect(wrapper.exists('[id="lat"][defaultValue="' + lat + '"]')).toBeTruthy();
      expect(wrapper.exists('[id="lng"][defaultValue="' + lng + '"]')).toBeTruthy();
    });

    // snapshot render
    it('renders map to snapshot', () => {
      expect(shallow(<Provider store={store}><Map {...props} /></Provider>)).toMatchSnapshot();
    });
  });
});