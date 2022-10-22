import React from 'react';
import { shallow } from 'enzyme';

import FormFieldLabel from '../form-field-label';

describe('components', () => {

  describe('form-field-label', () => {

    let props:any;

    beforeEach(() => {
        props = {
            label: 'Test'
        }
    });

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<FormFieldLabel {...props} />)).toMatchSnapshot();
    });

  });
});