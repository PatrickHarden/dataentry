using System;
using Newtonsoft.Json;

namespace dataentry.Utility
{
    public class IntOrStringJsonConverter : JsonConverter
    {
        public IntOrStringJsonConverter() { }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer
        )
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType == typeof(int))
                {
                    return default(int);
                }
                else if (objectType == typeof(string) || objectType == typeof(int?))
                {
                    return null;
                }
            }

            string stringValue = reader.Value.ToString();

            if (objectType == typeof(string))
                return stringValue;
            if (objectType == typeof(int))
                return int.Parse(stringValue);
            if (objectType == typeof(int?))
                return new int?(int.Parse(stringValue));
            throw new FormatException($"Invalid return type for {nameof(IntOrStringJsonConverter)}.{nameof(ReadJson)}");
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string) || objectType == typeof(int) || objectType == typeof(int?);
        }
    }
}
