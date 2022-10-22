import Enzyme from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';
import 'jest-styled-components';

jest.mock('mapbox-gl/dist/mapbox-gl', () => ({
    Map: () => ({}),
  }));
  
  
// React 16 Enzyme adapter
Enzyme.configure({ adapter: new Adapter() });