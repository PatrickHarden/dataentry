import React, { FC } from 'react';
import { Contact } from '../../types/listing/contact';
import styled from 'styled-components';
// images:
import EmailIcon from '../../assets/images/png/icon-email.png'
import LicenseIcon from '../../assets/images/png/icon-license.png'
import PhoneIcon from '../../assets/images/png/icon-phone.png'
// import SmartPhoneIcon from '../../assets/images/png/icon-smartphone.png'
import DeleteIcon from '../../assets/images/png/deleteIcon.png'
import ImageAvatar, { ImageAvatarSize, PlaceholderSize } from '../image-avatar/image-avatar';
import { generateInitials } from './util/generate-initials';

interface Props {
    contact: Contact,
    index: number,
    showContactId?: boolean,
    editContact?: (contact:Contact) => void,
    deleteContact?: (index:number) => void,
}

const ContactCard: FC<Props> = (props) => {
    const { contact, index, showContactId, editContact, deleteContact } = props;

    if (!contact) {
        return null;
    }

    return (
        <Card>
            <div className="contactLeftCol">
                <ImageAvatar avatarSize={ImageAvatarSize.MEDIUM} placeholderSize={PlaceholderSize.MEDIUM} 
                    allowUpload={false} imageURL={contact.avatar ? contact.avatar : ''} placeholder={generateInitials(contact)}/>
                { editContact && <Edit id={"editContact" + contact.contactId} onClick={() => editContact(contact)}>Edit</Edit>}
            </div>
            <div className="contactRightCol">
                { deleteContact && <span onClick={() => deleteContact(index)}><img src={DeleteIcon} /></span>}
                <ContactName id="contactNameWrapper">{(contact.firstName ? contact.firstName : '')} {(contact.lastName ? contact.lastName : '')} 
                    {showContactId && <ContactId className="dispContactId">&nbsp;(ID: {contact.contactId})</ContactId>}
                </ContactName>
                <ContactAddress id="contactLocationWrapper">{contact.location}</ContactAddress>
                <p id="emailWrapper"><a href={'mailto: ' + contact.email}><img src={EmailIcon} />{contact.email}</a></p>
                <p id="phoneWrapper"><a href={'tel: ' + contact.phone}><img src={PhoneIcon} />{contact.phone}</a></p>
                {contact.additionalFields && contact.additionalFields.license !== '' &&
                <p id="licenseWrapper"><img src={LicenseIcon} />{contact.additionalFields.license}</p>}
            </div>
        </Card>
    )
}

const Card = styled.div`
    padding:10px 5px 5px 0;
    background:#fff;
    overflow:hidden;
    display:flex;
    margin-top:15px;
    box-shadow: 0 4px 6px rgba(204,204,204,0.12), 0 2px 4px rgba(204,204,204,0.24);
    transition: all 0.3s cubic-bezier(.25,.8,.25,1);
    &:hover {
        box-shadow: 0 7px 14px rgba(204,204,204,0.20), 0 5px 5px rgba(204,204,204,0.17);
    }
    .contactLeftCol {
        width:30%;
    }
    .contactRightCol {
        width:70%;
        color:#666;
        position:relative;
        > span {
            position:absolute;
            top:0;
            right:6px;
            cursor:pointer;
            img {
                height:22px;
                width:17px;
            }
        }
        p {
            > a {
                color:inherit;
                text-decoration:none;
            }
            img {
                margin-right:6px;
                position:relative;
                top:5px;
            }
        }
    }
`;

const Edit = styled.p`
    font-size:14px;
    text-align:center;
    color: rgb(0, 106, 77);
    cursor:pointer;
    margin-top:10px;
`;

const ContactName = styled.p`
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
    color:#666;
`;

const ContactId = styled.span`
    font-family: ${props => props.theme.font ? props.theme.font.primary : 'helvetica'};
    font-size: 10px;
`;

const ContactAddress = styled.p`
    font-size:12px;
    margin:-10px 0 20px 0;
`;

export default React.memo(ContactCard,  (prevProps, nextProps) => nextProps.contact !== prevProps.contact && nextProps.contact !== undefined)