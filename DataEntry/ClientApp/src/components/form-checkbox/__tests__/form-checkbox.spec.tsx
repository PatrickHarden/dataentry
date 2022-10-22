import React from 'react';
import { mount, shallow } from 'enzyme';

import FormCheckbox from '../form-checkbox';

describe('components', () => {

  describe('form-checkbox', () => {

    let props:any;
    let wrapper:any;

    beforeEach(() => {
        props = {
            name: "testme",
            label: "I am a checkbox",
            defaultValue: true,
            changeHandler: jest.fn()
        }
    });

    // it is a checkbox
    it("it should be a checkbox (type = checkbox)", () => {
        wrapper = mount(<FormCheckbox {...props}/>);
        expect(wrapper.find('input').props().type).toEqual("checkbox")
    });

    // it sets the checkbox as selected by default (field.value = true)
    it("should be checked (with value of true)", () => {
        wrapper = mount(<FormCheckbox {...props}/>);
        expect(wrapper.find('input').props().checked).toEqual(true)
    });

    // it has a label of "I am a checkbox"
    it("renders the expected label", () => {
        wrapper = mount(<FormCheckbox {...props}/>);
        expect(wrapper.find('h5').text()).toEqual("I am a checkbox");
    });

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<FormCheckbox {...props} />)).toMatchSnapshot();
    });
  });
});