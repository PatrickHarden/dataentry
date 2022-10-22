namespace dataentry.Data.DBContext.Model
{
    public class Media : IMedia
    {
        public string Url { get; set; }
        public string DisplayText { get; set; }
        public bool Active { get; set; }
        public bool Primary { get; set; }
    }
}
