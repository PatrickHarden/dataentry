import React, { FC, useState, useCallback, useRef } from 'react';
import styled, { css } from 'styled-components';
import FormFieldLabel from '../form-field-label/form-field-label';
import Thumbnail, { ThumbnailSize, ThumbnailOverlay } from './thumbnail';
import PopOver, { PopOverItem } from '../popover/popover';
import { useDropzone } from 'react-dropzone'
import { GLFile, WatermarkProcessStatus } from '../../types/listing/file';
import { Col, Row, Grid } from 'react-styled-flexboxgrid';
import Lightbox from 'react-image-lightbox';
import "react-image-lightbox/style.css";
import { postFileNoWatermarkCheck, postFileWithWatermarkCheck, checkImage } from '../../api/glAxios';
import FormCheckbox from '../form-checkbox/form-checkbox'
import { useSelector } from 'react-redux';
// images
import DeleteIcon from '../../assets/images/png/deleteIcon.png';
import StarIcon from '../../assets/images/png/blueStarIcon.png';
import InactiveIcon from '../../assets/images/png/eyeIcon.png';
import CBRELogo from '../../assets/images/png/cbre-logo-white.png';
import WatermarkIcon from '../../assets/images/png/icon-watermark.png'
import { checkFileForWatermarkProblems, problemWatermarkStatuses } from '../../utils/image-check';
import { featureFlagSelector } from '../../redux/selectors/featureFlags/feature-flag-selector';
import { refreshImagesSelector } from '../../redux/selectors/entry/refresh-images-selector';
import ErrorIcon from '../../assets/images/png/icon-error.png';
import { User, FeatureFlags } from '../../types/state';
import { userSelector } from '../../redux/selectors/system/user-selector';
import PDFPreview from './pdf-preview';
import { sortGLFiles } from '../../utils/sort-files';


export interface FormUploadProps {
    name: string,
    label: string,
    options?: any,
    prompt?: string,
    doSort?: boolean
    title: string,
    accepted: string,
    description: string,
    showPrimary: boolean,
    manualError?: boolean,
    manualErrorMessage?: string,
    updatePhotoPayload: any,
    singleUpload?: boolean,
    singleFileUploadWarning?: string,
    files: any,
    watermark?: boolean,
    allowWatermarkingDetect: boolean,   // additional flag to indicate whether the upload even allows detect (i.e., brochures dont)
    useRestb?: boolean,
    defaultWaterMarkTrue?: boolean
}

const FormUpload: FC<FormUploadProps> = (props) => {

    // variables from props:
    const { name, accepted, description, title, manualError, manualErrorMessage, label, files, singleUpload, singleFileUploadWarning,
             allowWatermarkingDetect, useRestb, defaultWaterMarkTrue } = props;
    // used to toggle popover - only one popover can be open at once for ux purposes
    const [popover, setPopover] = useState(-1);
    // used to trigger shadowbox
    const [isOpen, setIsOpen] = useState(false);
    // used to trigger pdf preview
    const [pdfPreviewURL, setPdfPreviewURL] = useState<string>("");
    // index for image array
    const [photoPreviewURL, setPhotoPreviewURL] = useState<string>("");
    // warning message
    const [warningMessage, setWarningMessage] = useState<string | undefined>(undefined);
    // feature flags
    const featureFlags:FeatureFlags = useSelector(featureFlagSelector);
    // image refresh bucket
    const imageRefreshBucket: GLFile[] = useSelector(refreshImagesSelector);
    // user selector
    const user:User = useSelector(userSelector);

    const watermarkCheckbox = useRef(defaultWaterMarkTrue ? defaultWaterMarkTrue : false);

    const findRefreshStatus = (file:GLFile) => {
        if(!imageRefreshBucket){
            return file.watermarkProcessStatus;
        }
        let watermarkProcessStatus:number | undefined = file.watermarkProcessStatus;
        imageRefreshBucket.forEach((refreshFile:GLFile) => {
            if(file.id && refreshFile.id && refreshFile.id === file.id){
                watermarkProcessStatus = refreshFile.watermarkProcessStatus;
            }
        });
        return watermarkProcessStatus;
    }


    let tempPayload: GLFile[] = [];

    let watermarkProcessChanged:boolean = false;  // a flag we will change if a change has been detected in status (from the refresh bucket)

    if (files && files.length !== 0) {
        if (props.watermark) {
            tempPayload = [];
            files.forEach((file:any) => {
                const checkedStatus = findRefreshStatus(file);
                if(checkedStatus !== file.watermarkProcessStatus){
                    watermarkProcessChanged = true;
                }
                const tempPayloadFile = {
                    id: file.id,
                    url: file.url,
                    displayText: file.displayText,
                    active: file.active,
                    primary: file.primary,
                    userOverride: file.userOverride ? file.userOverride : false,
                    watermark: file.watermark ? file.watermark : null,
                    loadingMsg: file.loadingMsg ? file.loadingMsg : null,
                    errorDisplay: file.errorDisplay ? file.errorDisplay : null,
                    watermarkProcessStatus: checkedStatus,
                    order: file.order
                }
                tempPayload.push(tempPayloadFile);
            });
        } else {
            tempPayload = [];
            files.forEach((file:any) => {
                const checkedStatus = findRefreshStatus(file);
                if(checkedStatus !== file.watermarkProcessStatus){
                    watermarkProcessChanged = true;
                }

                const tempPayloadFile = {
                    id: file.id,
                    url: file.url,
                    displayText: file.displayText,
                    active: file.active,
                    primary: file.primary,
                    userOverride: file.userOverride ? file.userOverride : false,
                    loadingMsg: file.loadingMsg ? file.loadingMsg : null,
                    errorDisplay: file.errorDisplay ? file.errorDisplay : null,
                    watermarkProcessStatus: checkedStatus,
                    order: file.order
                }
                tempPayload.push(tempPayloadFile);
            });
        }
    }
    const [payload, setPayload] = useState(tempPayload);

    if(watermarkProcessChanged && props.updatePhotoPayload){
        // if one or more watermark processes has changed, we need to inform the change handler
        props.updatePhotoPayload(tempPayload);
    }

    // trigger to display error warning
    const [showError, setShowError] = useState('');
    // file upload limit size - currently 5mb
    const uploadLimit: number = 28000000;
    // force update equivalent:
    const [, updateState] = useState();
    const forceUpdate = useCallback(() => updateState({}), []);

    const checkAndSetPrimaryImage = (filesArr:GLFile[]) => {
        if(filesArr.length > 0){
            // check to see if we have a primary file
            const primaryFileArr:GLFile[] = filesArr.filter(file => file.primary === true)
            if(primaryFileArr.length === 0){
                // find the candidates that could be a primary file, we'll select the first in line to become the new primary
                const primaryFileCandidates:GLFile[] = filesArr.filter((tempFile:GLFile) => {
                    if(tempFile && tempFile.active && 
                        (!featureFlags.watermarkDetectionFeatureFlag) || (tempFile.watermarkProcessStatus 
                            && problemWatermarkStatuses.indexOf(tempFile.watermarkProcessStatus) === -1)){
                            return tempFile;
                        }
                });
                if(primaryFileCandidates && primaryFileCandidates.length > 0){
                    primaryFileCandidates[0].primary = true;
                }
            }
            filesArr.sort((a: any, b: any) => (a.primary ? a : b));
            setPayload(filesArr);
        }
    }

    const checkRefreshBucket = (file:GLFile):GLFile => {
        if(imageRefreshBucket && imageRefreshBucket.length > 0){
            imageRefreshBucket.forEach((imageRefreshItem:GLFile) => {
                if(file.id && file.id === imageRefreshItem.id){
                    file.watermarkProcessStatus = imageRefreshItem.watermarkProcessStatus;
                }
            });
        }
        return file;
    }

    // Function used for File Dropzone - it checks to make sure it only adds accepted file types
    // This function is additionally called whenever a user clicks on the input and selects a file
    const onDrop = useCallback(acceptedFiles => {
        const fileArray: any = Array();
        const temp:GLFile[] = payload;

        if(singleUpload && acceptedFiles.length > 1){
            acceptedFiles = [acceptedFiles[0]];
            setWarningMessage(singleFileUploadWarning);
        }

        setShowError('');
        for (const file of acceptedFiles) {
            const acceptedFileTypes = accepted.split(",");
            const tempFileName = file.name.substr(file.name.lastIndexOf('.'), file.name.length);
            for (const acceptedFileType of acceptedFileTypes) {
                if (tempFileName.toLowerCase() === acceptedFileType.replace(/\s/g, '').toLowerCase()) {
                    if (file.size <= uploadLimit) {
                        fileArray.push(file)
                        let obj: GLFile;
                        if (props.watermark) {
                            obj = {
                                id: -1,
                                url: '/img/loading.png',
                                // url: "",
                                displayText: file.name,
                                active: false,
                                primary: false,
                                watermark: watermarkCheckbox.current,
                                loadingMsg: "Loading",
                                watermarkProcessStatus: WatermarkProcessStatus.SENDING_FILE_TO_SERVER
                            }
                        } else {
                            obj = {
                                id: -1,
                                url: '/img/loading.png',
                                // url: "",
                                displayText: file.name,
                                active: false,
                                primary: false,
                                loadingMsg: "Loading",
                                watermarkProcessStatus: WatermarkProcessStatus.SENDING_FILE_TO_SERVER
                            }
                        }
                        temp.push(obj);
                    } else {
                        setShowError('File size exceeds ' + Math.round(uploadLimit / 1000000) + 'mb limit')
                    }
                }
            }
        }

        setPayload(temp);

        // sort by smallest file size first
        const checkForWatermark:boolean = featureFlags.watermarkDetectionFeatureFlag;

        fileArray.sort((a: any, b: any) => (a.size - b.size));

        const delayBetweenFiles:number = 2000;  // the time in MS between each sendAndCheck file call (doesn't affect non watermark)
        const additionalDelay:number = 0;    // this is just to make the math easier but still add a slight delay (1/10 second in this case)

        for (let i: number = 0; i < fileArray.length; i++) {
            const fileextention = fileArray[i].name.substr(fileArray[i].name.lastIndexOf('.'), fileArray[i].name.length);
            if (checkForWatermark && (fileextention.toLowerCase() !== ".pdf")) {
                setTimeout(() => sendAndCheckFile(fileArray[i],i), (i*delayBetweenFiles)+additionalDelay);
            }
            else {
                sendFile(fileArray[i], i);
            }
        }

        checkAndSetPrimaryImage(payload);

        rerender();

    }, []);
    const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop, disabled: false }) // dropzone hooks

    // sends files to the server, receives image url, then reconstruct the payload
    async function sendFile(theFile: any, i: number) {
        await postFileNoWatermarkCheck(theFile, (data: any) => {
            const temp: GLFile[] = payload;
            // make sure data returns http, otherwise remove placeholder and show an error

            const fileExtension = theFile.name.substr(theFile.name.lastIndexOf('.'), theFile.name.length);
            const setWatermarkProcessStatus:boolean = fileExtension && fileExtension.toLowerCase() === ".pdf" ? false : true;

            if (data.url && data.url.includes('http')) {
                let obj: GLFile;
                if (props.watermark) {
                    obj = {
                        id: data.id,
                        url: data.url,
                        displayText: theFile.name,
                        active: true,
                        primary: false,
                        watermark: watermarkCheckbox.current
                    }
                    if(setWatermarkProcessStatus){
                        obj.watermarkProcessStatus = data.watermarkProcessStatus;
                    }

                } else {
                    obj = {
                        id: data.id,
                        url: data.url,
                        displayText: theFile.name,
                        active: true,
                        primary: (payload[0].url === '/img/loading.png' && i === 0)
                    }
                    if(setWatermarkProcessStatus){
                        obj.watermarkProcessStatus = data.watermarkProcessStatus;
                    }
                }
                for (let index: number = 0; index < temp.length; index++) {
                    // if (temp[index].url === '/img/loading.png') {
                    if (temp[index].displayText === theFile.name) {
                        temp[index] = obj;
                        break;
                    }
                }
                const modifiedPayload = replaceFileInArray(temp, theFile, obj);
                checkAndSetPrimaryImage(modifiedPayload);
                setPayload(modifiedPayload);
                props.updatePhotoPayload(temp)
                rerender();
            } else {
                let obj: GLFile;
                const errMsg = "Error uploading file.";
                obj = {
                    url: '/img/loading.png',
                    displayText: theFile.name,
                    active: true,
                    primary: false,
                    watermark: null,
                    errorDisplay: errMsg
                }

                const modifiedPayload = replaceFileInArray(temp, theFile, obj)
                setPayload(modifiedPayload);
                setShowError('Error uploading');
                rerender();
            }
        })
    }
    // sends files to the server, receives image url, then reconstruct the payload
    async function sendAndCheckFile(theFile: any, i: number) {

        // console.log("sendAndCheckFile", new Date().toISOString().substr(11, 8));

        let fileUploadFunc = postFileNoWatermarkCheck;
        if(useRestb){   // useRestb needs to be true in order to hit the watermark check function
            fileUploadFunc = postFileWithWatermarkCheck;    
        }

        await fileUploadFunc(theFile, (data: any) => {

            const temp: GLFile[] = payload;
            // make sure data returns http, otherwise remove placeholder and show an error
            if (data.url && data.url.includes('http') && data.watermarkProcessStatus !== WatermarkProcessStatus.SENDING_FILE_TO_SERVER) {
                let obj: GLFile;
                if (props.watermark) {
                    obj = {
                        id: data.id,
                        url: data.url,
                        displayText: theFile.name,
                        active: true,
                        primary: false,
                        watermark: watermarkCheckbox.current,
                        watermarkProcessStatus: data.watermarkProcessStatus
                    }
                } else {
                    obj = {
                        id: data.id,
                        url: data.url,
                        displayText: theFile.name,
                        active: true,
                        primary: false,
                        watermarkProcessStatus: data.watermarkProcessStatus
                    }
                }
                
                const modifiedPayload = replaceFileInArray(temp, theFile, obj);
                checkAndSetPrimaryImage(modifiedPayload);
                setPayload(modifiedPayload);
                props.updatePhotoPayload(temp)
                rerender();
            } else {
                let obj: any;
                let errMsg = "Error uploading file.";
                
                if (data.watermarkCode && data.watermarkCode === 1){                   
                    errMsg = "External Watermark Detected. Please choose another Image.";
                } 
                else if (data.watermarkCode && data.watermarkCode === 2){
                    errMsg = "Unable to upload image file. Watermark Detection Process failed.";
                }
                else if (data.watermarkCode && data.watermarkCode === 3){
                    errMsg = "Unable to upload image file. Watermark Detection Process unable to run.";
                }

                obj = {
                    url: '/img/loading.png',
                    displayText: theFile.name,
                    active: true,
                    primary: false,
                    watermark: null,
                    errorDisplay: errMsg
                }

                const modifiedPayload = replaceFileInArray(temp, theFile, obj)
                setPayload(modifiedPayload);
                rerender();
            }
        })
        
    }

    const replaceFileInArray = (arr:any, file: any, fileData:GLFile) => {
        
        if(arr && arr.length > 0){
            for (let index: number = 0; index < arr.length; index++) {
                const currentFile:GLFile = arr[index];
                if (currentFile.displayText === file.name && currentFile.id === -1) {
                    arr[index] = fileData;
                    break;
                }
                
            }
        }

        return arr;
    }

    const removeFailedProcess = (id: number) => {

        const temp: GLFile[] = payload;
        if(temp && temp.length > 0){

            let toDelete:number | null = null;
            let primaryFile:boolean = false;

            temp.forEach((file:GLFile, index:number) => {

                if(id && file.id === id){
                    toDelete = index;
                    primaryFile = file.primary ? file.primary : false;
                }
            });
 
            if(toDelete !== null && toDelete > -1){
                temp.splice(toDelete,1);
            }

            checkAndSetPrimaryImage(temp);
            setPayload(temp);
            props.updatePhotoPayload(temp);
            rerender();
        }        
    }

    const overrideFailure = (id:number) => {
        if(id && payload && payload.length > 0){
            payload.forEach((file:GLFile) => {
                if(id && file.id === id){
                    file.userOverride = true;
                }
            });
        }
        setPayload(payload);
        props.updatePhotoPayload(payload);
        rerender(); 
    }

    async function retryImage(fileId: string) {
        // Example calling re-try check image
        await checkImage(fileId, (data: any) => {
            const id = data.id;
            let refresh:boolean = false;
            payload.forEach((file:GLFile) => {
                if(file.id === id){
                    file.watermarkProcessStatus = data.watermarkProcessStatus;               
                    refresh = true;     
                }
            });
            if(refresh){
                rerender();
            }
        });
    }

    const retryError = (id:number) => {
        if(id && id > 0){
            retryImage(id.toString());
        }
    }

    // removes the file via the index in the array
    const deleteFile = (index: number) => {
        const temp: GLFile[] = payload;
        const isPrimary: boolean | null = temp[index].primary;
        temp.splice(index, 1);
        checkAndSetPrimaryImage(temp);
        setPayload(temp);
        setWarningMessage(undefined)
        props.updatePhotoPayload(temp);
    }

    // removes info message the index in the array
    const deleteFileMsg = (index: number) => {
        const temp: GLFile[] = payload;
        temp.splice(index, 1)
        
        setPayload(temp);
        rerender()
        // props.updatePhotoPayload(temp)
    }

    // // removes the file via the index in the array and opens option to replace
    // const replaceFile = (index: number) => {
    //     const temp: GLFile[] = payload;
    //     const isPrimary: boolean | null = temp[index].primary;
    //     temp.splice(index, 1)
    //     if (isPrimary && temp[0]) {
    //         temp[0].primary = true
    //     }
    //     setPayload(temp);
    //     props.updatePhotoPayload(temp);
    // }

    // when popover is triggered, update the index of popover's location - used to only have one open at a time
    const updatePopover = (index: number) => {
        if (popover !== index) {
            setPopover(index)
        } else {
            setPopover(-1)
        }
    }

    // sets the primary image by saving the index of the file within the array to the state primary
    const makePrimary = (index: number) => {
        const temp: GLFile[] = payload;
        for (const uno of temp) {
            uno.primary = false;
        }
        temp[index].primary = true;
        const sortedFiles = sortGLFiles(temp);
        setPayload(sortedFiles);
        props.updatePhotoPayload(sortedFiles);
        rerender();
    }

    // pushes the index of the file into this array which keeps track of inactive files
    const markInactive = (index: number) => {
        const temp: GLFile[] = payload;
        const isPrimary: boolean | null = temp[index].primary;
        const isActive: boolean | null = temp[index].active;
        temp[index].active = !(temp[index].active)        
        checkAndSetPrimaryImage(temp);
        setPayload(temp)
        props.updatePhotoPayload(temp)
    }

    // update the index inside the nameArray with the name the user updated in thumbnail
    const setFileName = (fileName: string, extension: string, index: number) => {
        if (payload[index]) {
            const temp: GLFile[] = payload;
            temp[index].displayText = fileName + extension;
            setPayload(temp)
            props.updatePhotoPayload(temp)
        }
    }

    // watermark individual thumbnail
    const applyWaterMark = (index: number) => {
        const temp: GLFile[] = payload;
        temp[index].watermark = temp[index].watermark ? false : true;
        setPayload(temp);
        props.updatePhotoPayload(temp);
        rerender();
    }

    // turn on/off auto-watermark on upload
    const waterMarkAllImages = () => {
        watermarkCheckbox.current = !watermarkCheckbox.current;
        forceUpdate();
    }

    // function used to open the image shadowbox
    const openShadowBox = (url: string) => {
        setPhotoPreviewURL(url);
        setIsOpen(true)
        setTimeout(() => { getImageData(); }, 150)
    }

    // function used to open the pdf preview
    const openPDFPreview = (url:string) => {
        document.body.style.overflow = "hidden";
        document.body.style.marginRight = "18px";
        setPdfPreviewURL(url);
    }

    const closePDFPreview = () => {
        document.body.style.overflow = "auto";
        document.body.style.marginRight = "0";
        setPdfPreviewURL("");   // closes out on re-render
    }

    // hit watermark, used during gallery forward/prev functionality
    const hideWatermark = () => {
        const cbreWatermark = document.getElementById('theWatermark');
        if (cbreWatermark) {
            cbreWatermark.style.height = '0px';
        }
    }

    // make sure image element exists before rendering watermark on top of it
    async function getImageData() {
        let imagesrc: HTMLImageElement | any;
        let elem: HTMLImageElement | any;
        const checkExist = setInterval(() => {
            imagesrc = document.getElementsByClassName("ril-image-current ril__image");
            elem = document.getElementsByClassName('ril__inner');
            if (imagesrc[0] && imagesrc[0].src && imagesrc[0].complete && imagesrc[0].clientHeight !== 0 && elem[0]) { // eslint-disable-next-line
                clearInterval(checkExist);
                applyWatermark(imagesrc, elem)
            }
        }, 100);
    }

    // add height to the watermark, displaying it on the gallery
    const applyWatermark = (imagesrc: any, elem: any) => {
        if (imagesrc[0] && elem[0]) {
            const currentImage = imagesrc[0].src;
            for (const file of payload) {
                if (currentImage === file.url && file.watermark) {
                    // let height = imagesrc[0].clientHeight;
                    // // if image height is larger than viewport, use viewport height minus padding
                    // if (height > window.innerHeight) {
                    //     height = window.innerHeight - 20;
                    // }
                    const cbreWatermark = document.getElementById('theWatermark');
                    if (!cbreWatermark) {
                        const img = document.createElement("img");
                        img.src = CBRELogo;
                        img.id = 'theWatermark';
                        // img.style.height = (height * 0.05) + 'px';
                        img.style.height = '15px';
                        elem[0].appendChild(img)
                    } else {
                        cbreWatermark.style.height = '15px';
                    }
                }
            }
        }
    }

    // force update equivalent
    const rerender = () => {
        setTimeout(() => { forceUpdate() }, 1)
    }

    // function used to open the shadowbox

    const getPopOverOptions = (itemActive: boolean) => {
        const options:PopOverItem[] = [];
        
        // delete is included in all current scenarios
        options.push({icon: DeleteIcon, name: 'Delete', clickHandler: deleteFile});
        // if the files accepted props includes images, (or image is active) include the primary object, otherwise omit it
        if (props.showPrimary && itemActive && props.watermark) {
            options.push({icon: StarIcon, name: 'Primary', clickHandler: makePrimary});
            options.push({icon: InactiveIcon, name: 'Inactive', clickHandler: markInactive});
            options.push({icon: WatermarkIcon, name: 'Watermark', clickHandler: applyWaterMark});
        }else if(props.showPrimary && itemActive){
            options.push({icon: StarIcon, name: 'Primary', clickHandler: makePrimary});
            options.push({icon: InactiveIcon, name: 'Inactive', clickHandler: markInactive});
        }else if(!props.showPrimary && props.watermark){   
            options.push({icon: WatermarkIcon, name: 'Watermark', clickHandler: applyWaterMark});
        }else{
            options.push({icon: InactiveIcon, name: 'Inactive', clickHandler: markInactive});
        }
        
        return options;
    }

    // render methods
    const displayProcessedFiles = () => {
        if(payload && payload[0] && payload[0].url !== ''){
            return <>
                {payload.map((payloadObj:any, index:number) => {
                    // check the refresh images to make sure nothing here matches up
                    const payloadFile:any = checkRefreshBucket(payloadObj);
                    if(!checkFileForWatermarkProblems(payloadFile,featureFlags,allowWatermarkingDetect)){
                        return (
                            <Col key={index + payloadFile.displayText}>
                                <div className={payloadFile.active ? 'thumb' : 'thumb inactive'}>
                                    {payload[index].errorDisplay && <DeleteMsg  key={index + 'deleteMsg'} onClick={()=>deleteFileMsg(index)} >X</DeleteMsg>}
                                    <Thumbnail file={payloadFile.url} showNameEdit={true} size={ThumbnailSize.NORMAL} loadingMsg={payloadFile.loadingMsg} errorMsg={payloadFile.errorDisplay} hideStar={(!props.showPrimary) ? true : false} openShadowBox={openShadowBox} 
                                        openPDFPreview={openPDFPreview} name={payloadFile.displayText} key={index + payloadFile.displayText} setFileName={setFileName} index={index} watermark={payloadFile.watermark} primary={(payloadFile.primary) ? true : false} />
                                    {!(payload[index].loadingMsg || payload[index].errorDisplay) && <PopOver index={index} handlePopUpAction={true} key={index + 'popover'} options={getPopOverOptions(payloadFile.active)} inactive={payloadFile.active} popoverEnabled={(popover === index) ? true : false} updatePopover={updatePopover} location={{top: 2, left: 5}} />} {/* tslint:disable-line */}
                                </div>
                            </Col>
                        )
                    }else{
                        return <></>;
                    }
                })}
            </>;
        }
        return <></>;
    }

    const getThumbnailOverlay = (file:GLFile):ThumbnailOverlay => {

        if(!file || file.watermarkProcessStatus === undefined || file.watermarkProcessStatus === null 
            || (!featureFlags.watermarkDetectionFeatureFlag)){
            return { show: false, message: "", icon: "", color: "", allowRemove: false, allowOverride: false, allowRetry: false, retryLabel: ""};
        }

        let overlayMessage = "";
        let overlayIcon = null;
        let overlayColor = "black";
        let overlayAllowRemove = false;
        let overlayAllowOverride = false;
        let overlayRetry = false;
        let overlayRetryLabel = "Retry";
        
        if(file.watermarkProcessStatus === WatermarkProcessStatus.WATERMARK){
            overlayMessage = "Watermark detected.";
            overlayColor = "red";
            overlayIcon = ErrorIcon;
            overlayAllowRemove = true;
            overlayAllowOverride = true;
        }else if(file.watermarkProcessStatus === WatermarkProcessStatus.CRE_WATERMARK){
            overlayMessage = "Watermark detected. \n Approval Required.";
            overlayColor = "red";
            overlayIcon = ErrorIcon;
            overlayAllowRemove = true;
            overlayAllowOverride = user && user.isAdmin ? true : false;    // only allow override if the user is an admin
        }else if(file.watermarkProcessStatus === WatermarkProcessStatus.WATERMARK_ERROR){
            overlayMessage = "Unable to process image \n(Code: WME)";
            overlayColor = "red";
            overlayIcon = ErrorIcon;
            overlayAllowRemove = true;
            overlayRetry = true;
            overlayRetryLabel = "Retry";
            overlayAllowOverride = user && user.isAdmin ? true : false;    // allow override if the user is an admin
        }else if (file.watermarkProcessStatus === WatermarkProcessStatus.NOT_PROCESSED){
            overlayMessage = "Unable to process image \n(Code: NOP)";
            overlayColor = "red";
            overlayIcon = ErrorIcon;
            overlayAllowRemove = true;
            overlayRetry = true;
            overlayRetryLabel = "Retry";
            overlayAllowOverride = user && user.isAdmin ? true : false;    // allow override if the user is an admin
        }else if (file.watermarkProcessStatus === WatermarkProcessStatus.PROCESSING){
            overlayMessage = "Unable to process image \n(Code: PRO)";
            overlayColor = "red";
            overlayIcon = ErrorIcon;
            overlayAllowRemove = true;
            overlayRetry = true;
            overlayRetryLabel = "Retry";
            overlayAllowOverride = user && user.isAdmin ? true : false;    // allow override if the user is an admin
        }else if (file.watermarkProcessStatus === WatermarkProcessStatus.READY_TO_PROCESS){
            overlayMessage = "Unable to process image \n(Code: RTP)";
            overlayColor = "red";
            overlayIcon = ErrorIcon;
            overlayAllowRemove = true;
            overlayRetry = true;
            overlayRetryLabel = "Retry";
            overlayAllowOverride = user && user.isAdmin ? true : false;    // allow override if the user is an admin
        }

        // using the file, set a message and icon for the overlay  
        const overlay: ThumbnailOverlay = {
            show: true,
            message: overlayMessage,
            icon: overlayIcon,
            color: overlayColor,
            allowRemove: overlayAllowRemove,
            allowOverride: overlayAllowOverride,
            allowRetry: overlayRetry,
            retryLabel: overlayRetryLabel
        }

        return overlay;
    }

    const createRandomKey = ():string => {
        return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
    }

    const displayWatermarkIssues = () => {
        if(!payload || payload.length === 0){
            return <></>;
        }

        // create a list of the pending files so we know if we should display the section at all
        const problemFiles:GLFile[] = [];
        payload.forEach((payloadObj:GLFile) => {
            const payloadFile:GLFile = checkRefreshBucket(payloadObj);
            if(checkFileForWatermarkProblems(payloadFile,featureFlags,allowWatermarkingDetect)){
                problemFiles.push(payloadFile);
            }
        });

        if(problemFiles && problemFiles.length > 0){
            return ( 
                <ProblemWatermarkFiles>
                    <ProblemWatermarkFilesHeader>We’ve detected a watermark in the uploaded images. Please select override if you have permission to use this photo, remove the photo from the listing, or reach out to the Global Listings team for help.</ProblemWatermarkFilesHeader>
                    <Grid>
                        <Row>
                            {problemFiles.map((payloadFile:GLFile, index:number) => {
                                const overlay:ThumbnailOverlay = getThumbnailOverlay(payloadFile);
                                return (
                                    <Col key={"wmissue-" + createRandomKey()}>
                                        <div className={'small-thumb'}>
                                            <Thumbnail id={payloadFile.id} file={payloadFile.url} showNameEdit={false} size={ThumbnailSize.SMALL} 
                                                overlay={overlay} removeFailedProcess={removeFailedProcess} 
                                                overrideFailure={overrideFailure} retryError={retryError} openShadowBox={openShadowBox} loadingMsg={null} errorMsg={null} hideStar={true} 
                                                name={payloadFile.displayText}  index={index} watermark={payloadFile.watermark} primary={(payloadFile.primary) ? true : false} />                                                    
                                        </div>
                                    </Col>
                                );
                            })}
                        </Row>
                    </Grid>
                </ProblemWatermarkFiles>
            );
        }
        return <></>;
    }
    
    let inputClicked: boolean = false;

    return (
        <FormUploadContainer className={(props.showPrimary) ? 'jestTest isPrimary' : 'jestTest'}>
            <StyledHeader {...props} className={props.watermark ? 'formUploadHeader' : ''}><StyledTitle {...props}>{title}</StyledTitle> {description} {props.watermark && <FormCheckbox name="watermark" label="Watermark on upload" defaultValue={watermarkCheckbox.current} value={watermarkCheckbox.current} changeHandler={waterMarkAllImages} />}</StyledHeader>
            {manualError && <StyledError>{manualErrorMessage}</StyledError>}
            {!manualError && warningMessage && <StyledWarning>{warningMessage}</StyledWarning>}
            <BaseInputContainer>
                <div style={{ background: isDragActive ? '#f6f6f6' : 'inherit' }} className="file-drop">
                    <div className="file-drop-target" {...getRootProps({
                        onClick: (event: any) => !inputClicked ? event.stopPropagation() : ''
                    })}>
                        <Grid>
                            <Row>
                                { displayProcessedFiles() }
                                <Col>
                                    {!(payload && payload[0] && singleUpload) &&
                                        <FileUploadButton className="file-upload-button" onClick={() => inputClicked = true}>
                                            <span className="plus" style={{ fontSize: "20px", "marginTop": "25px" }}>+</span>
                                            <FormFieldLabel label={label} error={manualError} styles={{fontSize: "14px"}} />
                                            <BaseFormInput {...getInputProps()} name={name} type="file" id={name} accept={accepted} className="form-control" />
                                        </FileUploadButton>
                                    }
                                    {showError !== '' && <p style={{ color: 'red', fontSize: '12px', textIndent: '4px', maxWidth: '200px' }}>
                                        {(showError.includes('Error uploading')) ? <span>Error uploading file. Please click <a onClick={() => window.location.replace(window.location.origin)} style={{ color: '#DF7909', cursor: 'pointer' }}>here</a> to reload application.</span>
                                            : showError}</p>}
                                </Col>
                                {payload && payload.length === 0 &&
                                    <p id="dropIndicator">Drag & Drop files here</p>
                                }
                                {/* {JSON.stringify(payload)} */}
                            </Row>
                        </Grid>
                    </div>
                </div>
                { displayWatermarkIssues() }
            </BaseInputContainer>
            {isOpen && (
                <Lightbox
                    mainSrc={photoPreviewURL}
                    onCloseRequest={() => setIsOpen(false)}
                />
            )}
            { pdfPreviewURL && pdfPreviewURL.length > 0 && <PDFPreview url={pdfPreviewURL} closeHandler={closePDFPreview}/>}
        </FormUploadContainer>
    )
}

const ProblemWatermarkFiles = styled.div`
    padding: 5px 10px 10px 10px;
    border: 1px solid #666666;
    margin: 10px 0 0 0;
    background: #F6F6F6;
    width: 100%;
`;

const ProblemWatermarkFilesHeader = styled.div`
    text-align: center;
    color: ${props => (props.theme.colors ? props.theme.colors.secondaryAccent : '#00B2DD')};
    margin-bottom: 10px;
`;

const FormUploadContainer = styled.div`
    > h5 {
        height: 15px;
        width: 601px;   
        color: 'red';
        font-family: "futura PT Book italic";
        font-size: 12px;
        font-weight:300;
        font-family: ${props => props.theme.font ? props.theme.font : 'italic'};    
        line-height: 15px;
        margin-bottom:15px;
    }
    > h5.formUploadHeader {
        width: auto;
        display: flex;
        align-items: center;
        justify-content: left;
        margin: 30px 0;
        > div {
            width: auto;
            display: block;
            margin-top: 0;
            margin-left: 30px;
            > h5 {
                margin-top:0;
            }
        }
    }
    .plus {
        text-align:center;
        font-size:60px;
         color: ${props => (props.theme.colors ? props.theme.colors.secondaryAccent : '#00B2DD')};
         width:200px;
         position:absolute;
         top:25px;
    }
    .file-drop { width:100%; outline:0;border:0;}
    input:focus {outline:0;}
    .file-drop-target {
        display:flex;
        justify-content:flex-end;
        :focus {
            outline-color: ${props => props.theme.colors ? props.theme.colors.inputFocus : '#00B2DD'};  
        }
    }
    .file-drop-target > div {padding:0; width:100% !important;}

    #dropIndicator {
        line-height:117px;
        text-align:center;
        width: calc(100% - 310px);
        font-weight: ${props => (props.theme.font ? props.theme.font.weight.normal : 'normal')};
        font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
        color:#999;
    } 
`;

const BaseInputContainer = styled.div`
    width:100%;
    font-weight: ${props => (props.theme.font ? props.theme.font.weight.normal : 'normal')};
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
    .inactive {
        > div {
            .img-thumbnail { filter:grayscale(1); opacity:.3;}
        }
    }
    .thumb {
        position:relative;
        max-width:200px;
        width:200px;
    }
    .small-thumb{
        position:relative;
        max-width:125px;
        > div {
            .img-thumbnail { filter:grayscale(1); opacity:.3;}
        }
    }
    .file-drop-target-columns .thumb {
        max-width:none;
    }
`;

const BaseFormInput = styled.input`
    width:100%;
`;

const FileUploadButton = styled.div`
    position:relative;
    width:100%;
    cursor:pointer;
    text-align:center;
    height:150px;
    outline:0;
    display:flex;
    > h5  {
        font-size:20px;
        color: ${props => (props.theme.colors ? props.theme.colors.secondaryAccent : '#00B2DD')};
        margin:0;
        padding-top:75px;
        width:200px;
        border: 1px rgba(0,0,0,0.15) dashed;
        background:#F6F6F6;
    }
    > input {
        position: absolute;
        left: 0;
        top: 0;
        opacity: 0;
        cursor:pointer;
        z-index:1;
        min-height:150px;
    }
`;

const StyledHeader = styled.h5`
  color: ${(headerProps: FormUploadProps) => headerProps.manualError ? css` 
  ${props => (props.theme.colors ? props.theme.colors.error : 'red')}; 
  ` : '#8E9A9D'};
  font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
  letter-spacing: .03rem;
  margin-block-end: 0.5em;
`;

const StyledTitle = styled.span`
    height: 21px;
    width: auto;
    color: ${(headerProps: FormUploadProps) => headerProps.manualError ? css` 
        ${props => (props.theme.colors ? props.theme.colors.error : 'red')}; 
        ` : '#8E9A9D'};
    font-family: 'Futura Md BT',helvetica,arial,sans-serif;
    font-size: 16px;
    font-family: ${props => props.theme.font ? props.theme.font : 'helvetica'};     
    line-height: 21px;
    margin-right:10px;
`;

const StyledError = styled.div`
    width: 100%;
    color: ${props => props.theme.colors ? props.theme.colors.error : 'red'};
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
    margin-bottom: 10px;
`;

const StyledWarning = styled.div`
    width: 100%;
    color: ${props => props.theme.colors ? props.theme.colors.error : 'red'};
    font-family: ${props => (props.theme.font ? props.theme.font.primary : 'inherit')}; 
    margin-bottom: 10px;
`;

const DeleteMsg = styled.div`
    cursor: pointer;
    margin-top: 10px;
    margin-right: 10px;
    font-size: 16px;
    color: #02c2f2;
    float: right;
`

export default React.memo(FormUpload, (prevProps, nextProps) => nextProps.files !== prevProps.files);