import React from 'react';
import { mount, shallow } from 'enzyme';
import DraggableAccordion from '../draggable-accordion';

describe('components', () => {

    describe('draggable-accordion', () => {

        const items: any[] = [
            {
                id: 1,
                availableFrom: "02/20/2020",
                brochures: [],
                contactBrokerForPrice: false,
                showPriceWithUoM: false,
                floorplans: [],
                leaseTerm: "monthly",
                maxPrice: 25.55,
                maxSpace: 9950,
                measure: "sf",
                minPrice: 0,
                minSpace: 0,
                name: "Top Floor",
                photos: [{
                    active: true,
                    displayText: "Top floor view",
                    externalURL: "",
                    fileRef: [],
                    fileType: "png",
                    id: "0a94684e-978d-4fb5-b27c-0df9d261874c",
                    order: 1,
                    primary: true,
                    url: "http://cbre.azure/images/0a94684e-978d-4fb5-b27c-0df9d261874c"
                }],
                spaceId: "257e85b4-200f-4c7c-9fbc-525837d3afbd",
                spaceType: "office",
                status: "available",
            },
            {
                id: 52,
                availableFrom: "02/20/2020",
                brochures: [],
                contactBrokerForPrice: false,
                showPriceWithUoM: false,
                floorplans: [],
                leaseTerm: "monthly",
                maxPrice: 15.75,
                maxSpace: 500,
                measure: "sf",
                minPrice: 0,
                minSpace: 0,
                name: "Back Office",
                photos: [{
                    active: true,
                    displayText: "Top floor view",
                    externalURL: "",
                    fileRef: [],
                    fileType: "png",
                    id: "0a94684e-978d-4fb5-b27c-0df9d261874c",
                    order: 1,
                    primary: true,
                    url: "http://cbre.azure/images/0a94684e-978d-4fb5-b27c-0df9d261874c"
                }],
                spaceId: "4cc09f57-62a1-4316-b487-565b5f1ecfab",
                spaceType: "office",
                status: "available",
            }]

        const inject: any = () => {
            return <div style={{ width: '80vw' }} id="target"><br /><br />pretend this is a form <br /><br /></div>
        }
        const header: string = "Flex spaces"
        const changeHandler = (values: any) => {
            console.log(values)
        }
        it('renders the correct title', () => {
            const component = mount(<DraggableAccordion name="testAccordion" dataProvider={items} inject={inject} header={header} 
                titleField="name" addLabel="Add Space" changeHandler={changeHandler} useLabelForHeader={true}/>);
            expect(component.find('h2').text()).toEqual("Flex spaces");
        });
        it('injects the correct component', () => {
            const component = mount(<DraggableAccordion name="testAccordion" dataProvider={items} inject={inject} header={header} 
                titleField="name" addLabel="Add Space" changeHandler={changeHandler} useLabelForHeader={true}/>);
            expect(component.find('#target').at(0).text()).toEqual("pretend this is a form ");
        });
    });
});