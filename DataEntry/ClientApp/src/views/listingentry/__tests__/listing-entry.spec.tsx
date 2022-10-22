import React from 'react';
import { mount } from 'enzyme';

import ListingEntry from '../listing-entry';
import { Config } from '../../../types/config/config';
import configureStore from 'redux-mock-store';
import { getInitialStore } from '../../../store/setup-intial-store';
import { getMockListing } from '../../../utils/tests/mock-listing';
import { ReplaceProperty } from '../../../utils/tests/replace-json';
import { TestConfig, getMockConfig } from '../../../utils/tests/mock-config';
import { Provider } from 'react-redux';
import { ThemeProvider } from 'styled-components';
import { theme } from '../../../themes/default/theme';

describe('views', () => {

  describe('listing-entry', () => {

    // let wrapper:any;
    // let store:any;
    // const mockStore = configureStore();

    // it("property type: spaces DO NOT clear out when a property type is selected if the property type before or after do not require spacesClearOn", () => {

    //     // we'll use Singapore for both cases, in this case, we switch from "office" to "industrial", which should not trigger a change
    //     const config:Config = getMockConfig(TestConfig.SG);
    //     const setupPropertyTypes:ReplaceProperty = {
    //         propertyName: "system.configDetails.config",
    //         replaceWith: config
    //     };

    //     // create our store
    //     const initialStore = getInitialStore(config, [setupPropertyTypes]); // use our initial store to setup state
    //     store = mockStore(initialStore);

    //     // setup current listing and ensure the current property type is "office"
    //     const replacePropertyType:ReplaceProperty = { propertyName: "propertyType", replaceWith: "office"}
    //     const replaceListingType:ReplaceProperty = { propertyName: "listingType", replaceWith: "lease"}
    //     // setup three mock spaces
    //     const replaceSpaces:ReplaceProperty = {
    //         propertyName: "spaces",
    //         replaceWith: [
    //             {id: 1, name: "", nameSingle: "space1", spaceDescription: [], spaceDescriptionSingle: "", specifications: {}, photos: [], floorplans: [], brochures: [], video: "", walkthrough: ""},
    //             {id: 2, name: "", nameSingle: "space2", spaceDescription: [], spaceDescriptionSingle: "", specifications: {}, photos: [], floorplans: [], brochures: [], video: "", walkthrough: ""},
    //             {id: 3, name: "", nameSingle: "space3", spaceDescription: [], spaceDescriptionSingle: "", specifications: {}, photos: [], floorplans: [], brochures: [], video: "", walkthrough: ""}
    //         ]
    //     }
    //     const mockListing = getMockListing([replacePropertyType,replaceListingType,replaceSpaces]);

    //     const listingProps = {
    //         navTitle: "Test",
    //         anchors: [],
    //         listing: mockListing
    //     }

    //     wrapper = mount(<Provider store={store}><ThemeProvider theme={theme}><ListingEntry {...listingProps}/></ThemeProvider></Provider>);
    //     wrapper.find('Select#propertyType').first().instance().selectOption({value: "industrial"});
    //     expect(wrapper.find('.propertyType__single-value').first().text()).toEqual('Industrial');
    //     expect(wrapper.update().find('.space-pane')).toHaveLength(3);
    // });

    // it("property type: spaces DO clear out when a property type before or after is selected that requires spacesClearOn", () => {

    //     // in this case, we switch from "office" to "flex", which SHOULD trigger a change
    //     const config:Config = getMockConfig(TestConfig.SG);
    //     const setupPropertyTypes:ReplaceProperty = {
    //         propertyName: "system.configDetails.config",
    //         replaceWith: config
    //     };

    //     // create our store
    //     const initialStore = getInitialStore(config, [setupPropertyTypes]); // use our initial store to setup state
    //     store = mockStore(initialStore);

    //     // setup current listing and ensure the current property type is "office"
    //     const replacePropertyType:ReplaceProperty = { propertyName: "propertyType", replaceWith: "office"}
    //     const replaceListingType:ReplaceProperty = { propertyName: "listingType", replaceWith: "lease"}
    //     // setup three mock spaces
    //     const replaceSpaces:ReplaceProperty = {
    //         propertyName: "spaces",
    //         replaceWith: [
    //             {id: 1, name: "", nameSingle: "space1", spaceDescription: [], spaceDescriptionSingle: "", specifications: {}, photos: [], floorplans: [], brochures: [], video: "", walkthrough: ""},
    //             {id: 2, name: "", nameSingle: "space2", spaceDescription: [], spaceDescriptionSingle: "", specifications: {}, photos: [], floorplans: [], brochures: [], video: "", walkthrough: ""},
    //             {id: 3, name: "", nameSingle: "space3", spaceDescription: [], spaceDescriptionSingle: "", specifications: {}, photos: [], floorplans: [], brochures: [], video: "", walkthrough: ""}
    //         ]
    //     }
    //     const mockListing = getMockListing([replacePropertyType,replaceListingType,replaceSpaces]);

    //     const listingProps = {
    //         navTitle: "Test",
    //         anchors: [],
    //         listing: mockListing
    //     }

    //     wrapper = mount(<Provider store={store}><ThemeProvider theme={theme}><ListingEntry {...listingProps}/></ThemeProvider></Provider>);
    //     wrapper.find('Select#propertyType').first().instance().selectOption({label: "Flexible Space", value: "flex", order: 4, color: "#00B2DD"});
    //     expect(wrapper.find('.propertyType__single-value').first().text()).toEqual('Flexible Space');
    //     expect(wrapper.update().find('.space-pane')).toEqual({});
    // });

    it("is a placeholder", () => {
        const temp = true;
        expect(temp).toBe(true)
    })

  });
});