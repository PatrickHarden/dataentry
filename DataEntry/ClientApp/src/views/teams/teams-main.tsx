import React, { FC } from 'react';
import { Team } from '../../types/team/team';
import TeamSingle from './team-single';
import { Col, Row } from 'react-styled-flexboxgrid';
import styled from 'styled-components'

export interface Props {
    readonly teams: Team[]
}

const TeamsMain: FC<Props> = (props) => {

    const { teams } = props;
    return (
        <TeamsSection>
            <Row>
                {teams && teams.map((team:Team, index:number) => (
                    <Col xs={12} key={team.id + new Date().getTime() + index + ' ' + String((Math.floor((Math.random() * 100) + 1)/Math.floor((Math.random() * 100) + 1)) * Math.floor((Math.random() * 100) + 1))} className="teams-card">
                        <TeamSingle edit={false} team={team} key={index} />
                    </Col>
                ))}
            </Row>
        </TeamsSection>
    )
};

const TeamsSection = styled.div`
    .teams-card {
        > div {
            
        }
    }
    margin-bottom:260px;
`

export default TeamsMain;