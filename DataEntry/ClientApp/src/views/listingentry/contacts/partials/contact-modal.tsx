import React, { FC, useState } from 'react';
import { Col, Row } from 'react-styled-flexboxgrid';
import StyledButton from '../../../../components/styled-button/styled-button';
import styled from 'styled-components';
import ContactsForm from './contacts-form';
import { Contact } from '../../../../types/listing/contact';
import ImageAvatar, { ImageAvatarSize, PlaceholderSize } from '../../../../components/image-avatar/image-avatar';
import { uploadFile } from '../../../../api/files/file-upload';
import cloneDeep from 'clone-deep';
import { generateInitials } from '../../../../components/contact-card/util/generate-initials';
import { saveContactFromModal } from '../../../../redux/actions/contacts/save-contact';

interface Props {
    contact: Contact,
    finishHandler: (contact: Contact) => void,
    closeHandler: () => void
}

const ContactsModal: FC<Props> = (props) => {

    const { contact, closeHandler, finishHandler } = props;

    const [contactChanges, setContactChanges] = useState<Contact>(Object.assign({},contact));
    // really don't want to have to manage this separately, but there's a "gotcha" with the form where if we remove the memoization it doesn't allow us to navigate
    // the form as expected with tabbing, so we manage the contact changes and the the avatar separately and then combine when the user closes the window
    const [avatar, setAvatar] = useState<string>(contact.avatar ? contact.avatar : ''); 
    // errors
    const [uploadErrors, setUploadErrors] = useState<string>("");
    const [saveError, setSaveError] = useState<string>("");

    const validations = {};

    const formChangeHandler = (values: any) => {
        // handle bubbled up changes from the form
        setContactChanges(Object.assign({},values));
    }

    const finish = () => {

        //  on finish, we save to the DB and when we get the result, we call the finish handler
        if(avatar && avatar.length > 0){
            contactChanges.avatar = avatar;
        }

        saveContactFromModal(contactChanges).then((result:Contact) => {
            finishHandler(result);
        }).catch((error:string) => {
            setSaveError(error);
        });
    }

    const uploadAvatar = (file:any) => {
        uploadFile(file,"/api/FileUpload/ContactAvatar/").then((imgURL:any) => {
            // we have a URL returned from the server.  verify that it meets our criteria before continuing
            const img = new Image();
            img.onload = () => {
                if(img.width !== img.height){
                    setAvatar("");
                    setUploadErrors("Image dimensions must be equal.");
                }else{
                    setAvatar(imgURL);
                }
            }
            img.src = imgURL;
        }).catch(err => {
            setUploadErrors("Server error uploading image.");
        });
    }

    const clearErrors = () => {
        setUploadErrors("");
    }

    return (
        <ContactModal>
            <ContactModalContainer>
                <ContactModalStyles>
                    <Row id="modalTitleRow">
                        <Col sm={6}>
                            <h2>{contact.contactId ? "Edit Contact" : "Create Contact"}</h2>
                        </Col>
                        <Col sm={6}>
                            <ButtonContainer>
                                <StyledButton onClick={() => closeHandler()} primary={false}>Cancel</StyledButton>
                                <StyledButton onClick={() => finish()} primary={false}>{contact.contactId ? <>Save</> : <>Add</>}</StyledButton>
                            </ButtonContainer>
                        </Col>
                    </Row>
                    <Row id="modalFormRow">
                        <Col sm={4}>
                            <ImageAvatar key={"ia_"+new Date().getTime()} avatarSize={ImageAvatarSize.LARGE} placeholderSize={PlaceholderSize.LARGE}
                                placeholder={generateInitials(contactChanges)} imageURL={avatar} 
                                allowUpload={true} uploadPhotoLabel={avatar && avatar.length > 0 ? 'Change Photo' : 'Add Photo'} 
                                uploadFile={uploadAvatar} uploadErrors={uploadErrors} clearErrors={clearErrors} />
                        </Col>
                        <Col sm={8}>
                            { saveError && <ErrorText>{saveError}</ErrorText>}
                            <ContactsForm validations={validations} values={contactChanges} changeHandler={formChangeHandler} />
                        </Col>
                    </Row>
                </ContactModalStyles>
            </ContactModalContainer>
        </ContactModal>
    )
}

const ErrorText = styled.div`
    color: darkred;
    font-size: 14px;
    margin: 15px 0 0 0;
`;

const ContactModalStyles = styled.div`
    padding:10px 20px 20px 20px;
    #modalTitleRow {
        border-bottom:solid 1px #333;
        padding:20px 0 15px 0;
        h2 {
            color: #626262;
            text-transform:uppercase;
        }
    }
    #modalFormRow {
        > div:first-of-type {
            text-align:center;
        }
    }
    max-width:800px;
    margin:0 auto;
`;

const ContactModalContainer = styled.div`
    background-color:#fff;
`
const ButtonContainer = styled.div`
    display:flex;
    width:100%;
    margin-top:15px;
    justify-content:flex-end;
    button {
        width:auto;
        flex-grow:inherit;
    }
    > button:first-of-type {
        margin-right:6px;
        background-color:transparent;
        color: #7F7F7F;
    }
`;

const ContactModal = styled.div`
    position:absolute;
    top:0;
    bottom:0;
    right:0;
    left:0;
    background:rgba(0,0,0,0.6);
    z-index:2000;
    > span {
        color:#fff;
        font-size:35px;
        position:fixed;
        top:5px;
        right:15px;
        cursor:pointer
    }
    > div {
        position:fixed;
        top:0;
        left:0;
        right:0;
        background:#fff;
    }
`;

export default React.memo(ContactsModal);