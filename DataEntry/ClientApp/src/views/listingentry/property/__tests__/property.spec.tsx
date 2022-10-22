import React from 'react';
import { mount, shallow } from 'enzyme';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';

import Property from '../property';

describe('components', () => {
  describe('PropertyView', () => {
    let props: any;
    let wrapper: any;
    let store: any;

    const initialState = {
      system: {
        configDetails: {
          loaded: true,
          config: {
            featureFlags: {
              defaultWaterMarkTrue: false
            }
          }
        }
      },
      entry : {
        refreshImages: []
      },
      mapping: {
        country: {
        }
      }
    };
    const mockStore = configureStore();

    beforeEach(() => {
      props = {
        listing: {
          propertyType: {},
          walkthrough: ""
        },
        validations: {
          'field1': {required: true}
        }
      }
      store = mockStore(initialState);
    });

    it("check the existence of 3D space tag inside the property component", () => {
      wrapper = mount(<Provider store={store}><Property {...props} /></Provider>);
      expect(wrapper.exists('[name="walkThrough"]')).toBeTruthy()
    });

    it("check the existence of Building Display Name tag inside the property component", () => {
      wrapper = mount(<Provider store={store}><Property {...props} /></Provider>);
      expect(wrapper.exists('[name="propertyName"]')).toBeTruthy()
    });

    it("given a prop the value is set", () => {
      const walkthrough = "www.CBREWalkthrough.com";
      props.listing.walkThrough = walkthrough;
      wrapper = mount(<Provider store={store}><Property {...props} /></Provider>);
      expect(wrapper.exists('[id="walkThrough"][defaultValue="' + walkthrough + '"]')).toBeTruthy()
    });

    it("given a prop the value is set", () => {
      const propertyName = "Building Name";
      props.listing.propertyName = propertyName ;
      wrapper = mount(<Provider store={store}><Property {...props} /></Provider>);
      expect(wrapper.exists('[id="propertyName"][defaultValue="' + propertyName + '"]')).toBeTruthy()
    });

    // snapshot render
    it('renders property to snapshot', () => {
      expect(shallow(<Provider store={store}><Property {...props} /></Provider>)).toMatchSnapshot();
    });
  });
});