using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace dataentry.Utility
{
    public static class GlobalizationUtility
    {
        public class CurrencyData
        {
            public string Name { get; internal set; }
            public string NativeName { get; internal set; }
            public string Code { get; internal set; }
            public string Symbol { get; internal set; }
            public CurrencyData(string name, string nativeName, string code, string symbol)
            {
                Name = name;
                NativeName = nativeName;
                Code = code;
                Symbol = symbol;
            }
        }
        private static IDictionary<string, CurrencyData> currencyByCurrencyCode;
        private static IDictionary<string, CurrencyData> currencyByCurrencyName;
        private static IDictionary<string, CurrencyData> currencyByRegionCode;
        private static IDictionary<string, RegionInfo> regionInfoByCultureCode;
        private static IDictionary<string, RegionInfo> regionInfoByName;
        private static IDictionary<string, CultureInfo> cultureInfoByLanguageName;

        public static IReadOnlyDictionary<string, CurrencyData> CurrencyByRegionCode =>
            (IReadOnlyDictionary<string, CurrencyData>) currencyByRegionCode;

        static GlobalizationUtility()
        {
            var cultureInfo = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return (culture, region: new RegionInfo(culture.Name));
                    }
                    catch
                    {
                        return (culture, null);
                    }
                })
                .Where(r => r.region != null);

            var regionInfo = cultureInfo
                .GroupBy(c => c.region.TwoLetterISORegionName, StringComparer.OrdinalIgnoreCase)
                .Select(c => c.First().region)
                .ToList();

            currencyByCurrencyCode = regionInfo
                .GroupBy(r => r.ISOCurrencySymbol, StringComparer.OrdinalIgnoreCase)
                .Select(r => r.First())
                .ToDictionary(
                    r => r.ISOCurrencySymbol,
                    r => new CurrencyData(
                        r.CurrencyEnglishName,
                        r.CurrencyNativeName,
                        r.ISOCurrencySymbol,
                        r.CurrencySymbol.ToUpper()),
                    StringComparer.OrdinalIgnoreCase);

            currencyByCurrencyName = currencyByCurrencyCode.Values
                .GroupBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
                .Concat(currencyByCurrencyCode.Values
                    .GroupBy(c => c.NativeName, StringComparer.OrdinalIgnoreCase))
                .GroupBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, c => c.First().First(), StringComparer.OrdinalIgnoreCase);

            currencyByRegionCode = regionInfo
                .ToDictionary(
                    r => r.TwoLetterISORegionName,
                    r => currencyByCurrencyCode[r.ISOCurrencySymbol],
                    StringComparer.OrdinalIgnoreCase);

            regionInfoByCultureCode = cultureInfo
                .GroupBy(c => $"{c.culture.TwoLetterISOLanguageName}-{c.region.TwoLetterISORegionName}", StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    c => c.Key,
                    c => c.First().region,
                    StringComparer.OrdinalIgnoreCase);

            
            regionInfoByName = regionInfo.GroupBy(c => c.TwoLetterISORegionName, StringComparer.OrdinalIgnoreCase) // example: IT
                .Concat(regionInfo.GroupBy(c => c.NativeName, StringComparer.OrdinalIgnoreCase)) // example: Italia
                .Concat(regionInfo.GroupBy(c => c.EnglishName, StringComparer.OrdinalIgnoreCase)) // example: Italy
                .GroupBy(c => c.Key, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    r => r.Key,
                    r => r.First().First(),
                    StringComparer.OrdinalIgnoreCase
                );

            var neutralCultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            cultureInfoByLanguageName = neutralCultures.GroupBy(c => c.TwoLetterISOLanguageName, StringComparer.OrdinalIgnoreCase) // example: it
                .Concat(neutralCultures.GroupBy(c => c.NativeName, StringComparer.OrdinalIgnoreCase)) // example: italiano
                .Concat(neutralCultures.GroupBy(c => c.EnglishName, StringComparer.OrdinalIgnoreCase)) // example: Italian
                .GroupBy(c => c.Key, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    c => c.Key,
                    c => c.First().First(),
                    StringComparer.OrdinalIgnoreCase
                );

            // ISO name is "Pound Sterling", but saw this in MIQ UAT
            if (currencyByRegionCode.TryGetValue("GB", out var gb)) currencyByCurrencyName.TryAdd("Great Britain Pound", gb);

            // Override Singapore Dollar symbol
            if (currencyByCurrencyCode.TryGetValue("SGD", out var sgd)) sgd.Symbol = "S$";

            // Override Euros for Poland (Might change? Existing data was using Euros)
            if (currencyByCurrencyCode.TryGetValue("EUR", out var eur)) currencyByRegionCode["PL"] = eur;

        }

        public static bool TryGetCurrencyDataFromCode(
            string currencyCode,
            out CurrencyData currencyData)
        {
            if (currencyCode == null)
            {
                currencyData = null;
                return false;
            }
            return currencyByCurrencyCode.TryGetValue(currencyCode, out currencyData);
        }

        public static bool TryGetCurrencyDataFromName(
            string currencyName,
            out CurrencyData currencyData)
        {
            if (currencyName == null)
            {
                currencyData = null;
                return false;
            }
            return currencyByCurrencyName.TryGetValue(currencyName, out currencyData);
        }

        public static bool TryGetCurrencyDataFromRegionCode(
            string regionCode,
            out CurrencyData currencyData)
        {
            if (regionCode == null)
            {
                currencyData = null;
                return false;
            }
            return currencyByRegionCode.TryGetValue(regionCode, out currencyData);
        }

        public static bool TryGetCurrencyDataFromCultureCode(
            string cultureCode,
            out CurrencyData currencyData)
        {
            currencyData = null;
            return regionInfoByCultureCode.TryGetValue(cultureCode, out var regionInfo) &&
                TryGetCurrencyDataFromRegionCode(regionInfo.TwoLetterISORegionName, out currencyData);
        }

        public static CultureInfo CultureInfoFromLanguageName(string cultureName) {
            cultureInfoByLanguageName.TryGetValue(cultureName, out var cultureInfo);
            return cultureInfo;
        }

        public static RegionInfo RegionInfoFromName(string regionName) {
            if (regionInfoByName.TryGetValue(regionName, out var regionInfo)) return regionInfo;
            return null;
        }
        
        public static string FormatCultureCode(CultureInfo culture, RegionInfo region) {
            if (culture == null && region == null) return null;
            if (culture == null) return region.TwoLetterISORegionName;
            if (region == null) return culture.TwoLetterISOLanguageName;
            return $"{culture.TwoLetterISOLanguageName}-{region.TwoLetterISORegionName}";
        }
    }
}