using System;

namespace dataentry.AutoGraph.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class DescriptionAttribute : AutoGraphAttributeBase
    {
        public string Value { get; set; }
        public DescriptionAttribute(string value, TargetGraphType targetGraphType = TargetGraphType.Both) : base(targetGraphType)
        {
            Value = value;
        }
    }
}
