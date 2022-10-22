import React, { FC, useState } from 'react';
import styled from 'styled-components';
import { Team } from '../../types/team/team';
import { TeamMember } from '../../types/team/teamMember';
import { Col, Row } from 'react-styled-flexboxgrid';
import FormInput from '../../components/form-input/form-input';
import StyledButton from '../../components/styled-button/styled-button';
import cloneDeep from 'clone-deep';
import TeamMemberSingle from './team-member-single';
import { useDispatch } from 'react-redux';
import { saveTeam } from '../../redux/actions/teams/save-team';
import { deleteTeam } from '../../redux/actions/teams/delete-team';
import EditIcon from '../../assets/images/png/icon-edit.png';
import { setConfirmDialog } from '../../redux/actions/system/set-confirm-dialog';
import { AutoCompleteResult, AutoCompleteRequest } from '../../types/common/auto-complete';
import SearchableInput, { SearchableInputNoDataProps } from '../../components/searchable-input/searchable-input';
import DeleteIcon from '../../assets/images/png/deleteIcon.png'
import { getUsersForSearch } from '../../api/users/users';


interface Props {
    team: Team,
    edit: boolean,
    createNew?: boolean,
    doneCreate?(): void
}


const TeamSingle: FC<Props> = (props) => {

    const { team, edit, createNew, doneCreate } = props;

    const [teamState, setTeamState] = useState(team);
    const [tempState, setTempState] = useState(cloneDeep(team));
    const [editing, setEditing] = useState(edit);
    const dispatch = useDispatch();

    const findRemoteDataProvider = (request: AutoCompleteRequest): Promise<AutoCompleteResult[]> => {

        return new Promise((resolve, reject) => {
            resolve(getUsersForSearch(request));
        });
    }

    const updateValue = (value: any) => {
        const tempteam = cloneDeep(tempState);
        tempteam.name = value;
        setTempState(tempteam);
    }

    const noDataProps: SearchableInputNoDataProps = {
        showNoData: true,
        noDataMessage: 'No users found...',
        showNoDataButton: false
    }

    const addUser = (suggestion: AutoCompleteResult) => {
        const user = suggestion.value;
        // add user to team
        const tempteam = cloneDeep(tempState);
        tempteam.users.push(user);
        setTempState(tempteam);
    }

    const removeUser = (email: string) => {
        const tempteam = cloneDeep(tempState);
        const idx = tempteam.users.findIndex((u: TeamMember) => u.email === email);
        tempteam.users.splice(idx, 1);
        setTempState(tempteam);
    }


    const cancelChanges = () => {
        setTeamState(team);
        setTempState(cloneDeep(team));
        setEditing(false);
        if (doneCreate !== undefined) {
            doneCreate();
        }
    }

    const saveChanges = () => {
        setTeamState(tempState);
        if (doneCreate !== undefined) {
            doneCreate();
        }
        dispatch(saveTeam(tempState));
        setEditing(false);

    }

    const editTeam = () => {
        setEditing(true);
    }

    const confirmDeleteThisTeam = () => {
        dispatch(deleteTeam(teamState));
    }
    const deleteThisTeam = () => {
        dispatch(setConfirmDialog({ show: true, message: "Are you sure you want to Delete the team " + tempState.name + " ?", confirmTxt: "Delete", confirmFunc: confirmDeleteThisTeam }));
    }

    const getLatestUserIds = (): string[] => {
        const filterArray: string[] = tempState.users.map((teamMember: TeamMember) => (teamMember.email));
        return filterArray;
    }

    return (
        <>
            {(editing || !createNew) &&
                <BaseTeamCard>
                    <NameSection>
                        {editing &&
                            <Row>
                                <Col xs={4}>
                                    <FormInput name={tempState.name} inputvalue={tempState.name} useOnChange={true} placeholder={"Enter a name for your team"} blurHandler={updateValue} />
                                </Col>
                                <Col xs={4}>
                                    <SearchableInput key={"userSearch" + new Date().getTime()} name="userSearch" placeholder="Lookup and add members" remoteDataProvider={findRemoteDataProvider}
                                        autoCompleteFinish={addUser} defaultValue="" clearAfterSelect={true} error={false} showSearchIcon={true} noDataProps={noDataProps} extraData={JSON.stringify(getLatestUserIds())} />
                                </Col>
                                <Col xs={4} style={{ "textAlign": "right" }}>
                                    <StyledButton name={"createTeam"} onClick={cancelChanges} style={{ marginTop: '38px', marginRight: "8px", textAlign: 'center' }} styledSpan={true}>Cancel</StyledButton>
                                    {!createNew && <StyledButton name={"deleteTeam"} onClick={deleteThisTeam} style={{ marginTop: '30px', marginRight: "8px", textAlign: 'center' }} styledSpan={true}>Delete</StyledButton>}
                                    <StyledButton name={"saveTeam"} onClick={saveChanges} style={{ marginTop: '38px', textAlign: 'center' }} styledSpan={true}>Save</StyledButton>
                                </Col>
                            </Row>
                        }
                        {!editing &&
                            <Row>
                                <Col xs={10}><TeamNameTxt>{teamState.name}</TeamNameTxt></Col>
                                <Col xs={2}><EditButton onClick={editTeam}><img src={EditIcon} /> Edit Team</EditButton></Col>
                            </Row>
                        }
                    </NameSection>
                    {tempState.users && tempState.users.map((teamMember: TeamMember, index: number) => (
                        <Row style={{ borderBottom: "1px solid #e8e8e8" }} key={String(index)}>
                            <Col xs={11} key={teamMember.teamMemberId + new Date().getTime() + index + ' ' + String((Math.floor((Math.random() * 100) + 1) / Math.floor((Math.random() * 100) + 1)) * Math.floor((Math.random() * 100) + 1))}>
                                <TeamMemberSingle teamMember={teamMember} key={index} />
                            </Col>
                            <Col xs={1}>

                                {editing && (tempState.users.length > 1) && <span onClick={() => removeUser(teamMember.email)}><img style={{ width: "20px", marginTop: "18px", cursor: "pointer" }} src={DeleteIcon} /></span>}

                            </Col>
                        </Row>
                    ))}
                </BaseTeamCard>
            }
        </>
    );
};

export default React.memo(TeamSingle);

const BaseTeamCard = styled.div`
    width: auto;
    margin: auto;
    padding: 15px;
    overflow:visible;
    position:relative;
    margin-bottom: 40px;
    border: 1px solid #ddd;
    box-shadow: 0 4px 6px rgba(204,204,204,0.12), 0 2px 4px rgba(204,204,204,0.24);
            transition: all 0.3s cubic-bezier(.25,.8,.25,1);
            /* &:hover {
                box-shadow: 0 7px 14px rgba(204,204,204,0.20), 0 5px 5px rgba(204,204,204,0.17);
            } */
            background-color: white;
`;

const NameSection = styled.div`
    border-bottom: 1px solid #ddd;
    padding-bottom: 25px;
    #userSearch_cont {
        position:relative;
        top: 8px;
        #userSearch {
            padding: 1.08em;
        }
        #react-autowhatever-1 {
            overflow-y: auto;
            max-height: 50vh;
        }
    }
`;

const EditButton = styled.div`
    color: ${props => (props.theme.colors ? props.theme.colors.tertiaryAccent : 'deepskyblue')};
    cursor:pointer;
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};	
    font-size: 14px;
    float:right;
    margin-right:10px;
`;

const TeamNameTxt = styled.div`
    font-size:20px;
    font-weight:bold;
    color: #777;
`;

