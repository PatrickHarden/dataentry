import React, { FC, useState } from 'react';
import styled, { css } from 'styled-components';
import { Button } from '../../types/components/button';
import IconButton from '../icon-button/icon-button';
import PopOver, { PopOverProps } from '../popover/popover';

interface StateProps {
    collapsed: boolean;
}

export interface AccordionButton extends Button {
    hasMenu?: boolean,
    popOverMenuOptions?: PopOverProps,
    flip?: boolean
}

export interface AccordionProps {
    paneId: string,
    title: string,
    open?: boolean,
    alertOn?: boolean,
    titlePrefix?: string,
    UIDPrefix?: string,
    buttons?: AccordionButton[],
    menu?: AccordionMenu,
    toggleIcon?: any,
    miqId?: any,
    collapseHandler(paneId: string, open: boolean): void
}

export interface AccordionMenu {
    show: boolean,
    buttons: Button[]
}

const Accordion: FC<AccordionProps> = ({
    paneId,
    title,
    open,
    alertOn,
    titlePrefix,
    UIDPrefix,
    buttons,
    toggleIcon,
    collapseHandler,
    miqId,
    ...props
}) => {

    let prefix = null;

    if (titlePrefix) {
        prefix = <PaneHeaderPrefix>{titlePrefix}</PaneHeaderPrefix>;
    }

    let alert = null;
    if (alertOn) {
        alert = <Alert>!</Alert>
    }

    const toggleCollapse = () => {
        collapseHandler(paneId, !open);
    }

    const toggleBtnSetup: AccordionButton = {
        id: "toggle",
        icon: toggleIcon,
        allowClick: true,
        itemIndex: -1,
        flip: open ? true : false,
        clickHandler: toggleCollapse
    }

    const toggleButton: AccordionButton | undefined = toggleIcon ? toggleBtnSetup : undefined;

    return (
        <AccContainer>
            <PaneHeaderContainer>
                <PaneHeaderPrefixContainer>
                    {prefix}
                </PaneHeaderPrefixContainer>
                <PaneTitleText id={UIDPrefix + paneId}>
                    {title}
                    {miqId && miqId !== null && 
                        <AppendedTitle>
                            <span>MIQ ID:</span>
                            {miqId}
                        </AppendedTitle>
                    }
                </PaneTitleText>
                {alert}
                <PaneButtonsContainer>
                    <IconButtonsContainer>
                        {buttons && buttons.map((button: AccordionButton) => {
                            return (
                                <>
                                    <IconButtonWrapper {...button}>
                                        <IconButton key={"k" + paneId + "_" + button.id} button={button} uniqueId={paneId} />
                                        {button.hasMenu && <PopOver {...button.popOverMenuOptions} />}
                                    </IconButtonWrapper>
                                    <IconButtonWrapper {...button}><VerticalLine /></IconButtonWrapper>
                                </>
                            );
                        })
                        }
                        {toggleButton && <IconButtonWrapper {...toggleButton}>
                            <IconButton key={"k" + paneId + "_" + toggleButton.id} button={toggleButton} uniqueId={paneId} />
                        </IconButtonWrapper>}
                    </IconButtonsContainer>
                </PaneButtonsContainer>
            </PaneHeaderContainer>
            <BaseDetails collapsed={!open}> {props.children} </BaseDetails>
        </AccContainer>
    )
};

const btnLookToggle = css` 

${(props: StateProps) => props.collapsed ? "scale(-1,1.8)" : "scale(1,1.8)"};
`

const AppendedTitle = styled.span`
    float:right;
    margin-right:10px;
    font-size: 14px;
    > span {
        color: #999 !important;
        margin-right:6px;
    }
`

const AccContainer = styled.div`
    border: 1px solid #dfdfdf;
    border-radius: 4px;
    overflow:hidden;
    width: 100%;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')};
`;

const PaneHeaderContainer = styled.div`
    background-color: #f1f1f1;
    width:100%;
    font-size: 18px;
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
    color: #666666;
    height: 50px;
    display: flex;
`;

const PaneHeaderPrefixContainer = styled.div`
    height: 50px;
    display: inline-flex;
    vertical-align: top;
    width: 7%;
`;


const PaneHeaderPrefix = styled.div`
    width:50px;
    background-color: #00B2DD;
    line-height: 50px;
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
    text-align: center;
    color:white;
`;

const PaneTitleText = styled.div`
    display: inline-flex;
    line-height: 50px;
`;

const PaneButtonsContainer = styled.div`
    display: inline-flex;
    vertical-align: top;
    margin-left: auto;
`;

const IconButtonsContainer = styled.div`
    display: table;
    width: auto;
    margin-left: auto;
    height: 50px;
`;

const IconButtonWrapper = styled.div`
    display: table-cell;
    text-align: center;
    vertical-align: middle;
    margin-left: 5px;
    margin-right: 5px;
    img {
        -webkit-transform: ${(props: AccordionButton) => (props.flip ? 'scaleY(-1);' : '')};
        transform: ${(props: AccordionButton) => (props.flip ? 'scaleY(-1);' : '')};
    }
`;

const Alert = styled.div`
  background-color: #cf8a90;
  border-radius: 50%;
  color: #fff;
  display: inline-block;
  line-height: 18px;
  margin-left: 8px;
  text-align: center;
  width: 18px;
  font-size:14px;
`;

const BaseDetails = styled.div`
    background-color: #fdfdfd;
    max-height: ${(props: StateProps) => !props.collapsed ? "100%" : "0"};
    overflow: hidden;
    padding: ${(props: StateProps) => !props.collapsed ? "2em " : "0 2em"};
    transition: all 0.3s ease-out;
`;

const VerticalLine = styled.div`
    height: 30px;
    border: 1px solid #CCCCCC;
`;

export default Accordion;