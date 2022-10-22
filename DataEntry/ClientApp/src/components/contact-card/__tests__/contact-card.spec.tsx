
import React from 'react';
import { shallow } from 'enzyme';

import ContactCard from '../contact-card';

describe('components', () => {

  describe('contact-card', () => {

    const contact = {
        contactId: "3c113c1f-78df-4886-8f95-b65f6f8c7eec",
        email: "jane.smith@cbre.com",
        firstName: "Jane",
        lastName: "Smith",
        license: "123344zzz444",
        location: "Dallas, TX",
        phone: "(202) 345-5678"
    }
    
    const deleteCard = () => {
        console.log('delete')
    }
    
    const openModal = () => {
        console.log('open modal')
    }
    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow( <ContactCard contact={contact} index={0} deleteContact={deleteCard} openModal={openModal} />)).toMatchSnapshot();
    });

  });
});