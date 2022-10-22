import React, { FC } from 'react';
import styled from 'styled-components';
import GLTableRow, { GLTableRowStyles } from './gl-table-row';
import { Column } from './types/Column';
import { generateKey } from '../../utils/keys';
import { Grid } from 'react-styled-flexboxgrid';
import { RowReplacement } from './types/RowReplaceColumns';
import StyledButton from '../styled-button/styled-button';
import IconInfo from '../../assets/images/png/icon-info.png';
import { AlternatePostalAddresses } from '../../types/listing/listing';

export interface Props {
    dataProvider: any[],
    columns: Column[],
    replaceColsForRow?: RowReplacement[],    // if any rows have differing column configurations, pass in here
    showHeadersBeforeRow?: number[],
    actionButton?: ActionButton,
    assignBtn: boolean,
    assignHandler?: () => void,
    unpublishBtn: boolean,
    unpublishHandler?: () => void,
    styles?: GLTableStyles,
    spacesImport?: any
}

export interface GLTableStyles {
    table?: TableStyles,
    styledGrid?: GridStyles,
    tableRow?: GLTableRowStyles
}

export interface ActionButton {
    label: string,
    clickHandler: () => void
}

const GLTable: FC<Props> = (props) => {

    const { dataProvider, columns, replaceColsForRow, showHeadersBeforeRow, assignBtn, assignHandler, unpublishBtn, unpublishHandler, actionButton, spacesImport } = props;
    let { styles } = props;
    const alternateAddressData: AlternatePostalAddresses = dataProvider && dataProvider[0].alternatePostalAddresses;
    // if we have an action column, we need to add a max width to the table style
    if (actionButton) {
        if (!styles) { styles = {} };
        if (!styles.table) { styles.table = {} };
        styles.table.maxWidth = "90%";
    }

    // this is a helper function that will get replacements for any property on RowReplacement that we may need
    const findReplacementRow = (atIndex: number): RowReplacement | undefined => {
        const replacementRows: RowReplacement[] | undefined = replaceColsForRow && replaceColsForRow.filter((replaceCol: RowReplacement) => replaceCol.replaceAt === atIndex);
        return replacementRows && replacementRows.length > 0 ? replacementRows[0] : undefined;
    }

    const msgIcon: any = IconInfo;

    return (
        <Table {...styles} data-testid="gl-table">
            <HeaderContent>
                {alternateAddressData && <StyledMessageContainer style={{ "color": '#555555' }}>
                    <StyledMessageContent>
                        <StyledMessageIcon src={msgIcon} />
                        Alternate addresses exist for this property. Create a listing to select the best address!
                    </StyledMessageContent>
                </StyledMessageContainer>}
            </HeaderContent>

            <StyledGrid data-testid="gl-table-styled-grid" spacesImport={spacesImport}>
                {dataProvider.map((data: any, index: number) => {
                    const replacementRow: RowReplacement | undefined = findReplacementRow(index);
                    return <GLTableRow key={generateKey()}
                        data={data}
                        columns={replacementRow && replacementRow.columns ? replacementRow.columns : columns}
                        showHeader={showHeadersBeforeRow && showHeadersBeforeRow.indexOf(index) > -1 ? true : false}
                        styles={replacementRow && replacementRow.styles ? replacementRow.styles : styles && styles.tableRow}
                    />
                })}
            </StyledGrid>
            {
                actionButton && !spacesImport && <ActionColumn>
                    <ActionButton onClick={actionButton.clickHandler} data-testid="gl-table-action-button">{actionButton.label}</ActionButton>
                    {assignBtn && <ActionButton onClick={assignHandler} style={{ marginTop: "5px" }} data-testid="gl-table-assign-button">Assign</ActionButton>}
                    {unpublishBtn && <ActionButton onClick={unpublishHandler} style={{ marginTop: "5px" }} data-testid="gl-table-unpublish-button">Unpublish</ActionButton>}
                </ActionColumn>
            }
        </Table>

    )
}

// style interfaces
interface TableStyles {
    fontFamily?: string,
    background?: string,
    border?: string,
    boxSizing?: string,
    boxShadow?: string,
    borderRadius?: string,
    padding?: string,
    maxWidth?: string,
    height?: string,
    overflowY?: string
}

interface GridStyles {
    padding?: string
}

const Table = styled.div`
    > div {
        max-width: ${(props: GLTableStyles) => props.table && props.table.maxWidth ? props.table.maxWidth : "100%"};
    }
    font-family: ${(props: GLTableStyles) => props.table && props.table.fontFamily ? props.table.fontFamily : "'Futura Md BT', helvetica, arial, sans-serif;"};
    background: ${(props: GLTableStyles) => props.table && props.table.background ? props.table.background : '#FFFFFF;'};
    border: ${(props: GLTableStyles) => props.table && props.table.border ? props.table.border : '1px solid rgba(204, 204, 204, 0.5);'};
    box-sizing: ${(props: GLTableStyles) => props.table && props.table.boxSizing ? props.table.boxSizing : 'border-box;'};
    box-shadow: ${(props: GLTableStyles) => props.table && props.table.boxShadow ? props.table.boxShadow : '0px 4px 5px rgba(225, 225, 225, 0.5);'};
    border-radius: ${(props: GLTableStyles) => props.table && props.table.borderRadius ? props.table.borderRadius : '7px;'};
    padding: ${(props: GLTableStyles) => props.table && props.table.padding ? props.table.padding : '15px'};
    height: ${(props: GLTableStyles) => props.table && props.table.height ? props.table.height : '15px'};
    overflow-y: ${(props: GLTableStyles) => props.table && props.table.overflowY ? props.table.overflowY : 'none'};
`;

const StyledGrid = styled(Grid as any)`
    padding: ${(props: GLTableStyles) => props.styledGrid && props.styledGrid.padding ? props.styledGrid.padding : '0'}; 
    display: inline-block;
    ${(props: any) => props.spacesImport && `
        > div:nth-of-type(1), > div:nth-of-type(2) {
            display:none;
        }
    `}
    margin-top: 10px;
`;

const ActionColumn = styled.div`
    font-size: 20px;
    width: 10%;
    vertical-align: top;
    padding-top: 10px;
    text-align: right;
    float: right;
`;

const ActionButton = styled(StyledButton)`
    background-color: #006A4D;
    border-radius: 4px;
    padding-left: 8px;
    padding-right: 8px;
`;

const StyledMessageContent = styled.div`
float: left;
`
const StyledMessageContainer = styled.div`
    text-align: center;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    border-bottom: ${props => (props.theme.border ? props.theme.border : '#cccccc')};
    background-color: #f5f3ed;
    border-radius: 50px;
    height: 30px;
    padding: 10px
    padding-bottom: 0px;
`;

const StyledMessageIcon = styled.img`
    margin-right: 10px;
    margin-bottom: -5px;
`;

const HeaderContent = styled.div`
    display: flex;
    justify-content: center;
    padding-top: 16px;
    padding-left: 36px;
`;


export default GLTable;