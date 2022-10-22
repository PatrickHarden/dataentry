using dataentry.Data.DBContext.Model;
using dataentry.Repository;
using dataentry.ViewModels.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Images
{
    public class ImageService : IImageService
    {
        private IDataEntryRepository _dataEntryRepository;

        public ImageService(IDataEntryRepository dataEntryRepository)
        {
            _dataEntryRepository = dataEntryRepository;
        }

        public async Task<ImageDetectionViewModel> AddImage(string url, int initState, ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            var img = new Image(){
                Url = url,
                WatermarkProcessStatus = initState,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = user.Identity.Name
            };
            
            var image =  await _dataEntryRepository.AddImage(img);
            return Map(image);
        }

        public async Task<ImageDetectionViewModel> UpdateImage(int id, int watermarkProcessStatus)
        {
            var image = await _dataEntryRepository.UpdateImage(id, watermarkProcessStatus);
            return Map(image);
        }

        public async Task<IEnumerable<ImageDetectionViewModel>> GetImages(int? listingId, List<int?> imageIds, ClaimsPrincipal user)
        {
            var query = await _dataEntryRepository.GetImages(listingId, imageIds, user);
            var images = query.Select(i => Map(i));
            return images;
        }

        public async Task<IEnumerable<ImageDetectionViewModel>> ResetImagesWatermarkProcessState(int targetState, int finalState)
        {
            var query = await _dataEntryRepository.ResetImagesWatermarkProcessState(targetState, finalState);
            return query.Select(i => Map(i));
        }

        public async Task<IEnumerable<ImageDetectionViewModel>> GetImagesByWatermarkProcessState(int state, bool includeDeleted = true, int? imageType = null)
        {
            var query = await _dataEntryRepository.GetImagesByWatermarkProcessState(state, includeDeleted, imageType);
            return query.Select(i => Map(i));
        }

        private ImageDetectionViewModel Map(Image image)
        {
            return new ImageDetectionViewModel(){
                Id = image.ID,
                Url = image.Url,
                WatermarkProcessStatus = image.WatermarkProcessStatus,
                UpdatedAt = image.UpdatedAt
            };
        } 
    }
}
