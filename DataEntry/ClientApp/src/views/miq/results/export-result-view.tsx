import React, { FC } from 'react';
import styled from 'styled-components';
import { Col, Row } from 'react-styled-flexboxgrid';
import { MIQExportMessage, MIQResultType } from '../../../types/miq/miqExportMessaging';
import iconSuccess from '../../../assets/images/png/icon-success-lg.png';
import iconError from '../../../assets/images/png/icon-error.png';
import { TeamMember } from '../../../types/team/teamMember';
import TeamMemberSingle from '../../teams/team-member-single';

interface Props {
    exportMessage: MIQExportMessage
    users?: TeamMember[]
}

const ExportResultView: FC<Props> = (props) => {

    const { exportMessage, users } = props;

    const getIcon = () => {
        let icon = iconSuccess;
        switch(exportMessage.type){
            case MIQResultType.SUCCESS:
                icon = iconSuccess;
                break;
            case MIQResultType.ERROR:
                icon = iconError;
                break;
        }
        if(icon){
            return <IndicatorIcon><img src={icon}/></IndicatorIcon>
        }
        return <></>;
    }
  
    return (
        <Container>
            <Header>
                { getIcon() }
                {exportMessage.type === MIQResultType.SUCCESS && <HeaderText>{exportMessage.header}</HeaderText>}
                {exportMessage.type === MIQResultType.ERROR && <ErrorHeader>{exportMessage.header}</ErrorHeader>}
            </Header>
            <Body>
                {(exportMessage.type === MIQResultType.SUCCESS) && users && users.map((teamMember: TeamMember, index: number) => (
                            <Row style={{ borderBottom: "1px solid #e8e8e8" }}>
                                <Col xs={12} key={teamMember.teamMemberId + new Date().getTime() + index + ' ' + String((Math.floor((Math.random() * 100) + 1) / Math.floor((Math.random() * 100) + 1)) * Math.floor((Math.random() * 100) + 1))}>
                                    <TeamMemberSingle teamMember={teamMember} key={index} />
                                </Col>
                            </Row>
                ))}
                {exportMessage.type === MIQResultType.ERROR && exportMessage.body && exportMessage.body.length > 0 &&
                    <ErrorMessage>{exportMessage.body}</ErrorMessage>
                }

                {!users && exportMessage.body}
            </Body>
        </Container>
    );
};

const Container = styled.div`
    width: 100%;
    min-height: 250px;
    background: rgba(216, 216, 216, 0.1);
    border: 1px solid rgba(204, 204, 204, 0.7);
    box-sizing: border-box;
    margin-bottom: 35px;
    padding: 40px;
`;

const Header = styled.div`
    width: 100%;
`;  

const IndicatorIcon = styled.div`
    display: inline-block;
    margin-right: 40px;
    margin-top: 3px;
`;

const HeaderText = styled.div`
    display: inline-block;
    color: #006A4D;
    font-size: 21px;
    font-weight: bold;
    vertical-align: top;
`;

const Body = styled.div`
    color: #666666;
    font-size: 21px;
    margin-top: 35px;
    margin-left: 60px;
    width: 100%;
    font-weight: normal;
`;

const ErrorHeader = styled.div`
    color: darkred;
    display: inline-block;
    font-size: 21px;
    font-weight: bold;
    vertical-align: top;
`;

const ErrorMessage = styled.div`
    color: darkred;
`;

export default ExportResultView;