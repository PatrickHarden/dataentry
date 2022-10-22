using GraphQL.Language.AST;
using System.Collections.Generic;
using System.Linq;

namespace dataentry.Extensions
{
    public static class GraphQLExtensions
    {
        public static bool HasSubField(this Field field, string fieldName)
        {
            return GetSubField(field, fieldName) != null;
        }

        public static Field GetSubField(this Field field, string fieldName)
        {
            return SubFields(field).FirstOrDefault(subField => subField.Name == fieldName);
        }

        public static IEnumerable<Field> SubFields(this Field field)
        {
            return field.SelectionSet.Selections.OfType<Field>();
        }
    }
}
