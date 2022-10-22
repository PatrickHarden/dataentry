import React from 'react';
import { shallow, mount } from 'enzyme';
import ImageAvatar, { ImageAvatarSize, PlaceholderSize } from '../image-avatar';

describe("ImageAvatar", () => {

    let props: any;
    let wrapper: any;

    describe("No Upload Allowed", () => {

        beforeEach(() => {
            props = {
                allowUpload: false,
                avatarSize: ImageAvatarSize.SMALL,
                placeholderSize: PlaceholderSize.SMALL,
                uploadErrors: "",
                placeholder: "IN"
            }
        });

        // test that the placeholder works and is displayed when no image url
        it("Placeholder text is set as expected", () => {
            wrapper = mount(<ImageAvatar {...props}/>);
            expect(wrapper.find('div[data-test="ia-placeholder"]').text()).toEqual("IN");
        });

        // test that if we have an image, the placeholder should not render
        it("Placeholder text is set as expected", () => {
            props.imageURL = "http://placehold.jp/150x150.png";
            wrapper = mount(<ImageAvatar {...props}/>);
            expect(wrapper.find('div[data-test="ia-placeholder"]')).toEqual({});
        });

        // test that if we have an image, the image should have the expected source
        it("Image src url is set as expected", () => {
            props.imageURL = "http://placehold.jp/150x150.png";
            wrapper = mount(<ImageAvatar {...props}/>);
            expect(wrapper.find('img[data-test="ia-avatar"]').prop("src")).toEqual("http://placehold.jp/150x150.png");
        });
    });

    describe("Image Upload is Allowed", () => {

        beforeEach(() => {
            props = {
                allowUpload: true,
                avatarSize: ImageAvatarSize.SMALL,
                placeholderSize: PlaceholderSize.SMALL,
                uploadErrors: "",
                placeholder: "IN"
            }
        });

        // test that the placeholder works and is displayed when no image url
        it("Placeholder text is set as expected", () => {
            wrapper = mount(<ImageAvatar {...props}/>);
            expect(wrapper.find('div[data-test="ia-placeholder"]').text()).toEqual("IN");
        });

        // test that if we have an image, the placeholder should not render
        it("Placeholder text is set as expected", () => {
            props.imageURL = "http://placehold.jp/150x150.png";
            wrapper = mount(<ImageAvatar {...props}/>);
            expect(wrapper.find('div[data-test="ia-placeholder"]')).toEqual({});
        });

        // test that if we have an image, the image should have the expected source
        it("Image src url is set as expected", () => {
            props.imageURL = "http://placehold.jp/150x150.png";
            wrapper = mount(<ImageAvatar {...props}/>);
            expect(wrapper.find('img[data-test="ia-avatar"]').prop("src")).toEqual("http://placehold.jp/150x150.png");
        });

        // test that if we have errors set, the circle is an exclamation mark
        it("Errors are set: exclamation mark shows", () => {
            props.uploadErrors = "Something went wrong";
            wrapper = mount(<ImageAvatar {...props}/>);
            expect(wrapper.find('div[data-test="ia-error-indicator"]').text()).toEqual("!");
        });

        // test that if we have errors set, the error text displays
        it("Errors are set: exclamation mark shows", () => {
            props.uploadErrors = "Something went wrong";
            wrapper = mount(<ImageAvatar {...props}/>);
            expect(wrapper.find('div[data-test="ia-errors"]').text()).toEqual("Something went wrong");
        });
    });
});