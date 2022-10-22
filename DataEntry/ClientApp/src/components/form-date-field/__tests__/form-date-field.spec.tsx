import React from 'react';
import { mount, shallow } from 'enzyme';

import FormDateField from '../form-date-field';

describe('components', () => {

  describe('form-date-field', () => {

    let props:any;
    let wrapper:any;

    beforeEach(() => {
        props = {
            name: "datefieldtest",
            label: "Date Field Test",
            error: false,
            changeHandler: jest.fn()
        }
    });

    // it has the correct default value set
    it("has the correct default value", () => {
        props.defaultValue = new Date("12/25/2019");
        wrapper = mount(<FormDateField {...props}/>);
        expect(wrapper.find('input').instance().value).toEqual("12/25/2019");
    })
    
    // label is correct
    it("renders the expected label", () => {
        wrapper = mount(<FormDateField {...props}/>);
        expect(wrapper.find('h5').text()).toEqual("Date Field Test");
    });

    // placeholder is correct
    it("placeholder is correct", () => {
        props.placeholder = "Placeholder test";
        wrapper = mount(<FormDateField {...props}/>);
        expect(wrapper.find('input').props().placeholder).toEqual("Placeholder test");
    });

    // test out hitting the backspace key, value should be null
    it("when backspace is hit, it removes date value", () => {
        props.defaultValue = new Date("12/25/2019");
        wrapper = mount(<FormDateField {...props}/>);
        const input = wrapper.find('input');
        input.simulate('keydown',{keyCode: 8});
        expect(input.instance().value).toEqual("");
    });

    // test out hitting the delete key, value should be null
    it("when delete is hit, it removes date value", () => {
        props.defaultValue = new Date("12/25/2019");
        wrapper = mount(<FormDateField {...props}/>);
        const input = wrapper.find('input');
        input.simulate('keydown',{keyCode: 46});
        expect(input.instance().value).toEqual("");
    });
  });
});