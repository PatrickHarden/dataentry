using System.Threading.Tasks;
using dataentry.Data.DBContext.Model;

namespace dataentry.Services.Business.Publishing
{
    public interface IPublishingTarget
    {
        string Name { get; }
        bool StopOnException { get; }

        Task Publish(Listing listing);
        Task PublishPreview(Listing listing);
        Task Unpublish(Listing listing);
        Task UnpublishPreview(Listing listing);
    }
}