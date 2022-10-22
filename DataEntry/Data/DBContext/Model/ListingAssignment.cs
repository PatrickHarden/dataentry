using System;
using System.Collections.Generic;

namespace dataentry.Data.DBContext.Model
{
    public class ListingAssignment
    {
        public string AssignedBy { get; set; }
        public bool? AssignmentFlag { get; set; }
        public DateTime? AssignedDate { get; set; }
    }
}