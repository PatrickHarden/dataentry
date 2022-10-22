using GraphQL;
using GraphQL.Types;

namespace dataentry.ViewModels.GraphQL
{
    public class DataEntrySchema : Schema
    {
        public DataEntrySchema(IDependencyResolver resolver) 
            : base(resolver)
        {
            Query = resolver.Resolve<DataEntryQuery>();
            Mutation = resolver.Resolve<DataEntryMutation>();
        }
    }
}
