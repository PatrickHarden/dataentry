using dataentry.ViewModels.GraphQL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dataentry.Services.Business.Listings
{
    public class ListingSerializer : IListingSerializer
    {
        private readonly JsonSerializer _serializer;

        public ListingSerializer()
        {
            _serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.None
            });
        }

        public string Serialize(ListingViewModel vm)
        {
            StringBuilder result = new StringBuilder();
            using (var writer = new StringWriter(result))
            {
                _serializer.Serialize(writer, vm, typeof(ListingViewModel));
            }
            return result.ToString();
        }

        public ListingViewModel Deserialize(string value)
        {
            using (var reader = new StringReader(value))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return _serializer.Deserialize<ListingViewModel>(jsonReader);
            }
        }
    }
}
