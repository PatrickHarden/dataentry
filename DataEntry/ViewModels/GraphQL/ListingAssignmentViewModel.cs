using System;
using System.Collections.Generic;
using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("ListingAssignment")]
    [AutoInputObjectGraphType("ListingAssignmentInput")]
    public class ListingAssignmentViewModel
    {
        public string AssignedBy { get; set; }
        public bool? AssignmentFlag { get; set; }
        public DateTime? AssignedDate { get; set; }
    }
}