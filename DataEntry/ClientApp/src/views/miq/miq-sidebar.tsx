import React, { FC, useState, useEffect, useMemo } from 'react';
import styled from 'styled-components';
// images
import SearchIcon from '../../assets/images/svg/search-on.svg'

import GraySearchIcon from '../../assets/images/svg/search-off.svg';


interface AnchorProps {
    anchors: string[]
}

const MIQSidebar: FC<AnchorProps> = (props) => {
    const [activeIcon, setActiveIcon] = useState(0);

    const { anchors } = props;

    const imageArray: any = [ useMemo(() => (SearchIcon), [])];
    const grayImageArray: any = [ useMemo(() => (GraySearchIcon), []), ]

    // note: this replaced using the routeSelector.  In the future, we may want this to be re-factored to be more wired to the router, but we need it to update state correctly.
    // this fixed a bug where the user would create a listing, save, and then the anchors wouldn't update properly so they'd get taken to a new create page again.
    const pathname = "miq/import";

    const updateIconIndex = (index: number) => {
       setTimeout(() => setActiveIcon(index), 80); // Activates Icon after scrolling 
    }
    const updateScrollIconIndex = (index:number) => {
        setActiveIcon(index)
    }

    const scrollEventListener = () => {
        const items = anchors
            .map((id, index) => {
                const element = document.getElementById(id);
                if (element) {
                    return {
                        inView: isInView(element),
                        element, index
                    };
                } else {
                    return;
                }
            })
        const firstTrueItem = items.find(item => !!item && item.inView);
        if (!firstTrueItem) {
            return; // dont update state
        } else {
            items.map(item => {
                if (item === firstTrueItem) {
                 updateScrollIconIndex(item.index);
                }
            });
        }
    }
    const isInView = (element: HTMLElement) => {
        const offset = 2;
        const rect = element.getBoundingClientRect();

        return rect.top >= 0 - offset && rect.bottom <= window.innerHeight + offset;
    };

    useEffect(() => {
        window.addEventListener('scroll', scrollEventListener);
        return () => {
            window.removeEventListener('scroll', scrollEventListener);
        };
    }, []);

    return (
        <Sidebar>
            <Container>
                {
                    <>
                        {anchors.map((anchor, index) => (
                            <Icon id={"anchor_" + anchor} onClick={() => updateIconIndex(index)} key={index + String(new Date().getTime()) + anchor}>
                                <a href={pathname + '#' + anchor}>
                                    <img src={(index === activeIcon) ? imageArray[index] : grayImageArray[index]} />
                                    <p style={{ color: (index === activeIcon) ? '#006A4D' : '#ccc' }}>{anchor}</p>
                                </a>
                            </Icon>
                        ))}          
                    </> 
                }
            </Container>
        </Sidebar>
    );
};

const Sidebar = styled.div`
    position:sticky;
    top:61px;
`

const Container = styled.div`
    padding-top:45px;
    position:relative;
    left:-8px;
`

const Icon = styled.div`
    width:100px;
    text-align:center;
    min-height: 100px;
    a {
        text-decoration:none; color:inherit;
        p {
            font-size: 12px;
            font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
            line-height: 15px;
            text-transform:uppercase;
            margin-top:4px;
            margin-bottom:24px;
        }
    }
`;

export default MIQSidebar;