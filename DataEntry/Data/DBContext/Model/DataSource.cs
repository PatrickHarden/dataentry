using System.Collections.Generic;

namespace dataentry.Data.DBContext.Model
{
    public class DataSource
    {
        public IEnumerable<string> DataSources { get; set; }
        public string Other { get; set; }
    }
}