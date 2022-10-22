using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using dataentry.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace dataentry.Extensions
{
    public static class ListingAdapterExtensions
    {

        public static string RefactorString(this string input, bool nullAllowed = false)
        {
            return string.IsNullOrEmpty(input) ? nullAllowed ? null : " " : input;
        }

        public static string CleanPhoneNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return " ";
            }
            input = input.Replace("_", "").Trim();
            return RefactorString(input);
        }

        public static string ReturnValueOrNull(this string input) => string.IsNullOrEmpty(input) ? null : input;

        public static string ConvertToString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
        
    public static List<T> JoinTwoLists<T>(this List<T> first, List<T> second)
        {
            if (first == null) {
                return second;
            }
            if (second == null) {
                return first;
            }
            return first.Concat(second).ToList();
        }
    }
}
