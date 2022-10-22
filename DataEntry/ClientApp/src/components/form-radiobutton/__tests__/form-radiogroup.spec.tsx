import React from 'react';
import { shallow } from 'enzyme';

import FormRadioGroup from '../form-radiogroup';

describe('components', () => {

  describe('form-radiogroup', () => {

    let props:any;

    beforeEach(() => {
        props = {
            name: 'testme',
            label: 'Turn up the radio!',
            options: [{value: 'one', label: 'meow'}, {value: 'two', label: 'bark'}],
            error: false,
            changeHandler: jest.fn()
        }
    });

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<FormRadioGroup {...props} />)).toMatchSnapshot();
    });
  });
});