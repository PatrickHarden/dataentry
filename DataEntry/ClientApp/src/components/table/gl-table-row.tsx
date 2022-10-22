import React, { FC } from 'react';
import { Row, Col } from 'react-styled-flexboxgrid';
import styled from 'styled-components';
import { generateKey } from '../../utils/keys';
import { Column } from './types/Column';

export interface Props {     
    data: any,
    staticDisplay?: string,
    columns: Column[],
    showHeader?: boolean,
    styles?:GLTableRowStyles   
}

// styles supported by this component : note, interfaces for the sections are below the FC below
export interface GLTableRowStyles { 
    headerRow?: StyledHeaderRowStyles,
    colHeader?: ColHeaderStyles,
    styledRow?: StyledRowStyles,
    styledCol?: StyledColStyles
}

const GLTableRow: FC<Props> = (props) => {
    
    const { data, staticDisplay, columns, showHeader, styles } = props;
     
    return (
        <>  
            {showHeader && 
                <StyledHeaderRow {...styles} data-testid="gl-table-row-header">
                    {
                        columns.map((column:Column) => {
                            return <ColHeader key={generateKey()} xs={column.size ? column.size : 3} {...styles} data-testid="gl-table-row-col-header">
                                    { column.header }
                            </ColHeader>
                        })
                    }
                </StyledHeaderRow>
            }
            <StyledRow {...styles} data-testid="gl-table-row">
            {
                staticDisplay ? <StyledCol>{staticDisplay}</StyledCol> 
                : 
                columns.map((column:Column) => {
                    return <StyledCol key={generateKey()} xs={column.size ? column.size : 3} {...styles} data-testid="gl-table-row-cell">
                            { column.renderFunction ? column.renderFunction(data,column.dataField,column.defaultValue) 
                                : column.staticDisplay ? column.staticDisplay 
                                : data && column.dataField && data[column.dataField] }
                    </StyledCol>
                })
            }
            </StyledRow>
        </> 
    )
}

// interfaces for styles
interface StyledHeaderRowStyles {
    lineHeight?: string,
    padding?: string
}

interface ColHeaderStyles {
    fontWeight?: string,
    fontSize?: string,
    color?: string
}

interface StyledRowStyles {
    lineHeight?: string,
    padding?: string,
    borderTop?: string
}

interface StyledColStyles {
    fontWeight?: string,
    fontSize?: string,
    color?: string,
    alignItems?: string
}

// header styles
const StyledHeaderRow = styled(Row as any)`
    max-width: 100%;
    line-height: ${(props: GLTableRowStyles) => props.headerRow && props.headerRow.lineHeight ? props.headerRow.lineHeight : '20px'};
    padding: ${(props: GLTableRowStyles) => props.headerRow && props.headerRow.padding ? props.headerRow.padding : '5px 0 5px 0'};
`;

const ColHeader = styled(Col as any)`
    font-weight: ${(props: GLTableRowStyles) => props.colHeader && props.colHeader.fontWeight ? props.colHeader.fontWeight : '500'};
    font-size: ${(props: GLTableRowStyles) => props.colHeader && props.colHeader.fontSize ? props.colHeader.fontSize : '12px'};
    color: ${(props: GLTableRowStyles) => props.colHeader && props.colHeader.color ? props.colHeader.color : '#666666'};
`;

// normal row/column styles
const StyledRow = styled(Row as any)`
    max-width: 100%;
    line-height: ${(props: GLTableRowStyles) => props.styledRow && props.styledRow.lineHeight ? props.styledRow.lineHeight : '20px'};
    padding: ${(props: GLTableRowStyles) => props.styledRow && props.styledRow.padding ? props.styledRow.padding : '5px 0 5px 0'};
    border-top: ${(props: GLTableRowStyles) => props.styledRow && props.styledRow.borderTop ? props.styledRow.borderTop : '1px solid #CCCCCC'};
`;

const StyledCol = styled(Col as any)`
    font-weight: ${(props: GLTableRowStyles) => props.styledCol && props.styledCol.fontWeight ? props.styledCol.fontWeight : '500'}
    font-size: ${(props: GLTableRowStyles) => props.styledCol && props.styledCol.fontSize ? props.styledCol.fontSize : '14px'};
    color: ${(props: GLTableRowStyles) => props.styledCol && props.styledCol.color ? props.styledCol.color : '#555555'};
    display: flex !important;
    align-items: ${(props: GLTableRowStyles) => props.styledCol && props.styledCol.alignItems ? props.styledCol.alignItems : 'center'};
`;
  
export default GLTableRow;