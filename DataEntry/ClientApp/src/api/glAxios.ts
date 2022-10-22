import Axios from 'axios';
import { adalApiFetch } from '../adalConfig';

export const axiosGet = (url: string) => {
  return adalApiFetch(Axios.get, url)
    .then(response => {
      return response.data
    })
    .catch((error) => {
      return error.message
    })
}

export const axiosPost = (url: string, gqlData: {query:string, variables?:any} ) => {

  return adalApiFetch(Axios, url, {
    method: "post",
    headers: { "Content-Type": "application/json", Accepted: "application/json" },
    data: gqlData
  })
  .then(res => {
    return res.data
  })
  .catch((error) => {
    console.log(error);
    return error.message
  })
}

export const axiosDownloadReport = (url: string) => {

  return adalApiFetch(Axios, url, {
    method: "get",
    headers: { 'Content-Type': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'Content-Disposition': "attachment; filename=DEReport.xlsx"},
    responseType:'arraybuffer'
  })
  .then(res => {
    return res.data
  })
  .catch((error) => {
    console.log(error);
    return error.message
  })
}


export const axiosGetParam = (url: string, paramName: string, paramValue: string, callback: (data: string) => void) => {

  const urlparams = url + "?" + paramName + "=" + paramValue;

  // return adalApiFetch(Axios.get, urlparams, {
  //   headers: {"Access-Control-Allow-Origin":"*"}
  // })

  return adalApiFetch(Axios, urlparams, {
    method: "get",
    headers: { "Content-Type": "application/json", Accepted: "application/json", "Access-Control-Allow-Origin":"*"}
  })
  .then(response => {

    if (response.status === 200) {
      callback(response.data);
    } else {
      callback('error');
    }
  })
  .catch((error) => {
    console.log(error);
    return error.message
  })
}

export const fetchPostFile = (url: string, data: any, callback: (data: string) => void) => {
  const formData = new FormData();
  formData.append('file', data);

  return adalApiFetch(fetch, url, {
    method: "post",
    body: formData,
    headers: {
      Accept: 'application/json',
    },
  })
    .then(response => {
 
      if (response.ok) {
        response.json().then((json: any) => {
          callback(json)
        });
      } else {
        callback('error');
      }
    })
    .catch((error) => {
      callback('error')
      console.log(error);
    })
}

export async function getData(url: string) {
  return await axiosGet(url)
}

export async function postData(gqlData: {query:string, variables?:any}) {
  return await axiosPost('/graphql/', gqlData)
}

export async function postFileNoWatermarkCheck(body: any, callback: any) {
  return await fetchPostFile('/api/FileUpload/UploadImageNoChecks', body, callback);
}

// NOTE: this is the API endpoint that hits RESTB, so only use it when we are ready to incur API hits.
export async function postFileWithWatermarkCheck(body: any, callback: any) {
  return await fetchPostFile('/api/FileUpload/UploadImage', body, callback)
}

// this call is for our "retry" function when there is an error on an image check
export async function checkImage(imageId: string, callback: any) {
  return await axiosGetParam('/api/FileUpload/CheckImage', 'id', imageId, callback)
}

// this call is for admin watermark processing to check process status
export async function watermarkDetectionProcessStatus() {
  return await axiosGet('/api/WatermarkDetect/CheckImageProcessStatus')
}

// this call is for admin watermark processing to start/stop process - run needs to be a bool string ("true" or "false")
export async function watermarkDetectionProcessSetStatus(runcode: string, callback: any) {
  return await axiosGetParam('/api/WatermarkDetect/SetImageProcessStatus', 'runcode', runcode, callback);
}

export async function postBulkUploadFile(body: File, callback: any) {
  return await fetchPostFile('/api/BulkUpload', body, callback);
}