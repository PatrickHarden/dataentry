export interface UserSearchFilter {
    term?: string,
    blacklist? : string[],
    skip?: number,
    take?: number
}