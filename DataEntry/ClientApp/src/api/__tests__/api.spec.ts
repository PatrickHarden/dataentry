import { getData, postData } from '../glAxios'

describe('Testing react-adal axios calls', () => {
    it("should invoke adalApiFetch", () => {
        getData('api/test/helloworld')
        .then(response => {
            expect(response).toEqual('User login is required')
        });
    });
});

describe('Testing GraphQL Post', () => {
    it("should invoke adalApiFetch", () => {
        postData('{listings{name}}')
        .then(response => {
            expect(response).toEqual('User login is required')
        });
    });
});
