using System;
using System.IO;
using System.Threading.Tasks;
using dataentry.Data;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Repository;
using dataentry.Services.Business.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using dataentry.Services.Background.WatermarkDetection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using dataentry.ViewModels.GraphQL;
using System.Linq;


namespace dataentry.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private class ImageWatermarkResult
        {
            public string FileUrl { get; set; }
            public WatermarkDetectResult WatermarkCode { get; set; }
        }


        private readonly IAzureStorageRepository AzureStorage;
        private readonly IConfiguration Configuration;
        private readonly ILogger<FileUploadController> _logger;

        private readonly System.Net.Http.IHttpClientFactory _clientFactory;
        private readonly IImageService _imageService;

        // private readonly IWatermarkDetectionService _watermarkDetectionService;


        public FileUploadController(IAzureStorageRepository AzureStorageRepository, IConfiguration configuration
        , ILogger<FileUploadController> logger, System.Net.Http.IHttpClientFactory clientFactory, IImageService imageservice) //, IWatermarkDetectionService watermarkDetectionService
        {
            AzureStorage = AzureStorageRepository;
            Configuration = configuration;
            _logger = logger;
            _clientFactory = clientFactory;
            _imageService = imageservice;
        }

        [HttpPost]
        [RequestSizeLimit(30000000)] //default size limit of 28.6 Mb
        public async Task<IActionResult> Image(IFormFile file)
        {
            string ValidationError = file.ValidateImage(Configuration["ImageFileFormats"]);

            if (string.IsNullOrEmpty(ValidationError))
            {
                var extension = Path.GetExtension(file.FileName);
                var FileName = string.Format("{0}{1}", Guid.NewGuid(), extension);
                using (MemoryStream filestream = new MemoryStream())
                {
                    await file.CopyToAsync(filestream);
                    string FileResizedUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.originalimages.ToString(), filestream);
                    var finalStream = filestream.ImageResizeHandler(extension);
                    string FileUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.photos.ToString(), finalStream);                   
                    if (!string.IsNullOrWhiteSpace(FileUrl))
                    {
                        var image = await _imageService.AddImage(FileUrl, (int)WatermarkDetectResult.Not_Processed, User);
                        return Ok(image);
                    }
                }
            }

            return BadRequest(ValidationError);
        }


        [HttpPost]
        [RequestSizeLimit(30000000)] //default size limit of 28.6 Mb
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            string ValidationError = file.ValidateImage(Configuration["ImageFileFormats"]);


            if (string.IsNullOrEmpty(ValidationError))
            {

                string FileUrl = string.Empty;
                var extension = Path.GetExtension(file.FileName);
                var FileName = string.Format("{0}{1}", Guid.NewGuid(), extension);
                var image = new ImageDetectionViewModel()
                {
                    Id = 0,
                    Url = string.Empty,
                    WatermarkProcessStatus = 0,
                    UpdatedAt = DateTime.Now
                };

                using (MemoryStream filestream = new MemoryStream())
                {

                    _logger.LogInformation("Watermark Detection - initial file save: " + FileName);
                    await file.CopyToAsync(filestream);
                    string FileResizedUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.originalimages.ToString(), filestream);
                    var finalStream = filestream.ImageResizeHandler(extension);
                    FileUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.photos.ToString(), finalStream);
                    image = await _imageService.AddImage(FileUrl, (int)WatermarkDetectResult.Ready_To_Process, User);
                }

                Boolean checkProcessingImgs = true;
                int maxSecondCheck = 30;
                int numCheck = 0;

                while (checkProcessingImgs && (numCheck < maxSecondCheck)) 
                {
                    DateTime twentySecondsAgo = DateTime.UtcNow.AddSeconds(-20);
                    var inProcessResult = await _imageService.GetImagesByWatermarkProcessState((int)WatermarkDetectResult.Processing, true);
                    List<ImageDetectionViewModel> imagesInProcess = inProcessResult.ToList();

                    if (imagesInProcess.Count > 0){
                        foreach (ImageDetectionViewModel processingImg in imagesInProcess){
                            // set any processing image with update time of null or older than 20 seconds to "error"
                            if ((processingImg.UpdatedAt < twentySecondsAgo) || (processingImg.UpdatedAt == DateTime.MinValue)){
                                ImageDetectionViewModel updateprocessingImage = await _imageService.UpdateImage(processingImg.Id, (int)WatermarkDetectResult.Watermark_Error);
                            }
                        }

                        numCheck++;
                        await Task.Delay(1300);
                        
                    }
                    else
                    {
                        image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Processing);
                        checkProcessingImgs = false;
                    }

                }
                
                if (!checkProcessingImgs){
                        try
                        {
                            string response = string.Empty;
                            var client = _clientFactory.CreateClient("WatermarkDetectionHttpClient");

                            var queryString = new Dictionary<string, string>()
                            {
                                {"model_id", Configuration["WatermarkDetection:ModelId"] },
                                {"image_url", FileUrl },
                                {"client_key", Configuration["WatermarkDetection:ClientKey"] }
                            };

                            String paramStr = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("", queryString);

                            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, paramStr);
                            Boolean watermarkDetected = false;

                            // String postParams = "{\"model_id\":\"" + Configuration["WatermarkDetection:ModelId"] 
                            //         + "\",\"image_url\":\"" + FileUrl 
                            //         + "\",\"client_key\":\"" + Configuration["WatermarkDetection:ClientKey"] + "\"}";
                            // request.Headers.Add("Accept", "application/json");
                            // request.Content = new System.Net.Http.StringContent(postParams,
                            //             System.Text.Encoding.UTF8, 
                            //             "application/json");//CONTENT-TYPE header

                      
                        System.Net.Http.HttpResponseMessage responseMessage = await client.SendAsync(request);
                            if (responseMessage.IsSuccessStatusCode)
                            {
                                response = await responseMessage.Content.ReadAsStringAsync();
                            }

                            if (!string.IsNullOrWhiteSpace(response))
                            {
                                dynamic jo = JObject.Parse(response);

                                if (jo?.error != null && jo?.error != "false")
                                {
                                    String errorMsg = string.Format("ID : {0}, {1}, time stamp: {2}", jo?.error_id, jo?.message, jo?.time);                            
                                    _logger.LogError("Watermark Detection RestB API Error. " + errorMsg);
                                    image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark_Error);
                                }

                                else if (jo?.response != null) 
                                {
                                    if (jo?.response.solutions != null && jo?.response.solutions.re_logos_custom_1 != null)
                                    {

                                        JArray detections = (JArray)jo?.response.solutions.re_logos_custom_1.detections;
                                        if (detections.Count > 0)
                                        {
                                            if (jo?.response.solutions.re_logos_custom_1.detections[0].label == "cre_watermark")
                                            {
                                                // External Watermark Detected
                                                watermarkDetected = true;
                                                image = await _imageService.AddImage(FileUrl, (int)WatermarkDetectResult.Cre_Watermark, User);
                                            }
                                        }
                                    }

                                    // if No cre_watermark test for generic watermarks.
                                    if (!watermarkDetected && jo?.response.solutions != null && jo?.response.solutions.re_logo != null)
                                    {
                                        JArray detections = (JArray)jo?.response.solutions.re_logo.detections;
                                        if (detections.Count > 0)
                                        {
                                            if (jo?.response.solutions.re_logo.detections[0].label == "watermark")
                                            {
                                                // External Watermark Detected
                                                watermarkDetected = true;
                                                image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark);
                                            }
                                        }
                                    }

                                    // if no watermarks detected, image passed
                                    if (!watermarkDetected)
                                    {
                                        image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.No_Watermark);
                                    }
                                }

                                else 
                                {
                                    _logger.LogError("Unexpected Watermark Detection RestB response. ");
                                    image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark_Error);
                                }
                            }
                        }
                        catch (System.Exception e)
                        {
                            String errorMsg = e.Message;
                            if (e.InnerException != null)
                            {
                                errorMsg = errorMsg + ": " + e.InnerException.Message;
                            }
                            _logger.LogError("Unexpected Watermark Detection Processing Error Occurred: " + errorMsg);
                            image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark_Error);
                        }
                    }
                else {
                    // wait over 25 seconds, save a not processed image to check later
                    image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Not_Processed);
                }

                return Ok(image);

            }

            return BadRequest(ValidationError);

        }


        [HttpPost]
        [RequestSizeLimit(30000000)] //default size limit of 28.6 Mb
        public async Task<IActionResult> UploadImageMockWatermark(IFormFile file)
        {
            string ValidationError = file.ValidateImage(Configuration["ImageFileFormats"]);


            if (string.IsNullOrEmpty(ValidationError))
            {

                string FileUrl = string.Empty;
                var extension = Path.GetExtension(file.FileName);
                var FileName = string.Format("{0}{1}", Guid.NewGuid(), extension);
                var image = new ImageDetectionViewModel()
                {
                    Id = 0,
                    Url = string.Empty,
                    WatermarkProcessStatus = 0,
                    UpdatedAt = DateTime.Now
                };

                using (MemoryStream filestream = new MemoryStream())
                {

                    _logger.LogInformation("Watermark Detection - initial file save: " + FileName);
                    await file.CopyToAsync(filestream);
                    string FileResizedUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.originalimages.ToString(), filestream);
                    var finalStream = filestream.ImageResizeHandler(extension);
                    FileUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.photos.ToString(), finalStream);

                }

                try
                {

                    Boolean watermarkDetected = false;
                   // string response = "{\"response\": {\"solutions\": {\"re_logos_custom_1\": {\"detections\": [{\"label\": \"cre_watermark\"}]}, \"re_logo\": {\"detections\": [{\"label\": \"watermark\"}]}}}, \"error\": \"false\", \"time\": \"2020-05-19T03:03:18.910151\", \"correlation_id\": \"fa472cae-90d5-45c4-be2b-4735137b68f2\", \"version\": \"2\"}";
                   // string response = "{\"error\": \"false\", \"response\": {\"solutions\": {\"re_logos_custom_1\": {\"detections\": []}, \"re_logo\": {\"detections\": []}}}, \"time\": \"2020-06-12T14:12:22.516087\", \"correlation_id\": \"01cea8cb-bf8b-4d9a-b0a6-698e85330131\", \"version\": \"2\"}";
                    string response = "{\"error\": \"true\", \"message\": \"Account Limited\", \"error_id\": \"006\", \"time\": \"2020-06-09T19:37:46.198538\", \"correlation_id\": \"8ff96876-89ca-4b82-8d33-1f7e134a42e5\", \"version\": \"2\"}";



                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        dynamic jo = JObject.Parse(response);

                                if (jo?.error != null && jo?.error != "false")
                                {
                                    String errorMsg = string.Format("ID : {0}, {1}, time stamp: {2}", jo?.error_id, jo?.message, jo?.time);                            
                                    _logger.LogError("Watermark Detection RestB API Error. " + errorMsg);
                                    image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark_Error);
                                }

                                else if (jo?.response != null) 
                                {
                                    if (jo?.response.solutions != null && jo?.response.solutions.re_logos_custom_1 != null)
                                    {

                                        JArray detections = (JArray)jo?.response.solutions.re_logos_custom_1.detections;
                                        if (detections.Count > 0)
                                        {
                                            if (jo?.response.solutions.re_logos_custom_1.detections[0].label == "cre_watermark")
                                            {
                                                // External Watermark Detected
                                                watermarkDetected = true;
                                                image = await _imageService.AddImage(FileUrl, (int)WatermarkDetectResult.Cre_Watermark, User);
                                            }
                                        }
                                    }

                                    // if No cre_watermark test for generic watermarks.
                                    if (!watermarkDetected && jo?.response.solutions != null && jo?.response.solutions.re_logo != null)
                                    {
                                        JArray detections = (JArray)jo?.response.solutions.re_logo.detections;
                                        if (detections.Count > 0)
                                        {
                                            if (jo?.response.solutions.re_logo.detections[0].label == "watermark")
                                            {
                                                // External Watermark Detected
                                                watermarkDetected = true;
                                                image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark);
                                            }
                                        }
                                    }

                                    // if no watermarks detected, image passed
                                    if (!watermarkDetected)
                                    {
                                        image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.No_Watermark);
                                    }
                                }

                                else 
                                {
                                    _logger.LogError("Unexpected Watermark Detection RestB response. ");
                                    image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark_Error);
                                }
                            
                    }
                }
                catch (System.Exception e)
                {
                    String errorMsg = e.Message;
                    if (e.InnerException != null)
                    {
                        errorMsg = errorMsg + ": " + e.InnerException.Message;
                    }
                    _logger.LogError("Watermark Detection Processing Error Occurred: " + errorMsg);
                    image = await _imageService.AddImage(FileUrl, (int)WatermarkDetectResult.Watermark_Error, User);
                }


                return Ok(image);

            }

            return BadRequest(ValidationError);

        }


        [HttpGet]
        public async Task<IActionResult> CheckImage(string ID)
        {
            List<int?> imgIdList = new List<int?>();
            imgIdList.Add(Int32.Parse(ID));

            var resultImgs = await _imageService.GetImages(null, imgIdList, User);
            List<ImageDetectionViewModel> resultImgList = resultImgs.ToList();

            if (resultImgList.Count > 0) {

                ImageDetectionViewModel image = resultImgList[0];
                string FileUrl = image.Url;

                Boolean checkProcessingImgs = true;
                int maxSecondCheck = 30;
                int numCheck = 0;

                while (checkProcessingImgs && (numCheck < maxSecondCheck)) 
                {
                    DateTime twentySecondsAgo = DateTime.UtcNow.AddSeconds(-20);
                    var inProcessResult = await _imageService.GetImagesByWatermarkProcessState((int)WatermarkDetectResult.Processing, true);
                    List<ImageDetectionViewModel> imagesInProcess = inProcessResult.ToList();

                    if (imagesInProcess.Count > 0){
                        foreach (ImageDetectionViewModel processingImg in imagesInProcess){
                            // set any processing image with update time of null or older than 20 seconds to "error"
                            if ((processingImg.UpdatedAt > twentySecondsAgo) || (processingImg.UpdatedAt == DateTime.MinValue)){
                                ImageDetectionViewModel updateprocessingImage = await _imageService.UpdateImage(processingImg.Id, (int)WatermarkDetectResult.Watermark_Error);
                            }
                        }

                        numCheck++;
                        await Task.Delay(1300);
                    }
                    else
                    {
                        image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Processing);
                        checkProcessingImgs = false;
                    }

                }
                
                if (!checkProcessingImgs){

                    try
                    {
                        string response = string.Empty;
                        var client = _clientFactory.CreateClient("WatermarkDetectionHttpClient");

                        var queryString = new Dictionary<string, string>()
                        {
                            {"model_id", Configuration["WatermarkDetection:ModelId"] },
                            {"image_url", FileUrl },
                            {"client_key", Configuration["WatermarkDetection:ClientKey"] }
                        };

                        String paramStr = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("", queryString);

                        var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, paramStr);
                        Boolean watermarkDetected = false;

                    System.Net.Http.HttpResponseMessage responseMessage = await client.SendAsync(request);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            response = await responseMessage.Content.ReadAsStringAsync();
                        }

                        if (!string.IsNullOrWhiteSpace(response))
                        {
                            dynamic jo = JObject.Parse(response);

                            if (jo?.error != null  && jo?.error != "false")
                            {
                                String errorMsg = string.Format("ID : {0}, {1}, time stamp: {3}", jo?.error_id, jo?.message, jo?.time);                            
                                _logger.LogError("Watermark Detection RestB API Error. " + errorMsg);
                                image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark_Error);
                            }

                            else if (jo?.response != null) 
                            {
                                if (jo?.response.solutions != null && jo?.response.solutions.re_logos_custom_1 != null)
                                {

                                    JArray detections = (JArray)jo?.response.solutions.re_logos_custom_1.detections;
                                    if (detections.Count > 0)
                                    {
                                        if (jo?.response.solutions.re_logos_custom_1.detections[0].label == "cre_watermark")
                                        {
                                            // External Watermark Detected
                                            watermarkDetected = true;
                                            image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Cre_Watermark);
                                        }
                                    }
                                }

                                // if No cre_watermark test for generic watermarks.
                                if (!watermarkDetected && jo?.response.solutions != null && jo?.response.solutions.re_logo != null)
                                {
                                    JArray detections = (JArray)jo?.response.solutions.re_logo.detections;
                                    if (detections.Count > 0)
                                    {
                                        if (jo?.response.solutions.re_logo.detections[0].label == "watermark")
                                        {
                                            // External Watermark Detected
                                            watermarkDetected = true;
                                            image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark);
                                        }
                                    }
                                }

                                // if no watermarks detected, image passed
                                if (!watermarkDetected)
                                {
                                    image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.No_Watermark);
                                }
                            }

                            else 
                            {
                                _logger.LogError("Unexpected Watermark Detection RestB response. ");
                                image = await _imageService.UpdateImage(image.Id,  (int)WatermarkDetectResult.Watermark_Error);
                            }

                        }
                    }
                    catch (System.Exception e)
                    {
                        String errorMsg = e.Message;
                        if (e.InnerException != null)
                        {
                            errorMsg = errorMsg + ": " + e.InnerException.Message;
                        }
                        _logger.LogError("Unexpected Watermark Detection Processing Error Occurred: " + errorMsg);
                        image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark_Error);
                    }
                    
                }
                else {
                    // wait over 15 seconds, save a not processed image to check later
                    image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Not_Processed);
                }
                return Ok(image);

            }
            return BadRequest("Invalid Image Id");

        }


        [HttpPost]
        public async Task<IActionResult> MockCheckImage(string ID)
        {
            
            List<int?> imgIdList = new List<int?>();
            imgIdList.Add(Int32.Parse(ID));

            var resultImgs = await _imageService.GetImages(null, imgIdList, User);
            List<ImageDetectionViewModel> resultImgList = resultImgs.ToList();

            if (resultImgList.Count > 0) {

                ImageDetectionViewModel image = resultImgList[0];
                string FileUrl = image.Url;

                try
                {
                    
                    Boolean watermarkDetected = false;
                    string response = "{\"response\": {\"solutions\": {\"re_logos_custom_1\": {\"detections\": [{\"label\": \"cre_watermark\"}]}, \"re_logo\": {\"detections\": [{\"label\": \"watermark\"}]}}}, \"error\": \"false\", \"time\": \"2020-05-19T03:03:18.910151\", \"correlation_id\": \"fa472cae-90d5-45c4-be2b-4735137b68f2\", \"version\": \"2\"}";

                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        dynamic jo = JObject.Parse(response);

                        if (jo?.response.solutions != null && jo?.response.solutions.re_logos_custom_1 != null)
                        {

                            JArray detections = (JArray)jo?.response.solutions.re_logos_custom_1.detections;
                            if (detections.Count > 0)
                            {
                                if (jo?.response.solutions.re_logos_custom_1.detections[0].label == "cre_watermark")
                                {
                                    // External Watermark Detected
                                    watermarkDetected = true;
                                    image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Cre_Watermark);
                                }
                            }
                        }

                        // if No cre_watermark test for generic watermarks.
                        if (!watermarkDetected && jo?.response.solutions != null && jo?.response.solutions.re_logo != null)
                        {
                            JArray detections = (JArray)jo?.response.solutions.re_logo.detections;
                            if (detections.Count > 0)
                            {
                                if (jo?.response.solutions.re_logo.detections[0].label == "watermark")
                                {
                                    // External Watermark Detected
                                    watermarkDetected = true;
                                    image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark);
                                }
                            }
                        }

                        // if no watermarks detected, image passed
                        if (!watermarkDetected)
                        {
                            image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.No_Watermark);
                        }

                    }
                }
                catch (System.Exception e)
                {
                    _logger.LogError("Watermark Detection Processing Error Occurred: " + e.InnerException.Message);
                    image = await _imageService.UpdateImage(image.Id, (int)WatermarkDetectResult.Watermark_Error);
                }
                return Ok(image);
                         
            }
            return BadRequest("Invalid Image Id");

        }


        [HttpGet]
        public async Task<IActionResult> ResizeImage(string ID)
        {
            List<int?> imgIdList = new List<int?>();
            imgIdList.Add(Int32.Parse(ID));

            var resultImgs = await _imageService.GetImages(null, imgIdList, User);
            List<ImageDetectionViewModel> resultImgList = resultImgs.ToList();

            if (resultImgList.Count > 0) 
            {
                ImageDetectionViewModel image = resultImgList[0];
                string FileUrl = image.Url;

                //
                using (System.Net.WebClient client = new System.Net.WebClient()) 
                {
              
                    using (MemoryStream stream = new MemoryStream(client.DownloadData(FileUrl)))
                    {
                
                        var extension = Path.GetExtension(FileUrl);
                        var FileName = Path.GetFileName(FileUrl);
                        bool originalExists = await AzureStorage.ExistsAsync(FileName, ContainerEnum.originalimages.ToString());
                    
                    if (!originalExists)
                    {
                        string FileResizedUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.originalimages.ToString(), stream);
                        var finalStream = stream.ImageResizeHandler(extension);
                        FileUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.photos.ToString(), finalStream);
                    }
                        
                    
                }
            }

                
            return Ok(image);

            }
        return BadRequest("Invalid Image Id");

        }




        [HttpPost]
        [RequestSizeLimit(30000000)] //default size limit of 28.6 Mb
        public async Task<IActionResult> UploadImageNoChecks(IFormFile file)
        {
            string ValidationError = file.ValidateImage(Configuration["ImageFileFormats"]);


            if (string.IsNullOrEmpty(ValidationError))
            {
                string FileUrl = string.Empty;
                var extension = Path.GetExtension(file.FileName);
                var FileName = string.Format("{0}{1}", Guid.NewGuid(), extension);

                using (MemoryStream filestream = new MemoryStream())
                {
                    await file.CopyToAsync(filestream);
                    string FileResizedUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.originalimages.ToString(), filestream);
                    var finalStream = filestream.ImageResizeHandler(extension);

                    FileUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.photos.ToString(), finalStream);

                }

                // Call Service to Create Image Record.
                var image = await _imageService.AddImage(FileUrl, (int)WatermarkDetectResult.Not_Processed, User);

                return Ok(image);

            }

            return BadRequest(ValidationError);

        }

        [HttpPost]
        [RequestSizeLimit(10000000)] //default size limit of 10 Mb
        public async Task<IActionResult> ContactAvatar(IFormFile file)
        {
            string ValidationError = file.ValidateImage(Configuration["AvatarFormats"]);

            if (string.IsNullOrEmpty(ValidationError))
            {
                var extension = Path.GetExtension(file.FileName);
                var FileName = string.Format("{0}{1}", Guid.NewGuid(), extension);
                using (MemoryStream filestream = new MemoryStream())
                {
                    await file.CopyToAsync(filestream);
                    var finalStream = filestream.ImageResizeHandler(extension);
                    string FileUrl = await AzureStorage.UploadBlobAsync(FileName, ContainerEnum.avatar.ToString(), finalStream);
                    return Created(FileUrl, FileUrl);
                }
            }

            return BadRequest(ValidationError);
        }
    }
}