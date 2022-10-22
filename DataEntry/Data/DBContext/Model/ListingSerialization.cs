using dataentry.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Data.DBContext.Model
{
    public class ListingSerialization
    {
        public ListingSerializationType Type { get; set; }
        public string Data { get; set; }
    }
}
