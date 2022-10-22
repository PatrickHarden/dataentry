import React from 'react';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import ChargesAndModifiers, { Props } from '../charges-and-modifiers';
import MockStoreContainer from '../../../../utils/tests/mock-store';
import { singleListingState } from '../../../../utils/tests/mock-states/single-listing-state';


describe('views', () => {

    describe('Charges and Modifiers', () => {

        let props: Props | any;

        beforeEach(() => {

            props = {
                listing: singleListingState
            }
        });


        it('renders and can create a new row', () => {
            render(
                <MockStoreContainer config="sg-comm">
                    <ChargesAndModifiers {...props} />
                </MockStoreContainer>
            );


            // check if the component renders the one charges row (due to singleListingState)
            const chargesRow:HTMLElement[] = screen.getAllByTestId("charges-and-modifiers-row");
            expect(chargesRow.length).toBe(1);

            
            // fire add row button
            const addRowButton: HTMLElement = screen.getByTestId("add-charges-and-modifiers");
            userEvent.click(addRowButton);

            
            // check to see if there's two rows now
            const newChargesRow:HTMLElement[] = screen.getAllByTestId("charges-and-modifiers-row");
            expect(newChargesRow.length).toBe(2);

        });
    });
});