using Newtonsoft.Json.Linq;

namespace dataentry.Data
{
    public interface ISiteMapsConfigDataProvider
    {
        JObject GetSitemapConfig();
    }
}