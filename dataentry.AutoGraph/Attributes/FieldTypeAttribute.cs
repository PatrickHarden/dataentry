using System;

namespace dataentry.AutoGraph.Attributes
{
    public class FieldTypeAttribute : AutoGraphAttributeBase
    {
        public Type Value { get; set; }
        public FieldTypeAttribute(Type value, TargetGraphType targetGraphType = TargetGraphType.Both) : base(targetGraphType)
        {
            Value = value;
        }
    }
}
