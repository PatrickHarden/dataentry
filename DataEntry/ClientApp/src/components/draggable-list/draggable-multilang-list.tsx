import React, { useState, FC, useMemo, useCallback } from "react";
import styled from 'styled-components';
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import FormInput from '../form-input/form-input';
import StyledButton from '../styled-button/styled-button';
// images
import DeleteIcon from '../../assets/images/png/deleteIcon.png';
import DragIcon from '../../assets/images/png/dragIcon.png';
import { MultiLangString } from '../../types/listing/multi-lang-string';
import FormTabbedTextArea, { FormTabbedTextAreaProps } from '../form-tabbed-text-area/form-tabbed-text-area';
import { useSelector } from 'react-redux';
import { configSelector } from '../../redux/selectors/system/config-selector';
import { Config, FieldConfig, AvailableFromConfig, StatusConfig } from '../../types/config/config';
import { generateKey } from "../../utils/keys";

interface DraggableMultiLangListItem {
    order: number;
    value: MultiLangString[];
}

export interface DraggableMultiLangListProps {
    items: DraggableMultiLangListItem[],
    isDragDisabled?: boolean,
    changeHandler(value: any): void
}

const DraggableMultiLangList: FC<DraggableMultiLangListProps> = (props) => {

    let theProps = props.items;
    if (!theProps) {
        theProps = [];
    }
    const [items, setItems] = useState(theProps);
    const [nextFocus, setNextFocus] = useState(-1);

    const addButtonName: string = "addHighlights"; // todo: make dynamic along with button label as well ("Add Highlight at bottom")
    const config: Config = useSelector(configSelector);

    let UIType: string = "singlelang";
    if (config && config.languages) {
        UIType = "multilang";
    }

    // const [, updateState] = useState();
    // const forceUpdate = useCallback(() => updateState({}), []);

    const initial = Array.from({ length: items.length }, (v, index) => index).map(index => {
        let custom:any = {};
        let cultureCodeString = "en-US";
        if (config && config.defaultCultureCode) {
            cultureCodeString = config.defaultCultureCode;
        }
        if (items[index] && items[index].value && items[index].value.length > 0) {
            const getLangHighlight = items[index].value.filter((h: any) => {
                return h.cultureCode === cultureCodeString;
            });
            if (getLangHighlight.length > 0) {
                custom = {...getLangHighlight[0]};
            }
        }
        
        custom.id = `${index}`;
        custom.value = `${custom.text || ""}`;
        custom.cultureCode = `${custom.cultureCode || cultureCodeString}`;

        return custom;
    });

    const [state, setState] = useState({ highlights: initial });

    const [stateMultiLang, setStateMultiLang] = useState({ highlightsMultiLang: props.items });

    const reorder = (list: any, startIndex: number, endIndex: number) => {
        const result = Array.from(list);
        const [removed] = result.splice(startIndex, 1);
        result.splice(endIndex, 0, removed);
        return result;
    };

    const onDragStart = () => {
        if (document && document.activeElement) {
            (document.activeElement as HTMLElement).blur();
        }
    }

    // function executed when user drops an item - used to update state with new order.
    const onDragEnd = (result: any) => {

        if (!result.destination) {
            return;
        }
        if (result.destination.index === result.source.index) {
            return;
        }
        if (UIType === "multilang") {
            const mhighlights: any[] = reorder(
                stateMultiLang.highlightsMultiLang,
                result.source.index,
                result.destination.index
            );
            const multiLang = { highlightsMultiLang: mhighlights }
            setStateMultiLang(multiLang);
            handleMultiLangChange(mhighlights);
        }
        else {
            const highlights = reorder(
                state.highlights,
                result.source.index,
                result.destination.index
            );
            setState({ highlights });
            handleChange(highlights);
        }

    }

    const deleteItem = (index: number) => {
        if (document && document.activeElement) {
            (document.activeElement as HTMLElement).blur();
        }
        if (UIType === "multilang") {
            const currentArr: any = [...stateMultiLang.highlightsMultiLang];
            currentArr.splice(index, 1);
            const filtered = currentArr.filter((filteredItem: any) => {
                return filteredItem != null;
            });

            const multiLang = { highlightsMultiLang: filtered }
            setStateMultiLang(multiLang);
            handleMultiLangChange(multiLang.highlightsMultiLang);
            setItems(filtered);
            props.changeHandler(filtered);
        } else {
            const temp: any = [...state.highlights];
            temp.splice(index, 1);
            const filtered = temp.filter((el: any) => {
                return el != null;
            });
            setState({ highlights: filtered })
            const tempItems: any = filtered.map((highlight: any, i: any) => {
                // return highlight.value
                const val: any = {
                    order: i,
                    value: [{...highlight, text: `${highlight.value ? highlight.value : ""}`}]
                }
                return val;
            })
            setItems(tempItems)
            // handleChange(state.highlights);
            props.changeHandler(tempItems);
        }
    }


    let typingTimer: any;
    const updateValue = (index: number, value: any) => {
        clearTimeout(typingTimer)

        const tempState: any = state;
        if (tempState.highlights[index]) {
            tempState.highlights[index].value = value;
            tempState.highlights[index].cultureCode = "en-US";
            if (config && config.defaultCultureCode) {
                tempState.highlights[index].cultureCode = config.defaultCultureCode;
            }
        }

        typingTimer = setTimeout(() => {
            handleChange(tempState.highlights);
        }, 400)
    }

    const updateMultiLangHighlightPayload = (index: number, highlightPayload: any) => {
        const tempState: any = stateMultiLang;
        if (tempState) {
            if (tempState.highlightsMultiLang[index]) {
                tempState.highlightsMultiLang[index].value = highlightPayload;
            }
        }
        handleMultiLangChange(tempState.highlightsMultiLang);
    }

    const addItem = () => {
        const id: string = String(Date.now());
        const newItem: any = {
            id: `${id}`,
            order: `${items.length}`,
            value: []
        };

        if (UIType === "multilang") {
            // dont construct the multi arr
            // ensure the items has the multi arr values
            // const newItems = stateMultiLang && stateMultiLang.highlightsMultiLang ? [...stateMultiLang.highlightsMultiLang] : [];
            // newItems.push(newItem);
            // setStateMultiLang({ highlightsMultiLang: newItems });
            // props.changeHandler(newItems);
            const temp: any = items;
            const result: any = {
                order: items.length,
                value: [
                    { cultureCode: 'nl-NL', text: '' },
                    { cultureCode: 'en-NL', text: '' }
                ]
            };
            temp.push(result);
            setItems(temp);
            props.changeHandler(temp);
            // forceUpdate();
        }
        else {
            items.push(newItem);
            const results = state.highlights;
            results.push({ id, value: [] });
            setNextFocus(results.length - 1);
            setState({ highlights: results });
            props.changeHandler(results);
        }
    }


    const handleMultiLangChange = (changedValues: any) => {
        // remove [empty] values if object is deleted from array
        const filtered: any = changedValues.filter((el: any) => {
            return el != null;
        });

        // // before we bubble data up, get it back into the form that it was sent in (array of DraggableMultiLangListItems)
        // // value: `${changedValues[index].value ? changedValues[index].value : ""}`
        let valueArray: any = []
        if (filtered.length > 0) {
            valueArray = Array.from({ length: changedValues.length }, (v, index) => index).map((index: any) => {
                if (changedValues[index] && changedValues[index].value.length > 0) {
                    const val: DraggableMultiLangListItem = {
                        order: index,
                        value: changedValues[index].value
                    }
                    return val;
                } else {
                    return null;
                }
            });
        }

        const filteredValues: any = valueArray.filter((el: any) => {
            return el != null;
        });

        props.changeHandler(filteredValues);
    }

    const handleChange = (changedValues: any) => {

        // remove [empty] values if object is deleted from array
        const filtered: any = changedValues.filter((el: any) => {
            return el != null;
        });

        // before we bubble data up, get it back into the form that it was sent in (array of DraggableMultiLangListItems)
        // value: `${changedValues[index].value ? changedValues[index].value : ""}`
        let valueArray: any = []
        if (filtered.length > 0) {
            valueArray = Array.from({ length: changedValues.length }, (v, index) => index).map((index: any) => {
                if (changedValues[index] && changedValues[index].value !== "") {
                    const multiLangData: MultiLangString[] = [{...changedValues[index], text: `${changedValues[index].value ? changedValues[index].value : ""}`}];
                    const val: DraggableMultiLangListItem = {
                        order: index,
                        value: multiLangData
                    }
                    return val;
                } else {
                    return null;
                }
            });
        }

        const filteredValues: any = valueArray.filter((el: any) => {
            return el != null;
        });


        props.changeHandler(filteredValues);
    }



    const getTextFieldLangSettings = (placeholderName: string) => {
        let langSettings: any = [];
        if (config && config.languages) {
            let translations: any[] = [];
            if (config.translations) {
                translations = Array.from(config.translations);
            }

            const langs: string[] = Array.from(config.languages);
            langSettings = Array.from({ length: langs.length }, (v, idx) => idx).map(idx => {
                let languageName = langs[idx];
                let placeholder = "Enter Text...";
                if (translations.length > 0) {
                    const itemData = translations.filter((d: any) => {
                        return d.cultureCode === langs[idx];
                    });
                    if (itemData.length > 0) {
                        languageName = itemData[0].languageName;
                        placeholder = itemData[0][placeholderName];
                    }
                }

                const settings: any = {
                    cultureCode: `${langs[idx]}`,
                    lang: `${languageName}`,
                    placeholder: `${placeholder}`
                };
                return settings;
            });
        }

        return langSettings;
    }

    const HighLightMemo = useMemo(() =>
        state.highlights.map((highlight: any, index: number) => (
            <Draggable draggableId={highlight.id} index={index} key={highlight.id + highlight.value + index} isDragDisabled={props.isDragDisabled}>
                {provided => ( // tslint:disable-line
                    <Highlight
                        ref={provided.innerRef}
                        {...provided.draggableProps}
                        {...provided.dragHandleProps}>
                        <FormInput name={(highlight.value === ' ') ? index : highlight.value} index={index} useOnChange={true} indexedBlurHandler={updateValue} defaultValue={highlight.value} forceFocus={nextFocus === index} />

                        <img src={DeleteIcon} onClick={() => deleteItem(index)} />
                        <img src={DragIcon} />
                    </Highlight>
                )}
            </Draggable>
        )), [state]);

    const HighLightMultiLangMemo = useMemo(() =>
        items.map((highlight: any, index: number) => (
            highlight &&
            <Draggable draggableId={String(highlight.order)} index={index} key={generateKey()} isDragDisabled={true}>
                {provided => ( // tslint:disable-line
                    <Highlight
                        ref={provided.innerRef}
                        {...provided.draggableProps}
                        {...provided.dragHandleProps}>
                        <FormTabbedTextArea data={highlight.value} index={index} tabsettings={getTextFieldLangSettings("plBuildingDescription")} updateIndexedTabData={updateMultiLangHighlightPayload} />
                        <DeleteIconButton src={DeleteIcon} onClick={() => deleteItem(index)} />
                        {/* <img src={DragIcon} /> */}
                    </Highlight>
                )}
            </Draggable>
        )), [props]);

    const hightlightsUI = () => {
        if (UIType === "multilang") {
            return (
                <>{HighLightMultiLangMemo}</>
            );
        } else {
            return (
                <>{HighLightMemo}</>
            )
        }
    }

    return (
        <div>
            <DragDropContext onDragStart={onDragStart} onDragEnd={onDragEnd}>
                <Droppable droppableId="theDraggableList">
                    {provided => ( // tslint:disable-line
                        <div ref={provided.innerRef} {...provided.droppableProps}>
                            {hightlightsUI()}
                            {provided.placeholder}
                        </div>
                    )}
                </Droppable>
            </DragDropContext>
            <StyledButton name={addButtonName} onClick={addItem} style={{ marginTop: '25px' }} styledSpan={true} buttonStyle="2">
                <span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp; {(config && config.highlight && config.highlight.addButtonLabel) ? config.highlight.addButtonLabel : "Add Highlight"}
            </StyledButton>
        </div>
    );
}

const Highlight = styled.div`
  width: 100%;
  max-width:700px;
  display:flex;
  margin-bottom: 4px;
  background-color: transparent;
  padding: 8px;
  align-items:center;
  border:0;
  outline:0;
  > div {
      max-width:600px
  }
  > img {
      max-height: 25px;
      margin-left:25px;
      margin-top:15px;
      background:transparent;
  }
`;

const DeleteIconButton = styled.img`
  cursor: pointer;
`

export default DraggableMultiLangList;
