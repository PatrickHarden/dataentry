import React, { FC } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { getMIQProperties } from '../../../api/miq/miq';
import SearchableInput, { SearchableInputNoDataProps } from '../../../components/searchable-input/searchable-input';
import { featureFlagSelector } from '../../../redux/selectors/featureFlags/feature-flag-selector';
import { miqCurrentSelectionSelector } from '../../../redux/selectors/miq/miq-current-selection-selector';
import { miqStatusSelector } from '../../../redux/selectors/miq/miq-status-selector';
import { configSelector } from '../../../redux/selectors/system/config-selector';
import { AutoCompleteRequest, AutoCompleteResult } from '../../../types/common/auto-complete';
import { Config } from '../../../types/config/config';
import { MIQSearchResult } from '../../../types/miq/miqSearchResultInt';
import { MIQStatus } from '../../../types/miq/miqStatus';
import { FeatureFlags } from '../../../types/state';
import { generateKey } from '../../../utils/keys';

interface SelectRecordProps {
    selectionHandler: (result:AutoCompleteResult) => void
}

const SelectRecordView: FC<SelectRecordProps> = (props) => {

    const { selectionHandler } = props;

    const status:MIQStatus = useSelector(miqStatusSelector);
    const selectedOption:MIQSearchResult = useSelector(miqCurrentSelectionSelector);
    const config:Config = useSelector(configSelector);
    const miqCountryCode:string = config.miqCountryCode;
    const featureFlags:FeatureFlags = useSelector(featureFlagSelector);

    const findRemoteDataProvider = (request: AutoCompleteRequest): Promise<AutoCompleteResult[]> => {
        if(featureFlags && featureFlags.miqLimitSearchToCountryCodeFeatureFlag){
            request.countryCodes = [miqCountryCode];
        }        
        return new Promise(resolve => {
            resolve(getMIQProperties(request));
        });
    }

    const noDataProps: SearchableInputNoDataProps = {
        showNoData: true,
        noDataMessage: 'No properties found...',
        showNoDataButton: false
    }

    return (
        <SearchableInputContainer>
            <SearchableInput
                key={generateKey()}
                name="miqSearch"
                placeholder="Enter address or property name"
                delayMS={200}
                showSearchIcon={true}
                showLoadingAnimation={true}
                showGreenSearchIcon={true}
                remoteDataProvider={findRemoteDataProvider}
                noDataProps={noDataProps}
                autoCompleteFinish={selectionHandler}
                disabled={status.loading}
                defaultValue={selectedOption && selectedOption.name && selectedOption.name.trim().length > 0 ? selectedOption.name : ""}
            />
        </SearchableInputContainer>
    );
}


const SearchableInputContainer = styled.div`
    max-width:900px;
    margin: 90px auto 0;
    #miqSearch_cont {
        > div {
            > div {
                > #searchInputContainerGreen {
                    background-color:transparent;
                    border-top:none;
                    border-left:none;
                    border-right:none;
                    outline: none;
                    position:relative;
                    #miqSearch {
                        background:transparent;
                        font-style: normal;
                        font-weight: 800;
                        font-size: 24px;
                        padding-top:15px;
                        padding-bottom:15px;
                        padding-left:5px;
                    }
                }
                .react-autosuggest__suggestions-container--open {
                    max-height:300px;
                    overflow-y:scroll;
                    margin-top:18px;
                    border: 1px solid rgba(204, 204, 204, 0.5);
                    box-sizing: border-box;
                    box-shadow: 0px 4px 5px rgba(225, 225, 225, 0.5);
                    border-radius: 2px;
                    ul {
                        li {
                            font-size: 21px;
                            color: #666;
                            font-weight:800;
                            transition: .2s ease all;
                        }
                        li.react-autosuggest__suggestion--highlighted {
                            color: #fff;
                        }
                    }
                }
            }
        }
    }
`;

export default React.memo(SelectRecordView);