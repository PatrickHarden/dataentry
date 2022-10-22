import React from 'react';
import { mount, shallow } from 'enzyme';

import FormRadioButton from '../form-radiobutton';

describe('components', () => {

  describe('form-radiobutton', () => {

    let props:any;
    let wrapper:any;

    beforeEach(() => {
        props = {
            name: 'testme',
            label: 'Turn up the radio!',
            defaultValue: '',
            error: false,
            changeHandler: jest.fn()
        }
    });

    // has proper label
    it("shows its label properly", () => {
        wrapper = mount(<FormRadioButton {...props}/>);
        expect(wrapper.find('h5').text()).toEqual("Turn up the radio!");
    })

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<FormRadioButton {...props} />)).toMatchSnapshot();
    });
  });
});