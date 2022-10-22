import React, { FC, useContext, useState } from 'react';
import { Listing } from '../../../types/listing/listing';
import { Col, Row } from 'react-styled-flexboxgrid';
import { FormContext } from '../../../components/form/gl-form-context';
import SectionHeading from "../../../components/section-heading/section-heading";
import StyledButton from '../../../components/styled-button/styled-button';
import styled from 'styled-components';
import ContactCard from '../../../components/contact-card/contact-card';
import SearchableInput, { SearchableInputNoDataProps } from '../../../components/searchable-input/searchable-input';
import { searchContactsStatic } from '../../../api/contacts/contacts';
import { AutoCompleteResult } from '../../../types/common/auto-complete';
import ContactsModal from './partials/contact-modal';
import { createBlankContact } from './contact-util';
import { Contact } from '../../../types/listing/contact';
import { ManualError } from '../validations/published';
import { useSelector } from 'react-redux';
import { allContactsSelector } from '../../../redux/selectors/contacts/all-contacts-selector';
import ReactTooltip from 'react-tooltip';
import TooltipOff from '../../../assets/images/png/tooltip-off.png'
import TooltipOn from '../../../assets/images/png/tooltip-on.png'
import { generateKey } from '../../../utils/keys';

interface Props {
    listing: Listing,
    error: ManualError,
    validateHandler: Function
}

const Contacts: FC<Props> = (props) => {

    // setting a local variable equal to the listing
    const { listing, error, validateHandler } = props;
    // state used to update/edit contacts
    const contactArr:Contact[] = listing.contacts ? listing.contacts : [];
    const [contacts, setContacts] = useState<Contact[]>(contactArr);
    
    // boolean used to open/close modal
    const [isOpen, setIsOpen] = useState(false);
    // used to pass the contact to the modal
    const [createEditContact, setCreateEditContact] = useState<Contact>(createBlankContact());
    // these are the contacts actually passed into our search box
    const [filteredContacts, setFilteredContacts] = useState<Contact[]>([]);
    // all contacts (changes here will trigger the filtered contacts setup)
    const allContacts:Contact[] = useSelector(allContactsSelector);

    /* flag used to show certain bits of info for dev */
    let showDevInfo:boolean = false;
    const url:string = window.location.href;
    if(url.indexOf("localhost") > -1 || url.indexOf("dev") > -1 || url.indexOf("uat") > -1){
        showDevInfo = true;
    }

    // initial setup to find the set of filtered contacts 
    if(contacts && allContacts && allContacts.length > 0 && (!filteredContacts || filteredContacts.length === 0)){
        // filter out any contacts that are already added to this listing
        const currentIds:number[] = contacts.map((contact:Contact) => contact.contactId);

        const filtered:Contact[] = [];
        allContacts.forEach((contact:Contact) => {
            if(currentIds.indexOf(contact.contactId) === -1){
                filtered.push(contact);
            }
        });
        setFilteredContacts(filtered);
    }

    // pass any changes into the form controller so they can be bubbled up
    const formControllerContext = useContext(FormContext);
    const updateData = (contactUpdate: Contact[]) => {
        const values = {
            'contacts': [...contactUpdate]
        };
        formControllerContext.onFormChange(values);
        if(error && error.error && validateHandler){
            validateHandler(); // parent callback if previously in error state
        }
    }
    
    // data scenario handlers
    // add new contact from the search box
    const addContact = (suggestion:AutoCompleteResult) => {
        const contact = suggestion.value;
        // add contact to array
        const temp:Contact[] = [...contacts];
        temp.push(contact);
        setContacts(temp);
        updateData(temp);
        // remove the newly added contact from the available pool of contacts
        if(filteredContacts){
            filteredContacts.forEach((filteredContact:Contact) => {
                if(contact.contactId && filteredContact.contactId === contact.contactId){
                    filteredContacts.splice(filteredContacts.indexOf(filteredContact),1);
                }
            });
        }
    }

    // delete a contact (triggered from contact card)
    const deleteContact = (index: number) => {
        const temp:Contact[] = [...contacts];
        const contactToDelete:Contact = temp[index];
        temp.splice(index,1);        
        setContacts(temp);
        updateData(temp);
        if(contactToDelete && !contactToDelete.tempId){
            // add the removed contact to the available pool of contacts (if it has a real id - not just an add)
            filteredContacts.push(contactToDelete);
        }        
    }

    // modal save handler
    const contactSaved = (contact:Contact) => {
        const temp:Contact[] = [...contacts];
        temp.forEach((c,idx) => {
            if(c.contactId === contact.contactId){
                temp.splice(idx,1,contact);
            }
        });
        if(temp.indexOf(contact) === -1){
            temp.push(contact); // if not found
        }
        setContacts([...temp]);
        updateData(temp);
        setIsOpen(false);
    }

    // ui interactions
    // create a new contact (create contact button)
    const createContact = () => {
        const contact:Contact = createBlankContact();
        setCreateEditContact(contact);
        toggleModal(true);
    }

    // edit contact (edit contact from contact card)
    const editContact = (contact: Contact) => {
        setCreateEditContact(contact);
        toggleModal(true);
    }

    const toggleModal = (open: boolean) => {
        setIsOpen(open);
    }

    const contactKey = (contact:Contact) => {
        if(contact && contact.contactId){
            return contact.contactId + generateKey()
        }
        return "con" + generateKey();
    }

    const DisplayError = () => {
        return (
            <Row>
                <Col sm={12}><ErrorText>{error.message}</ErrorText></Col>
            </Row>
        )
    }

    const noDataProps:SearchableInputNoDataProps = {
        showNoData: true,
        noDataMessage: 'No contacts found...',
        showNoDataButton: true,
        noDataButtonLabel: "+ Create New Contact",
        noDataButtonCallback: createContact
    }

    return (
        <>
            <div id="contacts">&nbsp;</div>
            <ContactsHeading>
                <SectionHeading error={error && error.error}>
                    Contacts* <Subtext>Use CONTACTS to display your sales agent's information on CBRE's website</Subtext>
                </SectionHeading>
            </ContactsHeading>
            {error && error.error && <DisplayError/>}
            <Row>
                <Col sm={8}>
                    <Label>Select Sales Agent</Label>
                    <IconTag data-tip="true" data-for="contactTooltip"><img src={TooltipOff} onMouseOver={e => (e.currentTarget.src=TooltipOn)} onMouseOut={e => (e.currentTarget.src=TooltipOff)}/></IconTag>
                    <StyledTooltip id="contactTooltip" type='light' border={false} place='right' multiline={true}>
                        <span>You or your team will only need to create a Sales Agent one time. First search for an agent before creating a new one to avoid duplicate records in the database.</span>
                    </StyledTooltip>
                    <SearchableInput key={"contact-search" + new Date().getTime()} name="contactSearch" label="" placeholder="e.g. Select Sales Agent from previously created agents" staticDataProvider={searchContactsStatic} useSearchData={filteredContacts} 
                        autoCompleteFinish={addContact} defaultValue="" clearAfterSelect={true} error={false} showSearchIcon={true} noDataProps={noDataProps} /></Col>
                <Col sm={4}>
                    <CreateContactWrapper>
                        <StyledButton name="addContactButton" onClick={() => createContact()} styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp;Create New Contact</StyledButton>
                    </CreateContactWrapper>
                </Col>
            </Row>
            <Row style={{marginTop: '15px'}}>
                {contacts && contacts.map((contact, index) => (
                    <Col sm={4} key={contactKey(contact)}>
                        <ContactCard
                            contact={contact}
                            index={index}
                            editContact={editContact}
                            showContactId={showDevInfo}
                            deleteContact={deleteContact} />
                    </Col>
                ))}
            </Row>
            {isOpen &&
                <ContactsModal finishHandler={contactSaved} closeHandler={() => toggleModal(false)} contact={createEditContact} />
            }
        </>
    )
}

const ContactsHeading = styled.div`
    display:flex;
    justify-content:space-between;
    align-items:center;
    > span {
        flex-grow:inherit;
    }
`;

const IconTag = styled.a`
  margin-block-start: 2em;
  margin-left: .5em;
`;

const StyledTooltip = styled(ReactTooltip)`
  box-shadow: 0 0 4px 2px grey;
  width: 250px;
  font-family: "Futura Md BT", helvetica, arial, sans-serif;
  font-size: 12px;
  font-weight: 500;
  text-transform: none;
`;

const CreateContactWrapper = styled.div`
    margin-top: 55px;
    > span {
        float:right;
    }
`;

const ErrorText = styled.div`
    color: ${props => (props.theme.colors ? props.theme.colors.error : 'red')}; 
`;

const Label = styled.span`
    font-size:16px;
    color: gray;
    margin-block-end: 0.5em;
    text-transform: capitalize;
    vertical-align: top;
`;

const Subtext = styled.span`
    font-size: 12px;
    font-family: Arial, Helvetica, sans-serif;
    color: #666666;
    font-style: italic;
    text-transform: none;
`

export default Contacts;