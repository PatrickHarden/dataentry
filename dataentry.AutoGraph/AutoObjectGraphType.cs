using GraphQL.Types;
using System.Collections.Generic;

namespace dataentry.AutoGraph
{
    public class AutoObjectGraphType<TSourceType> : ObjectGraphType<TSourceType>
    {
        private ICollection<string> IgnoredFields;
        public AutoObjectGraphType()
        {
            if (Name == null) Name = typeof(TSourceType).Name;
            IgnoredFields = new List<string>();
            this.Construct(IgnoredFields, false);
        }

        protected virtual void InitFields() { }

        protected void IgnoreField(string name)
        {
            IgnoredFields.Add(name);
        }
    }
}
