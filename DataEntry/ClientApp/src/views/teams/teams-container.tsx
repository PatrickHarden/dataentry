import React, { FC, useEffect, useState, useMemo } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import styled from 'styled-components';
import { mainMessageSelector } from '../../redux/selectors/system/main-message-selector';
import { alertMessageSelector } from '../../redux/selectors/system/alert-message-selector';
import { MainMessaging, AlertMessaging } from '../../types/state';
import MainMessage from '../common/messages/main-message';
import AlertMessage from '../common/messages/alert-message';
import LoadingContainer from '../../components/loader/loading-container'
import { clearAlertMessage } from '../../redux/actions/system/set-alert-message';
import TeamsMain from './teams-main';
import TeamsHeader from './teams-header';
import StyledButton from '../../components/styled-button/styled-button';
import { allTeamsSelector } from '../../redux/selectors/teams/teams-selector';
import { currentTeamSelector } from '../../redux/selectors/teams/current-team';
import TeamSingle from './team-single';
import { loadTeams } from '../../redux/actions/teams/load-teams';
import { Col, Row } from 'react-styled-flexboxgrid';

import defaultTeam from '../../api/defaults/team';
import cloneDeep from 'clone-deep';
import { authContext } from '../../adalConfig';

import { Team } from '../../types/team/team';



const TeamsContainer: FC = () => {

    const dispatch = useDispatch();

    const mainMessage: MainMessaging = useSelector(mainMessageSelector);
    const alertMessage: AlertMessaging = useSelector(alertMessageSelector);
    const teams: Team[] = useSelector(allTeamsSelector);
    const team: Team = useSelector(currentTeamSelector);
    const [createNewTeam, setCreateNewTeam] = useState(false);

    useEffect(() => {
        //  clearAlertMessage(dispatch);
        dispatch(loadTeams());
    }, []);

    useEffect(() => {
        clearAlertMessage(dispatch);
    }, [teams]);


    const newTeam = cloneDeep(defaultTeam);
    const currentUser = authContext.getCachedUser();
    newTeam.users.push({ teamMemberId: "", firstName: currentUser.profile.given_name, lastName: currentUser.profile.family_name, email: currentUser.userName });

    const doneCreateTeam = () => {
        setCreateNewTeam(false);
    }

    const TeamCreateMemo = useMemo(() => <TeamSingle edit={true} createNew={true} doneCreate={doneCreateTeam} key={"team" + new Date().getTime()} team={newTeam} />, [team]);

    const createTeam = () => {
        // dispatch(loadCurrentTeam(""));
        setCreateNewTeam(true);
    }
    return (
        <>
            {mainMessage.show ?
                        <>
                            <TeamsHeader showSearch={false} />
                            <MainMessage mainMessage={mainMessage} />
                        </> :
                        <>
                             {(teams && teams.length === 0) ?
                                <>
                                <TeamsHeader showSearch={false} />
                                {(createNewTeam) ?
                                    <>
                                        <TeamsMainContainer style={{ marginTop: '20px', marginBottom: '260px' }}>
                                            {TeamCreateMemo}
                                        </TeamsMainContainer>
                                    </> :
                                    <>
                                        <NoTeamsContainer style={{ marginTop: '60px' }}>
                                            <BgMsg>No teams to display</BgMsg>
                                            <StyledButton name={"createTeam"} onClick={createTeam} style={{ marginTop: '25px' }} styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp; Create New Team</StyledButton>
                                        </NoTeamsContainer>
                                    </>
                                }
                            </> :
                            <>
                                <TeamsHeader showSearch={false} />
                                {alertMessage && alertMessage.show && <AlertMessage alertMessage={alertMessage} />}
                                <LoadingContainer isLoading={mainMessage.show ? false : true}>
                                    <TeamsMainContainer style={{ marginTop: '20px' }}>
                                        <Row>
                                            <Col xs={12} style={{ float: "right", marginBottom: "20px" }}>
                                                {!createNewTeam && <StyledButton name={"createTeam"} onClick={createTeam} style={{ marginTop: '25px', float: "right" }} styledSpan={true} buttonStyle="2"><span style={{ fontSize: "18px" }}>+</span>&nbsp;&nbsp; Create New Team</StyledButton>}
                                            </Col>
                                        </Row>
                                        {createNewTeam && TeamCreateMemo}
                                        <TeamsMain teams={teams} />
                                    </TeamsMainContainer>
                                </LoadingContainer>
                            </>
                             }
                        </> 
            }

        </>
    );

};

const TeamsMainContainer = styled.div`
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    margin:0 auto;
`;

const NoTeamsContainer = styled.div`
    box-shadow: 0 4px 6px rgba(204,204,204,0.12), 0 2px 4px rgba(204,204,204,0.24);
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'sans-serif')};
    max-width: ${props => props.theme.container.maxWidth};
    width: ${props => props.theme.container.width};
    min-height:300px;
    background-color: white;
    margin:0 auto;
    text-align: center;
`;

const BgMsg = styled.div`
        line-height:80px;
        text-align:center;
        font-size:18px;
        font-family: ${props => (props.theme.font ? props.theme.font.bold : 'inherit')}; 
        color:#ddd;
`;

export default TeamsContainer;