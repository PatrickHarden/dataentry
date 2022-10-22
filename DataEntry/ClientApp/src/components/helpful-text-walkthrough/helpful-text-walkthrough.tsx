import React, { FC } from 'react';
import styled from 'styled-components';
import { Col, Row } from 'react-styled-flexboxgrid'

const HelpfulTextWalkthrough: FC<any> = () => {
   return (
         <Row>
            <Col xs={2} style={{borderRight:"1px solid #D1D2DD"}}>
               <Plans>
                  Plans
               </Plans>
               <ByCBRE>
                  By Cbre
               </ByCBRE>
            </Col>
            <Col xs={10}>
               <Body>
                  In need of a 3D tour? Floored Plans is available for free!<br/>
                  Get an interactive floor plan with a 3D tour.<br/> 
                  For details <a href="https://cbre.sharepoint.com/sites/intra-DigitalTechnology/SitePages/Technologies/Advisory/Floored---Plans.aspx" target="_blank">CLICK HERE</a> or <a href="https://cbreemail.com/s/b903b8de3ba6e51c97a6e874d438e0c682460355" target="_blank">ORDER HERE</a>.
               </Body>
            </Col>
         </Row>
   );
}

const Plans = styled.div`
   color: #696C93;
   text-transform: uppercase;
   text-align: center;
   font-family: 'Futura Md BT Bold';
   font-size: 1em;
   margin-bottom: 3px;
   margin-right:10px;
   margin-left:10px;
`;

const ByCBRE = styled.div`
   color: #696C93;
   text-transform: uppercase;
   word-spacing: 100vw;
   text-align: center;
   font-size: 0.8em;
   margin-right:10px;
   margin-left:10px;
`;

const Body = styled.div`
   color: #696C93;
   margin-left:20px;
   font-size: 0.85em;
   line-height: 1.5;
   position:relative;

   a {
      color: #79C83A;
      text-decoration: none;
   }
`;

export default HelpfulTextWalkthrough;