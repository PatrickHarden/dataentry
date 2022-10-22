import React from 'react';
import { mount, shallow } from 'enzyme';

import FormInput from '../form-input';

describe('components', () => {

  describe('form-input', () => {

    let props:any;
    let wrapper:any;

    beforeEach(() => {
        props = {
            name: "testme",
            defaultValue: "Some Value",
            label: "Test Me!",
            error: false,
            blurHandler: jest.fn()
        }
    });

    // it renders it's normal state properly (border should be #dedede)
    it("normal state shows normal border (#cccccc)", () => {
        wrapper = mount(<FormInput {...props}/>);
        expect(wrapper.find('input')).toHaveStyleRule("border","1px solid #cccccc");
    })

    // it renders it's error state properly (border should be red)
    it("error state shows red border", () => {
        props = {
            name: "testme",
            defaultValue: "Some Value",
            label: "Test Me!",
            error: true,
            blurHandler: jest.fn()
        }
        wrapper = mount(<FormInput {...props}/>);
        expect(wrapper.find('input')).toHaveStyleRule("border","1px solid red");
    })

    // it shows its normal border
    it("when not in an error state, it shows a normal border", () => {
        wrapper = mount(<FormInput {...props}/>);
        expect(wrapper.find('input')).toHaveStyleRule("border","1px solid #cccccc");
    })

    // it has the correct default value set
    it("has the correct default value", () => {
        wrapper = mount(<FormInput {...props}/>);
        expect(wrapper.find('input').instance().value).toEqual("Some Value");
    })
    
    // it has a label of "Test Me!"
    it("renders the expected label", () => {
        wrapper = mount(<FormInput {...props}/>);
        expect(wrapper.find('h5').text()).toEqual("Test Me!");
    });

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<FormInput {...props} />)).toMatchSnapshot();
    });
  });
});