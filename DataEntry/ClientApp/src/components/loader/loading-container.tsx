import React, { FC, useState, useEffect } from 'react';
import styled from 'styled-components';
import StyledButton from '../styled-button/styled-button'

interface LoadingContainerProps {
    isLoading: boolean,
    psuedoEditNav?: boolean,
    navTitle?: string,
    forceNavUp?: boolean
}

const LoadingContainer: FC<LoadingContainerProps> = (props) => {
    const [loading, setLoading] = useState(props.isLoading);
    const [fade, setFade] = useState(false);
    const [nav, setNav] = useState(true)

    const { forceNavUp } = props;

    // todo: we need to re-factor this to avoid multiple states

    useEffect(() => {
        if (!loading && props.isLoading) {
            setLoading(true)
        }
        if (loading && !fade) {
            setTimeout(() => setFade(true), 400)
            setTimeout(() => setNav(false), 1000)
        }
    })

    return (
        <LoadContainer className='loading-container'>
            {((nav && props.psuedoEditNav) || forceNavUp)  &&
                <Nav>
                    <BarContainer>
                        <Container className="container">
                            <NavTitle>
                                {props.navTitle && <>{props.navTitle}</>}
                            </NavTitle>
                            <Buttons>
                                &nbsp;
                            </Buttons>
                        </Container>
                    </BarContainer>
                </Nav>
            }
            <span className={fade ? 'isFade' : ''}>{props.children}</span>
        </LoadContainer>
    )
}

const LoadContainer = styled.div`
    > span {
        transition: opacity .2s;
        -webkit-transition: opacity .2s;
        opacity:0;
        display:flex;
    }
    > span.isFade {
        opacity:1;
    }
    position:relative;
`;


const Nav = styled.div`
    position:absolute;
    top:0;
    left:0;
    right:0;
    z-index:11;
    box-shadow: 0 4px 6px rgba(204,204,204,0.12), 0 2px 4px rgba(204,204,204,0.24);
`;

const BarContainer = styled.div`
    background: #006A4D;
    padding:12px 0;
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
`;

const Container = styled.div`
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
    display:flex;
    justify-content:space-between;
`;

const NavTitle = styled.div`
    padding-top:8px;
    width: 300px;	
    color: #FFFFFF;
    font-family: ${props => (props.theme.font ? props.theme.font.bold : 'sans-serif')};
    font-size: 18px;
    line-height: 21px;
`;

const Buttons = styled.div`
    > button {
        margin-right:8px;
    }
    > button:last-of-type {
        margin-right:0;
    }
    height: 36px;
`;

export default LoadingContainer;