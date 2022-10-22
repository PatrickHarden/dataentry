export enum MIQResultType {
    SUCCESS,
    ERROR
}

export interface MIQExportMessage {
    show: boolean,
    type?: MIQResultType,
    header?: string,
    body?: any
}