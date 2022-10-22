import { TeamMember } from './teamMember';

export interface Team {
    id: string,
    name: string,
    users: TeamMember[],
}