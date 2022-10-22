
import React, { FC, useContext, useState, useCallback } from 'react'
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import StyledButton from '../../../components/styled-button/styled-button';
import SectionHeading from "../../../components/section-heading/section-heading";
import styled from 'styled-components';
import { FormContext } from '../../../components/form/gl-form-context';
import { Listing } from '../../../types/listing/listing';
import { Tabs, TabList, Tab, TabPanel } from 'react-tabs';
import MultiDropdown from './multi-dropdown/multi-dropdown';
import { TransportationType, TransportationPlace } from '../../../types/listing/transportationType';
import Transit from '../../../assets/images/png/transit.png';
import Rail from '../../../assets/images/png/rail.png';
import Airport from '../../../assets/images/png/airport.png';
import Ferry from '../../../assets/images/png/ferry.png';
import Shipyard from '../../../assets/images/png/shipyard.png';
import Highway from '../../../assets/images/png/highway.svg';

interface Props {
    listing?: Listing,
    tt: TransportationType[] | undefined,
    types: string[],
    travelMode: any[],
    distanceUnits: any[],
    sectionHeading: string | undefined
}

const TransportationsType: FC<Props> = (props) => {

    const { tt, types, travelMode, distanceUnits, sectionHeading } = props;

    const imageArray = [Transit, Rail, Airport, Ferry, Shipyard, Highway]

    const [data, setData] = useState<TransportationType[]>(tt ? tt : []);
    // force update equivalent:
    const [, updateState] = useState();
    // tslint:disable-line 
    const forceUpdate = useCallback(() => updateState({}), []); // tslint:disable-line
    
    // value change interceptor
    const formControllerContext = useContext(FormContext);


    const changeHandler = (values: any, kind: string, index: number) => {

        const temp: any = data;

        data.forEach((ttSingle: any, i: number) => {
            if (ttSingle.type === kind) {
                temp[i].places[index] = values;
            }
        })




        formControllerContext.onFormChange({ transportationTypes: temp });
    }


    const addRow = (kind: string, index: number) => {
        const temp: TransportationType[] = data;

        let transportationKindExists: boolean = false;
        data.forEach((ttSingle: any, i: number) => {
            if (ttSingle && ttSingle.type === kind) {
                temp[i].places.push({
                    name: "",
                    distances: null,
                    distanceUnits: "",
                    duration: null,
                    travelMode: "",
                    order: temp[i].places.length + 1
                })
                transportationKindExists = true;
            }
        })

        if (!transportationKindExists) {
            temp.push({
                type: kind,
                places: [
                    {
                        name: "",
                        distances: null,
                        distanceUnits: "",
                        duration: null,
                        travelMode: "",
                        order: 1
                    }
                ]
            })
        }

        setData(temp);
        forceUpdate();
    }


    const reorder = (list: any, startIndex: number, endIndex: number) => {
        const result = Array.from(list);
        const [removed] = result.splice(startIndex, 1);
        result.splice(endIndex, 0, removed);
        return result;
    }


    const onDragEnd = (value: any, kind: string) => {
        if (!value.destination) {
            return;
        } else if (value.source.index === value.destination.index) {
            return;
        }

        const temp: TransportationType[] = data;
        let tempPlaces: any = [];

        const sourceIndex: number = value.source.index;
        const destination: number = value.destination.index;

        data.forEach((ttSingle: TransportationType, index: number) => {
            if (ttSingle.type === kind) {
                tempPlaces = reorder(ttSingle.places, sourceIndex, destination);
                // update order
                tempPlaces.map((place: TransportationPlace, z: number) => {
                    place.order = z + 1;
                    return place;
                })
                // set values
                temp[index].places = tempPlaces
            }
        })

        setData(temp);
        forceUpdate();
    }


    const deleteRow = (value: number, kind: string) => {
        const temp: any = data;
        let tempPlaces: any = [];
        let tempIndex: number = 0;

        data.forEach((ttSingle: TransportationType, index: number) => {
            if (ttSingle.type === kind) {
                tempIndex = index;
                tempPlaces = ttSingle.places;
            }
        })

        delete tempPlaces[value];

        // // remove 'empty'
        tempPlaces = tempPlaces.filter((el: any) => {
            return el;
        });

        temp[tempIndex].places = tempPlaces

        setData(temp);
        forceUpdate();
    }


    return (
        <>
            <SectionHeading error={undefined}>
                {sectionHeading}
            </SectionHeading>
            <STabs
                selectedTabClassName='is-selected'
                selectedTabPanelClassName='is-selected'
            >
                <STabList>
                    {types.map((kind: string, z: number) => (
                        <STab key={kind}>
                            <img src={imageArray[z]} />
                            <span>{kind}</span>
                        </STab>
                    ))}
                </STabList>
                <>
                    {types.map((kind: string, z: number) => (
                        <STabPanel>
                            <StyledButton name={'add new'} onClick={() => addRow(kind, z)} style={{ marginTop: '5px', paddingLeft: '0px' }} data-testid="add-transportation" styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp; Add New</StyledButton>
                            {data.map((ttSingle: TransportationType) => {
                                if (ttSingle.type === kind) {
                                    return (
                                        <div key={kind + z}>
                                            <DragDropContext onDragEnd={(event: any) => { onDragEnd(event, kind) }} >
                                                <Droppable droppableId={kind + String(Date.now())}>
                                                    {provided => ( // tslint:disable-line
                                                        <div ref={provided.innerRef} {...provided.droppableProps}>
                                                            {ttSingle.places.map((place: TransportationPlace, index: number) => (
                                                                <div key={kind + z + index}>
                                                                    <Draggable draggableId={index + z + kind} index={index} key={index + z + kind} isDragDisabled={false}>
                                                                        {provided => ( // tslint:disable-line
                                                                            <div
                                                                                ref={provided.innerRef}
                                                                                {...provided.draggableProps}
                                                                                {...provided.dragHandleProps}
                                                                            >
                                                                                <MultiDropdown name={'ttDropdown' + index + kind} key={index + kind + kind} prefix={''}
                                                                                    index={index} deleteRow={deleteRow} changeHandler={changeHandler}
                                                                                    transportationIndex={index}
                                                                                    kind={kind}
                                                                                    values={place}
                                                                                    defaultUnitOfMeasurement={''}
                                                                                    travelMode={travelMode}
                                                                                    distanceUnits={distanceUnits}
                                                                                />
                                                                            </div>
                                                                        )}
                                                                    </Draggable>
                                                                </div>
                                                            ))}
                                                        </div>
                                                    )}
                                                </Droppable>
                                            </DragDropContext>
                                        </div>
                                    )
                                }
                            })}
                        </STabPanel>
                    ))}
                </>
            </STabs>
        </>
    )
}



const STabs = styled(Tabs)`
  font-size: 12px;
  margin-top:5px;

`;

const STabList = styled(TabList)`
  list-style-type: none;
  padding: 0px 4px 4px 0px;
  display: flex;
  background-color: #f4f4f4;
  margin: 0;
  border-top: solid 1px #ccc;
  
  li.is-selected {

    border: solid 1px #ccc !important;
    border-bottom: solid 1px transparent !important;
}
li {
    span {
        color #666 !important;
    }
    img {
        margin-right: 12px;
        margin-left:5px;
    }
    border-top: solid 1px #ccc;
    margin-top:-1px;
}

`;

const STabPanel = styled(TabPanel)`
  display: none;
  /* min-height: 40vh; */
  border: 1px solid #d3d3d3;
  background-color: white;
  padding: 15px;
  margin-top: -5px;
  padding-bottom:20px;

  &.is-selected {
    display: block;
  }

  > span {
    color: #69BE28 !important;
    border: none !important;
  }
`;

const STab = styled(Tab)`
  margin-right: -1px;
  border: 1px solid #e9e9e9;
  border-bottom: 1px solid #d3d3d3;
  padding: 10px;
  user-select: none;
  cursor: pointer;
  color: 1px solid #d3d3d3;
  display: flex;
  min-width: 110px;
  justify-content: space-evenly;
  align-items: center;

  &.is-selected {
    border: 1px solid #d3d3d3;
    color: 1px solid #c6c6c6;
    border-bottom: 1px solid white;
    background-color: white;
    cursor: arrow;
  }

  &:focus {
    outline: none;
    box-shadow: 0 0 0 2px rgba(0, 0, 255, .5)
  }
`;

export default TransportationsType