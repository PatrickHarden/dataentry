using System;
using System.Collections.Generic;
using System.Text;

namespace dataentry.AutoGraph.Attributes
{
    [Flags]
    public enum TargetGraphType
    {
        ObjectGraphType = 1,
        InputObjectGraphType = 2,
        Both = ObjectGraphType | InputObjectGraphType
    }

    public abstract class AutoGraphAttributeBase : Attribute
    {
        public bool ForObjectGraphType { get; set; }
        public bool ForInputObjectGraphType { get; set; }

        public AutoGraphAttributeBase(TargetGraphType targetGraphType = TargetGraphType.Both)
        {
            ForObjectGraphType = targetGraphType.HasFlag(TargetGraphType.ObjectGraphType);
            ForInputObjectGraphType = targetGraphType.HasFlag(TargetGraphType.InputObjectGraphType);
        }
    }
}
