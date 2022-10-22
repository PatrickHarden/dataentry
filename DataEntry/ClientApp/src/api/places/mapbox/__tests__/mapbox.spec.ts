import { convertMapboxResults, createAddressFromMapboxResult,  } from '../mapbox'
import { AutoCompleteResult } from '../../../../types/common/auto-complete';
import { Address } from '../../../../types/listing/address';

describe('Convert Mapbox Results', () => {
    it("Should properly convert to the AutoCompleteResult object that the component will consume", () => {
        const results = [
            {
                'place_name': 'test1',
                'result': {},
            },
            {
                'place_name': 'test2',
                'result': 'mchicken',
            },
            {
                'place_name': 'test3',
                'result': [],
            }
        ];

        const converted:AutoCompleteResult[] = convertMapboxResults(results);
        expect(converted.length).toBe(3);
        expect(converted[0].name).toEqual('test1');
        expect(converted[0].value).toEqual({
            'place_name': 'test1',
            'result': {},
        });
        expect(converted[1].name).toEqual('test2');
        expect(converted[1].value).toEqual({
            'place_name': 'test2',
            'result': 'mchicken',
        });
        expect(converted[2].name).toEqual('test3');
        expect(converted[2].value).toEqual({
            'place_name': 'test3',
            'result': [],
        });
    });
});

describe('Create Address from Mapbox Result', () => {
    it("Should properly convert an address given a Mapbox Result", () => {
        const simulatedMapboxResult:AutoCompleteResult = {
            'name': 'result',
            'value': {
                'center': [103.8198, 1.3521],
                'context': [
                    {'id': 'postcode123', 'text': '80000'},
                    {'id': 'place123', 'text': 'Denver'},
                    {'id': 'region123', 'text': 'Colorado'},
                    {'id': 'country123', 'text': 'United States'},
                ],  
                'address': 'somewhere',
                'text': 'over the rainbow'
            }
        };

        const address:Address = createAddressFromMapboxResult(simulatedMapboxResult);
        expect(address.lat).toBe(1.3521);
        expect(address.lng).toBe(103.8198);
        expect(address.postalCode).toBe('80000');
        expect(address.city).toBe('Denver');
        expect(address.stateOrProvince).toBe('Colorado');
        expect(address.country).toBe('United States');
        expect(address.street).toBe('somewhere over the rainbow');
    });
});
