import React, { FC, useCallback, useState } from 'react';
import styled from 'styled-components';
import { useDropzone } from 'react-dropzone';
import Loader from '../loader/loader';

export enum ImageAvatarSize {
    SMALL = '40px',
    MEDIUM = '65px',
    LARGE = '125px'
}

export enum PlaceholderSize {
    SMALL = '12px',
    MEDIUM = '20px',
    LARGE = '30px'
}

export interface ImageAvatarProps {
    imageURL?: string,
    placeholder?: string,
    allowUpload?: boolean,
    uploadPhotoLabel?: string,
    avatarSize: ImageAvatarSize,
    placeholderSize: PlaceholderSize,
    uploadErrors?:string,
    uploadFile?: (file: any) => void,
    clearErrors?: () => void
}

interface AvatarSizeProps {
    avatarSize: string
}

interface PlaceholderSizeProps {
    placeholderSize: string,
    allowUpload?: boolean
}

const ImageAvatar: FC<ImageAvatarProps> = (props) => {

    const { imageURL, placeholder, allowUpload, uploadPhotoLabel, uploadFile, avatarSize, 
                placeholderSize, uploadErrors, clearErrors } = props;
    const [ pending, setPending ] = useState<boolean>(false);
    const [ errors, setErrors ] = useState<string>(uploadErrors ? uploadErrors : "");
    const accepted:string = ".png,.jpg,.jpeg";
    const acceptedFileTypes:string[] = [".png",".jpg",".jpeg"];
    // file upload limit size - currently 5mb
    const uploadLimit: number = 5000000;

    // handles file selection
    const onDrop = useCallback((acceptedFiles:any[]) => {
        // if more than one accepted files, only take the first one (not allowing multiples for the avatar)
        let file:any;
        if(acceptedFiles.length > 0){
            file = acceptedFiles[0];
        }
        // check the file and make sure it's acceptable
        const fileExtension:string = file.name.substr(file.name.lastIndexOf('.'), file.name.length).toLowerCase();

        if(file.size > uploadLimit){
            setErrors("File size is too large.");
        }
        else if(acceptedFileTypes.indexOf(fileExtension) === -1){
            setErrors("Invalid file type (must be " + accepted + ")");
        }
        else if(file && uploadFile){
            // perform the upload using the callback sent to the component
            setPending(true);
            uploadFile(file);
        }
    }, []);

    const tryAgain = (e:React.MouseEvent<HTMLAnchorElement>) => {
        e.preventDefault();
        if(clearErrors){
            clearErrors();
        }
    }

    const { getRootProps, getInputProps } = useDropzone({ onDrop, disabled: false }) // dropzone hooks
    let inputClicked: boolean = false;

    const showUploadErrors:boolean = errors && errors.length > 0 ? true : false;
    const showPending:boolean = !showUploadErrors && pending ? true : false;
    const showPlaceholder:boolean = !showUploadErrors && !showPending && !allowUpload ? true : false;
    const showAvatar:boolean = !showUploadErrors && !showPending && !showPlaceholder && allowUpload ? true : false;

    return (
        <BaseContainer>

            {showUploadErrors && 
                <>
                    <Circle avatarSize={avatarSize.valueOf()}>
                        <ErrorIndicator placeholderSize={placeholderSize.valueOf()} data-test="ia-error-indicator">!</ErrorIndicator>
                    </Circle>
                    <ErrorText data-test="ia-errors">{uploadErrors}</ErrorText>
                    <UploadLink><a href="#" onClick={tryAgain}>Try Again</a></UploadLink>
                </>
            }

            { showPending && 
                <Circle avatarSize={avatarSize.valueOf()}>
                    <LoaderContainer>
                        <Loader loaderType="ScaleLoader" size={60}/>
                    </LoaderContainer>
                </Circle>   
            }
        
            { showPlaceholder &&
                <Circle avatarSize={avatarSize.valueOf()}>
                    {(!imageURL || imageURL.length === 0) && placeholder && <Placeholder placeholderSize={placeholderSize.valueOf()} allowUpload={allowUpload} data-test="ia-placeholder">{placeholder}</Placeholder>}
                    {imageURL && imageURL.length > 0 && <Avatar avatarSize={avatarSize.valueOf()} src={imageURL} data-test="ia-avatar"/>}
                </Circle>
            }

            { showAvatar && 
                <div className="file-drop-target" {...getRootProps({
                    onClick: (event: any) => !inputClicked ? event.stopPropagation() : ''
                })}>
                    <Circle avatarSize={avatarSize.valueOf()}>
                        <FileUploadButton className="file-upload-button" onClick={() => inputClicked = true}>
                            {(!imageURL || imageURL.length === 0) && placeholder && <Placeholder placeholderSize={placeholderSize.valueOf()} allowUpload={allowUpload} data-test="ia-placeholder">{placeholder}</Placeholder>}
                            {imageURL && imageURL.length > 0 && <Avatar avatarSize={avatarSize.valueOf()} src={imageURL} data-test="ia-avatar"/> }
                            <BaseFormInput {...getInputProps()} name={name} type="file" id={name} accept={accepted} className="form-control"/>
                        </FileUploadButton>
                    </Circle>
                    <UploadLink>{uploadPhotoLabel ? uploadPhotoLabel : 'Add Photo'}</UploadLink>
                    <RecommendedSize>Recommended square headshot at 125 x 125 (px). JPEG only</RecommendedSize>
                </div>
                
            }
        </BaseContainer>
    );
};

const BaseContainer = styled.div`
    .file-drop { outline:0; border:0; } 
    input:focus { outline:0; }
    .file-drop-target {
        :focus {
            outline: 0;
            border: 0;  
        }
    }
`;

const LoaderContainer = styled.div`
    padding-top: 15px;
`;

const ErrorIndicator = styled.div`
    color: ${props => (props.theme.color ? props.theme.color.errorred : "darkred")};
    font-size: ${(props: PlaceholderSizeProps) => props.placeholderSize ? props.placeholderSize : '65px'}; 
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};
`;

const ErrorText = styled.div`
    color: ${props => (props.theme.color ? props.theme.color.errorred : "darkred")};
    font-size: 12px;
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};
`;

const Placeholder = styled.div`
    cursor: ${(props: PlaceholderSizeProps) => props.allowUpload ? 'pointer' : 'normal'}; ;
    width: 100%;
    text-align: center;
    font-size: ${(props: PlaceholderSizeProps) => props.placeholderSize ? props.placeholderSize : '65px'}; 
    font-family: ${props => props.theme.font ? props.theme.font.bold : 'helvetica'};
`;

const RecommendedSize = styled.div`
    font-size: 12px;
    font-family: "futura PT Book italic";
    color: ${props => (props.theme.color ? props.theme.color.notice : "#b0b0b0")};
    margin-top: 5px;
`;

const UploadLink = styled.div`
    margin-top: 5px;
    color: rgb(0, 166, 87);
    cursor: pointer;
    font-size: 14px;
    text-decoration: underline;
    font-family: ${props => (props.theme.font ? props.theme.font.bold : "Futura Md BT Bold', helvetica, arial, sans-serif")};
    color: ${props => (props.theme.colors ? props.theme.colors.primaryAccent : 'black')};
    a:visited, a:active, a:hover {
        color: ${props => (props.theme.colors ? props.theme.colors.primaryAccent : 'black')};
    }
`;

const Circle = styled.div`
    height: ${(props: AvatarSizeProps) => props.avatarSize ? props.avatarSize : '65px'}; 
    width: ${(props: AvatarSizeProps) => props.avatarSize ? props.avatarSize : '65px'};
    background:#EBEBEB;
    border-radius:50%;
    line-height: ${(props: AvatarSizeProps) => props.avatarSize ? props.avatarSize : '65px'};
    color:#fff;
    text-align:center;
    margin: ${(props: AvatarSizeProps) => props.avatarSize && props.avatarSize === ImageAvatarSize.LARGE ? '45px auto 0 auto' : '0 auto'};
    letter-spacing:-2px;
`;

const Avatar = styled.img`
    display: block;
    height: ${(props: AvatarSizeProps) => props.avatarSize ? props.avatarSize : '65px'};
    width: ${(props: AvatarSizeProps) => props.avatarSize ? props.avatarSize : '65px'};
    border-radius:50%;
    line-height: ${(props: AvatarSizeProps) => props.avatarSize ? props.avatarSize : '65px'};
    color:#fff;
    text-align:center;
    letter-spacing:-2px;
`;

const FileUploadButton = styled.div`
    position:relative;
    width:100%;
    outline: 0;
    cursor:pointer;
    text-align:center;
    height:150px;
    display:flex;
    > h5  {
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
        cursor:pointer;
        z-index:1;
        min-height:150px;
    }
`;

const BaseFormInput = styled.input`
    width:100%;
`;

export default ImageAvatar;