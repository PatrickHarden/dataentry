using dataentry.Utility;

namespace dataentry.Services.Business.Publishing
{
    public enum PublishActionType
    {
        Publish,
        Unpublish,
        [Alias("Publish Preview")]
        PublishPreview,
        [Alias("Unpublish Preview")]
        UnpublishPreview
    }
}