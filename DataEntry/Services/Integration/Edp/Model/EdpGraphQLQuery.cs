using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
namespace dataentry.Services.Integration.Edp.Model
{
    public abstract class EdpGraphQLQuery<T> where T : EdpGraphQLObject, new()
    {
        protected virtual string QueryType => "query";
        protected abstract string QueryName { get; }
        protected abstract IEnumerable<KeyValuePair<string, object>> Args { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(QueryType);
            sb.Append("{");
            sb.Append(QueryName);
            sb.Append("(");

            var first = true;
            foreach (var arg in Args)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(',');
                }
                sb.Append(arg.Key);
                sb.Append(": ");
                sb.Append(Serialize(arg.Value));
            }
            sb.Append(")");
            sb.Append(new T().ResultFields);
            sb.Append("}");
            return sb.ToString();
        }

        private string Serialize(object value) {
            var serializer = new JsonSerializer();
            var stringWriter = new StringWriter();
            using (var writer = new JsonTextWriter(stringWriter))
            {
                writer.QuoteName = false;
                writer.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                serializer.Serialize(writer, value);            
            }
            return stringWriter.ToString();
        }
    }
}