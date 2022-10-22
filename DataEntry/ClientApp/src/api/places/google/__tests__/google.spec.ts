import { convertGoogleResults } from '../google';
import { AutoCompleteResult } from '../../../../types/common/auto-complete';

describe('Convert Google Results', () => {
    it("Should properly convert to the AutoCompleteResult object that the component will consume", () => {
        const results:google.maps.places.AutocompletePrediction[] = [
            {
                place_id: "123456",
                description: "This is a place",
                matched_substrings: [],
                reference: "",
                structured_formatting: {
                    main_text: '',
                    main_text_matched_substrings: [],
                    secondary_text: ''
                },
                types: [],
                terms: []
            }
        ]
            
        const converted:AutoCompleteResult[] = convertGoogleResults(results);
        expect(converted.length).toBe(1);
        expect(converted[0].name).toEqual('This is a place');
        expect(converted[0].value).toEqual({
            place_id: "123456",
            description: "This is a place",
            matched_substrings: [],
            reference: "",
            structured_formatting: {
                main_text: '',
                main_text_matched_substrings: [],
                secondary_text: ''
            },
            types: [],
            terms: []
        });
    });
});