import React, { FC, useState } from 'react';
import styled from 'styled-components';
import PDFIcon from '../../assets/images/png/pdfIcon.png'
import PinkStar from '../../assets/images/png/hotPinkStar.png'
import EditIcon from '../../assets/images/png/icon-edit.png'
import CBRELogo from '../../assets/images/png/cbre-logo-white.png';
import ViewIcon from '../../assets/images/png/icon-view.png';
import { RingLoader, ScaleLoader, HashLoader } from 'react-spinners';
import Loader from '../loader/loader';

export enum ThumbnailSize {
    NORMAL,
    SMALL
}

export interface ThumbnailOverlay {
    show: boolean,
    message: string,
    icon: any,
    color: string,
    allowRemove: boolean,
    allowOverride: boolean,
    allowRetry: boolean,
    retryLabel: string
}

interface ThumbnailProps {
    file: any,
    primary: boolean,
    setFileName?: any,
    index: number,
    openShadowBox?: (url:string) => void,
    openPDFPreview? : (url:string) => void,
    constructImageArray?: any,
    id?: number,
    name: any,
    watermark?: boolean | null,
    hideStar: boolean,
    errorMsg?: string | null,
    loadingMsg?: string | null,
    showNameEdit: boolean,
    size: ThumbnailSize,
    overlay?: ThumbnailOverlay,
    removeFailedProcess?: (id:number) => void,
    overrideFailure?: (id:number) => void,
    retryError?: (id:number) => void
}

interface ContainerProps {
    imageHeight: number,
    imageWidth: number,
    overlay?: ThumbnailOverlay
}

const cleanFileName = (value:string) => {
    let result = '';
    if (value) {
        const extIndex = value.lastIndexOf('.');
        if (extIndex > -1) {
            result = value.substr(0, extIndex);
        } else {
            result = value;
        }
    }
    return result;
}

const Thumbnail: FC<ThumbnailProps> = (props) => {
    const [showInput, setShowInput] = useState(false);
    const [fileName, setFileName] = useState(cleanFileName(props.name));
    const [processingRetry, setProcessingRetry] = useState<boolean>(false);

    let fileExtension:string = '';
    if(props && props.file){
        fileExtension = props.file.substr(props.file.lastIndexOf('.'), props.file.length);
    }

    let imageHeight:number = 150;
    let imageWidth:number = 200;

    if(props.size === ThumbnailSize.SMALL){
        imageHeight = 93.75;
        imageWidth = 125;
    }

    // toggles between <input> element and p tag
    const toggleInput = () => {
        setShowInput(!showInput)
    }

    // updates local state and form-upload's parent state with the input's value
    const updateValue = (event: any) => {
        if(setFileName){
            const value = event.currentTarget.value;
            setFileName(value);
            props.setFileName(value, fileExtension, props.index);
        }
    }

    // reverts to title text from input if the user hits esc or enter
    const handleKeyPress = (event: any) => {
        const value = event.currentTarget.value;
        setFileName(value)
        if (event.keyCode === 13) {
            if (fileName !== '') {
                toggleInput();
                props.setFileName(value, fileExtension, props.index);
            }
        } else if (event.keyCode === 27) {
            if (fileName !== '') {
                toggleInput();
            }
        }
    }

    const openShadowBoxFunc = () => {
        if(props.openShadowBox){
            props.openShadowBox(props.file);
        }
    }

    const openPDFPreview = () => {
        if(props.openPDFPreview){
            props.openPDFPreview(props.file);
        }
    }

    const removeProccessingError = () => {
        if(props.removeFailedProcess && props.id){
            props.removeFailedProcess(props.id);
        }
    }

    const override = () => {
        if(props.overrideFailure && props.id){
            props.overrideFailure(props.id);
        }
    }

    const retry = () => {
        setProcessingRetry(true);
        if(props.retryError && props.id){
            props.retryError(props.id);
        }
    }
  
    
    if (props.loadingMsg === "Loading" && (!props.overlay || props.overlay.show === false)) {
        return(
            // style={{width: "100px", height: "100px", backgroundColor: "#dedede"}}
            <InfoThumbnailContainer key={props.file + props.name}>                   

                <div className="bkg-thumbnail">
                    <div className="info-text">
                        <div style={{marginBottom:"5px"}}>Processing...</div>
                        <ScaleLoader
                        color={'#dedede'}
                        loading={true}
                        />
                    </div>               
                </div>          
            <p>{fileName}</p>
        
            </InfoThumbnailContainer>
        )
    }
    else if (props.errorMsg !== undefined && props.errorMsg !== null) {
        return(
            // style={{width: "100px", height: "100px", backgroundColor: "#dedede"}}
            <InfoThumbnailContainer key={props.file + props.name}>                   

                <div className="bkg-thumbnail">
                    <div className="error-text">
                        <div style={{marginBottom:"5px"}}>{props.errorMsg}</div>
                    </div>               
                </div>          
            <p>{fileName}</p>
        
            </InfoThumbnailContainer>
        )
    }
    else if (!props.file) {
        return null
    } 

    return (
        <ThumbnailContainer key={props.file + props.name} imageHeight={imageHeight} imageWidth={imageWidth} overlay={props.overlay}>                   
            {(fileExtension.includes('.png') || fileExtension.includes('.jpg') || fileExtension.includes('.jpeg') || fileExtension.includes('.PNG') || fileExtension.includes('.JPG') || fileExtension.includes('JPEG')) ?
                <div className={props.watermark ? 'watermark' : ''}>
                    { props.overlay && props.overlay.show && props.file !== '/img/loading.png' && 
                        <Overlay imageHeight={imageHeight} imageWidth={imageWidth}>
                            <OverlayHeader>
                                {!processingRetry && props.overlay && props.overlay.allowOverride === true && <OverlayButton onClick={override}>Override</OverlayButton>}
                                {!processingRetry && props.overlay && props.overlay.allowRetry === true && <OverlayButton onClick={retry}>{props.overlay.retryLabel}</OverlayButton>}
                                {!processingRetry && props.overlay && props.overlay.allowRemove === true && <OverlayRemove onClick={removeProccessingError}>X</OverlayRemove>}
                            </OverlayHeader>
                            <OverlayBody onClick={() => openShadowBoxFunc()} imageHeight={imageHeight} imageWidth={imageWidth} overlay={props.overlay}>
                                {!processingRetry && props.overlay && props.overlay.message && <OverlayMessage>{props.overlay.message}</OverlayMessage>}
                                {processingRetry && <OverlayMessage>Checking Image...</OverlayMessage>}
                            </OverlayBody>
                        </Overlay>    
                    }
                    {props.watermark && <img src={CBRELogo} onClick={() => openShadowBoxFunc()} />}
                    <div onClick={() => openShadowBoxFunc()}
                        style={{ backgroundImage: `url(${props.file})` }}
                        className="img-thumbnail"
                    />
                </div> 
                :
                <PDFContainer className="pdfIcon" onClick={openPDFPreview}><img src={PDFIcon} /></PDFContainer>
            }
            
            {(showInput) ? <input onKeyDown={handleKeyPress} onChange={(e: any) => (updateValue(e))} id="test" type="text" value={fileName} /> :
                props.showNameEdit ? <p onClick={toggleInput}>{fileName}<img src={EditIcon} /></p> : <p>{fileName}</p>}
            {props.primary && fileExtension !== '.pdf' && !fileExtension.includes('.xl') && !props.hideStar &&
                <PrimaryStar><img src={PinkStar} /></PrimaryStar>}
        </ThumbnailContainer>
    )
}

const PDFContainer = styled.div`
    cursor: pointer;
`;

const PrimaryStar = styled.span`
    height:25px;
    width:25px;
    position:absolute;
    top:2px;
    right:5px;
    color:#fff;
    text-align:center;
    font-size:18px;
`;

const InfoThumbnailContainer = styled.div`
    p {
        text-align:left;
        color: #666666;
        font-size: 13px;
        font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};    
        line-height: 15px;
        padding-left:10px;
        position:relative;
        padding-right:8px;
        word-break: break-word;
    }
    .bkg-thumbnail {
        height:120px; 
        width:170px;
        padding:15px;
        text-align:center;
        border: 1px rgba(0,0,0,0.15) dashed;
        background:#F6F6F6;      
    }
    .info-text {
        padding-top:40px;
        color: #dcdcdc;
    }
    .error-text {
        padding-top:40px;
        color: red;
    }
`;

const ThumbnailContainer = styled.div`
    p {
        text-align:left;
        cursor:pointer;
        color: #666666;
        font-size: 13px;
        font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};    
        line-height: 15px;
        padding-left:10px;
        transition:.2s ease all;
        position:relative;
        padding-right:8px;
        > img {
            position:absolute;
            top:1px;
            right:1px;
            width: 13px;
            height: 15px;
        }
    }
    .watermark {
        position:relative;
        > img {
            position:absolute;
            height:5%;
            left:43%;
            top:48%;
            cursor:pointer;
            opacity: .9; 
        }
    }
    p:hover {
        top:1px;
    }
    .img-thumbnail {
        height: ${(props:ContainerProps) => props.imageHeight ? props.imageHeight + 'px' : '200px'}; 
        width: ${(props:ContainerProps) => props.imageWidth ? props.imageWidth + 'px' : '200px'};
        background-position:center center;
        background-size: cover;
        background-repeat: no-repeat;
        cursor:pointer;
    }
    input {
        display:flex;
        margin:1rem auto;
        color: #999;    
        font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};    
        font-size: 13px;
        width:90%;
        margin-top:8px;
        margin-bottom:0;
    }
    .pdfIcon {
        background:#f6f6f6;
        text-align:center;
        width:200px;
        height:150px;
        > img {
            margin-top:40px;
        }
    }
    #pencilIcon {
        opacity:.7;
        height:14px;
        top:0;
        left:190px;
        bottom:0;
    }
`;

const Overlay = styled.div`
    height: ${(props:ContainerProps) => props.imageHeight ? props.imageHeight + 'px' : '200px'};
    width: ${(props:ContainerProps) => props.imageWidth ? props.imageWidth + 'px' : '200px'}; 
    position: absolute;
    z-index: 2;
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};
    font-size: 12px;
    color:  ${(props:ContainerProps) => props.overlay && props.overlay.color ? props.overlay.color : 'black' };
    text-align: center;
`;

const OverlayHeader = styled.div` 
    height: 25px;
    line-height: 25px;
    width: 100%;
    background: black;
    text-align: left;
`;

const OverlayBody = styled.div`
    margin-top: ${(props:ContainerProps) => props.overlay && props.overlay.allowOverride ? '0' : '0'};
    cursor: pointer;
`;

const OverlayMessage = styled.div`
    margin-top: 10px;
    white-space: pre-wrap;
`;

const OverlayRemove = styled.a`
    color: white;
    font-size: 14px;
    font-weight: bold;
    position: absolute;
    top: 0;
    right: 5px;
    cursor: pointer;
`;

const OverlayButton = styled.a`
    background: #00a657;
    color: white;
    font-size: 11px;
    font-weight: bold;
    cursor: pointer;
    padding: 3px;
    margin-left: 2px;
`;

export default React.memo(Thumbnail);