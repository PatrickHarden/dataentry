using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using dataentry.Data.Enums;
using System;
using Microsoft.Extensions.Configuration;
using dataentry.Data.DBContext.Model;
using dataentry.Repository;


namespace dataentry.Services.Background.WatermarkDetection
{
    public class WatermarkDetectionService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly IConfiguration _configuration;

        private readonly System.Net.Http.IHttpClientFactory _clientFactory;

        private bool _processImages;

        private int _runCode;

        public WatermarkDetectionService(ILogger<WatermarkDetectionService> logger, IServiceScopeFactory scopeFactory, System.Net.Http.IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _clientFactory = clientFactory;
            _configuration = configuration;
            _processImages = false;
            _runCode = (int)WatermarkProcessRunOptions.Stopped;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }


        public WatermarkProcessRunStatus CheckIfImageProcessingOn()
        {
            return new WatermarkProcessRunStatus(_processImages, _runCode);            
        }

        public WatermarkProcessRunStatus SetImageProcessing(int runCode)
        {
            _processImages = (runCode > 0);
            _runCode = runCode;

            switch(_runCode){
                case (int)WatermarkProcessRunOptions.Stopped:
                     _timer?.Change(Timeout.Infinite, 0);
                break;
                case (int)WatermarkProcessRunOptions.BatchOfTenFast:
                    _timer?.Change(0, 14000);
                break;
                case (int)WatermarkProcessRunOptions.OneAndHalfSeconds:
                    _timer?.Change(0, 1500);
                break;
                case (int)WatermarkProcessRunOptions.TwoSeconds:
                    _timer?.Change(0, 2000);
                break;
                case (int)WatermarkProcessRunOptions.ThreeSeconds:
                    _timer?.Change(0, 3000);
                break;
                case (int)WatermarkProcessRunOptions.BatchOfTenSlow:
                    _timer?.Change(0, 20000);
                break;
                default:
                    _timer?.Change(Timeout.Infinite, 0);
                break;
            }

            // if (_processImages)
            // {
            //      _timer?.Change(0, 14000);
            // }
            // else
            // {
            //     _timer?.Change(Timeout.Infinite, 0);
            // }

            return new WatermarkProcessRunStatus(_processImages, _runCode); 
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Watermark Detection Auto Check Service Start");
            _timer = new Timer(ProcessImages, null, Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        
        public async void ProcessImages(object state)
        { 
            // process only runs if timer interval is set/on
            using (var scope = _scopeFactory.CreateScope())
            {               
                var _dataEntryRepository =  scope.ServiceProvider.GetRequiredService<IDataEntryRepository>();
                
                // Check and clean up anything currently in process
                bool imagesProcessing = ImagesCurrentlyProcessing(_dataEntryRepository).Result;
                
                if (!imagesProcessing)
                {
                    var resultimgs = await _dataEntryRepository.GetImagesByWatermarkProcessState((int)WatermarkDetectResult.Not_Processed, false, (int)ImageCategory.Photo);                   
                    List<Image> unprocessedImgs = resultimgs.ToList();
                    _logger.LogInformation("Unprocessed images:" + unprocessedImgs.Count.ToString());    

                    if (unprocessedImgs.Count > 0)
                    {

                        switch(_runCode)
                        {
                            case (int)WatermarkProcessRunOptions.BatchOfTenSlow:
                            case (int)WatermarkProcessRunOptions.BatchOfTenFast:
                            {
                                int numberToProcess = 10;
                                if (unprocessedImgs.Count < 10)
                                {
                                    numberToProcess = unprocessedImgs.Count;
                                }
                                
                                for (int i = 0; i < numberToProcess; i++)
                                {
                                    if (_runCode != (int)WatermarkProcessRunOptions.BatchOfTenFast && _runCode != (int)WatermarkProcessRunOptions.BatchOfTenSlow)
                                        break;

                                    Image takeImg = unprocessedImgs[i];

                                    takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID, (int)WatermarkDetectResult.Processing);
                                    try
                                    {
                                        string response = CheckImageForWatermarks(takeImg.Url).Result;
                                        
                                        WatermarkDetectResult responseResult = ProcessWatermarkResponse(response);
                                        takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID, (int)responseResult);
                                                                
                                    }
                                    catch (System.Exception e)
                                    {
                                        String errorMsg = e.Message;
                                        if (e.InnerException != null)
                                        {
                                            errorMsg = errorMsg + ": " + e.InnerException.Message;
                                        }
                                        _logger.LogError("Unexpected Watermark Detection Processing Error Occurred: " + errorMsg);
                                        takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID, (int)WatermarkDetectResult.Watermark_Error);
                                    }
                                    
                                    if (_runCode == (int)WatermarkProcessRunOptions.BatchOfTenFast )
                                    {
                                        await Task.Delay(1100);
                                    }
                                    else
                                    {
                                        await Task.Delay(1500);
                                    }
                                }

                            }
                            break;

                            case (int)WatermarkProcessRunOptions.OneAndHalfSeconds:
                            case (int)WatermarkProcessRunOptions.TwoSeconds:
                            case (int)WatermarkProcessRunOptions.ThreeSeconds:
                            {   
                                Image takeImg = unprocessedImgs.First();
                                takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID, (int)WatermarkDetectResult.Processing);
                                try
                                {
                                    string response = CheckImageForWatermarks(takeImg.Url).Result;
                                    
                                    WatermarkDetectResult responseResult = ProcessWatermarkResponse(response);
                                    takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID, (int)responseResult);
                                                            
                                }
                                catch (System.Exception e)
                                {
                                    String errorMsg = e.Message;
                                    if (e.InnerException != null)
                                    {
                                        errorMsg = errorMsg + ": " + e.InnerException.Message;
                                    }
                                    _logger.LogError("Unexpected Watermark Detection Processing Error Occurred: " + errorMsg);
                                    takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID, (int)WatermarkDetectResult.Watermark_Error);
                                }
                            }
                            break;

                            default:
                            break;

                        }

                        
                     
                        
                    }
                    else
                    {
                        // No images to process, stop processing
                        SetImageProcessing((int)WatermarkProcessRunOptions.Stopped);
                    }            
                }
            }

        }

        private async Task<bool> ImagesCurrentlyProcessing(IDataEntryRepository dataEntryRepository)
        {
            //  Check for any images currently processing
            DateTime twentyfiveSecondsAgo = DateTime.UtcNow.AddSeconds(-25);
            var inProcessResult = await dataEntryRepository.GetImagesByWatermarkProcessState((int)WatermarkDetectResult.Processing, true);
            List<Image> imagesInProcess = inProcessResult.ToList();

            if (imagesInProcess.Count > 0){
                foreach (Image processingImg in imagesInProcess){
                    // set any processing image with update time of null or older than 25 seconds to "not processed"
                    if ((processingImg.UpdatedAt > twentyfiveSecondsAgo) || (processingImg.UpdatedAt == DateTime.MinValue)){
                        Image updateprocessingImage = await dataEntryRepository.UpdateImage(processingImg.ID, (int)WatermarkDetectResult.Not_Processed);
                    }
                }
                return true;
            }
            else 
            {
                return false;
            }
        }

        private WatermarkDetectResult ProcessWatermarkResponse(string response)
        {
            Boolean watermarkDetected = false;
            WatermarkDetectResult watermarkResult = WatermarkDetectResult.Watermark_Error;
            if (!string.IsNullOrWhiteSpace(response))
            {
                dynamic jo = JObject.Parse(response);

                if (jo?.error != null  && jo?.error != "false")
                {
                    String errorMsg = string.Format("ID : {0}, {1}, time stamp: {3}", jo?.error_id, jo?.message, jo?.time);                            
                    _logger.LogError("Watermark Detection RestB API Error. " + errorMsg);
                    watermarkResult = WatermarkDetectResult.Watermark_Error;
                    // takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID, (int)WatermarkDetectResult.Watermark_Error);
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
                                watermarkResult = WatermarkDetectResult.Cre_Watermark;
                                //  takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID, (int)WatermarkDetectResult.Cre_Watermark);
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
                                watermarkResult = WatermarkDetectResult.Watermark;
                                //  takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID, (int)WatermarkDetectResult.Watermark);
                            }
                        }
                    }

                    // if no watermarks detected, image passed
                    if (!watermarkDetected)
                    {
                        watermarkResult = WatermarkDetectResult.No_Watermark;
                        // takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID, (int)WatermarkDetectResult.No_Watermark);
                    }
                }

                else 
                {
                    _logger.LogError("Unexpected Watermark Detection RestB response. ");
                    watermarkResult = WatermarkDetectResult.Watermark_Error;
                //    takeImg = await _dataEntryRepository.UpdateImage(takeImg.ID,  (int)WatermarkDetectResult.Watermark_Error);
                }

            }
            return watermarkResult;
                            
        }

        private async Task<string> CheckImageForWatermarks(string imgUrl)
        {
            string response = string.Empty;
            var client = _clientFactory.CreateClient("WatermarkDetectionHttpClient");
            var queryString = new Dictionary<string, string>()
            {
                {"model_id", _configuration["WatermarkDetection:ModelId"] },
                {"image_url", imgUrl },
                {"client_key", _configuration["WatermarkDetection:ClientKey"] }
            };

            String paramStr = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("", queryString);
            var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, paramStr);
            
            // Calling RestBApi
            // _logger.LogWarning("sending " + imgUrl + " " + DateTime.Now.TimeOfDay);
            System.Net.Http.HttpResponseMessage responseMessage = await client.SendAsync(request);
            if (responseMessage.IsSuccessStatusCode)
            {
                response = await responseMessage.Content.ReadAsStringAsync();
            }

            //  Mock
            // response = "{\"response\": {\"solutions\": {\"re_logos_custom_1\": {\"detections\": [{\"label\": \"cre_watermark\"}]}, \"re_logo\": {\"detections\": [{\"label\": \"watermark\"}]}}}, \"error\": \"false\", \"time\": \"2020-05-19T03:03:18.910151\", \"correlation_id\": \"fa472cae-90d5-45c4-be2b-4735137b68f2\", \"version\": \"2\"}";
            
            return response;
        }


        public async void CleanUpAllProcessingImages()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _dataEntryRepository =  scope.ServiceProvider.GetRequiredService<IDataEntryRepository>();
                var resetImgs = await _dataEntryRepository.ResetImagesWatermarkProcessState((int)WatermarkDetectResult.Processing,
                                                               (int)WatermarkDetectResult.Not_Processed);
            }
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            CleanUpAllProcessingImages();
            _logger.LogInformation("Watermark Detection Auto Check Service Stop");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }

    public enum WatermarkProcessRunOptions
    {       
        Stopped,
        BatchOfTenFast,
        BatchOfTenSlow,
        OneAndHalfSeconds,
        TwoSeconds,
        ThreeSeconds
    }

    public class WatermarkProcessRunStatus
    {
        public bool Run {get; set;}
        public int RunCode {get; set;}
        public WatermarkProcessRunStatus(bool run, int runCode)
        {
            Run = run;
            RunCode = runCode;
        }
        
    }
}