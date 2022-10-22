using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Consumption
{
    public class Entity
    {
        public int id { get; set; }
        public IEnumerable<SourceLineage> source_lineage { get; set; }
    }

    public class SourceLineage
    {
        public string source_system { get; set; }
        public string source_unique_id { get; set; }
    }
}