import React from 'react';
import { mount, shallow } from 'enzyme';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';

import ContactsForm from '../partials/contacts-form';

describe('components', () => {
  describe('ContactsFormView', () => {
    let props: any;
    let wrapper: any;
    let store: any;

    const initialState = {
      system: {
        configDetails: {
          loaded: true,
          config: {
            "contacts": {
                "homeOffice": {
                  "show": true,
                  "label": "Home Office",
                  "required": false,
                  "options": [],
                  "fieldType": "FORM_INPUT"
                }
            }
          }
        }
      }
    };
    const mockStore = configureStore();

    beforeEach(() => {
      props = {
        values: {
          propertyType: {},
          location: ""
        },
        validations: {
          'field1': {required: true}
        }
      }
      store = mockStore(initialState);
    });

    it("check the existence of location tag inside the contacts form component", () => {
      wrapper = mount(<Provider store={store}><ContactsForm {...props} /></Provider>);
      expect(wrapper.exists('[name="location"]')).toBeTruthy()
    });

    it("given a prop the value is set", () => {
      const location = "Dallas";
      props.values.location = location;



      wrapper = mount(<Provider store={store}><ContactsForm {...props} /></Provider>);
      expect(wrapper.exists('[id="location"][defaultValue="' + location + '"]')).toBeTruthy()
    });

    // snapshot render
    it('renders property to snapshot', () => {
      expect(shallow(<Provider store={store}><ContactsForm {...props} /></Provider>)).toMatchSnapshot();
    });
  });
});