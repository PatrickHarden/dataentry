using dataentry.Data.DBContext.Model;
using dataentry.Services.Business.Publishing;
using dataentry.Services.Integration.Edp.Ingestion;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace dataentry.Services.Integration.Edp
{
    public class PublishingTarget : IPublishingTarget
    {
        private readonly IIngestionService _edpService;

        public string Name => "EDP";
        public bool StopOnException => false;

        public PublishingTarget(IIngestionService edpService)
        {
            _edpService = edpService ?? throw new ArgumentNullException(nameof(edpService));
        }

        public async Task Publish(Listing listing)
        {
            if (listing == null) return;

            var homeSiteID = listing.Region?.HomeSiteID;
            if (homeSiteID == null) return;

            if (!_edpService.EnabledInRegion(homeSiteID)) return;

            await _edpService.SubmitListing(listing);
        }

        public Task Unpublish(Listing listing)
        {
            //No action on unpublish
            return Task.CompletedTask;
        }

        public Task PublishPreview(Listing listing)
        {
            //No action on preview
            return Task.CompletedTask;
        }

        public Task UnpublishPreview(Listing listing)
        {
            //No action on preview
            return Task.CompletedTask;
        }
    }
}
