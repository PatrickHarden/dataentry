using System;
using System.Data;

namespace dataentry.Extensions
{
    public static class DataRowExtensions
    {
        public static T GetFieldIfExist<T>(this DataRow dataRow, string columnName)
        {
            if (!dataRow.Table.Columns.Contains(columnName))
                return default;

            if (dataRow.IsNull(columnName))
                return default;

            var result = dataRow[columnName];
            if (typeof(T) == typeof(string) && !(result is string))
            {
                var stringResult = result.ToString();
                if (!stringResult.StartsWith("[")) return (T)(object)stringResult;
            }

            return (T)result;
        }
    }
}
