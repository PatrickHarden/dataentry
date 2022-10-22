import React, { FC, useState, useEffect } from 'react';
import styled from 'styled-components';

export interface PopOverItem {
    icon: any,
    overIcon?: any,
    name: string,
    clickHandler: Function
}

export interface PopOverLocation {
    top?: number,
    left?: number
}

export interface PopOverProps {
    index?: number,
    handlePopUpAction?: boolean,
    inactive?: boolean,
    updatePopover?: any,
    popoverEnabled?: boolean,
    options?: PopOverItem[],
    borderColor?: string,
    location?: PopOverLocation
}

type Props = PopOverProps & PopOverLocation; 

const PopOver: FC<Props> = (props) => {

    // isActive determines whether the popover menu is visible
    const [isActive, setIsActive] = useState(props.popoverEnabled);
    // current over if set
    const [mouseOver, setMouseOver] = useState<any | undefined>(undefined);
    
    const { index, handlePopUpAction, options } = props;

    const togglePopover = () => {
        props.updatePopover(props.index)
    }

    const selectPopOverOption = (option:PopOverItem) => {
        option.clickHandler(index !== undefined && index > -1 ? index : option.name);
        if(handlePopUpAction){
            togglePopover();
        }
    }

    const setOver = (option:PopOverItem, over:boolean) => {
        if(option.overIcon){
            if(over){
                setMouseOver(option.overIcon);
            }else{
                setMouseOver(undefined);
            }
        }
    }
    
    useEffect(() => {
        if (isActive !== props.popoverEnabled){
            setIsActive(props.popoverEnabled)
        }
    }, [props.popoverEnabled]);

    return (
        <PopoverWrapper {...props}>
            { handlePopUpAction && <PopoverToggle onClick={togglePopover}><ThreeDots> ... </ThreeDots></PopoverToggle> }
            {isActive && 
                <PopOverMenu>
                    { options && options.map((option:PopOverItem, idx:number) => {
                        return <PopOverMenuItem {...option} onMouseOver={() => setOver(option,true)} onMouseOut={() => setOver(option,false)} onClick={() => {selectPopOverOption(option);}} key={"popOverOption"+idx}>
                            <img src={mouseOver && mouseOver === option.overIcon ? mouseOver : option.icon} />
                            {(!props.inactive && (option.name === 'Inactive')) ? 'Active' : option.name}
                        </PopOverMenuItem>
                    })}
                </PopOverMenu> 
            } 
        </PopoverWrapper>
    )
}

const PopoverWrapper = styled.div`
    display:flex;
    width:auto;
    font-weight: ${props => (props.theme.font ? props.theme.font.weight.normal : 'normal')};
    position:absolute;
    top: ${(props:PopOverProps) => (props.location ? props.location.top + 'px' : '')};
    left: ${(props:PopOverProps) => (props.location ? props.location.left + 'px' : '')};
    z-index: ${(props:PopOverProps) => (!props.handlePopUpAction ? '1000' : '')};
`;

const PopoverToggle = styled.span`
    transform: rotate(-90deg);
    color: ${props => (props.theme.colors ? props.theme.colors.tertiaryAccent : 'deepskyblue')};
    font-weight: ${props => (props.theme.font ? props.theme.font.weight.bolder : 'bolder')};
    background:#fff;
    height:21px;
    width:24px;
    border-radius:50%;
    padding-bottom:3px;
    text-align:center;
    font-size:14px;
    letter-spacing:1px;
    cursor:pointer;
`;

const ThreeDots = styled.span`
    position:relative; 
    top:0; 
    left:0; 
`;

const PopOverMenu = styled.div`
    background:#fff;
    color: ${props => (props.theme.colors ? props.theme.colors.tertiaryAccent : 'deepskyblue')};
    border: 0.5px solid #CCCCCC;
    box-sizing: border-box;
    box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.15);
    min-width: 185px;
`;

const PopOverMenuItem = styled.p`
    cursor:pointer;
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
    font-size: 14px;
    line-height: 28px;
    position:relative;
    transition:.2s ease all;
    text-align: left;
    padding: 5px 15px 5px 5px;
    img {
        max-height:20px;
        max-width:20px;
        width:17px;
        margin-right:20px;
        margin-left:10px;
        position:relative;
        top:5px;
    }
    :hover {
        background: #00B2DD;
        color: #FFF;
        ${(props:PopOverItem) => (!props.overIcon ? 'img { -webkit-filter: brightness(0) invert(1); filter: brightness(0) invert(1);}' : '')};  
    }
`;

export default React.memo(PopOver);