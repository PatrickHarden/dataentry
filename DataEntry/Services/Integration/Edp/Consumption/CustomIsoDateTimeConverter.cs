using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace dataentry.Services.Integration.Edp.Consumption
{
    internal class CustomIsoDateTimeConverter : IsoDateTimeConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (FormatException ex) {
                if (ex.Message.Contains("is not supported in calendar")) {
                    return DateTime.MinValue;
                }
                throw;
            }
        }
    }
}