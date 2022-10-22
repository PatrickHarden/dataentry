import React, { FC, useState } from 'react';
import styled from 'styled-components';
import GLTable, { GLTableStyles } from '../../../components/table/gl-table';
import GLBadge, { BadgeStyles } from '../../../components/badge/gl-badge';
import { Column } from '../../../components/table/types/Column';
import { RowReplacement } from '../../../components/table/types/RowReplaceColumns';
import { FormCheckboxProps, FormCheckboxStyles } from '../../../components/form-checkbox/form-checkbox';
import FormCheckboxWrapper from '../../../components/form-checkbox/form-checkbox-wrapper';
import { Listing } from '../../../types/listing/listing';
import { GLFile } from '../../../types/listing/file';
import { Config, ConfigDetails } from '../../../types/config/config';
import { findPriceUsingSpecs, findSizeUsingSpecs, renderAddressDom } from '../../../utils/listing-display-values';
import { Space } from '../../../types/listing/space';
import { useSelector, useDispatch } from 'react-redux';
import { checkEnvironment } from '../../../utils/helpers/check-environment';
import { configSelector } from '../../../redux/selectors/system/config-selector';
import DefaultPropertyImage from '../../../assets/images/png/property-default.png';
import { setMiqSpacesResult } from '../../../redux/actions/miq/set-miq-spaces';

export interface Props {
    record: Listing,
    configDetails: ConfigDetails,
    displayActionBtns: boolean,
    allowAssign: boolean,
    actionHandler?: (record: Listing, selectedAvailabilities?: Space[]) => void,
    assignHandler?: (record: Listing, selectedAvailabilities?: Space[]) => void,
    allowUnpublish: boolean,
    spacesImport?: boolean,
    updateSpaces?: (space: any, selected: boolean) => void,
    unpublishHandler?: (record: Listing) => void
}

const PropertyResultView: FC<Props> = (props) => {

    const { record, configDetails, displayActionBtns, allowAssign, actionHandler, assignHandler, allowUnpublish, unpublishHandler, spacesImport, updateSpaces } = props;

    const [selectedAvailabilities] = useState<Space[] | undefined>(record.spaces);     // select all by default
    // const [ showImportButton, setShowImportButton ] = useState<boolean>(false)

    const config: Config = useSelector(configSelector);

    const dispatch = useDispatch();

    // HELPER FUNCTIONS ===================================================================================================
    // Local utility functions
    // ====================================================================================================================

    const findCoverImage = () => {
        // find the cover image to display or return a placeholder image if we can't find one
        let primaryPhotos: GLFile[] | undefined;
        record && record.photos ? primaryPhotos = record.photos.filter((photo: GLFile) => photo.primary === true) : primaryPhotos = undefined;
        // if we don't find a primary photo, double check to see if we have any photos and if so just take the first one for display
        if ((!primaryPhotos || primaryPhotos.length === 0) && record.photos && record.photos.length > 0) {
            primaryPhotos = [record.photos[0]];
        }

        let coverImageURL: any = DefaultPropertyImage;
        if (primaryPhotos && primaryPhotos.length > 0) {
            const firstPhoto: GLFile = primaryPhotos[0]; // if we have more than one primary, just pick the first in the list
            if (firstPhoto.base64String && firstPhoto.base64String.length > 0) {
                // if we have a base64 string, we will use that to display the photo - this is converted from AWS
                coverImageURL = firstPhoto.base64String;
            } else if (firstPhoto && firstPhoto.url && firstPhoto.url.length > 0) {
                coverImageURL = firstPhoto.url;
            }
        }
        return coverImageURL;
    }

    // EVENT HANDLERS =====================================================================================================
    // Handle events like checkbox selection, button clicks, etc.
    // ====================================================================================================================

    const selectAvailability = (data: any, selected: boolean) => {
        if (spacesImport && updateSpaces){
            updateSpaces(data, selected)
        } else if (data.id === 0){
            dispatch(setMiqSpacesResult(data, selected))
            if (selected && selectedAvailabilities && selectedAvailabilities.indexOf(data) === -1) {
                selectedAvailabilities.push(data);
            } else if (!selected && selectedAvailabilities && selectedAvailabilities.indexOf(data) > -1) {
                selectedAvailabilities.splice(selectedAvailabilities.indexOf(data), 1);
            }
        } else {
            if (selected && selectedAvailabilities && selectedAvailabilities.indexOf(data) === -1) {
                selectedAvailabilities.push(data);
            } else if (!selected && selectedAvailabilities && selectedAvailabilities.indexOf(data) > -1) {
                selectedAvailabilities.splice(selectedAvailabilities.indexOf(data), 1);
            }
        }
    }

    // handle create/edit button click
    const handleCreateEdit = () => {
        if (actionHandler) {
            console.log(record, selectedAvailabilities)
            actionHandler(record, selectedAvailabilities);
        }
    }

    // handle assign button click
    const handleAssign = () => {
        if (assignHandler) {
            assignHandler(record, selectedAvailabilities);
        }
    }

    const handleUnpublish = () => {
        if (allowUnpublish && unpublishHandler) {
            unpublishHandler(record);
        }
    }

    // RENDER FUNCTIONS ===================================================================================================
    // These functions handle individual cell rendering.  They are situational and defined by table config.
    // ====================================================================================================================

    // render the property cell
    const renderProperty = () => {
        // show the MIQ id only in localhost, dev, uat
        const environmentCheck: boolean = checkEnvironment(config, { localhost: true, dev: true, uat: true, prod: false });
        return (
            <PropertyCellContainer>
                <PropertyImage src={findCoverImage()} data-testid="property-result-view-property-image" />
                <PropertyAddress data-testid="property-result-view-address">
                    <AddressHeader data-testid="property-result-view-address-header">{record.propertyName}</AddressHeader>
                    {renderAddressDom(record, AddressLine)}
                </PropertyAddress>
                {record.miqId && environmentCheck && <IDDisplay>MIQ ID: {record.miqId}</IDDisplay>}
            </PropertyCellContainer>
        );
    }

    // render the property type badge
    const renderPropertyType = (data: any) => {

        // for property type, at the listing level we use propertyType but at the space level we use spaceType
        // figure out which is defined and use it
        const listingLevel: string = "propertyType";
        const spaceLevel: string = "spaceType";

        // check for the listing level property first, then the space level property
        const lookup: string | undefined = data && data[listingLevel] ? data[listingLevel] : data && data[spaceLevel] ? data[spaceLevel] : undefined;

        if (lookup) {

            const backgroundColor: string | undefined = configDetails.propertyTypeColors && configDetails.propertyTypeColors.get(lookup) ?
                configDetails.propertyTypeColors.get(lookup) : undefined;

            const badgeStyles: BadgeStyles = {
                background: backgroundColor,
                color: backgroundColor ? "#FFFFFF" : "#006A4D",
                border: backgroundColor ? "none" : "1px solid #006A4D",
                borderRadius: "2px"
            }

            const propertyTypeLabel: string | undefined = configDetails.propertyTypeLabels && configDetails.propertyTypeLabels.get(lookup) ?
                configDetails.propertyTypeLabels.get(lookup) : undefined;

            return <BadgeContainer><GLBadge label={propertyTypeLabel} styles={badgeStyles} /></BadgeContainer>;
        }

        return <></>;
    }

    // render the listing type badge
    const renderListingType = (data: any, dataField?: string, defaultValue?: string) => {

        let label: string | undefined = defaultValue;
        if (data && dataField && data[dataField] && data[dataField].length > 0) {
            label = data[dataField];
        }
        return label ? <BadgeContainer><GLBadge label={label} /></BadgeContainer> : <>-</>;
    }


    const renderSize = (data: any, dataField?: string) => {
        if (data && dataField) {
            return <>{findSizeUsingSpecs(data[dataField])}</>;
        }
        return "-";
    }

    const renderPrice = (data: any, dataField?: string) => {
        const currencySymbol = configDetails && configDetails.config ? configDetails.config.currencySymbol : "$";
        const listingType = data.listingType ? data.listingType : "sale";
        if (data && dataField) {
            return <>{findPriceUsingSpecs(currencySymbol, listingType, data[dataField])}</>
        }
        return "-";
    }

    // render the availability (space) checkbox (first cell of each availability row)
    const renderAvailabilityCheck = (data: any, dataField?: string) => {

        const nameField: string = dataField ? dataField : "name";

        const checkStyles: FormCheckboxStyles = {
            marginTop: "0px",
            padding: "0px",
            labelStyles: {
                fontWeight: "800",
                fontSize: "16px"
            },
            backgroundColor: "#006A4D",
            checkColor: "#FFFFFF",
            hoverColor: "#00a657"
        }
        // the space name is wrapped in the culture code, so we need to pull it out of an array [{cultureCode,text}]
        let availabilityLabel: string = "";

        if (data && data[nameField]) {
            if (Array.isArray(data[nameField])) {
                if (data[nameField].length > 0) {
                    const valueObj = data[nameField][0];
                    availabilityLabel = valueObj.text ? valueObj.text : "";
                }
            } else if (typeof data[nameField] === "string") {
                availabilityLabel = data[nameField];
            }
        }

        const checkboxProps: FormCheckboxProps = {
            label: (availabilityLabel && availabilityLabel.trim().length > 0) ? availabilityLabel : "-",
            styles: checkStyles,
            testId: "property-result-view-availability-checkbox",
        }

        if (spacesImport || recordExists) {
            if (data && dataField && data[dataField] && data[dataField].length > 0) {
                return <FormCheckboxWrapper data={data} disabled={data && data.id !== 0} selected={data && data.id !== 0}
                    checkboxProps={checkboxProps} changeHandler={selectAvailability} />;
            }
        } else {
            if (data && dataField && data[dataField] && data[dataField].length > 0) {
                return <FormCheckboxWrapper data={data} disabled={record && record.id > 0} selected={selectedAvailabilities && selectedAvailabilities.indexOf(data) > -1 ? true : false}
                    checkboxProps={checkboxProps} changeHandler={selectAvailability} />;
            }
        }
        return <>Data Error</>;
    }

    // DATA SETUP =========================================================================================================
    // Setup the data provider and column definitions needed to render the table component per design
    // ====================================================================================================================

    // construct the data provider (combine property, a header row, and availabilities)
    // in this case, the data provider is combining two different data types in order to construct two separate "rows" 
    // in order to supply data to the table component
    let dataProvider: any[] = [];
    dataProvider.push(record);        // first row will be the property record
    if (record.spaces && record.spaces.length > 0) {
        dataProvider.push({ ph: "Availabilities" })   // second row will be a "header" for availabilities
        // the rest of the data provider will be comprised of the availability rows so just append them
        dataProvider = [...dataProvider, ...record.spaces];
    }

    // create the column configurations needed to tell the table how to render

    // we need to provide a default listing type value because we don't have the concept at the space level currently
    const defaultListingType = record && record.listingType ? record.listingType : undefined;
    // common columns : these are columns that are common between rows, whether its the property row or availability row
    const commonColumns: Column[] = [
        { size: 1.5, header: "", renderFunction: renderPropertyType },
        { size: 1.5, dataField: "listingType", header: "Type", renderFunction: renderListingType, defaultValue: defaultListingType },
        { size: 1.5, dataField: "specifications", header: "Size", renderFunction: renderSize },
        { size: 1.5, dataField: "specifications", header: "Price", renderFunction: renderPrice }
    ];

    // setup two "row replacements": one for the property and one for the "Availabilities" header
    // these replacements are needed because the table needs to know how to render the specific data object passed in (in the case of property)
    const replacements: RowReplacement[] = [
        {
            replaceAt: 0,
            columns: [{
                size: 5,
                renderFunction: renderProperty
            }, ...commonColumns],
            styles: {
                styledRow: {
                    borderTop: 'none'
                },
                styledCol: {
                    alignItems: 'top'
                }
            }
        },
        {
            replaceAt: 1,
            columns: [{
                size: 12,
                staticDisplay: "Availabilities",
            }],
            styles: {
                styledRow: {
                    borderTop: 'none'
                },
                styledCol: {
                    fontWeight: 700,
                    fontSize: '16px',
                    color: "#666666"
                }
            }
        }
    ];

    // these columns will render the rest of our rows (availabilities)
    const columns: Column[] =
        [
            {
                size: 5,
                renderFunction: renderAvailabilityCheck,
                dataField: "name"
            }, ...commonColumns];

    // STYLE overrides required to render the table per design
    const styles: GLTableStyles = {
        table: {
            padding: "5px 15px 15px 15px",
            height: "450px",
            overflowY : "auto"
        },
        tableRow: {
            styledRow: {
                lineHeight: "30px"
            }
        }
    }

    const recordExists = record.id && record.id > 0 || false;
    const showAssignButton = !recordExists && displayActionBtns && allowAssign;
    const showUnpublishButton =
        allowUnpublish && (
            recordExists && record.state && record.state.toLowerCase() === "published"
            || !recordExists && record.externalId && true
        ) || false; 

    return (
        <ViewContainer>
            <TableHeaderContainer>
                {spacesImport ? 
                    <>
                        <Left>
                            <TableHeader>Import Spaces</TableHeader>
                            <TableHeaderHelper>You can import miq spaces</TableHeaderHelper>
                        </Left>
                        <Right>
                            <TableIdDisplay>Listing ID {record.id}</TableIdDisplay>
                        </Right>
                    </>
                    :
                    <>
                        <Left>
                            <TableHeader>{record && record.id > 0 ? "Edit Listing" : "Create New Listing"}</TableHeader>
                            <TableHeaderHelper>{record && record.id > 0 ? "You can edit the existing listing" : "You can create a new listing from scratch"}</TableHeaderHelper>
                        </Left>
                        <Right>
                            <TableIdDisplay>{record && record.id > 0 ? "Listing ID " + record.id : ""}</TableIdDisplay>
                        </Right>
                    </>
                }
            </TableHeaderContainer>
            <TableContainer>
                <GLTable
                    dataProvider={dataProvider}
                    columns={columns}
                    replaceColsForRow={replacements}
                    showHeadersBeforeRow={[0]}
                    spacesImport={spacesImport}
                    actionButton={displayActionBtns && {
                        label: spacesImport ? "Import" : (recordExists ? "Edit" : "Create"),
                        clickHandler: handleCreateEdit
                    } || undefined}
                    assignBtn={showAssignButton}
                    assignHandler={handleAssign}
                    unpublishBtn={showUnpublishButton}
                    unpublishHandler={handleUnpublish}
                    styles={styles} />
            </TableContainer>
        </ViewContainer>
    );
}

/* Styled Components */
const ViewContainer = styled.div``;

const TableHeaderContainer = styled.div`
    width: 100%;
    padding: 5px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 5px;
`;

const TableHeader = styled.div`
    font-size: 24px;
    color: #666666;
    font-weight: 800;
    display: inline-block;
    margin-right: 25px; 
`;

const Left = styled.div`
    display: inline-block;
`;

const Right = styled.div`
    display: inline-block;
`;

const TableHeaderHelper = styled.div`
    font-size: 16px;
    color: #666666;
    font-style: italic;
    display: inline-block;
`;

const TableIdDisplay = styled.div`
    font-size: 16px;
    color: #666666;
    display: inline-block;
    margin-right: 5px;
`;

const TableContainer = styled.div``;

/* property cells */
const PropertyCellContainer = styled.div`
    margin-top: -25px;
`;

const PropertyImage = styled.img`
    max-width: 140px;
    display: inline-block;
`;

const PropertyAddress = styled.div`
    margin-left: 10px;
    display: inline-block;
    vertical-align: top;
    width: 195px;
`;

const AddressHeader = styled.div`
    font-style: normal;
    font-weight: 800;
    font-size: 16px;
    line-height: 21px;
    color: #666666;
`;

const AddressLine = styled.div`
    font-weight: 500;
    font-size: 14px;
    line-height: 20px;
`;

const BadgeContainer = styled.div`
    max-width: 100px;
`;

const IDDisplay = styled.div`
    color: #0000FF;
    font-size: 12px;
`;

export default PropertyResultView;