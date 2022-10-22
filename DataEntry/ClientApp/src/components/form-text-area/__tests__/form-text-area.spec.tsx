import React from 'react';
import { mount, shallow } from 'enzyme';

import FormTextArea from '../form-text-area';

describe('components', () => {

  describe('form-text-area', () => {

    let props:any;
    let wrapper:any;

    beforeEach(() => {
        props = {
            name: 'testme',
            defaultValue: 'text area goodness',
            label: 'I am indeed a text area!',
            error: false
        }
    });

    // it renders it's normal state properly (border should be #cccccc)
    /*
    it("normal state shows normal border (#cccccc)", () => {
        wrapper = mount(<FormTextArea {...props}/>);
        expect(wrapper.find('textarea')).toHaveStyleRule("border","1px solid #cccccc");
    })
    */

    // it renders it's error state properly (border should be red)
    /*
    it("error state shows red border", () => {
        props = {
            name: 'testme',
            defaultValue: 'text area goodness',
            label: 'I am indeed a text area!',
            error: true
        }
        wrapper = mount(<FormTextArea {...props}/>);
        expect(wrapper.find('textarea')).toHaveStyleRule("border","1px solid red");
    })
    */

    // it has a label of "Test Me!"
    it("renders the expected label", () => {
        wrapper = mount(<FormTextArea {...props}/>);
        expect(wrapper.find('h5').text()).toEqual("I am indeed a text area!");
    });

    // it populates it's initial value correctly
    it("populates its initial value", () => {
        wrapper = mount(<FormTextArea {...props}/>);
        expect(wrapper.find('textarea').instance().value).toEqual("text area goodness");
    });

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<FormTextArea {...props} />)).toMatchSnapshot();
    });
  });
});