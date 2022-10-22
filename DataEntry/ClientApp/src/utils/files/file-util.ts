import { postFileNoWatermarkCheck, postFileWithWatermarkCheck } from "../../api/glAxios";
import { GLFile } from "../../types/listing/file";

export async function uploadFile(file:File, checkForWatermarks:boolean):Promise<GLFile | undefined>{
    const fileUploadFunc = checkForWatermarks ? postFileWithWatermarkCheck : postFileNoWatermarkCheck;
    return new Promise((resolve, reject) =>{
        try {
            fileUploadFunc(file, (data: any) => {
                if(!data){
                    reject("Error Uploading");
                }else{ 
                    resolve(data);
                }
            }); 
        } catch (err) {
            reject(err);                
        }
    });
}

export const convertBase64ToFile = (base64string:string, filename:string, fileExtension?:string):File | undefined => {
    const arr = base64string.split(',');
    // ensure file extension check has a prepended period
    if(fileExtension && fileExtension.indexOf(".") === -1){
        fileExtension = "." + fileExtension;
    }
    // ensure the filename has the extension or it could fail upon upload
    if(fileExtension && filename.indexOf(fileExtension) === -1){
        filename += fileExtension;
    }
    if(arr && arr.length > 0 && arr[0] !== null){
        const matches = arr[0].match(/:(.*?);/);
        if(matches && matches[1]){
            const mime = matches[1];
            const bstr = atob(arr[1]);
            let n = bstr.length;
            const u8arr = new Uint8Array(n);
            while(n--){
                u8arr[n] = bstr.charCodeAt(n);
            }
            return new File([u8arr], filename, {type:mime});
        }
    }
    return undefined;
}

