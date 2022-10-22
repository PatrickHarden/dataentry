import React, { FC, useContext } from 'react'
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { Col, Row } from 'react-styled-flexboxgrid'
import { GLAnalyticsContext } from '../../../components/analytics/gl-analytics-context';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEnvelope } from '@fortawesome/free-solid-svg-icons';

import { useSelector } from 'react-redux';
import { ConfigDetails } from '../../../types/config/config';
import { configDetailsSelector } from '../../../redux/selectors/system/config-details-selector';

import Logo from '../../../assets/images/png/GL-Logo-White.png'
import IconSupport from '../../../assets/images/png/icon-support.png'
import IconSupportArrow from '../../../assets/images/png/icon-support-arrow.png'

const Footer: FC = () => {

    const analytics = useContext(GLAnalyticsContext);
    const configDetails: ConfigDetails = useSelector(configDetailsSelector);

    const date = new Date();

    return (
        <FooterRoot>
            <FooterContainer>
                <Row id="topFooterRow">
                    <Col sm={1} md={1} lg={1}>
                        <Link to="/" onClick={() => analytics.fireEvent('footerLogo', 'click', 'Redirect home', 'beacon')}>
                            <img src={Logo} alt="footerLogo" />
                        </Link>
                    </Col>
                    {configDetails.config.supportLink && <Col sm={6} md={6} lg={6}>
                        <p onClick={() => analytics.fireEvent('footerContactSupport', 'click', 'External contact support link')}>
                            <a id="footerCustomerSupport" target="_blank" href="https://cbre.service-now.com/ep?id=ep_sc_cat_item&sys_id=04a269e41b40c4103d88caad1e4bcbde&sysparm_domain_restore=false&sysparm_stack=no">
                                <img src={IconSupport} />
                                <span>SUBMIT A SUPPORT REQUEST</span>
                                <img src={IconSupportArrow} />
                            </a>
                        </p>
                        {/* <span><a href="tel:1-877-435-7547">1-877-435-7547</a> | <a href="email:servicedesk@cbre.com">servicedesk@cbre.com</a></span> */}
                    </Col>}
                </Row>
            </FooterContainer>
            <div id="whiteFooterBorder">
                <FooterContainer>
                    <Row id="bottomFooterRow">
                        <Col md={4} sm={12}>
                            <p>&copy; {date.getFullYear()} CBRE. All Rights Reserved.</p>
                        </Col>
                    </Row>
                </FooterContainer>
            </div>
        </FooterRoot>
    );
};

const FooterRoot = styled.footer`
    background-color: #006A4D;
    color:#fff;
    font-size:12px;

    a {
        color:#fff;
        text-decoration:none;
    }

    #whiteFooterBorder {
        border-top: 1px solid rgba(255,255,255,0.4);
        padding:5px 0 15px 0;
    }

    #topFooterRow {
        p {
            text-transform:uppercase;
            font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
            margin-bottom:2px;
            margin-top:-2px;
            svg {
                margin-right:7px;
            }
        }
        padding:30px 0 25px 0;
        align-items:center;
    }
    #footerCustomerSupport {
        display: flex;
        align-items: center;
        span {
            margin: 0 10px;
        }
    }
`;

const FooterContainer = styled.footer`
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
`;

export default Footer;