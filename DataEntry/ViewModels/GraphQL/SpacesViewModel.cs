using dataentry.AutoGraph.Attributes;
using GraphQL.Types;
using System;
using System.Collections.Generic;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Spaces")]
    [AutoInputObjectGraphType("SpacesInput")]
    public class SpacesViewModel
    {
        public int Id { get; set; }
        [Description("The ID of this availability when sent to EDP")]
        public string ExternalId { get; set; }
        [Description("Unused")]
        public string PreviewId { get; set; }
        [Description("The ID of this availability imported from MarketIQ.")]
        public string MiqId { get; set; }
        public IEnumerable<TextTypeViewModel> Name { get; set; }
        public IEnumerable<TextTypeViewModel> SpaceDescription { get; set; }
        public string Status { get; set; }
        public string SpaceType { get; set; }
        [FieldType(typeof(CustomDateTimeGraphType))]
        [Description("The date this space becomes avaialable for purchase or lease.")]
        public DateTime? AvailableFrom { get; set; }
        public IEnumerable<ImagesViewModel> Photos { get; set; }
        public IEnumerable<ImagesViewModel> Floorplans { get; set; }
        public IEnumerable<MediaViewModel> Brochures { get; set; }
        [NonNull]
        public SpecificationsViewModel Specifications { get; set; }
        public IEnumerable<PropertySizesViewModel> SpaceSizes { get; set; }
        public string Video { get; set; }
        public string WalkThrough { get; set; }
    }
}