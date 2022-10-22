import { createSelector } from 'reselect';
import { configDetailsSelector } from './config-details-selector';
import { ConfigDetails } from '../../../types/config/config';

export const configSelector = createSelector(
    configDetailsSelector, (configDetails:ConfigDetails) => {
    return configDetails.config;
});