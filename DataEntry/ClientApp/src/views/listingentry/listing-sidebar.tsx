import React, { FC, useState, useEffect, useMemo } from 'react';
import styled from 'styled-components';
import { Listing } from '../../types/listing/listing';
// images
import BuildingIcon from '../../assets/images/svg/property-on.svg';
import SpecIcon from '../../assets/images/svg/specs-on.svg';
import SpacesIcon from '../../assets/images/svg/spaces-on.svg';
import ContactsIcon from '../../assets/images/svg/contacts-on.svg';
import TeamsIcon from '../../assets/images/svg/team-on.svg';
import HighlightsIcon from '../../assets/images/svg/highlights-on.svg'

import GrayBuildingIcon from '../../assets/images/svg/property-off.svg';
import GraySpecIcon from '../../assets/images/svg/specs-off.svg';
import GraySpacesIcon from '../../assets/images/svg/spaces-off.svg';
import GrayContactsIcon from '../../assets/images/svg/contacts-off.svg';
import GrayTeamsIcon from '../../assets/images/svg/team-off.svg';
import GrayHighlightsIcon from '../../assets/images/svg/highlights-off.svg';
import { routeSelector } from '../../redux/selectors/router/route-selector';
import { useSelector } from 'react-redux';


interface AnchorProps {
    anchors: string[],
    listing: Listing,
    overrideSpecification: string
}

const ListingSidebar: FC<AnchorProps> = (props) => {
    const [activeIcon, setActiveIcon] = useState(0);

    const { anchors, listing, overrideSpecification } = props;

    const imageArray: any = [useMemo(() => (TeamsIcon), []), useMemo(() => (BuildingIcon), []), useMemo(() => (HighlightsIcon), []), useMemo(() => (SpecIcon), []), useMemo(() => (SpacesIcon), []), useMemo(() => (ContactsIcon), [])];
    const grayImageArray: any = [useMemo(() => (GrayTeamsIcon), []), useMemo(() => (GrayBuildingIcon), []), useMemo(() => (GrayHighlightsIcon), []), useMemo(() => (GraySpecIcon), []), useMemo(() => (GraySpacesIcon), []), useMemo(() => (GrayContactsIcon), [])]
    
    const anchorLinkDisplayOverrides = {
        "team": "Data Entry Teams"
    }

    const pathname = useSelector(routeSelector);

    const updateIconIndex = (index: number) => {
       setTimeout(() => setActiveIcon(index), 80); // Activates Icon after scrolling 
    }
    const updateScrollIconIndex = (index:number) => {
        setActiveIcon(index)
    }

    const findLinkDisplay = (anchor:string) => {
        if (anchor==="specifications" && overrideSpecification) {
            return overrideSpecification;
        }
        if (anchor==="highlights" && overrideSpecification) {
            return "specifications";
        }
        return anchorLinkDisplayOverrides && anchorLinkDisplayOverrides[anchor] ? anchorLinkDisplayOverrides[anchor] : anchor;
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
                    listing.propertyType && listing.propertyType.length > 0 && listing.listingType && listing.listingType.length > 0 ?
                    <>
                        {anchors.map((anchor, index) => (
                            <Icon id={"anchor_" + anchor} onClick={() => updateIconIndex(index)} key={index + String(new Date().getTime()) + anchor}>
                                <a href={pathname + '#' + anchor}>
                                    <img src={(index === activeIcon) ? imageArray[index] : grayImageArray[index]} />
                                    <p style={{ color: (index === activeIcon) ? '#006A4D' : '#ccc' }}>{findLinkDisplay(anchor)}</p>
                                </a>
                            </Icon>
                        ))}          
                    </> :
                    <> 
                         {anchors.map((anchor, index) => (
                            <React.Fragment key={anchor + String(new Date().getTime())}>
                                {(anchor !== "specifications" && anchor !== "spaces") ?
                                    <Icon id={"anchor_" + anchor} onClick={() => updateIconIndex(index)} key={index + String(new Date().getTime()) + anchor}>
                                        <a href={pathname + '#' + anchor}>
                                            <img src={(index === activeIcon) ? imageArray[index] : grayImageArray[index]} />
                                            <p style={{ color: (index === activeIcon) ? '#006A4D' : '#ccc' }}>{findLinkDisplay(anchor)}</p>
                                        </a>
                                    </Icon> :
                                    null
                                }
                            </React.Fragment>
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

export default ListingSidebar;