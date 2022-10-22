import { Column } from './Column';

export interface RowReplacement {
    replaceAt: number,       // index of row to replace column definitions
    columns:Column[],        // the columns to replace
    alternateData?: any,     // the data to use for this row,
    styles?: any             // override styles for this row
}