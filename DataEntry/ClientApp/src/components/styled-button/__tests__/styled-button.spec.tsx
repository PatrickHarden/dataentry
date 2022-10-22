import React from 'react';
import { mount, shallow } from 'enzyme';

import StyledButton from '../styled-button';

describe('components', () => {

  describe('styled-button', () => {

    let props:any;
    

    beforeEach(() => {
        props = {
            buttonStyle: '1',
            primary: false
        }
    });

    

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<StyledButton {...props} />)).toMatchSnapshot();
    });
  });
});