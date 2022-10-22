import { adalApiFetch } from '../../adalConfig'
import Axios from 'axios';

export const uploadFile = (file:any, uploadURL:string):Promise<any> => {

    const formData = new FormData();
    formData.append('file', file);

    return new Promise((resolve, reject) => {
        adalApiFetch(fetch, uploadURL, {
            method: "post",
            body: formData,
            headers: {
              Accept: 'application/json',
            },
        }).then(response => {
            if(response.ok && response.json) {
                resolve(response.json());
            }
        }).catch(error => {
            reject("upload error");
        });
    });
}