using dataentry.AutoGraph.Attributes;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace dataentry.AutoGraph
{
    internal static class ComplexGraphTypeExtensions
    {
        private static readonly Dictionary<Type, Type> TypeDictionary = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, Type> InputTypeDictionary = new Dictionary<Type, Type>();

        public static void Construct<TSourceType>(this ComplexGraphType<TSourceType> obj, IEnumerable<string> ignoredFields, bool isInputType)
        {
            bool attributeFilter(AutoGraphAttributeBase a) => a.ForObjectGraphType && !isInputType || a.ForInputObjectGraphType && isInputType;
            obj.Description = typeof(TSourceType).GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>().FirstOrDefault(attributeFilter)?.Value;
            obj.DeprecationReason = typeof(TSourceType).GetCustomAttributes(typeof(DeprecatedAttribute), true).Cast<DeprecatedAttribute>().FirstOrDefault(attributeFilter)?.Value;

            foreach (var propertyInfo in typeof(TSourceType).GetProperties())
            {
                var fieldName = char.ToLowerInvariant(propertyInfo.Name[0]) + propertyInfo.Name.Substring(1);
                if (ignoredFields.Contains(fieldName)) continue;
                if (obj.Fields.Any(f => f.Name == fieldName)) continue;

                if (propertyInfo.GetCustomAttributes(typeof(IgnoreAttribute), true).Cast<IgnoreAttribute>().Any(attributeFilter)) continue;
                var isNullable = !(
                    propertyInfo.GetCustomAttributes(typeof(NonNullAttribute), true).Cast<NonNullAttribute>().Any(attributeFilter) || 
                    propertyInfo.PropertyType.IsEnum);
                var descriptionAttribute = propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>().FirstOrDefault(attributeFilter);
                var deprecatedAttribute = propertyInfo.GetCustomAttributes(typeof(DeprecatedAttribute), true).Cast<DescriptionAttribute>().FirstOrDefault(attributeFilter);
                var fieldTypeAttribute = propertyInfo.GetCustomAttributes(typeof(FieldTypeAttribute), true).Cast<FieldTypeAttribute>().FirstOrDefault(attributeFilter);

                Type graphType;
                if (fieldTypeAttribute != null)
                {
                    graphType = fieldTypeAttribute.Value;
                }
                else
                {
                    try
                    {
                        graphType = GetAutoGraphTypeFromType(propertyInfo.PropertyType, isInputType, isNullable);
                    }
                    catch (ArgumentException)
                    {
                        // This type is unknown, ignore it
                        continue;
                    }
                }
                obj.Field(graphType, fieldName, description: descriptionAttribute?.Value, deprecationReason: deprecatedAttribute?.Value);
            }
        }

        public static void RegisterType(Type objectType, Type graphType, bool isInputType)
        {
            if (isInputType) InputTypeDictionary[objectType] = graphType;
            else TypeDictionary[objectType] = graphType;
        }

        public static bool IsSystemType(this Type type)
        {
            type = type.GetNullableType();

            return !type.IsEnum && (type.IsPrimitive() || Type.GetTypeCode(type) != TypeCode.Object);
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.IsPrimitive
                || type == typeof(string)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset);
        }

        public static Type GetNullableType(this Type type)
        {
            return  type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
        }

        //https://github.com/graphql-dotnet/graphql-dotnet/blob/a81ccaa0754de9703580eb79c0cd466688db7d4b/src/GraphQL/TypeExtensions.cs
        //TODO: fork graphql instead of duplicating code
        private static Type GetAutoGraphTypeFromType(Type type, bool isInputType, bool isNullable)
        {
            if (type.IsSystemType())
            {
                return type.GetGraphTypeFromType(isNullable);  
            }
            
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
                if (isNullable == false)
                {
                    throw new ArgumentOutOfRangeException(nameof(isNullable),
                        $"Explicitly nullable type: Nullable<{type.Name}> cannot be coerced to a non nullable GraphQL type. \n");
                }
            }

            Type graphType = null;

            if (type.IsArray)
            {
                var clrElementType = type.GetElementType();
                var elementType = GetAutoGraphTypeFromType(clrElementType, isInputType, isNullable);
                graphType = typeof(ListGraphType<>).MakeGenericType(elementType);
            }
            else if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type) && !type.IsArray)
            {
                var clrElementType = GetEnumerableElementType(type);
                var elementType = GetAutoGraphTypeFromType(clrElementType, isInputType, isNullable);
                graphType = typeof(ListGraphType<>).MakeGenericType(elementType);
            }
            else
            {
                if (isInputType)
                {
                    InputTypeDictionary.TryGetValue(type, out graphType);
                }
                else
                {
                    TypeDictionary.TryGetValue(type, out graphType);
                }
            }

            if (graphType == null)
            {
                if (type.IsEnum)
                {
                    graphType = typeof(EnumerationGraphType<>).MakeGenericType(type);
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(type), $"The type: {type.Name} cannot be coerced effectively to a GraphQL type");
            }

            if (!isNullable)
            {
                graphType = typeof(NonNullGraphType<>).MakeGenericType(graphType);
            }

            return graphType;
        }

        private static readonly Type[] _untypedContainers = { typeof(IEnumerable), typeof(IList), typeof(ICollection) };

        private static readonly Type[] _typedContainers = { typeof(IEnumerable<>), typeof(List<>), typeof(IList<>), typeof(ICollection<>), typeof(IReadOnlyCollection<>) };

        private static Type GetEnumerableElementType(this Type type)
        {
            if (_untypedContainers.Contains(type)) return typeof(object);

            if (type.IsConstructedGenericType)
            {
                var definition = type.GetGenericTypeDefinition();
                if (_typedContainers.Contains(definition))
                {
                    return type.GenericTypeArguments[0];
                }
            }

            throw new ArgumentOutOfRangeException(nameof(type), $"The element type for {type.Name} cannot be coerced effectively");
        }
    }
}
