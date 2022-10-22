using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace dataentry.Extensions
{
    public static class JsonReaderExtensions
    {
        public static IEnumerable<T> SelectTokensWithRegex<T>(
            this JsonReader jsonReader,
            Regex regex,
            JsonToken tokenType = JsonToken.StartObject
        )
        {
            JsonSerializer serializer = new JsonSerializer();
            while (jsonReader.Read())
            {
                if (jsonReader.TokenType == tokenType && regex.IsMatch(jsonReader.Path))
                {
                    yield return serializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}
