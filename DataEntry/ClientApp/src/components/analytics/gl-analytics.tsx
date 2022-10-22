import React, { FC, useEffect } from 'react'
import { useSelector, useDispatch } from 'react-redux';
import { GLAnalyticsContext } from './gl-analytics-context';
import { analyticsBooleanSelector } from '../../redux/selectors/system/analytics-boolean-selector';
import { configSelector } from '../../redux/selectors/system/config-selector';
import { setAnalyticsBoolean } from '../../redux/actions/system/set-analytics-boolean';
import { routeSelector } from '../../redux/selectors/common/route-selector'
import ReactGA from 'react-ga';
import { Config } from '../../types/config/config'

const GLAnalytics: FC = (props) => {
    const dispatch = useDispatch()

    const { children } = props;

    const analytics: boolean = useSelector(analyticsBooleanSelector);
    const config: Config = useSelector(configSelector);
    const route: any = useSelector(routeSelector);

    const isBrowser = (typeof window !== 'undefined');

    const url: string = window.location.href;

    const fireEvent = async (...eventData: Array<string | boolean>) => {
        if (analytics && isBrowser) {
            analyticsEvent(...eventData);
        }
    }

    const fireTracking = async () => {
        ReactGA.pageview(window.location.href)
    }

    const initializeAnalytics = (id: string) => {
        if (!url.includes('uat') && !url.includes('dev') && !url.includes('localhost')){
            ReactGA.initialize(id);
            fireTracking();
            dispatch(setAnalyticsBoolean(true))
        }
    }

    // Initialize analytics, set global boolean to true if analytics id exists
    useEffect(() => {
        if (!analytics && config && config.googleTrackingId && Boolean(config.googleTrackingId) && isBrowser) {
            initializeAnalytics(config.googleTrackingId);
        }
    }, [config]);

    // Fire analytics each time the route changes
    useEffect(() => {
        // check for headless browser and analytic initialization
        if (isBrowser && analytics) {
            fireTracking();
        }
    }, [route])

    return (
        <GLAnalyticsContext.Provider value={{
            "fireEvent": fireEvent,
            "fireTracking": fireTracking
        }}>
            {children}
        </GLAnalyticsContext.Provider>
    );
};

// arguments passed in: (eventCategory, eventAction, eventLabel, transport, noninteraction)
// example: analyticsEvent('propertyClick', 'click', 'Property clickthrough', 'beacon', true)
// beacon is useful for click events that trigger route change
// noninteraction: true sends an event when a user doesn't do something
export const analyticsEvent = (...eventData: Array<string | boolean>) => {

    if (eventData.length > 0 && eventData.length < 6){

        // Check to make sure the first four types are strings to prevent errors
        if (eventData.length === 5){
            for (let i = 0; i < eventData.length - 2; i++){
                if (typeof eventData[i] !== 'string'){
                    return;
                }
            }
        } else {
            for (let i = 0; i < eventData.length - 1; i++){
                if (typeof eventData[i] !== 'string'){
                    return;
                }
            }
        }

        // setup the eventModel object to replicate
        const eventModel = {
            category: '',
            action: '',
            label: '',
            transport: '',
            nonInteraction:  false,
        } 
    
        // slice the object above depending on the amount of arguments passed in
        const event: any = Object.keys(eventModel).slice(0, eventData.length).reduce((result, key) => {
            result[key] = eventModel[key];
            return result;
        }, {});
        
        // set the new sliced object equal to values passed in
        for (const key in event){
            if(event.hasOwnProperty(key)){
                // get the index of the key (eg. 0,1,2) to set the data of the array passed in equal to the current object key. 
                const index = Object.keys(event).indexOf(key);
                event[key] = eventData[index];          
             }
        }

        // send the new sliced object to google analytics
        ReactGA.event(event);

    } else {
        console.log('Event Analytics Error: No arguments, too many arguments, or a boolean was provided in the wrong index.')
    }

}

export default GLAnalytics;