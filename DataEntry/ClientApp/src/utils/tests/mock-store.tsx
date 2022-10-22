import React, { FC } from 'react';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { TestConfig, getMockConfig } from  './mock-config';
import { ReplaceProperty } from './replace-json';
import { initialState } from './mock-states/initial-state';
import { State } from '../../types/state';
import { findAndReplace } from './replace-json';


export interface Props {
    config?: string,
    replaceConfigProperty?: ReplaceProperty[],
    replaceStoreProperty?: ReplaceProperty[],
    state?: State | any
};

const MockStoreContainer: FC<Props> = ({config, replaceConfigProperty, replaceStoreProperty, state, children}) => {


    // setup config or fallback to US
    let theConfig: any;
    if (config === 'us-comm') {
        theConfig = Object.assign(getMockConfig(TestConfig.US, replaceConfigProperty), {});
    } else if (config === 'sg-comm') {
        theConfig = Object.assign(getMockConfig(TestConfig.SG, replaceConfigProperty), {})
    } else if (config === 'in-comm') {
        theConfig = Object.assign(getMockConfig(TestConfig.IN, replaceConfigProperty), {});
    } else if (config === 'nl-comm') {
        theConfig = Object.assign(getMockConfig(TestConfig.NL, replaceConfigProperty), {});
    } else {
        theConfig = Object.assign(getMockConfig(TestConfig.US, replaceConfigProperty), {});
    }


    // setup initial store
    let initialStore: any;
    if (state){
        initialStore = Object.assign(JSON.parse(state), {});
    } else {
        initialStore = Object.assign(initialState, {});
    }


    // update initialStore with selected config
    initialStore.system.configDetails.config = theConfig;


    // replace store with arguments passed in
    if (replaceStoreProperty && replaceStoreProperty.length > 0){
        initialStore = findAndReplace(initialStore, replaceStoreProperty);
    }

    // create store
    const mockStore = configureMockStore([thunk]);
    const store = mockStore(initialState)


    return (
        <Provider store={store}>
            {children}
        </Provider>
    );
};


export default MockStoreContainer;



