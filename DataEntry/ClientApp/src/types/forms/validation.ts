export interface Validation {
    fieldName: string,
    type: FieldType,
    required?: boolean,
    min?: number,
    max?: number,
    regex?: string,
    notOneOf?: string[],
    messages:object
}

export enum FieldType {
    STRING = "STRING",
    NUMBER = "NUMBER",
    DATE = "DATE",
    BOOLEAN = "BOOLEAN",
    ARRAY = "ARRAY"
}

export enum MessageKey {
    REQUIRED = 'required',
    MIN = 'min',
    MAX = 'max',
    REGEX = 'regex',
    NOTONEOF = 'notoneof'
}