export interface Button {
    id: string,
    label?: string,
    icon?: any,
    overIcon?: any,
    itemIndex?: number,
    allowFocus?: boolean,
    allowClick?: boolean,
    clickHandler?: Function
}