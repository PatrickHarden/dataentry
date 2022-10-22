using System;

namespace dataentry.AutoGraph.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class DeprecatedAttribute : AutoGraphAttributeBase
    {
        public string Value { get; set; }
        public DeprecatedAttribute(string value, TargetGraphType targetGraphType = TargetGraphType.Both) : base(targetGraphType)
        {
            Value = value;
        }
    }
}
