import React, { FC, useContext } from 'react';
import styled, { css } from 'styled-components';
import { TeamMember } from '../../../types/team/teamMember';
import { Team } from '../../../types/team/team';

interface Props {
    teamMember?: TeamMember,
    team?: Team,
    owner?: boolean,
    deleteBtn?: boolean,
    isTeam?: boolean,
    index: number,
    deleteFunc?(member: TeamMember): void,
    deleteTeamFunc?(memberTeam: Team): void
}

const ListingTeamMemberCard: FC<Props> = (props) => {

    const { teamMember, team, owner, deleteBtn, isTeam, index, deleteFunc, deleteTeamFunc } = props;

    
    const callDelete = () => {
        if (deleteFunc !== undefined) {
            if (teamMember) {
                deleteFunc(teamMember);
            }
        }
        if (deleteTeamFunc !== undefined) {
            if (team) {
                deleteTeamFunc(team);
            }
        }
    }

    return (
        <>
            <ListingUserThumbnail style={isTeam ? { backgroundColor: "#E9F3F1" } : { backgroundColor: "#fff" }}>
                <UserAvatar>
                    {teamMember && (teamMember.firstName? teamMember.firstName.substr(0, 1): '') + ' ' + (teamMember.lastName? teamMember.lastName.substr(0, 1) : '')}
                    {team && (team.name? team.name.substr(0, 1).toUpperCase() : '')}
                </UserAvatar>
                {deleteBtn && <DeleteIconDiv onClick={callDelete} >X</DeleteIconDiv>}
                <MemberNameText>
                    {teamMember && (teamMember.firstName? teamMember.firstName: '') + ' ' + (teamMember.lastName? teamMember.lastName : '')}
                    {team && team.name}
                </MemberNameText>
            </ListingUserThumbnail>
            {owner && <DescriptionText>Owner</DescriptionText>}
            {isTeam && <DescriptionText>Team</DescriptionText>}
        </>
    );
};

export default React.memo(ListingTeamMemberCard);

const ListingUserThumbnail = styled.div`
    border: 1px solid #ddd;
    padding: 8px;
    box-shadow: 0 4px 6px rgba(204,204,204,0.12), 0 2px 4px rgba(204,204,204,0.24);
    transition: all 0.3s cubic-bezier(.25,.8,.25,1);
    width: 115px;
    height: 100px;
    overflow: hidden;
    /* &:hover {
        box-shadow: 0 7px 14px rgba(204,204,204,0.20), 0 5px 5px rgba(204,204,204,0.17);
    } */
    /* background-color: white; */
`;

const MemberNameText = styled.div`
    font-size: 12px;
    font-family: Futura Md BT Bold;
    line-height: 15px;
    padding-top:8px;
    color:#9EA8AB;
`;

const DescriptionText = styled.div`
    font-size: 12px;
    line-height: 50px;
    color:#777;
    margin-top: -8px;
    margin-left: 8px;
`;

const DeleteIconDiv = styled.div`
    cursor: pointer;
    margin-top: -45px;
    margin-right: 7px;
    font-size: 16px;
    color: #02c2f2;
    float: right;
`

const UserAvatar = styled.div`
    margin-top: 5px;
    cursor:pointer;
    background:#ddd;
    border-radius:50%;
    color:#fff;
    font-family: Futura Md BT Bold;
    text-align:center;
    height:50px;
    width:50px;
    line-height: 50px;
    margin-right:13px;
    letter-spacing:-1px;
`;