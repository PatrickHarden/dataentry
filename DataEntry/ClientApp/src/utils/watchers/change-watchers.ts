import { ChangeWatcherSetup, ChangeWatcher, ChangeWatcherSystem, TriggerField } from '../../types/common/change-watchers';

/* tslint:disable:no-string-literal */
window["changeWatcherFunc"] = (changeObj:any) => {
    if(changeObj.system && changeObj.system.triggerFields){
        changeObj.system.triggerFields.forEach((tf:TriggerField) => {
            if(tf.field === changeObj.field){
                tf.value = changeObj.value;
            }
            if(changeObj.system.triggerCallback){
                changeObj.system.triggerCallback(tf);
            }

        });
    }
}

export const setupChangeWatchers = (changeWatcherSetups:ChangeWatcherSetup[], callbackFunction: Function) : ChangeWatcherSystem => {
    
    const changeWatchers:ChangeWatcher[] = [];
    const triggerFields:TriggerField[] = [];

    changeWatcherSetups.forEach(cwSetup => {
        changeWatchers.push(createChangeWatcher(cwSetup));
        triggerFields.push(createTriggerField(cwSetup));
    });

    const system:ChangeWatcherSystem = {
        'changeWatchers': changeWatchers,
        'triggerFields': triggerFields,
        'triggerCallback': callbackFunction
    }
    
    return system;
}

const createChangeWatcher = (changeWatcherSetup:ChangeWatcherSetup) => {
    const functionBody:string = 'changeWatcherFunc(changeObj);';
    // const functionBody:string = 'return '
    const cw:ChangeWatcher = {
        'field': changeWatcherSetup.field,
        'func': new Function('changeObj', functionBody)
    }
    return cw;
}

const createTriggerField = (changeWatcherSetup:ChangeWatcherSetup) => {
    const tf:TriggerField = {
        'field': changeWatcherSetup.field,
        'value': null
    }
    return tf;
}

export const checkChangeWatchers = (changeObject:object, previous: object, system:ChangeWatcherSystem) => {

    if(!system || !system.changeWatchers){
        return;
    }

    system.changeWatchers.forEach(cw => {
        if(changeObject[cw.field] !== previous[cw.field]){
            const changeObj = {
                'field': cw.field,
                'value': changeObject[cw.field],
                'system': system
            }
            cw.func(changeObj);
        }
    });
}