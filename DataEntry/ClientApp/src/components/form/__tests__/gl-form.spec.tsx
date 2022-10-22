import React from 'react';
import { mount } from 'enzyme';
import GLForm from '../gl-form';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import { Field } from 'formik';
import FormInput from '../../form-input/form-input';

beforeEach(() => {
    jest.resetModules();
});

// this returns a component with the form context api wrapping it properly
const getForm = (context = {setErrors: jest.fn(), onFormChange: jest.fn()}) => {
    jest.doMock('../gl-form-context', () => {
        return {
            FormContext: {
                Consumer: (props:any) => props.children(context)
            }
        }
    });
    return GLForm;
}

describe("Global Listings Form", () => {

    const initialStore = {};
    const mockStore = configureStore();

    let store:any;
    let initVals:object;
    let validationAdapter:Function;
    let validationJSON:object;

    beforeEach(() => {
        store = mockStore(initialStore);
        initVals = {'field1': 'value1'};
        validationAdapter = jest.fn();
        validationJSON = {'field1': {required: true}};
    });

    it("renders correctly", ()=> {
        const GLFormFC = getForm();
        const wrapper = mount(<Provider store={store}><GLFormFC initVals={initVals} validationAdapter={validationAdapter} validationJSON={validationJSON}/></Provider>);
        expect(wrapper.render());
    });

    it("renders {children}: 3 Text Inputs (2 Normal, 1 Custom)", ()=> {
        const GLFormFC = getForm();
        const wrapper = mount(<Provider store={store}><GLFormFC initVals={initVals} validationAdapter={validationAdapter} validationJSON={validationJSON}>
                <Field type="text" />
                <Field component={FormInput} label="test"/>
                <Field type="text"/>
            </GLFormFC>
        </Provider>);
        expect(wrapper.find('input').length).toBe(3);
    });
});