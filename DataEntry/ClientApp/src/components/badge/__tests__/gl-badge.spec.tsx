import React from 'react';
import GLBadge, { Props } from '../gl-badge';
import { render, screen } from '@testing-library/react';
import { hexToRgb } from '../../../utils/tests/test-utils';

describe('components', () => {

  describe('gl-badge', () => {

    let props:Props;

    beforeEach(() => {
        props = {
            label: "Some Badge",
            styles: {}
        }
    });

    // check to ensure the label text is set as expected
    it('shows the expected label', () => {
        render(<GLBadge {...props}/>);
        screen.getByText(props.label);
    });

    // check on default styles
    it('shows the correct default styles', () => {
        render(<GLBadge {...props}/>);
        // because we are using styled components, the classname will be generated differently every time
        const ele:HTMLElement = screen.getByTestId("gl-badge");
        const styles = window.getComputedStyle(ele);
        expect(styles.backgroundColor).toBe(hexToRgb("#ffffff"));
        expect(styles.border).toBe("1px solid #006a4d");
        expect(styles.borderRadius).toBe("0");
        expect(styles.height).toBe("24px");
        expect(styles.minWidth).toBe("50px");
        expect(styles.padding).toBe("0px 10px 0px 10px");
        expect(styles.textAlign).toBe("center");
        expect(styles.fontSize).toBe("11px");
        expect(styles.textTransform).toBe("uppercase");
    });

    // check on styles that are changed
    it('shows the correct styles when specified', () => {
        props.styles = {
            background: "#ff0000",
            border: "3px dotted #000066",
            borderRadius: "2px",
            height: "50px",
            minWidth: "100px",
            padding: "5px",
            textAlign: "left",
            fontSize: "16px",
            textTransform: "capitalize"
        };
        render(<GLBadge {...props}/>);
        // because we are using styled components, the classname will be generated differently every time
        const ele:HTMLElement = screen.getByTestId("gl-badge");
        const styles = window.getComputedStyle(ele);
        expect(styles.backgroundColor).toBe(hexToRgb("#ff0000"));
        expect(styles.border).toBe("3px dotted #000066");
        expect(styles.borderRadius).toBe("2px");
        expect(styles.height).toBe("50px");
        expect(styles.minWidth).toBe("100px");
        expect(styles.padding).toBe("5px");
        expect(styles.textAlign).toBe("left");
        expect(styles.fontSize).toBe("16px");
        expect(styles.textTransform).toBe("capitalize");
    });

  });
});