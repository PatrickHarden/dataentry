import React, { FC } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { configDetailsSelector } from '../../../redux/selectors/system/config-details-selector';
import { ConfigDetails } from '../../../types/config/config';
import { Listing } from '../../../types/listing/listing';
import { Space } from '../../../types/listing/space';
import { generateKey } from '../../../utils/keys';
import PropertyResultView from './property-result-view';

interface RecordsDisplayProps {
    records: Listing[],
    assignMode: boolean,
    allowAssign: boolean,
    actionHandler: (record: Listing) => void,
    assignHandler?: (record: Listing) => void,
    allowUnpublish: boolean,
    spacesImport?: boolean,
    updateSpaces?: (space: any, selected: boolean) => void,
    unpublishHandler?: (record: Listing) => void

}

const RecordsDisplayView: FC<RecordsDisplayProps> = (props) => {

    const { records, assignMode, allowAssign, actionHandler, assignHandler, allowUnpublish, unpublishHandler, spacesImport, updateSpaces } = props;

    const configDetails: ConfigDetails = useSelector(configDetailsSelector);

    // when the user clicks an add/edit button, prepare the data to be sent over to the create/edit listing page
    const prepareRecordForAddEdit = (record: Listing, selectedAvailabilities?: Space[]) => {
        // we need to remove any spaces that aren't in the selected availabilities so they don't get sent over to the add/edit listing page
        if (record && record.spaces) {
            record.spaces = record.spaces.filter((space: Space) => selectedAvailabilities && selectedAvailabilities.indexOf(space) > -1);
        }
        actionHandler(record);
    }

    const prepareRecordForAssign = (record: Listing, selectedAvailabilities?: Space[]) => {
        if (assignHandler) {
            // we need to remove any spaces that aren't in the selected availabilities so they don't get saved
            if (record && record.spaces) {
                record.spaces = record.spaces.filter((space: Space) => selectedAvailabilities && selectedAvailabilities.indexOf(space) > -1);
            }
            assignHandler(record);
        }
    }

    return (
        <RecordsDisplayContainer>
            {
                records.map((record: Listing, index: number) => {
                    return (
                        <React.Fragment key={String(index)}>
                            {((assignMode && record.id <= 0) || !assignMode) &&
                                <PropertyResultContainer key={generateKey()}>
                                    <PropertyResultView key={generateKey()} updateSpaces={updateSpaces}
                                        record={record} spacesImport={spacesImport ? true : false}
                                        configDetails={configDetails}
                                        displayActionBtns={!assignMode}
                                        allowAssign={allowAssign}
                                        actionHandler={prepareRecordForAddEdit}
                                        assignHandler={prepareRecordForAssign}
                                        allowUnpublish={allowUnpublish}
                                        unpublishHandler={unpublishHandler} />
                                </PropertyResultContainer>
                            }
                        </React.Fragment>
                    )
                })
            }
        </RecordsDisplayContainer>
    );
}

const RecordsDisplayContainer = styled.div``;

const PropertyResultContainer = styled.div`
    margin-bottom: 50px;
`;

export default RecordsDisplayView;