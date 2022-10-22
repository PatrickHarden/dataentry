using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.AutoGraph
{
    internal static class ReflectionExtensions
    {
        public static bool IsSubclassOfOpenGeneric(this Type toCheck, Type generic)
        {
            return GetNearestOpenGeneric(toCheck, generic) != null;
        }

        public static Type GetNearestOpenGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return toCheck;
                }
                toCheck = toCheck.BaseType;
            }
            return null;
        }
    }
}
