import React from 'react';
import { mount, shallow } from 'enzyme';

import SearchableInput from '../searchable-input';

describe('components', () => {

  describe('searchable-input', () => {

    let props:any;
    let wrapper:any;


    beforeEach(() => {
        props = {
            name: "searchable",
            label: "Search Me",
            defaultValue: "Some Value",
            placeholder: "Search...",
            error: false,
            delayMS: 1000
        }
    });

    // it displays a default value as expected
    it("default value displays as expected", () => {
        wrapper = mount(<SearchableInput {...props}/>);
        expect(wrapper.find('input').instance().value).toEqual("Some Value");
    });

    // it sets the default label as expected
    it("renders the expected label", () => {
      wrapper = mount(<SearchableInput {...props}/>);
      expect(wrapper.find('h5').text()).toEqual("Search Me");
    });

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<SearchableInput {...props} />)).toMatchSnapshot();
    });
  });
});