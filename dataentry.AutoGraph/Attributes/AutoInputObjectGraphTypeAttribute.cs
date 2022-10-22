using System;

namespace dataentry.AutoGraph.Attributes
{
    public class AutoInputObjectGraphTypeAttribute : Attribute
    {
        public string Name { get; private set; }
        public AutoInputObjectGraphTypeAttribute(string name = null)
        {
            Name = name;
        }
    }
}
