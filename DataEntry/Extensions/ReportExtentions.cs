using System;

namespace dataentry.Extensions
{
    public static class ReportExtentions
    {
        public static string ReportCellRowFormat(this string str) => ReplaceSring(str);

        private static string ReplaceSring(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            return str.Replace("|", Environment.NewLine);
        }
    }
}
