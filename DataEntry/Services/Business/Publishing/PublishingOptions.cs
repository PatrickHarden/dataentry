using System.Collections.Generic;
using System.Security.Claims;
using dataentry.AutoGraph.Attributes;

namespace dataentry.Services.Business.Publishing
{
    [AutoInputObjectGraphType]
    public class PublishingOptions
    {
        public int ListingId { get; set; }

        [Ignore(TargetGraphType.Both)]
        public ClaimsPrincipal UserPrincipal { get; set; }
        
        public PublishActionType PublishAction { get; set; }
        public bool Validate { get; set; }
        public List<string> PublishingTargets { get; set; }
        public bool UpdatePublishingState { get; set; }

        [Ignore(TargetGraphType.InputObjectGraphType)]
        public bool IsPreview => PublishAction == PublishActionType.PublishPreview || PublishAction == PublishActionType.UnpublishPreview;

        public PublishingOptions() : this(0, null) {}
        public PublishingOptions(int listingId, ClaimsPrincipal userPrincipal) : this(listingId, userPrincipal, PublishActionType.Publish) {}
        public PublishingOptions(int listingId, ClaimsPrincipal userPrincipal, PublishActionType publishAction) {
            ListingId = listingId;
            UserPrincipal = userPrincipal;
            PublishAction = publishAction;
            PublishingTargets = null;

            var isPreview = IsPreview;
            Validate = !isPreview;
            UpdatePublishingState = !isPreview;
        }
    }
}