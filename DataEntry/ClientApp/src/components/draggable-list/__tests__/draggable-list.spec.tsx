import React from 'react';
import { mount, shallow } from 'enzyme';
import renderer from 'react-test-renderer';
import DraggableList from '../draggable-list';

describe('components', () => {

    describe('draggable-list', () => {

        const initialValues: object = {
            'Ross': 'Ross',
            'Space-1': 'Space-1',
            'Space-2': 'Space-2',
            'WTC': 'WTC',
        }

        const changeHandler = (values: any) => {
            const newArray = values.filter(value => Object.keys(value).length !== 0);
            const highlights = { highlights: newArray }
            console.log(highlights)
        }

        // snapshot render
        it('renders inputs', () => {
            const component = mount(<DraggableList handleChange={changeHandler} items={Object.values(initialValues)} isDragDisabled={true} />); 
            expect(component.find('input').length).toEqual(4);
        });
    });
});