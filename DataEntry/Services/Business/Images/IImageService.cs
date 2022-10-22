using dataentry.ViewModels.GraphQL;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Images
{
    public interface IImageService
    {
        Task<ImageDetectionViewModel> AddImage(string url, int initState, ClaimsPrincipal user);
        Task<ImageDetectionViewModel> UpdateImage(int id, int watermarkProcessStatus);
        Task<IEnumerable<ImageDetectionViewModel>> GetImages(int? listingId, List<int?> imageIds, ClaimsPrincipal user);
        Task<IEnumerable<ImageDetectionViewModel>> GetImagesByWatermarkProcessState(int state, bool includeDeleted = true, int? imageType = null);
        Task<IEnumerable<ImageDetectionViewModel>> ResetImagesWatermarkProcessState(int targetState, int finalState);
    }
}
