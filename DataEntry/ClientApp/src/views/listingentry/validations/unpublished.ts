import { FieldType } from '../../../types/forms/validation';
import { Listing } from '../../../types/listing/listing';

export const unpublishedValidations:object = {
    'propertyName': {
        type: FieldType.STRING,
        required: true,
        messages: {
            'required': 'A property name is required.'
        }
    }
}