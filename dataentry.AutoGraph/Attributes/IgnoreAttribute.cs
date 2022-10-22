using System;

namespace dataentry.AutoGraph.Attributes
{
    public class IgnoreAttribute : AutoGraphAttributeBase {
        public IgnoreAttribute(TargetGraphType targetGraphType) : base(targetGraphType) { }
    }
}
