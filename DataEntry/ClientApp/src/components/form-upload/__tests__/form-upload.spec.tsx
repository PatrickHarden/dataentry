import React from 'react';
import { mount, shallow } from 'enzyme';
import FormUpload from '../form-upload';
import { act } from 'react-dom/test-utils'; //
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';

describe("FormUpload", () => {

    let wrapper: any;
    let props: any;
    let store: any;

    const initialState = {
        system: {
          configDetails: {
            loaded: true,
            config: {
                "featureFlags" : {
                    "leaseRateTypeEnabled" : true
                }
            }
          }
        },
        entry : {
            refreshImages: []
        }
      };
      const mockStore = configureStore();

    beforeEach(() => {
        props = {
            name: "testableUpload",
            title: "Photos",
            accepted: ".jpg, .png, .jpeg",
            options: {},
            description: "JPEG or PNG's only",
            field: { name: 'testme', defaultValue: 'Some value' },
            label: "Test Me!",
            photos: [],
            showPrimary: true,
            error: false,
            files: []
        }
        store = mockStore(initialState);
    });

    it("filters uploads via accepted props", () => {
        wrapper = mount(<Provider store={store}><FormUpload {...props} /></Provider>);
        expect(wrapper.find('input').prop('accept')).toEqual(".jpg, .png, .jpeg");
    })

    it("shows the correct label", () => {
        wrapper = mount(<Provider store={store}><FormUpload {...props} /></Provider>);
        expect(wrapper.find('h5').at(1).text()).toEqual('Test Me!');
    })

    it("enables the showPrimary prop ", () => {
        wrapper = mount(<Provider store={store}><FormUpload {...props} /></Provider>);
        expect(wrapper.find('.jestTest').at(0).hasClass('isPrimary')).toBe(true);
    })

    it("disables the showPrimary prop ", () => {
        props.showPrimary = false;
        wrapper = mount(<Provider store={store}><FormUpload {...props} /></Provider>);
        expect(wrapper.find('.jestTest').at(0).hasClass('isPrimary')).toBe(false);
    })

    // * Disabling this test for now, because image may validly not upload if watermark is detected *

    // it("should test the upload functionality", () => {
    //     const componentWrapper = mount(<FormUpload {...props} />);
    //     const component = componentWrapper.get(0);
    //     const fileContents = 'file contents';
    //     const expectedFinalState = ('{fileContents}');
    //     const file = new Blob([fileContents], { type: 'text/plain' });
    //     const readAsText = jest.fn();
    //     const addEventListener = jest.fn((_, evtHandler) => { evtHandler(); });
    //     const dummyFileReader = { addEventListener, readAsText, result: fileContents };
    //     window.FileReader = jest.fn(() => dummyFileReader); // tslint:disable-line

    //     act(() => {
    //         componentWrapper.find('input').simulate('change', { target: { files: [file] } });
    //     });
    //     // should find the backup placeholder image located in the thumbnail
    //     expect(componentWrapper.find('img'))
    // });

    // snapshot render
    it('renders according to snapshot', () => {
        expect(shallow(<Provider store={store}><FormUpload {...props} /></Provider>)).toMatchSnapshot();
    });
});