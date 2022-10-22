using System;
using GraphQL.Types;

namespace dataentry.ViewModels.GraphQL
{
    public class CustomDateTimeGraphType : DateTimeGraphType {
        public override object ParseValue(object value)
        {
            try {
                return base.ParseValue(value);
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