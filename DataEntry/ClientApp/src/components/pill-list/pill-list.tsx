import React, { useState, FC } from "react";
import styled from 'styled-components';
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import FormInput from '../form-input/form-input';
import StyledButton from '../styled-button/styled-button';
// images
import DeleteIcon from '../../assets/images/png/deleteIcon.png';
import DragIcon from '../../assets/images/png/dragIcon.png';

interface DraggableListItem{
    order:number;
    value:string;
}

interface Props {
    items: DraggableListItem[],
    isDragDisabled?: boolean,
    changeHandler(value: any): void
}

const PillList: FC<Props> = (props) => {
    let theProps = props.items;
    if(!theProps){
        theProps = [];
    }

    const [items, setItems] = useState(theProps);

    // this takes the data passed into the component and transforms it into the component will consume
    const initial = Array.from({ length: items.length }, (v, index) => index).map(index => {
        const custom: any = {
            id: `${index}`,
            content: `${items[index].value}`
        };
        return custom;
    });
    
    const [state, setState] = useState({ pillTags: initial });

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
        const pillTags = reorder(
            state.pillTags,
            result.source.index,
            result.destination.index
        );
        setState({ pillTags });
        handleChange(pillTags);
    }

    const deleteItem = (index: number) => {
        const temp: any = state.pillTags;
        for (let i = 0; i < temp.length; i++) {
            if (index === temp[i].id) {
                delete temp[i]
            }
        }
        const filtered = temp.filter((el: any) => {
            return el != null;
        });
        setState({ pillTags: filtered })
        const tempItems: any = filtered.map((pillTag: any, i: any) => {
            return pillTag.content
        })
        setItems(tempItems)
        handleChange(state.pillTags);
    }

    const updateValue = (index:number, value:any)  => {
        const tempState: any = state;
        if(tempState.pillTags[index]){
            tempState.pillTags[index].content = value;
        }
        handleChange(tempState.pillTags);
    }

    const addItem = () => {
        const temp: any = items;
        const custom: any = {
            order: temp.length,
            value: ""
        };
        temp.push(custom);
        setItems(temp);
        const results = state.pillTags;
        // trying to generate a sufficiently unique id to prevent double binding
        results.push({id: String(Date.now()), content: ''});
        setState({pillTags: results});
        handleChange(results);
    }

    const handleChange = (changedValues:any) => {
        // before we bubble data up, get it back into the form that it was sent in (array of DraggableListItems)
        const valueArray = Array.from({ length: changedValues.length }, (v, index) => index).map(index => {
           
            const val:DraggableListItem = {
                order: index,
                value: `${changedValues[index].content}`
            }
            
            return val;
        });
        props.changeHandler(valueArray);
    }


    return (
        <div>
            <DragDropContext onDragEnd={onDragEnd}>
                <Droppable droppableId="thePillList">
                    {provided => ( // tslint:disable-line
                        <div ref={provided.innerRef} {...provided.droppableProps}>
                            {state.pillTags.map((pillTag: any, index: number) => (
                                <Draggable draggableId={pillTag.id} index={index} key={pillTag.id + pillTag.content + index} isDragDisabled={props.isDragDisabled}>
                                    {provided => ( // tslint:disable-line
                                        <PillTag
                                            ref={provided.innerRef}
                                            {...provided.draggableProps}
                                            {...provided.dragHandleProps}
                                        >
                                            <FormInput name={(pillTag.content === ' ') ? index : pillTag.content} index={index} indexedBlurHandler={updateValue} defaultValue={pillTag.content} />
                                            <img src={DeleteIcon} onClick={() => deleteItem(pillTag.id)} />
                                            <img src={DragIcon} />
                                        </PillTag>
                                    )}
                                </Draggable>
                            ))}
                            {provided.placeholder}
                        </div>
                    )}
                </Droppable>
            </DragDropContext>
            <StyledButton onClick={addItem} style={{ marginTop: '25px' }} styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp; Add Tag</StyledButton>
        </div>
    );
}

const PillTag = styled.div`
  width: 100%;
  max-width:700px
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

export default PillList;
