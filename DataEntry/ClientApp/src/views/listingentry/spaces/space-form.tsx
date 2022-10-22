import React, { FC } from 'react';
import GLForm from '../../../components/form/gl-form';
import { convertValidationJSON } from '../../../utils/forms/validation-adapter';
import GLField from '../../../components/form/gl-field';
import { Col, Row } from 'react-styled-flexboxgrid';
import FormInput, { FormInputProps } from '../../../components/form-input/form-input'
import FormCheckbox, { FormCheckboxProps } from '../../../components/form-checkbox/form-checkbox';
import FormSelect, { FormSelectProps } from '../../../components/form-select/form-select';
import FormDateField, { FormDateFieldProps } from '../../../components/form-date-field/form-date-field';
import styled from 'styled-components';
import { Space } from '../../../types/listing/space';
import SpaceFiles, { SpaceFileData } from './files/space-files';
import FormTextArea, { FormTextAreaProps } from '../../../components/form-text-area/form-text-area';
import FormTabbedTextArea, { FormTabbedTextAreaProps } from '../../../components/form-tabbed-text-area/form-tabbed-text-area';
import { Config, ConfigFieldType } from '../../../types/config/config';
import { SpacesFieldSetup, SpacesListingTypeFields, SpacesPropertyTypeFields } from '../../../types/config/spaces/spaces';
import { ViewProperties } from './space';
import SizesAndMeasurements, { SizeType } from '../sizesAndMeasurements/sizes-and-measurements';

interface Props {
    idx: number,
    space: Space,
    viewProperties: ViewProperties,
    config: Config,
    fields: SpacesListingTypeFields,
    handleChange: (values: any) => void    
}

const SpaceFormView: FC<Props> = (props) => {

    const { idx, space, viewProperties, config, fields, handleChange } = props;

    const disableOfUnitMeasurement: boolean = false;

    const files:SpaceFileData = {
        photos: space.photos,
        floorplans: space.floorplans,
        brochures: space.brochures
    };

    // utility functions and helper methods to help us build our form dynamically
    const getColSize = <T extends {}>(fieldSetup: SpacesFieldSetup<T>): number => {
        if (fieldSetup && fieldSetup.show && fieldSetup.grid && fieldSetup.grid.colSize) {
            return fieldSetup.grid.colSize;
        }
        return 6;  // 6 is our current default for col size if not specified
    }

    // helper methods
    const getKey = (type:string, name:string):string => {
        return type + "_" + name + getAppendToId();
    }

    const getAppendToId = ():string => {
        return space && space.id && space.id > 0 ? space.id.toString() : "create" + idx;
    }

    const getTextFieldLangSettings = (placeholderName:string) => {
        let langSettings:any = [];
        if (config && config.languages) {
            let translations:any[] = [];
            if (config.translations){
                translations = Array.from(config.translations);         
            }

            const langs:string[] = Array.from(config.languages);

            
            langSettings = Array.from({ length: langs.length }, (v, indx) => indx).map(indx => {
                let languageName = langs[indx];
                let placeholder = "Enter Text...";
                if (translations.length>0){

                    const itemData = translations.filter((d: any) => {
                        return d.cultureCode === langs[indx];
                      });
                      if (itemData.length > 0) {
                        languageName = itemData[0].languageName;
                        placeholder = itemData[0][placeholderName];
                      }
                }

                

                const settings: any = {
                    cultureCode: `${langs[indx]}`,
                    lang: `${languageName}`,
                    placeholder: `${placeholder}`
                };
                return settings;
            });
        }

       
        return langSettings;
    }

    const handleTabChange = (payload: any) => {
        space.spaceDescription = payload;    
        handleChange(space);
    }

    const handleTabNameChange = (payload: any) => {
        space.name = payload;       
        handleChange(space);
    }

    const checkFocus = (field:any) => {
        if(field.properties.name === viewProperties.autoFocusField){
            return true;
        }
        return false;
    }

    const buildFormFields = (fieldset:SpacesListingTypeFields) => {

        // we need to build an array of fields and order them properly
        const exclude:string[] = [];
        const fieldsArray:any[] = [];

        Object.keys(fieldset).forEach(key => {
            if(exclude.indexOf(key) === -1){
                fieldsArray.push(fieldset[key]);
            }
        });

        // now, sort the fields using the "order" field setup in the configurations
        fieldsArray.sort((field1, field2) => (field1.order > field2.order) ? 1 : -1);

        // now that we have a sorted array, build the actual dom and fields dynamically
        return fieldsArray.map(field => {
            if(field.show){
                // add any properties we need to append to the field before rendering it : these would generally be properties this view sets dynamically in some way that
                // we can't just store in the configuration

                const instanceProperties = Object.assign({},field.properties);        // copy the field properties so we can modify as needed and then use in our configuration

                if(field.viewProperties){
                    Object.keys(field.viewProperties).forEach(attribute => {
                        // where the attribute is the attribute we are setting on the dom and the value [held in field.viewProperties[attribute]] is the property we are pulling from this view
                        if(viewProperties[field.viewProperties[attribute]] !== undefined){
                            // check to see if there is a field modifier for this attribute before we set a value for the key pair
                            let value:any = viewProperties[field.viewProperties[attribute]];   // grab the dynamic view data
                            if(field.modifiers && field.modifiers[attribute] && value){
                                // todo: if we add more modifiers, we'll need to think of a cleaner way to approach this [util func?]. may need to add order of operations too.
                                if(field.modifiers[attribute].prepend){
                                    value = field.modifiers[attribute].prepend + value;
                                }
                            }
                            instanceProperties[attribute] = value;
                        }
                    });
                }

                if(field.type === ConfigFieldType.FORM_INPUT.valueOf()){
                    return <Col key={getKey("kc",instanceProperties.name)} xs={getColSize(field)} className={field.className ? field.className : undefined}>
                                <GLField<FormInputProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()}
                                    suffix={instanceProperties.name === 'specifications.maxPrice' ? viewProperties.priceSuffix : ''} 
                                    {...instanceProperties} use={FormInput} />
                            </Col>;
                }
                else if(field.type === ConfigFieldType.FORM_TABBED_TEXT_AREA.valueOf()){
                    if (config && config.languages) {
                        return (
                            <Col key={getKey("kc",instanceProperties.name)} xs={getColSize(field)} className={field.className ? field.className : undefined}> 
                                <GLField<FormTabbedTextAreaProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()}  data={space[instanceProperties.name]} tabsettings={getTextFieldLangSettings("plBuildingDescription")}  updateTabData={space[instanceProperties.name] === 'name'? handleTabNameChange : handleTabChange} {...instanceProperties} use={FormTabbedTextArea} />
                            </Col>                          
                        );
                    }
                    return <Col key={getKey("kc",instanceProperties)} xs={getColSize(field)} className={field.className ? field.className : undefined}> 
                                <GLField<FormTextAreaProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()} 
                                    {...instanceProperties} use={FormTextArea} />
                            </Col>;
                }
                else if(field.type === ConfigFieldType.FORM_SPACE_DESC.valueOf()){
                    if (config && config.languages) {
                        return (
                            <Col key={getKey("kc",instanceProperties.name)} xs={getColSize(field)} className={field.className ? field.className : undefined}> 
                                <GLField<FormTabbedTextAreaProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()}  data={space.spaceDescription} tabsettings={getTextFieldLangSettings("plBuildingDescription")}  updateTabData={handleTabChange} {...instanceProperties} use={FormTabbedTextArea} />
                            </Col>                          
                        );
                    }
                    return <Col key={getKey("kc",instanceProperties.name)} xs={getColSize(field)} className={field.className ? field.className : undefined}> 
                                <GLField<FormTextAreaProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()} 
                                    {...instanceProperties} use={FormTextArea} />
                            </Col>;
                }
                else if(field.type === ConfigFieldType.FORM_SPACE_NAME.valueOf()){
                    if (config && config.languages) {
                        return (
                            <Col key={getKey("kc",instanceProperties.name)} xs={getColSize(field)} className={field.className ? field.className : undefined}> 
                                <GLField<FormTabbedTextAreaProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()}  data={space.name} tabsettings={getTextFieldLangSettings("plBuildingDescription")}  updateTabData={handleTabNameChange} {...instanceProperties} use={FormTabbedTextArea} />
                            </Col>                          
                        );
                    }                    
                    return <Col key={getKey("kc",instanceProperties)} xs={getColSize(field)} className={field.className ? field.className : undefined}> 
                                <GLField<FormInputProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()}
                                    {...instanceProperties} use={FormInput} />
                            </Col>;
                }
                else if(field.type === ConfigFieldType.FORM_TEXT_AREA.valueOf()){
                    return <Col key={getKey("kc",instanceProperties.name)} xs={getColSize(field)} className={field.className ? field.className : undefined}> 
                                <GLField<FormTextAreaProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()} 
                                    {...instanceProperties} use={FormTextArea} />
                            </Col>;
                }
                else if(field.type === ConfigFieldType.FORM_SELECT.valueOf()){
                    return <Col key={getKey("kc",instanceProperties.name)} xs={getColSize(field)} className={field.className ? field.className : undefined}> 
                                <GLField<FormSelectProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()} forceFocus={checkFocus(field)}
                                 disabled={(instanceProperties.name === 'specifications.measure' && disableOfUnitMeasurement)} {...instanceProperties} use={FormSelect} />
                            </Col>;
                }else if(field.type === ConfigFieldType.FORM_DATE_FIELD.valueOf()){
                    return <Col key={getKey("kc",instanceProperties.name)} xs={getColSize(field)} className={field.className ? field.className : undefined}> 
                                <GLField<FormDateFieldProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()}
                                 {...instanceProperties} use={FormDateField} />
                            </Col>;
                }else if(field.type === ConfigFieldType.FORM_CHECKBOX.valueOf()){
                    return <Col key={getKey("kc",instanceProperties.name)} xs={getColSize(field)} className={field.className ? field.className : undefined}> 
                                <GLField<FormCheckboxProps> key={getKey("kf",instanceProperties.name)} appendToId={getAppendToId()} forceFocus={checkFocus(field)}
                                 {...instanceProperties} use={FormCheckbox} />
                            </Col>;
                }else if(field.type === ConfigFieldType.CONDITIONAL_INCLUDE.valueOf()){
                    if(field.properties && field.properties.name === "includeFiles"){
                        return <Col xs={12} key={getKey("kc","includeFiles")}><SpaceFiles files={files} config={config} onChange={handleChange} /></Col>;
                    }else if(field.properties && field.properties.name === "spaceSizes"){
                        return <Col xs={12} key={getKey("kc","sandmeasure")}>
                            <SizesAndMeasurements config={config} sizeTypeSetup={field.properties.setup} sizeTypes={space.spaceSizes} changeHandler={(sizeTypes:SizeType[]) => { handleChange({spaceSizes: sizeTypes})}}/>
                        </Col>;
                    }


                    return <></>;
                }
                else{
                    return <></>;
                }
            }else{
                return <></>;
            }
        });
    }

    if(!fields){
        return (
            <Row>
                <Col xs={12}>
                    Error: Field configuration not properly setup.
                </Col>
            </Row>
        );
    }else{
        return (
            <GLForm initVals={space}
                validationAdapter={convertValidationJSON}
                validationJSON={{}}
                changeHandler={handleChange}>
                <DynamicCSSWrapper>
                    <>{buildFormFields(fields) }</>
                    { }


                </DynamicCSSWrapper>
            </GLForm>
        );
    }
}

/* add any dynamic css needed to this wrapper then inject using the configuraiton file */
const DynamicCSSWrapper = styled(Row as any)`
    .checkbox-wrapper {
        margin: 40px 0 0 30px;
    }
`;

export default SpaceFormView;