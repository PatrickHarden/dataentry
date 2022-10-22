import React, { FC } from 'react';
import styled from 'styled-components';
import { TeamMember } from '../../types/team/teamMember';
import { Col, Row } from 'react-styled-flexboxgrid';


interface Props {
    teamMember: TeamMember
}


const TeamMemberSingle: FC<Props> = (props) => {

    const { teamMember } = props;

    return (
        <BaseTeamMemberCard>
            <Row>
                <Col xs={1}>
                    <UserAvatar>
                        {(teamMember.firstName ? teamMember.firstName.substr(0, 1): '') + ' ' + (teamMember.lastName? teamMember.lastName.substr(0, 1):'')}
                    </UserAvatar>

                </Col>
                <Col xs={4}><MemberNameText>{(teamMember.firstName? teamMember.firstName : '')} {(teamMember.lastName? teamMember.lastName: '')}</MemberNameText></Col>
                <Col xs={4}><MemberText>{teamMember.email}</MemberText></Col>

            </Row>
        </BaseTeamMemberCard>
    );
};

export default React.memo(TeamMemberSingle);

const BaseTeamMemberCard = styled.div`
    height: 50px;
    width: auto;
    margin: auto;
    overflow:hidden;
    position:relative;
    padding:10px;
`;

const MemberNameText = styled.div`
    float:left;
    font-size: 16px;
    font-familty: Futura Md BT Bold;
    line-height: 50px;
    font-weight: 800;
    color:#777;
`;

const MemberText = styled.div`
    float:left;
    font-size: 16px;
    line-height: 50px;
    color:#333;
    font-weight: 500;
`;

const UserAvatar = styled.div`
    margin-top: 5px;
    cursor:pointer;
    background:#ddd;
    border-radius:50%;
    color:#fff;
    font-familty: Futura Md BT Bold;
    text-align:center;
    height:40px;
    width:40px;
    line-height:40px;
    margin-right:13px;
    letter-spacing:-1px;
`;
