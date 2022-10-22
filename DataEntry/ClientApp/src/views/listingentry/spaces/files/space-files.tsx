import React, { FC } from 'react';
import GLField from '../../../../components/form/gl-field';
import { Col, Row } from 'react-styled-flexboxgrid';
import FormUpload, { FormUploadProps } from '../../../../components/form-upload/form-upload';
import { GLFile } from '../../../../types/listing/file';
import { Config } from '../../../../types/config/config';

interface Props {
    files: SpaceFileData,
    onChange: (files:SpaceFileData) => void,
    config: Config
}

export interface SpaceFileData {
    photos: GLFile[],
    floorplans: GLFile[],
    brochures: GLFile[]
}

const SpaceFiles: FC<Props> = (props) => {

    const { files, onChange, config } = props;

    const updatePhotoPayload = (payload: any) => {
        files.photos = payload;
        onChange(files);
    }

    const updateFloorplanPayload = (payload: any) => {
        files.floorplans = payload;
        onChange(files);
    }

    const updateBrochurePayload = (payload: any) => {
        files.brochures = payload;
        onChange(files);
    }

    return (
        <>
            <Row style={{ width: '100%' }}>
                <Col xs={12}>
                    <GLField<FormUploadProps> showPrimary={true} name="photos" title="Photos" label="Add Photo"
                        accepted=".jpg, .png, .jpeg" description="JPEG or PNGs only" watermark={config.watermark}
                        files={files.photos} allowWatermarkingDetect={true} useRestb={true} 
                        updatePhotoPayload={updatePhotoPayload} use={FormUpload} />
                </Col>
            </Row>
            <Row style={{ width: '100%' }}>
                <Col xs={12}>
                    <GLField<FormUploadProps> showPrimary={false} name="floorplans" title="Floorplan" label="Add Floorplan"
                        singleUpload={true} singleFileUploadWarning={'Only 1 file allowed per space. Please preview to ensure the correct one uploaded'}
                        accepted=".jpg, .png, .jpeg, .pdf" description="JPEG, PNG or PDF" watermark={config.watermark}
                        files={files.floorplans} allowWatermarkingDetect={false} useRestb={true}
                        updatePhotoPayload={updateFloorplanPayload} use={FormUpload} />
                </Col>
            </Row>
            <Row style={{ width: '100%' }}>
                <Col xs={12}>
                    <GLField<FormUploadProps> showPrimary={false} name="brochures" title="Brochure" label="Add Brochure" 
                        singleUpload={true} singleFileUploadWarning={'Only 1 PDF allowed per space. Please preview to ensure the correct one uploaded'}
                        accepted=".pdf" description="PDF only"
                        files={files.brochures} allowWatermarkingDetect={false} updatePhotoPayload={updateBrochurePayload} use={FormUpload} />
                </Col>
            </Row>
        </>
    )
}

export default SpaceFiles;