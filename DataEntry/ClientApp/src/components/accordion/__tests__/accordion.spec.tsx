import React from 'react';
import { mount, shallow } from 'enzyme';

import Accordion from '../accordion';

describe('components', () => {

  describe('accordion', () => {

    let props:any;

    beforeEach(() => {
        props = {
            title: 'Accordion Title',
            loadOpen: true,
            alertOn: false,
            titlePrefix: "1"
        }
    });

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<Accordion {...props} />)).toMatchSnapshot();
    });

  });
});