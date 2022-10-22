namespace dataentry.Data.DBContext.Model
{
    public interface IMedia
    {
        string Url { get; }
        string DisplayText { get; }
        bool Active { get; }
        bool Primary { get; }
    }
}
