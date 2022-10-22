import React, { FC, useContext, useState } from 'react';
import { FormContext } from '../../../components/form/gl-form-context';
import { Listing } from '../../../types/listing/listing';
import { Col, Row } from 'react-styled-flexboxgrid';
import SectionHeading from "../../../components/section-heading/section-heading";
import { useSelector } from 'react-redux';
import { Config, AspectConfig } from '../../../types/config/config';
import { configSelector } from '../../../redux/selectors/system/config-selector';
import styled from 'styled-components';
import BlueCheck from '../../../assets/images/png/blue-check.png';
import GrayCheck from '../../../assets/images/png/gray-check.png';


export interface Props {
    listing: Listing
}

const Amenities: FC<Props> = (props) => {

    const { listing } = props;
    const config: Config = useSelector(configSelector);
    const listAspectsArr: string[] = listing.aspects ? listing.aspects : [];
    const aspectsArr: AspectConfig[] = (config.aspects && config.aspects.options) ? config.aspects.options : [];
    const [listAspects, setListAspects] = useState<string[]>(listAspectsArr);

    const formControllerContext = useContext(FormContext);
    const updateAspectsData = (aspectsUpdate: string[]) => {
        const values = {
            'aspects': [...aspectsUpdate]
        };
        formControllerContext.onFormChange(values);
    }

    const aspectKey = (aspect: AspectConfig) => {
        if (aspect) {
            return aspect.id + new Date().getTime();
        }
        return "aspect" + new Date().getTime();
    }

    const toggleAspect = (aspect: AspectConfig) => {
        const tempListAspects: string[] = [...listAspects];
        const idx = tempListAspects.findIndex((u: string) => u === aspect.id);
        if (idx > -1) {
            tempListAspects.splice(idx, 1);
        }
        else {
            tempListAspects.push(aspect.id);
        }
        setListAspects(tempListAspects);
        updateAspectsData(tempListAspects);
    }

    const removeAspect = (aspectId: string) => {
        const tempListAspects: string[] = [...listAspects];
        const idx = tempListAspects.findIndex((u: string) => u === aspectId);
        tempListAspects.splice(idx, 1);
        setListAspects(tempListAspects);
        updateAspectsData(tempListAspects);
    }


    return (
        <AmenityContainer>
            <Row id="aspects" style={{ marginTop: "25px" }}>
                <Col xs={12}><SectionHeading>Amenities</SectionHeading></Col>
                <Col xs={12}>
                    <Container>
                        {aspectsArr && aspectsArr.map((aspect: AspectConfig, index: number) => (
                            <div key={aspectKey(aspect)} >
                                <AmenityThumbnail data-testid={'amenity'} style={(listAspects.indexOf(aspect.id) > -1) ? { backgroundColor: '#E7F6FA', border: '1px #059FC4 solid' } : { backgroundColor: '#fff', border: '1px #dedede solid', color: '#999' }} onClick={() => toggleAspect(aspect)}>
                                    <AmenityIcon style={(listAspects.indexOf(aspect.id) > -1) ? { backgroundColor: '#E7F6FA' } : { backgroundColor: '#F6F6F6' }}>
                                        {(listAspects.indexOf(aspect.id) > -1) ? <img src={BlueCheck} alt="blue check mark" /> : <img src={GrayCheck} alt="gray check mark" />}
                                    </AmenityIcon>
                                    {aspect.display}
                                </AmenityThumbnail>
                            </div>
                        ))}
                    </Container>
                </Col>
            </Row>
        </AmenityContainer>
    );
}

const Container = styled.div`
    display:flex;
    flex-direction: row;
    flex-wrap: wrap;
    max-width:730px;

    > div {
        margin-right:11px;
        margin-top:15px;
    }
`

const AmenityContainer = styled.div`

`;

const AmenityThumbnail = styled.div`
    cursor: pointer;
    color: #666666;
    border-radius: 5px;
    width: auto;
    text-align: center;
    padding:8px;
    padding-right:12px;
    display:flex;
    justify-content: center;
    align-items:center;
    border: 1px solid #dedede;
    font-style: normal;
    font-weight: 500;
    font-size: 14px;
    line-height: 17px;
    color: #666666;

`;

const AmenityIcon = styled.div`
    margin-right: 7px;
    margin-top:1px;
    > img {
        margin-bottom:-1px;
    }
`;


export default React.memo(Amenities, (prevProps, nextProps) => nextProps.listing !== prevProps.listing);

