using dataentry.Services.Background.WatermarkDetection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using dataentry.Services.Business.Images;
using dataentry.Data.Enums;
using System.Collections.Generic;
using System.Linq;
using dataentry.ViewModels.GraphQL;


namespace dataentry.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WatermarkDetectController : ControllerBase
    {

        private readonly WatermarkDetectionService _watermarkDetectionService;
        private readonly IImageService _imageService;

        public WatermarkDetectController(IHostedService watermarkDetectionService, IImageService imageservice)
        {
            _watermarkDetectionService = watermarkDetectionService as WatermarkDetectionService;
            _imageService = imageservice;
        }

        [HttpGet]
        public async Task<IActionResult> CheckImageProcessStatus()
        {
            WatermarkProcessRunStatus watermarkProcessStatus = _watermarkDetectionService.CheckIfImageProcessingOn();
            var notProcessedResult = await _imageService.GetImagesByWatermarkProcessState((int)WatermarkDetectResult.Not_Processed, false, (int)ImageCategory.Photo);
            List<ImageDetectionViewModel> unprocessedImgs = notProcessedResult.ToList();
            return Ok(new { ProcessRunning = watermarkProcessStatus.Run.ToString(), 
                            UnprocessedImages = unprocessedImgs.Count,
                            RunOptionCodes = new[] {"Stopped","BatchOfTenFast","BatchOfTenSlow","OneAndHalfSeconds","TwoSeconds","ThreeSeconds"},
                            CurrentRunCode = watermarkProcessStatus.RunCode
                            });
        }

        

        [HttpGet]
        public async Task<IActionResult> SetImageProcessStatus(string RUNCODE)
        {
            WatermarkProcessRunStatus watermarkProcessStatus = _watermarkDetectionService.CheckIfImageProcessingOn();
            if (!string.IsNullOrEmpty(RUNCODE)){
                
                WatermarkProcessRunOptions runcodeEnum = (WatermarkProcessRunOptions) System.Enum.Parse(typeof(WatermarkProcessRunOptions), RUNCODE);
                watermarkProcessStatus = _watermarkDetectionService.SetImageProcessing((int)runcodeEnum);
            }
            var notProcessedResult = await _imageService.GetImagesByWatermarkProcessState((int)WatermarkDetectResult.Not_Processed, false, (int)ImageCategory.Photo);
            List<ImageDetectionViewModel> unprocessedImgs = notProcessedResult.ToList();
            return Ok(new { ProcessRunning = watermarkProcessStatus.Run.ToString(), 
                            UnprocessedImages = unprocessedImgs.Count,
                            RunOptionCodes = new[] {"Stopped","BatchOfTenFast","BatchOfTenSlow","OneAndHalfSeconds","TwoSeconds","ThreeSeconds"},
                            CurrentRunCode = watermarkProcessStatus.RunCode});
        }

    
    
    }

}