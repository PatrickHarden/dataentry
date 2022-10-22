import { convertValidationJSON } from '../validation-adapter';
import { FieldType } from '../../../types/forms/validation';

describe("validation-adapter", () => {

    let mockJson:object;
    let yupSchema:any;

    beforeEach(() => {
        mockJson = {
            'stringField': {
                type: FieldType.STRING,
                required: true,
                min: 5,
                max: 100,
                regex: 'abc+',
                messages: {
                    'required': 'stringField is required so do not forget it!',
                    'min': 'min is 5',
                    'max': 'max is 100',
                    'regex': 'regex is wrong'
                }
            },
            'numberField': {
                type: FieldType.NUMBER,
                required: true,
                min: 1,
                max: 5,
                messages: {} 
            }
        },
        yupSchema = convertValidationJSON(mockJson);     
    });

    it('yup schema is an object', () =>{
        expect(typeof yupSchema).toBe('object')
    });

    it('yup schema has a fields object', () =>{
        expect(yupSchema).toHaveProperty('fields');
    });

    it('has two fields to validate', () =>{
        expect(Object.keys(yupSchema.fields).map(key => key).length).toEqual(2)
    });

    it('stringField is required', () => {
        expect(yupSchema.fields.stringField.tests[3].OPTIONS.name).toEqual("required")
    });

    it('stringField has a custom required message', () => {
        expect(yupSchema.fields.stringField.tests[3].OPTIONS.message).toEqual("stringField is required so do not forget it!")
    });

    it('numberField has a max of 5', () => {
        expect(yupSchema.fields.numberField.tests[1].OPTIONS.params.max).toEqual(5)
    });

    it('numberField has a default message', () => {
        expect(yupSchema.fields.numberField.tests[1].OPTIONS.message).toEqual("numberField: Maximum value exceeded: 5")
    })
});