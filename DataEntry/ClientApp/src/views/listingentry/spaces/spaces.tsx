import React, { FC, useContext } from 'react'
import { Col, Row } from 'react-styled-flexboxgrid'
import { FormContext } from '../../../components/form/gl-form-context';
import { Listing } from '../../../types/listing/listing';
import styled from 'styled-components';
import DraggableAccordion from '../../../components/draggable-accordion/draggable-accordion';
import { flexSpaceNames } from '../../../api/lookups/space-flex-names';
import SpaceView from './space';
import { Config } from '../../../types/config/config';
import { useSelector } from 'react-redux';
import { configSelector } from '../../../redux/selectors/system/config-selector';
import { SpacesAccordionSetup } from '../../../types/config/spaces/spaces';

interface Props {
    listing: Listing,
    manualErrors: object,
    showSpacesModal: any,
    miqSpaces?: any,
    imageProcessingCheck: (valuesToUpdate:object) => void
}

const Spaces: FC<Props> = (props) => {

    const { listing, manualErrors, imageProcessingCheck, showSpacesModal, miqSpaces } = props;

    const config: Config = useSelector(configSelector);

    let header:string = "";
    let useLabelForPaneHeader: boolean = false;
    let getFieldLabelFromCultureCode: string = "";
    
    if(config && config.spaces && config.spaces.accordionSetups){
        // we have setup information for this accordion in our config, based on property type selected
        config.spaces.accordionSetups.forEach((setup:SpacesAccordionSetup) => {
            if(setup.propertyTypes && setup.propertyTypes.indexOf(listing.propertyType) > -1){
                header = setup.header;
                useLabelForPaneHeader = setup.useLabelForPaneHeader;
            }
        });
    }

    if(config && config.languages){
        getFieldLabelFromCultureCode = "any";
        if (config.defaultCultureCode){
            getFieldLabelFromCultureCode = config.defaultCultureCode;
        }
    }

    const formControllerContext = useContext(FormContext);

    // manual spaces error
    let spacesErrorMessage:string = "";
    const spacesKey:string = "spaces";
    if(manualErrors && manualErrors[spacesKey] && manualErrors[spacesKey].error){
        spacesErrorMessage = manualErrors[spacesKey].message;
    }

    const changeHandler = (values: any) => {
        const incomingValues:any = values;
        for (const value of incomingValues){
            if (!value.status){
                const key = 'status';
                value[key] = "available"
            }
    
            if (!value.specifications){
                const key = 'specifications';
                value[key] = {}
            }

            if (!value.name){
                const key = 'name';
                value[key] = []
            }

            if (!value.spaceDescription){
                const key = 'spaceDescription';
                value[key] = []
            }
        }

        const valueObj = {
            'spaces': [
                ...incomingValues
            ]
        }

        if(imageProcessingCheck){
            imageProcessingCheck(valueObj);
        }else{
            formControllerContext.onFormChange(valueObj);
        }
    }


    return (
        <SpacesContainer>
            <div id="spaces">&nbsp;</div>
            <Row>
                <Col xs={12}>
                    <DraggableAccordion name="sAcc" dataProvider={listing.spaces} inject={SpaceView} dataParams={listing} showSpacesModal={showSpacesModal}
                        manualError={spacesErrorMessage && spacesErrorMessage.length > 0 ? true : false} manualErrorMessage={spacesErrorMessage}
                        header={header} titleField={config.languages && config.languages.length > 0 ? "name" : "nameSingle" } 
                        addLabel="Add Space" changeHandler={changeHandler} getLabelFromCode={getFieldLabelFromCultureCode} showMiqSpaces={miqSpaces.length > 0 ? true : false}
                        useLabelForHeader={useLabelForPaneHeader} labelProvider={flexSpaceNames} displayMIQId={config.displayMIQId}/>
                </Col>
            </Row>
        </SpacesContainer>
    );
}

const SpacesContainer = styled.div`
    background: '#fdfdfd'; 
    marginBottom: '60px';
`;

export default React.memo(Spaces, (prevProps, nextProps) => nextProps !== prevProps)