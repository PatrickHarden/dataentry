import React from 'react';
import { mount } from 'enzyme';
import FormController from '../gl-form-controller';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';

beforeEach(() => {
    jest.resetModules();
});

// this returns a component with the form context api wrapping it properly
const getFormController = (context = {setErrors: jest.fn(), onFormChange: jest.fn()}) => {
    jest.doMock('../gl-form-context', () => {
        return {
            FormContext: {
                Consumer: (props:any) => props.children(context)
            }
        }
    });
    return FormController;
}

describe("Form Controller", () => {

    const initialStore = {};
    const mockStore = configureStore();

    let store:any;

    beforeEach(() => {
        store = mockStore(initialStore);
    });

    it("renders correctly", ()=> {
        const FormControllerFC = getFormController();
        const onChange = jest.fn();
        const wrapper = mount(<Provider store={store}><FormControllerFC changeHandler={onChange}><div/><div/></FormControllerFC></Provider>);
        expect(wrapper.render());
    });

    it("renders the correct # of {children} (2)", ()=> {
        const FormControllerFC = getFormController();
        const onChange = jest.fn();
        const wrapper = mount(<Provider store={store}><FormControllerFC changeHandler={onChange}><div/><div/></FormControllerFC></Provider>);
        expect(wrapper.find('div').length).toBe(2);
    });
});