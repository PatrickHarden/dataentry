import React from 'react';
import { mount, shallow } from 'enzyme';

import FormSelect from '../form-select';

describe('components', () => {

  describe('form-select', () => {

    let props:any;
    let wrapper:any;

    beforeEach(() => {

        props = {
            name: "testme",
            label: "Select Me",
            defaultValue: "Dog",
            prompt: "Select somethin...",
            error: false,
            disabled: false,
            changeHandler: jest.fn(),
            options: [
                {
                    label: "Buffalo",
                    value: "Buffalo",
                    order: 4
                },
                {
                    label: "Cat",
                    value: "Cat",
                    order: 3
                },
                {
                    label: "Dog",
                    value: "Dog",
                    order: 2
                },
                {
                    label: "Parrot",
                    value: "Parrot",
                    order: 1
                },
                {
                    label: "Rat",
                    value: "Rat",
                    order: 5
                }
            ]
        }
    });

    // has proper label
    it("shows its label properly", () => {
        wrapper = mount(<FormSelect {...props}/>);
        expect(wrapper.find('h5').text()).toEqual("Select Me");
    })

    // properly sets its value
    it("sets its value correctly", () => {
        wrapper = mount(<FormSelect {...props}/>);
        expect(wrapper.find('.is-selected'));
    });

    // it has the correct prompt (always first option, when set)
    it("has a prompt", () => {
        wrapper = mount(<FormSelect {...props}/>);
        expect(wrapper.find('.Dropdown-option'))
    });

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<FormSelect {...props} />)).toMatchSnapshot();
    });
  });
});