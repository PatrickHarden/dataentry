using dataentry.Data.DBContext.Model;
using dataentry.Services.Business.Publishing;
using dataentry.Services.Integration.StoreApi.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace dataentry.Services.Integration.StoreApi
{
    public class PublishingTarget : IPublishingTarget
    {
        private readonly IListingAdapter _listingAdapter;
        private readonly IStoreService _storeService;
        private readonly ILogger<PublishingTarget> _logger;

        public string Name => "StoreAPI";
        public bool StopOnException => true;

        public PublishingTarget(IListingAdapter listingAdapter, IStoreService storeService, ILogger<PublishingTarget> logger)
        {
            _listingAdapter = listingAdapter ?? throw new ArgumentNullException(nameof(listingAdapter));
            _storeService = storeService ?? throw new ArgumentNullException(nameof(storeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Publish(Listing listing)
        {
            // Map listing data to property listing (StoreAPI) schema
            PropertyListing propertyListing = _listingAdapter.ConvertToPropertyListing(listing);

            // Submit property listing to the store api
            string result = await _storeService.AddListing(propertyListing);
        }

        public async Task Unpublish(Listing listing)
        {
            // Map listing data to property listing (StoreAPI) schema
            PropertyListing propertyListing = _listingAdapter.ConvertToPropertyListing(listing);

            string result = await _storeService.RemoveListing(propertyListing.CommonPrimaryKey);
        }

        public async Task PublishPreview(Listing listing)
        {
            // Map listing data to property listing (StoreAPI) schema
            PropertyListing propertyListing = _listingAdapter.ConvertToPreviewPropertyListing(listing);

            // Submit property listing to the store api
            try
            {
                await _storeService.AddListing(propertyListing);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to preview {propertyListing.CommonPrimaryKey} on {DateTimeOffset.Now.ToString()}.  Error Message: {ex.Message}");
            }
        }

        public async Task UnpublishPreview(Listing listing)
        {
            // Map listing data to property listing (StoreAPI) schema
            PropertyListing propertyListing = _listingAdapter.ConvertToPreviewPropertyListing(listing);

            // Submit property listing to the store api
            try
            {
                await _storeService.RemoveListing(propertyListing.CommonPrimaryKey);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to unpreview {propertyListing.CommonPrimaryKey} on {DateTimeOffset.Now.ToString()}.  Error Message: {ex.Message}");
            }
        }
    }
}
