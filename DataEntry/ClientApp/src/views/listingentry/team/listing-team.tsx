import React, { FC, useContext, useState } from 'react';
import { FormContext } from '../../../components/form/gl-form-context';
import { Listing } from '../../../types/listing/listing';
import { Col, Row } from 'react-styled-flexboxgrid';
import SectionHeading from "../../../components/section-heading/section-heading";
import ListingTeamMemberCard from './listing-team-member-card';
import { TeamMember } from '../../../types/team/teamMember';
import { Team } from '../../../types/team/team';
import { authContext } from '../../../adalConfig';
import SearchableInput, { SearchableInputNoDataProps } from '../../../components/searchable-input/searchable-input';
import { AutoCompleteResult, AutoCompleteRequest } from '../../../types/common/auto-complete';
import { getUsersAndTeamsForSearch } from '../../../api/users/usersAndTeams';
import styled from 'styled-components';
import ReactTooltip from 'react-tooltip';
import TooltipOff from '../../../assets/images/png/tooltip-off.png'
import TooltipOn from '../../../assets/images/png/tooltip-on.png'


interface Props {
    listing: Listing
}

const ListingTeam: FC<Props> = (props) => {

    const { listing } = props;

    const userArr: TeamMember[] = listing.users ? listing.users : [];
    const teamArr: Team[] = listing.teams ? listing.teams : [];
    const [members, setMembers] = useState<TeamMember[]>(userArr);
    const [teamMembers, setTeamMembers] = useState<Team[]>(teamArr);

    const noDataProps: SearchableInputNoDataProps = {
        showNoData: true,
        noDataMessage: 'No users/teams found...',
        showNoDataButton: false
    }

    const formControllerContext = useContext(FormContext);

    const currentUser = authContext.getCachedUser();

    const updateTeamMemberData = (usersUpdate: TeamMember[]) => {
        const values = {
            'users': [...usersUpdate]
        };
        formControllerContext.onFormChange(values);
    }

    const updateTeamData = (teamsUpdate: Team[]) => {
        const values = {
            'teams': [...teamsUpdate]
        };
        formControllerContext.onFormChange(values);
    }


    const findRemoteDataProvider = (request: AutoCompleteRequest): Promise<AutoCompleteResult[]> => {

        return new Promise((resolve, reject) => {
            resolve(getUsersAndTeamsForSearch(request));
        });
    }

    const removeTeamMember = (member: TeamMember) => {
        const tempUsers: TeamMember[] = [...members];
        const idx = tempUsers.findIndex((u: TeamMember) => u.email === member.email);
        tempUsers.splice(idx, 1);
        setMembers(tempUsers);
        updateTeamMemberData(tempUsers);
    }

    const removeTeamMemberTeam = (memberTeam: Team) => {
        const tempTeam: Team[] = [...teamMembers];
        const idx = tempTeam.findIndex((u: Team) => u.name === memberTeam.name);
        tempTeam.splice(idx, 1);
        setTeamMembers(tempTeam);
        updateTeamData(tempTeam);
    }

    const addUser = (suggestion: AutoCompleteResult) => {
        const member = suggestion.value;
        // add user or team to listing

        if (member.isTeam) {
            const tempTeam: Team[] = [...teamMembers];
            const newTeam: Team = { id: member.name, name: member.name, users: [] };
            tempTeam.push(newTeam);
            setTeamMembers(tempTeam);
            updateTeamData(tempTeam);
        }
        else {
            const temp: TeamMember[] = [...members];
            const newUser: TeamMember = { teamMemberId: member.name, firstName: member.firstName, lastName: member.lastName, email: member.name };
            temp.push(newUser);
            setMembers(temp);
            updateTeamMemberData(temp);
        }
    }

    const getLatestUserIds = (): string[] => {
        const filterUserArray: string[] = members ? members.map((teamMember: TeamMember) => (teamMember.email)) : [];
        const filterTeamArray: string[] = teamMembers ? teamMembers.map((t: Team) => (t.name)) : [];
        const filterArray: string[] = filterUserArray.concat(filterTeamArray);
        return filterArray;
    }

    const memberKey = (member: TeamMember) => {
        if (member && member.email) {
            return member.email + new Date().getTime();
        }
        return "member" + new Date().getTime();
    }

    const memberTeamKey = (memberTeam: Team) => {
        if (memberTeam && memberTeam.name) {
            return memberTeam.name.replace(/ /g, '') + new Date().getTime();
        }
        return "memberTeam" + new Date().getTime();
    }

    return (
        <StyledRow id="team">
            <Col xs={12}>
                <SectionHeading>
                    Data Entry Team&nbsp;<Label>Use DATA ENTRY TEAM to grant access to edit this listing.</Label>
                </SectionHeading>
            </Col>
            <Col xs={8}>
                <Row>
                    {members && members.map((teamMember: TeamMember, index: number) => (
                        <Col xs={3} key={memberKey(teamMember)}>
                            <ListingTeamMemberCard teamMember={teamMember} index={index} owner={teamMember.email === listing.owner} deleteBtn={!((teamMember.email === currentUser.userName) && (currentUser.userName === listing.owner))}  deleteFunc={removeTeamMember} />
                        </Col>
                    ))}
                    {teamMembers && teamMembers.map((team: Team, index: number) => (
                        <Col xs={3} key={memberTeamKey(team)}>
                            <ListingTeamMemberCard team={team} index={index} owner={false} isTeam={true} deleteBtn={true} deleteTeamFunc={removeTeamMemberTeam} />
                        </Col>
                    ))}
                </Row>
            </Col>
            <Col xs={4}>
                <Label fontSize={16} fontWeight={'bold'} fontStyle={'None'} veriticalAlign={'top'}>Grant access to edit this listing</Label>
                <IconTag data-tip="true" data-for="teamTooltip"><img src={TooltipOff} onMouseOver={e => (e.currentTarget.src=TooltipOn)} onMouseOut={e => (e.currentTarget.src=TooltipOff)}/></IconTag>
                <StyledTooltip id="teamTooltip" type='light' border={false} place='right' multiline={true}>
                    <span>Can't find your colleague? The DATA ENTRY TEAM  directory populates when someone logs in to the system and is registered as a Data Entry user</span>
                </StyledTooltip>
                <br /><br />
                { 
                    <Label fontSize={11}>Search for your colleague by first name or TEAM (created in DATA ENTRY TEAMS).</Label>
                }                
                <SearchableInput key={"userSearch" + new Date().getTime()} name="userSearch" placeholder="Lookup and add a team or a member" remoteDataProvider={findRemoteDataProvider}
                    autoCompleteFinish={addUser} defaultValue="" clearAfterSelect={true} error={false} showSearchIcon={true} noDataProps={noDataProps} extraData={JSON.stringify(getLatestUserIds())} />
            </Col>
        </StyledRow>
    );
}

const StyledRow = styled(Row as any)`
    background-color: ${props => props.theme.colors && props.theme.colors.secondaryBackground || '#f2f2f2'};
    border: 1px solid ${props => props.theme.colors && props.theme.colors.border || '#cccccc'};
    padding: 1em 2em;
`;

interface LabelStyle {
    fontSize?: number;
    fontStyle?: string;
    fontWeight?: string;
    veriticalAlign?: string;
};

const Label = styled.span`
    font-size: ${(props: LabelStyle) => props.fontSize !== undefined ? props.fontSize + "px" : "15px"};
    font-style:  ${(props: LabelStyle) => props.fontStyle !== undefined ? props.fontStyle : "italic"};
    font-weight:  ${(props: LabelStyle) => props.fontWeight !== undefined ? props.fontWeight : "normal"};
    vertical-align: ${(props: LabelStyle) => props.veriticalAlign !== undefined ? props.veriticalAlign : "baseline"};
    text-transform: none;
    align-items: center;
    font-family: Arial, Helvetica, sans-serif;
    color: #666;
`;

const IconTag = styled.a`
  margin-block-start: 2em;
  margin-left: .5em; 
`;

const StyledTooltip = styled(ReactTooltip as any)`
  box-shadow: 0 0 4px 2px grey;
  width: 250px;
  font-family: "Futura Md BT", helvetica, arial, sans-serif;
  font-size: 12px;
  font-weight: 500;
  text-transform: none;
`;

export default React.memo(ListingTeam, (prevProps, nextProps) => nextProps.listing !== prevProps.listing);