import React, { FC } from 'react';
import styled from 'styled-components';
import { Button } from '../../types/components/button';

export interface IconButtonProps {
    uniqueId: string,
    button: Button;
}

const IconButton: FC<IconButtonProps> = (props) => {

    const { uniqueId, button } = props;

    const onKeyPress = (e:React.KeyboardEvent) => {
        const keyCode = e.charCode || e.which;
        if(keyCode === 13 || keyCode === 32){ // enter or spacebar
            e.preventDefault();
            const ele = document.getElementById(uniqueId + "_" + button.id);
            if(ele){
                ele.click();
            }
        }
    }

    const onClick = (e:any) => {
        if(button.clickHandler && button.itemIndex !== undefined){
            button.clickHandler(button.itemIndex, e);
        }
    }

    const ClickableButton = () => {
        return <StyledIconButton id={uniqueId + "_" + button.id} data-testid="styled-icon-button" onClick={onClick} onKeyPress={onKeyPress}><img data-testid="styled-icon-button-img" src={button.icon}/></StyledIconButton>;
    }

    const ImageOnly = () => {
        return <StyledImage data-testid="icon-button-styled-image"><img src={button.icon} data-testid="icon-button-img" /></StyledImage>
    }

    return (
        <>
            { button.allowClick && <ClickableButton/> }
            { !button.allowClick && <ImageOnly/> }
        </>
    );
};

const StyledIconButton = styled.button`
    border: none;
    background: inherit;
    outline: none;
    > img {
        cursor: pointer;
    };
    :focus {
        outline: 1px solid ${props => props.theme.colors ? props.theme.colors.inputFocus : '#29BC9C'};   
    }
    display: inline-block;
`;

const StyledImage = styled.div` 
    background: inherit;
    display: inline-block;
    > img {
        cursor: pointer;
    };
`;

export default IconButton;