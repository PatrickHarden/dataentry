import React, { FC } from 'react';
import styled, { css }   from 'styled-components';
import ImageAvatar, { ImageAvatarSize, PlaceholderSize } from '../image-avatar/image-avatar';
import { Contact } from '../../types/listing/contact';
import { generateInitials } from '../contact-card/util/generate-initials';


interface Props {
    contact: Contact
}

const CardBrokerView: FC<Props> = (props) => {

    const { contact } = props;

   return (
      <BaseCardBrokerArea>
            <ImageAvatar avatarSize={ImageAvatarSize.SMALL} placeholderSize={PlaceholderSize.SMALL} 
                allowUpload={false} imageURL={contact.avatar ? contact.avatar : ''} placeholder={generateInitials(contact)}/>
            {(contact.firstName? contact.firstName : '')}
            <br/>
            {(contact.lastName? contact.lastName : '')}
       </BaseCardBrokerArea>
   );
}
    
export default CardBrokerView;

const BaseCardBrokerArea  = styled.div`
    padding: 5px;
    display:inline-block;
    font-size:11px;
    color: #666666;
    flex:1;
    align-items:center;
`;