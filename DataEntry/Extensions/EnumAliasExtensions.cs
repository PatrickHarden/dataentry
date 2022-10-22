using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using dataentry.Utility;

namespace dataentry.Extensions
{
    public static class EnumAliasExtensions
    {
        private static Dictionary<string, Dictionary<string, List<Enum>>> _lookupTables = new Dictionary<string, Dictionary<string, List<Enum>>>();
        private static Dictionary<string, Dictionary<Enum, string>> _defaultAliasTables = new Dictionary<string, Dictionary<Enum, string>>();

        /// <summary>
        /// Initialize all public enum types in an assembly
        /// </summary>
        /// <param name="assembly">The assembly to search in</param>
        public static void Init(Assembly assembly, string ns = "")
        {
            var enumQuery = assembly
                .GetTypes()
                .Where(t => t.IsEnum && t.IsPublic && t.Namespace.StartsWith(ns));
            foreach (var enumType in enumQuery)
            {
                Init(enumType);
            }
        }

        /// <inheritdoc cref="Init(Type)"/>
        /// <typeparam name="T">The type to initialize</typeparam>
        public static void Init<T>() => Init(typeof(T));

        /// <summary>
        /// Initialize alias lookups for a single type
        /// </summary>
        /// <param name="enumType">The type to initialize</param>
        public static void Init(Type enumType)
        {
            foreach (var enumField in enumType.GetFields().Where(f => f.IsLiteral))
            {
                Init(enumType, enumField);
            }
        }

        /// <summary>
        /// Initialize alias lookups for a single <see cref="FieldInfo"/>
        /// </summary>
        /// <param name="enumField">The field to initialize. This field should be an enum.</param>
        public static void Init(Type enumType, FieldInfo enumField)
        {
            var enumValue = enumField.GetValue(null);
            foreach (var enumAttribute in enumField.GetCustomAttributes<AliasAttribute>())
            {
                if (!enumAttribute.Matches.Any()) continue;

                var key = GetAliasKey(enumType, enumAttribute.AliasType);

                if (!_defaultAliasTables.TryGetValue(key, out var defaultAliasTable))
                {
                    defaultAliasTable = new Dictionary<Enum, string>();
                    _defaultAliasTables[key] = defaultAliasTable;
                }
                defaultAliasTable[(Enum) enumValue] = enumAttribute.Matches[0];

                if (!_lookupTables.TryGetValue(key, out var lookupTable))
                {
                    lookupTable = new CaseInsensitiveDictionary<List<Enum>>();
                    _lookupTables[key] = lookupTable;
                }
                foreach (var match in new [] { enumValue.ToString() }.Concat(enumAttribute.Matches))
                {
                    if (!lookupTable.TryGetValue(match, out List<Enum> values))
                    {
                        lookupTable[match] = values = new List<Enum>();
                    }
                    if (enumAttribute.Primary)
                    {
                        
                        values.Insert(0, (Enum)enumValue);
                    }
                    else
                    {
                        values.Add((Enum)enumValue);
                    }
                }
            }
        }

/// <summary>
        /// Look up the enum of type <typeparamref name="T"/> for a given alias.
        /// </summary>
        /// <typeparam name="T"/> An enum type </typeparam>
        /// <param name="alias">The alias string defined with <see cref="AliasAttribute"/>.</param>
        /// <param name="aliasType">Which <see cref="AliasType"/> to look up.</param>
        /// <returns>The enum of type <typeparamref name="T"/> associated with the given alias, or null if the alias is not found.</returns>
        public static T? ToEnum<T>(this string alias, AliasType? aliasType = null) where T : struct, Enum => (T?) ToEnum(typeof(T), alias, aliasType);
        
        /// <summary>
        /// Returns the default alias for a given enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"/> An enum type </typeparam>
        /// <param name="enumValue">The enum value of type <typeparamref name="T"/> to look up.</param>
        /// <param name="aliasType">The <see cref="AliasType"/> to look up.</param>
        /// <returns>
        /// The default alias mapped to the given enum value. 
        /// If one does not exist, returns the result of <see cref="T.ToString()"/>.
        /// </returns>
        public static string ToAlias<T>(this T? enumValue, AliasType? aliasType = null) where T : struct, Enum => ToAlias(typeof(T), (Enum) enumValue, aliasType);

        /// <summary>
        /// Returns the default alias for a given enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"/> An enum type </typeparam>
        /// <param name="enumValue">The enum value of type <typeparamref name="T"/> to look up.</param>
        /// <param name="aliasType">The <see cref="AliasType"/> to look up.</param>
        /// <returns>
        /// The default alias mapped to the given enum value. 
        /// If one does not exist, returns the result of <see cref="T.ToString()"/>.
        /// </returns>
        public static string ToAlias<T>(this T enumValue, AliasType? aliasType = null) where T : struct, Enum => ToAlias(typeof(T), (Enum) enumValue, aliasType);
        
        public static IEnumerable<string> ToAliases<T>(this T enumValue, AliasType? aliasType = null) where T : struct, Enum => ToAliases(typeof(T), (Enum) enumValue, aliasType);

        private static Enum ToEnum(Type enumType, string alias, AliasType? aliasType)
        {
            if (alias == null) return null;

            if (_lookupTables.TryGetValue(GetAliasKey(enumType, aliasType), out var lookupTable))
            {
                if (lookupTable.TryGetValue(alias.Trim(), out var result))
                {
                    return result.FirstOrDefault();
                }
            }

            {
                if (Enum.TryParse(enumType, alias, true, out var result))
                {
                    return result as Enum;
                }
            }

            return null;
        }

        private static string ToAlias(Type enumType, Enum enumValue, AliasType? aliasType)
        {
            if (enumValue == null) return null;

            if (_defaultAliasTables.TryGetValue(GetAliasKey(enumType, aliasType), out var defaultAliasTable))
            {
                if (defaultAliasTable.TryGetValue(enumValue, out var result))
                {
                    return result;
                }
            }

            return enumValue.ToString();
        }
        
        private static IEnumerable<string> ToAliases(Type enumType, Enum enumValue, AliasType? aliasType)
        {
            if (enumValue == null) return new List<string>();

            if (_lookupTables.TryGetValue(GetAliasKey(enumType, aliasType), out var lookupTable))
            {
                return lookupTable
                    .Where(a => a.Value.Select(v => v.ToString()).Contains(enumValue.ToString()))
                    .Select(a => a.Key);
            }

            return new List<string>();
        }

        private static string GetAliasKey(Type enumType, AliasType? aliasType) => $"{enumType?.FullName ?? ""}::{(aliasType ?? AliasType.Default).ToString()}";
    }
}