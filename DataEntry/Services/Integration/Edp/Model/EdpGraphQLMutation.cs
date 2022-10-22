namespace dataentry.Services.Integration.Edp.Model
{
    public abstract class EdpGraphQLMutation<T> : EdpGraphQLQuery<T> where T : EdpGraphQLObject, new()
    {
        protected override string QueryType => "mutation";
    }
}