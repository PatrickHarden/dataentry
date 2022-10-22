import React from 'react';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Amenities, { Props } from '../amenities';
import MockStoreContainer from '../../../../utils/tests/mock-store';
import { singleListingState } from '../../../../utils/tests/mock-states/single-listing-state';
import SGConfig from '../../../../config/sg-comm.json';
// for the toHaveStyle function
import '@testing-library/jest-dom';


describe('views', () => {

    describe('Amenities', () => {

        let props: Props | any;

        beforeEach(() => {

            props = {
                listing: singleListingState
            }
        });


        it('renders and matches the config', () => {
            render(
                <MockStoreContainer config="sg-comm">
                    <Amenities {...props} />
                </MockStoreContainer>
            );

            const amenities:HTMLElement[] = screen.getAllByTestId("amenity");
            expect(amenities.length).toBe(SGConfig.aspects.options.length);
            
        });


        it('updates state on click and triggers css effect', () => {
            render(
                <MockStoreContainer config="sg-comm">
                    <Amenities {...props} />
                </MockStoreContainer>
            );

            const amenities:HTMLElement[] = screen.getAllByTestId("amenity");
            userEvent.click(amenities[0]);
            expect(amenities[0]).toHaveStyle('background-color: rgb(255, 255, 255)');
            
        });
    });
});