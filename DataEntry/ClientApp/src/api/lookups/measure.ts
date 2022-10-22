import { Option } from '../../types/common/option';

/* propertyType: office, retail, industrial */
export const measureStandard: Option[] = [
    { label: 'SM  (Square Meter)', value: 'sm', order: 1 },
    { label: 'SF  (Square Feet)', value: 'sf', order: 2 },
    { label: 'Acre', value: 'acre', order: 3 },
];

/* propertyType: flex */
export const measureFlex: Option[] = [
    { label: 'Per Person', value: 'person', order: 1 },
    { label: 'Per Desk', value: 'desk', order: 2 },
    { label: 'Per Room', value: 'room', order: 3 },
];