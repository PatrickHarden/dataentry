using System.Collections.Generic;
using GraphQL.Types;

namespace dataentry.AutoGraph
{
    public class AutoInputObjectGraphType<TSourceType> : InputObjectGraphType<TSourceType>
    {
        private ICollection<string> IgnoredFields;
        public AutoInputObjectGraphType()
        {
            if (Name == null) Name = typeof(TSourceType).Name + "Input";
            IgnoredFields = new List<string>();
            this.Construct(IgnoredFields, true);
        }

        protected virtual void InitFields() { }

        protected void IgnoreField(string name)
        {
            IgnoredFields.Add(name);
        }
    }
}
