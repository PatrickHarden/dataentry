import React from 'react';
import GLTableRow, { Props } from '../gl-table';
import { render, screen } from '@testing-library/react';
import { hexToRgb } from '../../../utils/tests/test-utils';

describe('components', () => {

  describe('gl-table', () => {

    let props:Props;

    beforeEach(() => {
        props = {
            dataProvider: [
                { "id": 1234, "label": "Row 1, Col 2", "desc": "First piece of data"},
                { "id": 5678, "label": "Row 2, Col 2", "desc": "Second piece of data"},
                { "id": 9999, "label": "Row 3, Col 2", "desc": "Third piece of data"}
            ],
            columns: [
                { "dataField": "id", "header": "ID", "size": 4 },
                { "dataField": "label", "header": "Label", "size": 4 },
                { "dataField": "desc", "header": "Description", "size": 4 }
            ],
            styles: {
                "tableRow": {}
            }
        }
    });

    // check the default state of the row given the base data setup in beforeEach:
    // (1) the number of rows should equal the length of the dataprovider (assuming no replacements)
    // (2) the number of columns should equal the length of the columns property
    // (3) check some random cells to ensure that text is set as expected
    it('renders a plain table with the number of rows and columns expected', () => {
        render(<GLTableRow {...props}/>);
        const rows:HTMLElement[] = screen.getAllByTestId("gl-table-row");
        expect(rows.length).toBe(props.dataProvider.length);
        const cells:HTMLElement[] = screen.getAllByTestId("gl-table-row-cell");
        // we have to divide here because we will have "x" columns for each row (which really is the # of cells) so we need to find the # of columns
        expect(cells.length / props.dataProvider.length).toBe(props.columns.length);     
        // check some cells to ensure the correct text is displayed
        // row 1, col 2
        expect(cells[1].textContent).toBe("Row 1, Col 2");
        // row 2, col 1
        expect(cells[3].textContent).toBe("5678");
        // row 3, col 3
        expect(cells[8].textContent).toBe("Third piece of data");
    });

    // this test is for replacement rows.  A "replacement" row is a row that perhaps has a differing data object or configuration that 
    // makes it different from the rest of our rows.  We need to ensure they are rendered according to expectation
    it('renders replacement rows as expected', () => {
        props.replaceColsForRow = [
            { replaceAt: 0, columns: [{"dataField": "pet", "size": 6}, {"dataField": "toy", "size": 6}]},
            { replaceAt: 1, columns: [{"staticDisplay": "More Rows Below Me", "size": 12}]}
        ];
        props.dataProvider = [
            { "pet": "Dog", "toy": "ball"},
            {},
            { "id": 1234, "label": "Row 2, Col 2", "desc": "First piece of data"},
            { "id": 5678, "label": "Row 3, Col 2", "desc": "Second piece of data"},
            { "id": 9999, "label": "Row 4, Col 2", "desc": "Third piece of data"}
        ];
        render(<GLTableRow {...props}/>);
        const cells:HTMLElement[] = screen.getAllByTestId("gl-table-row-cell");
        // row 1,1 cell should be the pet name "Dog"
        expect(cells[0].textContent).toBe("Dog");
        // row 1,2 cell should be the toy "ball"
        expect(cells[1].textContent).toBe("ball");
        // row 2,1 cell should be a static display row acting as a header for more rows
        expect(cells[2].textContent).toBe("More Rows Below Me");
        // row 3, 1 cell should be the first id "1234"
        expect(cells[3].textContent).toBe("1234");
        // row 5, 3 cell should be the final desc "Third piece of data"
        expect(cells[11].textContent).toBe("Third piece of data");
    });

    // test out rendering a header row.  An example header row would be a row that only has a single text field in it with a size of 12 - like a divider
    it('renders column headers as expected when showHeadersBeforeRow is set', () => {
        props.showHeadersBeforeRow = [0];
        render(<GLTableRow {...props}/>);
        const columnHeaders:HTMLElement[] = screen.getAllByTestId("gl-table-row-col-header");
        // first column header should be "ID"
        expect(columnHeaders[0].textContent).toBe("ID");
        // second column header should be "Label"
        expect(columnHeaders[1].textContent).toBe("Label");
        // third column header should be "Description"
        expect(columnHeaders[2].textContent).toBe("Description");
    });

    // test out including an action button (note: getting the element itself will fail the test if it doesn't exist)
    it('renders an action button when configured', () => {
        props.actionButton = {
            label: "Do Stuff!",
            clickHandler: jest.fn()
        }
        render(<GLTableRow {...props}/>);
        const actionButton:HTMLElement = screen.getByTestId("gl-table-action-button");
        // first column header should be "ID"
        expect(actionButton.textContent).toBe("Do Stuff!");
    });

    // perform a series of styling tests to ensure styles passed in are applied as expected
    // note: the tableRow style is a pass through only here and tested within the gl-table-row.spec.tsx set of tests
    it('applies style changes as expected', () => {
        props.styles = {
           "table": {
                "fontFamily": "Arial",
                "background": "#0000FF",
                "border": "1px dotted #000000",
                "boxSizing": "none",
                "boxShadow": "none",
                "borderRadius": "5px",
                "padding": "10px"
           },
           "styledGrid": {
                "padding": "10px 5px 10px 5px"
           },
           "tableRow": {}
        }

        render(<GLTableRow {...props}/>);
        
        // table element
        const tableElement:HTMLElement = screen.getByTestId("gl-table");
        const tableStyles = window.getComputedStyle(tableElement);
        expect(tableStyles.fontFamily).toBe("Arial");
        expect(tableStyles.backgroundColor).toBe(hexToRgb("#0000FF"));
        expect(tableStyles.border).toBe("1px dotted #000000");
        expect(tableStyles.boxSizing).toBe("none");
        expect(tableStyles.boxShadow).toBe("none");
        expect(tableStyles.borderRadius).toBe("5px");
        expect(tableStyles.padding).toBe("10px");
    });
  });
});