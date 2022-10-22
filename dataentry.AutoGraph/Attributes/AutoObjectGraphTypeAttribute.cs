using System;

namespace dataentry.AutoGraph.Attributes
{
    public class AutoObjectGraphTypeAttribute : Attribute
    {
        public string Name { get; private set; }
        public AutoObjectGraphTypeAttribute(string name = null)
        {
            Name = name;
        }
    }
}
