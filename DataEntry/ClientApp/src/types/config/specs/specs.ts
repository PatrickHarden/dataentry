import { FormSelectProps } from '../../../components/form-select/form-select';
import { FormInputProps } from '../../../components/form-input/form-input';
import { FormCheckboxProps } from '../../../components/form-checkbox/form-checkbox';
import { GridConfig } from '../common/grid';

export interface SpecsConfig {
    label: string,
    currency: string,
    normalFields: SpecsPropertyTypeFields,
    flexFields: SpecsPropertyTypeFields,
    shophouseFields: SpecsPropertyTypeFields
}

export interface SpecsPropertyTypeFields {
    values: string[],
    sale: SpecsListingTypeFields,
    lease: SpecsListingTypeFields,
    saleLease: SpecsListingTypeFields,
    investment: SpecsListingTypeFields
}

export interface SpecsListingTypeFields {
    leaseType: SpecsFieldSetup<FormSelectProps>,
    leaseRateType: SpecsFieldSetup<FormSelectProps>,
    measure: SpecsFieldSetup<FormSelectProps>,
    leaseTerm: SpecsFieldSetup<FormSelectProps>,
    minSpace: SpecsFieldSetup<FormInputProps>,
    maxSpace: SpecsFieldSetup<FormInputProps>,
    totalSpace: SpecsFieldSetup<FormInputProps>,
    spacePlaceholder: PlaceholderSetup,
    minPrice: SpecsFieldSetup<FormInputProps>,
    maxPrice: SpecsFieldSetup<FormInputProps>,
    salePrice:  SpecsFieldSetup<FormInputProps>,
    plusSalesTax: SpecsFieldSetup<FormCheckboxProps>,
    includingSalesTax: SpecsFieldSetup<FormCheckboxProps>,
    contactBrokerForPrice: SpecsFieldSetup<FormCheckboxProps>,
    showPriceWithUoM: SpecsFieldSetup<FormCheckboxProps>,
    bedrooms?: SpecsFieldSetup<FormCheckboxProps>
}

export interface SpecsFieldSetup<T> {
    show: boolean,
    grid: GridConfig,
    properties: T
}

export interface PlaceholderSetup extends GridConfig {
    show: boolean
}