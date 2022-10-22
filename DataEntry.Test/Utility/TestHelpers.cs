using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dataentry.Extensions;
using dataentry.Utility;

namespace dataentry.Test.Utility
{
    public static class TestHelpers
    {
        public static IEnumerable<object[]> EnumToTestData<T>() where T : Enum
        {
            foreach (var value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                yield return new object[] { value.ToString() };
            }
        }

        public static IEnumerable<object[]> AliasToTestData<T>(AliasType aliasType1, AliasType aliasType2) where T : struct, Enum
        {
            EnumAliasExtensions.Init<T>();
            foreach (var value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                yield return new object[] { value.ToAlias(aliasType1), value.ToAlias(aliasType2) };
            }
        }

        public static IEnumerable<object[]> AliasToTestData<T>(AliasType aliasType) where T : struct, Enum
        {
            EnumAliasExtensions.Init<T>();
            foreach (var value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                yield return new object[] { value.ToString(), value.ToAlias(aliasType) };
            }
        }
    }
}
