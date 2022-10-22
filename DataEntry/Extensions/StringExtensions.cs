using System;

namespace dataentry.Extensions
{
    public static class StringExtensions
    {
        public static string TrimEnd(this string str, string value) => TrimIndex(str, str.LastIndexOf(value));
        public static string TrimEnd(this string str, string value, StringComparison comparisonType) => TrimIndex(str, str.LastIndexOf(value, comparisonType));

        private static string TrimIndex(string str, int index)
        {
            if (index == -1) return str;
            return str.Substring(0, index);
        }
    }
}