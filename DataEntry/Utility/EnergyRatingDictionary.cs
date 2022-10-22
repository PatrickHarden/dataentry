using System.Collections.Generic;
using System.Linq;

namespace dataentry.Utility
{
    public static class EnergyRatingDictionary
    {
        private static readonly Dictionary<string, string> denmarkEPCRatings = new CaseInsensitiveDictionary<string>
        {
            ["A"] = "A",
            ["A+"] = "A2010",
            ["A++"] = "A2015",
            ["A+++"] = "A2020"
        };

        private static readonly Dictionary<string, string> italyEPCRatings = new CaseInsensitiveDictionary<string>
        {
            ["A"] = "A1",
            ["A+"] = "A2",
            ["A++"] = "A3",
            ["A+++"] = "A4"
        };

        private static readonly Dictionary<string, string> commonEPCRatings = new CaseInsensitiveDictionary<string>
        {
            ["B"] = "B",
            ["C"] = "C",
            ["D"] = "D",
            ["E"] = "E",
            ["F"] = "F",
            ["G"] = "G",
        };

        public static Dictionary<string, string> GetEPCRatings(string countryCode)
        {
            switch (countryCode)
            {
                case "IT": return italyEPCRatings.Concat(commonEPCRatings).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                case "DK": return denmarkEPCRatings.Concat(commonEPCRatings).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                default: return null;
            }

        }
    }
}
