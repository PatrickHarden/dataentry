import React, { useState, FC , useMemo} from "react";
import styled from 'styled-components';
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import FormInput from '../form-input/form-input';
import StyledButton from '../styled-button/styled-button';
// images
import DeleteIcon from '../../assets/images/png/deleteIcon.png';
import DragIcon from '../../assets/images/png/dragIcon.png';

interface DraggableListItem {
    order: number;
    value: string;
}

export interface DraggableListProps {
    items: DraggableListItem[],
    isDragDisabled?: boolean,
    changeHandler(value: any): void
}

const DraggableList: FC<DraggableListProps> = (props) => {
    let theProps = props.items;
    if (!theProps) {
        theProps = [];
    }
    const [items, setItems] = useState(theProps);
    const [nextFocus, setNextFocus] = useState(-1);

    const addButtonName: string = "addHighlights"; // todo: make dynamic along with button label as well ("Add Highlight at bottom")

    // this takes the data passed into the component and transforms it into the component will consume
    const initial = Array.from({ length: items.length }, (v, index) => index).map(index => {
        const custom: any = {
            id: `${index}`,
            value: `${items[index].value ? items[index].value : ""}`
        };
        return custom;
    });

    const [state, setState] = useState({ highlights: initial });

    const reorder = (list: any, startIndex: number, endIndex: number) => {
        const result = Array.from(list);
        const [removed] = result.splice(startIndex, 1);
        result.splice(endIndex, 0, removed);
        return result;
    };

    // function executed when user drops an item - used to update state with new order.
    const onDragEnd = (result: any) => {
        if (!result.destination) {
            return;
        }
        if (result.destination.index === result.source.index) {
            return;
        }
        const highlights = reorder(
            state.highlights,
            result.source.index,
            result.destination.index
        );
        setState({ highlights });
        handleChange(highlights);
    }

    const deleteItem = (index: number) => {
        const temp: any = state.highlights;
        for (let i = 0; i < temp.length; i++) {
            if (index === temp[i].id) {
                delete temp[i]
            }
        }
        const filtered = temp.filter((el: any) => {
            return el != null;
        });
        setState({ highlights: filtered })
        const tempItems: any = filtered.map((highlight: any, i: any) => {
            return highlight.value
        })
        setItems(tempItems)
        handleChange(state.highlights);
    }

    let typingTimer: any;
    const updateValue = (index: number, value: any) => {
        clearTimeout(typingTimer)

        const tempState: any = state;
        if (tempState.highlights[index]) {
            tempState.highlights[index].value = value;
        }

        typingTimer = setTimeout(() => {
            handleChange(tempState.highlights);
        }, 400)
    }

    const addItem = () => {
        const temp: any = items;
        const custom: any = {
            id: `${items.length}`,
            value: ""
        };
        temp.push(custom);
        setItems(temp);
        const results = state.highlights;
        // trying to generate a sufficiently unique id to prevent double binding
        const id: string = String(Date.now());
        results.push({ id, value: '' });
        setNextFocus(results.length - 1);
        setState({ highlights: results });
        props.changeHandler(results);
    }

    const handleChange = (changedValues: any) => {

        // remove [empty] values if object is deleted from array
        const filtered: any = changedValues.filter((el: any) => {
            return el != null;
        });

        // before we bubble data up, get it back into the form that it was sent in (array of DraggableListItems)
        let valueArray: any = []
        if (filtered.length > 0) {
            valueArray = Array.from({ length: changedValues.length }, (v, index) => index).map((index:any) => {
                if (changedValues[index] && changedValues[index].value !== "") {
                    const val: DraggableListItem = {
                        order: index,
                        value: `${changedValues[index].value ? changedValues[index].value : ""}`
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

    const HighLightMemo = useMemo(() =>
        state.highlights.map((highlight: any, index: number) => (
            <Draggable draggableId={highlight.id} index={index} key={highlight.id + highlight.value + index} isDragDisabled={props.isDragDisabled}>
                {provided => ( // tslint:disable-line
                    <Highlight
                        ref={provided.innerRef}
                        {...provided.draggableProps}
                        {...provided.dragHandleProps}>
                        <FormInput name={(highlight.value === ' ') ? index : highlight.value} index={index} useOnChange={true} indexedBlurHandler={updateValue} defaultValue={highlight.value} forceFocus={nextFocus === index} />
                        <img src={DeleteIcon} onClick={() => deleteItem(highlight.id)} />
                        <img src={DragIcon} />
                    </Highlight>
                )}
            </Draggable>
        )), [state]);

    return (
        <div>
            <DragDropContext onDragEnd={onDragEnd}>
                <Droppable droppableId="theDraggableList">
                    {provided => ( // tslint:disable-line
                        <div ref={provided.innerRef} {...provided.droppableProps}>
                            <>{HighLightMemo}</>
                            {provided.placeholder}
                        </div>
                    )}
                </Droppable>
            </DragDropContext>
            <StyledButton name={addButtonName} onClick={addItem} style={{ marginTop: '25px' }} styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp; Add Highlight</StyledButton>
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
      margin-top:30px;
      background:transparent;
  }
`;

export default DraggableList;
