
import React, { FC, useState } from 'react'
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import Accordion, { AccordionButton } from '../../components/accordion/accordion';
import styled from 'styled-components';
import SectionHeading from '../../components/section-heading/section-heading';
import StyledButton from '../styled-button/styled-button';
import { v1 } from 'uuid';
import { PopOverItem, PopOverProps } from '../../components/popover/popover';
// icons in use
import ToggleIcon from '../../assets/images/png/accordion-toggle.png';
import DragIcon from '../../assets/images/png/accordion-drag.png';
import MenuIcon from '../../assets/images/png/accordion-menu.png';
import DeleteIcon from '../../assets/images/png/deleteIcon.png';
import DuplicateIcon from '../../assets/images/png/duplicate-icon.png';
import DuplicateOverIcon from '../../assets/images/png/duplicate-icon-over.png';
import _ from 'lodash';
// types
import { Option } from '../../types/common/option';

export interface DraggableAccordionProps {
    name: string,
    dataProvider: any[],
    inject: any,
    dataParams?: object,
    header: string,
    titleField: string,
    addLabel: string,
    useLabelForHeader: boolean,
    getLabelFromCode?: string,
    labelProvider?: Option[],
    manualError?: boolean,
    manualErrorMessage?: string,
    displayMIQId?: boolean,
    importFromMiq?: boolean,
    showSpacesModal: any,
    showMiqSpaces?: boolean,
    changeHandler?(values: any): void
}

const DraggableAccordion: FC<DraggableAccordionProps> = (props) => {

    const { name, inject, dataParams, header, titleField, addLabel, useLabelForHeader, getLabelFromCode, labelProvider, manualError, manualErrorMessage, displayMIQId, changeHandler, showMiqSpaces } = props;
    const { dataProvider } = props;
    const [items, setItems] = useState(dataProvider);

    const [currentMenuIndex, setCurrentMenuIndex] = useState<number>(-1);
    const [openPanes, setOpenPanes] = useState<object>({});

    const multiLanguage: boolean = (getLabelFromCode !== undefined && getLabelFromCode !== "any" && getLabelFromCode.length > 0);

    let currentItems: any[] = items; // this is important for maintaining data integrity while dragging (basically, a second copy we use to capture updates)

    const UIDPrefix: string = "acc";  // header prefix so we can reference an accordion and set pane without complete re-render

    const $inject = inject;
    const validations = {};

    const assignPaneId = (open: boolean): string => {
        const randomId: string = v1();
        if (openPanes) {
            openPanes[randomId] = open;
        }
        return randomId;
    }

    // START PANE MANAGEMENT
    const setupPaneIds = () => {
        const ids: string[] = [];
        if (dataProvider && dataProvider.length > 0) {
            dataProvider.forEach((data: any, index: number) => {
                ids.push(assignPaneId(index === 0)); // generate some unique pane ids and set the first pane to open by default
            });
        }
        return ids;
    }
    const [paneIds, setPaneIds] = useState<string[]>(setupPaneIds());

    const togglePane = (paneId: string, open: boolean, updateItems: boolean = true) => {
        if (openPanes) {
            const paneChangeObj: object = Object.assign({}, openPanes);
            paneChangeObj[paneId] = open;
            setOpenPanes(paneChangeObj);
        }
        if (updateItems) {
            setItems(currentItems);
        }

    }

    const isPaneOpen = (index: number): boolean => {
        // helper method to return whether a pane is open
        if (paneIds[index] && openPanes[paneIds[index]]) {
            return openPanes[paneIds[index]];
        }
        return false;
    }

    // END PANE MANAGEMENT

    // add item
    const addItem = () => {
        const itemsArr: any[] = [...currentItems];
        itemsArr.push({});
        setItems(itemsArr);
        // ensure the new pane is opened
        const ids: string[] = [...paneIds];
        ids.push(assignPaneId(true));
        setPaneIds(ids);
        // bubble up
        triggerChange(itemsArr);
    }

    const closeMenu = () => {
        setCurrentMenuIndex(-1);
    }

    // delete
    const deleteItem = (index: number) => {
        const itemsArr: any[] = [...currentItems];
        itemsArr.splice(index, 1);
        setItems(itemsArr);
        // delete from the pane ids too
        paneIds.splice(index, 1);
        setPaneIds(paneIds);
        triggerChange(itemsArr);
        closeMenu();
    }

    // reorder array
    const reorder = (list: any, startIndex: number, endIndex: number) => {
        const result: any = list;
        const cutOut: any = result.splice(startIndex, 1)[0];
        result.splice(endIndex, 0, cutOut);
        return result;
    };

    const beforeDragStart = () => {
        // ensure focus shifts
        const el: any = document.querySelector(':focus');
        if (el) {
            el.blur();
        }
        setItems(currentItems);
    }

    // end drag (re-order array)
    const onDragEnd = (result: any) => {

        if (!result.destination) { return }
        if (result.destination.index === result.source.index) { return }

        const currentIndex: number = parseInt(result.draggableId, 10);
        const destinationIndex: number = result.destination.index;

        let itemsArr = [...currentItems];
        itemsArr = reorder(itemsArr, currentIndex, destinationIndex);
        setItems(itemsArr);

        // since we changed the position of one of the panes, we need the id for that pane to be reordered too so we can keep track of open panes properly using index
        let reorderedPaneIds = [...paneIds];
        reorderedPaneIds = reorder(reorderedPaneIds, currentIndex, destinationIndex);
        setPaneIds(reorderedPaneIds);

        triggerChange(itemsArr);
    }

    const duplicateItem = (index: number) => {
        const itemsArr = [...currentItems];
        const itemToCopy = itemsArr[index];

        if (itemToCopy) {
            const copiedItem = _.cloneDeep(itemToCopy);
            copiedItem.id = null;   // null out id (note: assuming id, may need to make this configurable)

            // logic to append (Copy) to the title field if it's not already there
            let title: string = "";
            let titleArr = [];

            if (!useLabelForHeader) {     // we won't worry about adding Copy to these types of spaces because they pull from a value and can't be changed
                if (multiLanguage) {
                    if (copiedItem[titleField] && copiedItem[titleField].length > 0) {
                        // if we have a code to get the value from, then find it within the value object
                        titleArr = copiedItem[titleField].filter((d: any) => {
                            return d.cultureCode === getLabelFromCode;
                        });
                        titleArr && titleArr.length > 0 && titleArr[0].text ? title = titleArr[0].text : title = "";
                    }
                } else {
                    // only one language, no tabbed experience in the spaces
                    title = copiedItem[titleField];
                }

                // now that we have a title, see if we need to append "(Copy)"
                if (title && title.length > 0) {
                    const copyText: string = "(Copy)";
                    const last4: string = title.substr(title.length - copyText.length);
                    if (copyText !== last4) {
                        title += " " + copyText;
                    }
                }

                // now, put it back where it needs to go
                if (multiLanguage) {
                    if (titleArr && titleArr.length > 0) {
                        titleArr[0].text = title;
                    }
                } else {
                    copiedItem[titleField] = title;
                }
            }

            // now add the copiedItem to the itemsArr so that we'll get new pane and space to work with
            itemsArr.push(copiedItem);
            setItems(itemsArr);
            // create a paneid we'll use and open the new item
            const ids: string[] = [...paneIds];
            ids.push(assignPaneId(true));
            // check to see if the pane id of the current item we are duplicating from is open
            // if so, we want to close it because we are opening the new one (request by business)
            if (paneIds[index]) {
                togglePane(paneIds[index], false, false);
            }
            setPaneIds(ids);
            // update the pane header
            updatePaneHeader(itemToCopy, index);
            // trigger the change and close menu
            triggerChange(itemsArr);
            closeMenu();
        }
    }

    const updatePaneHeader = (values: any, index: number) => {

        if (paneIds && paneIds[index]) {
            // get the title element based on index
            const titleEle = document.getElementById(UIDPrefix + paneIds[index]);
            // if it exists, find the correct pane header and change the element
            if (titleEle) {
                titleEle.textContent = getPaneHeader(values, titleField);
            }
        }
    }

    // handle an item update
    const updateItem = (values: any, index: number) => {
        const itemsArr = [...currentItems];
        itemsArr[index] = values;
        // update the accordion header (title) using the pane id (from index)
        updatePaneHeader(values, index);
        // replace the current items
        currentItems = [...itemsArr];
        triggerChange(itemsArr);
    }

    // common change function
    const triggerChange = (itemsArr: any) => {
        if (changeHandler !== undefined) {
            changeHandler(itemsArr);
        }
    }

    // convenience function to find any field level data from an array item
    const getPaneHeader = (item: any, propertyName: string) => {

        let selectedValue: string = "";
        if (useLabelForHeader && labelProvider) {
            // the value we need is in a labelProvider (i.e. combo box/property type)
            // use the value to find the label to apply instead (i.e, Singapore Flex Property Type)
            const selectedName: any = item[propertyName];
            let valueChosen: string = "";

            if (typeof selectedName === 'string') {
                valueChosen = selectedName;
            }
            else {
                if (selectedName && selectedName.length > 0) {
                    valueChosen = _.values(selectedName[0])[1];
                }
            }

            labelProvider.forEach((opt: Option) => {
                if (opt.value === valueChosen && opt.label) {
                    selectedValue = opt.label;
                }
            });
        } else {
            // we pull the value from something entered by the user
            if (multiLanguage) {
                // multi-language pulls from the title array object
                if (item[titleField] && item[titleField].length > 0) {
                    // if we have a code to get the value from, then find it within the value object
                    const titleArr = item[titleField].filter((d: any) => {
                        return d.cultureCode === getLabelFromCode;
                    });
                    titleArr && titleArr.length > 0 && titleArr[0].text ? selectedValue = titleArr[0].text : selectedValue = "";
                }
            } else {
                selectedValue = item[propertyName];
            }
        }
        // return the the selectedValue if not undefined, otherwise return a blank string
        return selectedValue ? selectedValue : "";
    }

    const toggleMenu = (index: number) => {
        const itemsArr = [...currentItems];
        setItems(itemsArr);
        updatePaneHeader(currentItems[index], index);
        setCurrentMenuIndex(currentMenuIndex === index ? -1 : index);
    }

    const getExtraButtons = (idx: number): AccordionButton[] => {

        const opts: PopOverProps = {
            index: idx,
            handlePopUpAction: false,
            options: getPopOverOptions(),
            popoverEnabled: currentMenuIndex === idx ? true : false,
            borderColor: '#CCCCCC'
        }
        return [
            { id: "menu", icon: MenuIcon, allowClick: true, allowFocus: false, itemIndex: idx, clickHandler: toggleMenu, hasMenu: true, popOverMenuOptions: opts },
            { id: "drag", icon: DragIcon, allowClick: false, allowFocus: false }
        ];
    }

    const getAccKey = (item: any, index: number) => {
        let extraKey: string = new Date().getTime().toString();
        if (item.id) {
            extraKey = item.id;
        }
        return "acc_" + index + "_" + extraKey;
    }

    const getPopOverOptions = () => {
        const options: PopOverItem[] = [];
        options.push({ icon: DuplicateIcon, overIcon: DuplicateOverIcon, name: 'Duplicate', clickHandler: duplicateItem });
        options.push({ icon: DeleteIcon, name: 'Delete', clickHandler: deleteItem });
        return options;
    }

    const generateAccordions = () => {
        return items.map((item: any, idx: number) => (
            <div className="space-pane">
                <Draggable draggableId={String(idx)} index={idx} key={"draggableAcc" + idx} isDragDisabled={false}>
                    {provided => (
                        <AccordionContainer ref={provided.innerRef} {...provided.draggableProps} {...provided.dragHandleProps}>
                            <Accordion paneId={paneIds[idx]} titlePrefix={String(idx + 1)} UIDPrefix={UIDPrefix} miqId={displayMIQId && item.miqId ? item.miqId : null}
                                key={getAccKey(item, idx)} collapseHandler={togglePane} title={getPaneHeader(item, titleField)}
                                open={isPaneOpen(idx)} alertOn={false} toggleIcon={ToggleIcon} buttons={getExtraButtons(idx)}>
                                <$inject data={Object.assign({}, item)} dataParams={dataParams} idx={idx} validations={validations} handleChange={updateItem} />
                            </Accordion>
                        </AccordionContainer>
                    )}
                </Draggable>
            </div>
        ));
    }

    return (
        <DraggableAccordionContainer>
            <DraggableAccordionHeader>
                <SectionHeading error={manualError}>{header}</SectionHeading>
                <div>
                    <StyledButton name={name + "_dgButton1"} onClick={addItem} styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp;{addLabel}</StyledButton>
                    {showMiqSpaces &&
                        <StyledButton name={name + "_dgButton2"} style={{ marginLeft: '15px' }} onClick={() => { props.showSpacesModal() }} styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp;Import from MIQ</StyledButton>
                    }
                </div>
            </DraggableAccordionHeader>
            {manualError && <StyledError>{manualErrorMessage}</StyledError>}
            <DragDropContext onDragEnd={onDragEnd} onBeforeDragStart={beforeDragStart}>
                <Droppable droppableId="draggableAccordion">
                    {provided => (
                        <div className="accordionItem" ref={provided.innerRef} {...provided.droppableProps}>
                            {items && items[0] && generateAccordions()}
                            {provided.placeholder}
                        </div>
                    )}
                </Droppable>
            </DragDropContext>
            {items && items[0] &&
                <StyledButton id="bottomSpaceButton" name={name + "_dgButton2"} onClick={addItem} styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp;{addLabel}</StyledButton>}
        </DraggableAccordionContainer>
    );
}

const DraggableAccordionContainer = styled.div`
    margin:30px 0;
    .accordionItem > div {
        margin-top:30px;
        outline:0;
    }
    > div:first-of-type {margin-bottom:-20px;}
    #bottomSpaceButton {margin-top:25px;}
`;

const AccordionContainer = styled.div`
    display:flex;
    justify-content:space-between;
    verticle-align:top;
    position:relative;
`;

const DraggableAccordionHeader = styled.div`
    span {
        width:auto;
        flex-grow:inherit;
        position:relative;
        left:8px;
    }
    display:flex;
    justify-content:space-between;
    align-items:center;
`;

const StyledError = styled.div`
    width: 100%;
    color: ${props => props.theme.colors ? props.theme.colors.error : 'red'};
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
    margin: 20px 0 10px 0;
`;

export default DraggableAccordion;