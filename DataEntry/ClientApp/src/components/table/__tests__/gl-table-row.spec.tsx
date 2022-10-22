import React from 'react';
import GLTableRow, { Props } from '../gl-table-row';
import { render, screen } from '@testing-library/react';
import { hexToRgb } from '../../../utils/tests/test-utils';

describe('components', () => {

  describe('gl-table-row', () => {

    let props:Props;

    beforeEach(() => {
        props = {
            data: { "id": 1234, "label": "My Label", "desc": "Testing out the table row"},
            columns: [
                { "dataField": "id", "header": "ID", "size": 4 },
                { "dataField": "label", "header": "Label", "size": 4 },
                { "dataField": "desc", "header": "Description", "size": 4 },
                
            ],
            styles: {}
        }
    });

    // check the default state of the row given the base data setup in beforeEach:
    // (1) the number of columns setup should match the length of the columns passed in
    // (2) check the cells to match data against the incoming data object (i.e., cell 1 should = 1234, 2 = "My Label", etc.)
    it('renders a plain set of columns as expected', () => {
        render(<GLTableRow {...props}/>);
        const elements:HTMLElement[] = screen.getAllByTestId("gl-table-row-cell");
        // column length match
        expect(elements.length).toBe(props.columns.length);
        // check specific cell data to ensure it's displayed as expected
        expect(elements[0].textContent).toBe("1234");
        expect(elements[1].textContent).toBe("My Label");
        expect(elements[2].textContent).toBe("Testing out the table row");

    });

    // for the next test, we will set "showHeader" to true.  In this case, we expect that we should have 
    // the correct number of column headers setup and that they match the data passed in via columns (i.e., 1 = "ID", 2 = "Label", etc.)
    it('renders the column headers', () => {
        props.showHeader = true;
        render(<GLTableRow {...props}/>);
        const headerElements:HTMLElement[] = screen.getAllByTestId("gl-table-row-col-header");
        // no column headers (since showHeader is undefined)
        expect(headerElements.length).toBe(3);
    });

    // test out the renderFunction functionality to ensure that when the function is passed in it is called and renders DOM as expected
    it('calls the renderFunction and displays the returned DOM as expected', () => {
        const myRenderFunction = (data: any, dataField?:string) => {
            if(dataField){
                return <div>I am rendered DOM from a render function and my id is {data[dataField]}</div>;
            }
            return <div>No dataField set</div>;
        }
        props.columns = [
            { "dataField": "id", "header": "ID", "size": 4, "renderFunction": myRenderFunction },
            { "dataField": "label", "header": "Label", "size": 4 },
            { "dataField": "desc", "header": "Description", "size": 4 },
        ];
        render(<GLTableRow {...props}/>);
        const elements:HTMLElement[] = screen.getAllByTestId("gl-table-row-cell");
        expect(elements[0].textContent).toBe("I am rendered DOM from a render function and my id is 1234");
    });

    // perform a series of styling tests to ensure styles passed in are applied as expected
    it('applies style changes as expected', () => {
        props.showHeader = true;
        props.styles = {
            headerRow: {
                "lineHeight": "40px",
                "padding": "35px"
            },
            colHeader: {
                "fontWeight": "300",
                "fontSize": "18px",
                "color": "#FF0000"
            },
            styledRow: {
                "lineHeight": "40px",
                "padding": "5px 10px 5px 10px",
                "borderTop": "none"
            },
            styledCol: {
                "fontWeight": "200",
                "fontSize": "11px",
                "color": "#00FF00"
            }
        }
        render(<GLTableRow {...props}/>);
        
        // header row
        const headerRowElements:HTMLElement[] = screen.getAllByTestId("gl-table-row-header");
        const headerRowStyles = window.getComputedStyle(headerRowElements[0]);
        expect(headerRowStyles.lineHeight).toBe("40px");
        expect(headerRowStyles.padding).toBe("35px");
        // column headers
        const columnHeaderElements:HTMLElement[] = screen.getAllByTestId("gl-table-row-col-header");
        const columnHeaderStyles = window.getComputedStyle(columnHeaderElements[0]);
        expect(columnHeaderStyles.fontWeight).toBe("300");
        expect(columnHeaderStyles.fontSize).toBe("18px");
        expect(columnHeaderStyles.color).toBe(hexToRgb("#FF0000"));
        // table rows
        const tableRowElements:HTMLElement[] = screen.getAllByTestId("gl-table-row");
        const tableRowStyles = window.getComputedStyle(tableRowElements[0]);
        expect(tableRowStyles.lineHeight).toBe("40px");
        expect(tableRowStyles.padding).toBe("5px 10px 5px 10px");
        expect(tableRowStyles.borderTop).toBe("");
        // table columns
        const tableRowColElements:HTMLElement[] = screen.getAllByTestId("gl-table-row-cell");
        const tableRowColStyles = window.getComputedStyle(tableRowColElements[0]);
        expect(tableRowColStyles.fontWeight).toBe("200");
        expect(tableRowColStyles.fontSize).toBe("11px");
        expect(tableRowColStyles.color).toBe(hexToRgb("#00FF00"));
    });

    // check the static display, which should ignore the data and dataField and display whatever is passed in
    it('renders the staticDisplay as expected', () => {
        props.columns = [
            { "dataField": "id", "header": "ID", "size": 4, "staticDisplay": "Just Display This Text Only" },
            { "dataField": "label", "header": "Label", "size": 4 },
            { "dataField": "desc", "header": "Description", "size": 4 },
        ];
        render(<GLTableRow {...props}/>);
        const elements:HTMLElement[] = screen.getAllByTestId("gl-table-row-cell");
        expect(elements[0].textContent).toBe("Just Display This Text Only");
    });
  });
});