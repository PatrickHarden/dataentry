using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using dataentry.AutoGraph.Attributes;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace dataentry.AutoGraph
{
    public static class IServiceCollectionExtensions
    {
        private static HashSet<Type> EnumTypes = new HashSet<Type>();

        public static void AddAllGraphTypes(this IServiceCollection services, Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                Type autoType = null;

                var attribute = type.GetCustomAttributes(typeof(AutoObjectGraphTypeAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    var name = ((AutoObjectGraphTypeAttribute) attribute).Name;
                    services.AddAutoGraphType(type, typeof(AutoObjectGraphType<>).MakeGenericType(type), false, name);
                }

                attribute = type.GetCustomAttributes(typeof(AutoInputObjectGraphTypeAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    var name = ((AutoInputObjectGraphTypeAttribute) attribute).Name;
                    services.AddAutoGraphType(type, typeof(AutoInputObjectGraphType<>).MakeGenericType(type), true, name);
                }

                if ((autoType = type.GetNearestOpenGeneric(typeof(AutoObjectGraphType<>))) != null)
                {
                    services.AddAutoGraphType(autoType.GenericTypeArguments[0], type, false);
                }
                else if ((autoType = type.GetNearestOpenGeneric(typeof(AutoInputObjectGraphType<>))) != null)
                {
                    services.AddAutoGraphType(autoType.GenericTypeArguments[0], type, true);
                }
                else if (typeof(IGraphType).IsAssignableFrom(type))
                {
                    services.AddSingleton(type);
                }
            }
            foreach (var type in EnumTypes) {
                services.AddSingleton(typeof(EnumerationGraphType<>).MakeGenericType(type));
            }
        }

        private static void AddAutoGraphType(this IServiceCollection services, Type viewModelType, Type autoType, bool isInputType, string name = null)
        {
            ComplexGraphTypeExtensions.RegisterType(viewModelType, autoType, isInputType);
            services.AddSingleton(autoType, serviceProvider =>
            {
                var instance = (IComplexGraphType) ActivatorUtilities.CreateInstance(serviceProvider, autoType);
                if (name != null) instance.Name = name;
                return instance;
            });

            foreach (var property in viewModelType.GetProperties())
            {
                var type = property.PropertyType;
                while (true)
                {
                    if (type.IsAssignableFrom(typeof(IEnumerable<>)))
                    {
                        while (type != typeof(IEnumerable<>))
                        {
                            type = type.BaseType;
                        }
                    }
                    if (type == typeof(Nullable<>) || type.IsAssignableFrom(typeof(IEnumerable<>)))
                    {
                        type = type.GenericTypeArguments[0];
                    }
                    else
                    {
                        break;
                    }
                }
                if (type.IsEnum)
                {
                    EnumTypes.Add(type);
                }
            }
        }
    }
}