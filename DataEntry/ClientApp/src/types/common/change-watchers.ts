export interface ChangeWatcherSetup{
    field: string
}

export interface ChangeWatcher extends ChangeWatcherSetup{
    func: Function
}

export interface TriggerField {
    field: string,
    value: any
}

export interface ChangeWatcherSystem {
    changeWatchers: ChangeWatcher[],
    triggerFields: TriggerField[],
    triggerCallback: Function
} 
