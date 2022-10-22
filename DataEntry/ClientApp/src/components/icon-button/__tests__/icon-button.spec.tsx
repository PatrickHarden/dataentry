import React from 'react';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import IconButton, { IconButtonProps } from '../icon-button';

describe('IconButton', () => {

    describe('Icon Button no click', () => {
  
      let props:IconButtonProps;
  
      beforeEach(() => {

          const buttonObj = {
              id: '91',
              label: 'label',
              itemIndex: 0,
              allowClick: false,
              icon: 'https://placekitten.com/200/300',
          }
  
          props = {
            uniqueId: 'noClick',
            button: buttonObj
          }
      });


    it('renders the button without allowing it to be clickable', () => {
          render(<IconButton {...props}/>);

          // check the image to see if it matchs the image prop passed in
          const buttonImageContainer:HTMLElement = screen.getByTestId("icon-button-img");
          expect(buttonImageContainer.getAttribute("src")).toBe(props.button.icon);
      });
    });

    describe('Icon Button with click', () => {
  
        let props:IconButtonProps;
    
        beforeEach(() => {
  
            const buttonObj = {
                id: '92',
                label: 'label',
                itemIndex: 0,
                allowClick: true,
                icon: 'https://placekitten.com/200/300',
                clickHandler: () => { console.log('click') }
            }
    
            props = {
              uniqueId: 'allowClick',
              button: buttonObj
            }
        });
  
  
      it('renders the clickable button', () => {
            render(<IconButton {...props}/>);

            // check the image to see if it matchs the image prop passed in
            const buttonImageContainer:HTMLElement = screen.getByTestId("styled-icon-button-img");
            expect(buttonImageContainer.getAttribute("src")).toBe(props.button.icon);
  
            // triggers click
            const buttonContainer:HTMLElement = screen.getByTestId("styled-icon-button");
            userEvent.click(buttonContainer);
        });
      });
})