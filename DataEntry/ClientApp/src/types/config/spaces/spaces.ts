import { ConfigFieldType, SizeTypeConfig } from '../config';
import { GridConfig } from '../common/grid';
import { FormInputProps } from '../../../components/form-input/form-input';
import { FormTabbedTextAreaProps } from '../../../components/form-tabbed-text-area/form-tabbed-text-area';
import { FormSelectProps } from '../../../components/form-select/form-select';
import { FormDateFieldProps } from '../../../components/form-date-field/form-date-field';
import { FormCheckboxProps } from '../../../components/form-checkbox/form-checkbox';
import { SizeTypeProps } from '../../../views/listingentry/sizesAndMeasurements/sizes-and-measurements';

export interface SpacesConfig {
    accordionSetups: SpacesAccordionSetup[],
    fields: SpacesPropertyTypeFields[]
}

export interface SpacesAccordionSetup{
    propertyTypes: string[],
    header: string,
    useLabelForPaneHeader: boolean
}

export interface DisplayModifier {
    viewProperty: string,
    prepend?: string,
    append?: string
}

export interface SpacesPropertyTypeFields {
    propertyTypes: string[],
    sale: SpacesListingTypeFields,
    lease: SpacesListingTypeFields,
    salelease: SpacesListingTypeFields,
    investment: SpacesListingTypeFields
}

export interface SpacesListingTypeFields {
    name: SpacesFieldSetup<FormTabbedTextAreaProps>,
    spaceDescription: SpacesFieldSetup<FormTabbedTextAreaProps>,
    measure: SpacesFieldSetup<FormSelectProps>,
    minSpace: SpacesFieldSetup<FormInputProps>,
    maxSpace: SpacesFieldSetup<FormInputProps>,
    status: SpacesFieldSetup<FormSelectProps>,
    availableFrom: SpacesFieldSetup<FormDateFieldProps>,
    leaseType: SpacesFieldSetup<FormSelectProps>,
    leaseRateType: SpacesFieldSetup<FormSelectProps>,
    leaseTerm: SpacesFieldSetup<FormSelectProps>,
    spaceType: SpacesFieldSetup<FormSelectProps>,
    minPrice: SpacesFieldSetup<FormInputProps>,
    maxPrice: SpacesFieldSetup<FormInputProps>,
    contactBrokerForPrice: SpacesFieldSetup<FormCheckboxProps>,
    showPriceWithUoM: SpacesFieldSetup<FormCheckboxProps>,
    includeFiles: boolean,
    spaceUnitOfMeasure: SpacesFieldSetup<FormSelectProps>,
    video: SpacesFieldSetup<FormInputProps>,
    walkThrough: SpacesFieldSetup<FormInputProps>,
    spaceSizes: SpacesFieldSetup<SizeTypeProps>
}

export interface SpacesFieldSetup<T> {
    show: boolean,
    type?: ConfigFieldType,
    order: number,
    properties: T,
    grid: GridConfig,
    viewProperties?: Map<string,string>,
    className?: string,
    modifiers?: Map<string,SpacesFieldModifier>
}

export interface SpacesFieldModifier {
    prepend: string
}