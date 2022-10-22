using dataentry.Data.DBContext;
using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Extensions
{
    public static class ListingExtensions
    {
        public static T GetListingData<T>(this Listing listing) where T : new() => 
            GetListingData(listing, typeof(T).Name).Deserialize<T>();
        public static T GetListingData<T>(this Listing listing, string language) where T : new() =>
            GetListingData(listing, typeof(T).Name, language).Deserialize<T>();
        public static IEnumerable<T> GetListingDataArray<T>(this Listing listing) where T : new() => 
            GetListingData(listing, typeof(T).Name).Deserialize<List<T>>();
        public static IEnumerable<T> GetListingDataAllLanguages<T>(this Listing listing) where T : LanguageField, new() => 
            GetListingDataAllLanguages(listing, typeof(T).Name).Select(Deserialize<T>);

        private static ListingData GetListingData(this Listing listing, string dataType)
        {
            if (listing is null)
            {
                throw new System.ArgumentNullException(nameof(listing));
            }
            var result = listing.ListingData?.FirstOrDefault(d => d.DataType == dataType) ?? new ListingData();
            result.Language = CleanLanguage(result.Language, listing.Region?.CultureCode);
            return result;
        }

        private static ListingData GetListingData(this Listing listing, string dataType, string language)
        {
            if (listing is null)
            {
                throw new System.ArgumentNullException(nameof(listing));
            }

            var result = listing.ListingData?.FirstOrDefault(d => d.DataType == dataType && d.Language == language) ?? new ListingData();
            result.Language = CleanLanguage(result.Language, listing.Region?.CultureCode);
            return result;
        }

        private static IEnumerable<ListingData> GetListingDataAllLanguages(this Listing listing, string dataType)
        {
            if (listing is null)
            {
                throw new System.ArgumentNullException(nameof(listing));
            }

            var result = listing.ListingData?.Where(d => d.DataType == dataType) ?? new List<ListingData>();
            foreach (var item in result)
            {
                item.Language = CleanLanguage(item.Language, listing.Region?.CultureCode);
            }
            return result;
        }

        public static T Deserialize<T>(this ListingData listingData) where T : new()
        {
            if (string.IsNullOrEmpty(listingData.Data) || listingData.Data.Equals("\"\""))
                return new T();

            var result = JsonConvert.DeserializeObject<T>(listingData.Data);
            if (result is LanguageField languageField)
            {
                languageField.CultureCode = listingData.Language;
            }

            return result;
        }

        public static string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static void SetListingData<T>(this Listing listing, T data) => 
            listing.SetListingData(typeof(T).Name, data);
        public static void SetListingDataArray<T>(this Listing listing, IEnumerable<T> data) => 
            listing.SetListingData(typeof(T).Name, data);
        public static void SetListingDataAllLanguages<T>(this Listing listing, IEnumerable<T> data) where T : LanguageField => 
            listing.SetListingDataAllLanguages(typeof(T).Name, data, false);
        public static void SetListingDataArrayAllLanguages<T>(this Listing listing, IEnumerable<T> data) where T : LanguageField =>
            listing.SetListingDataAllLanguages(typeof(T).Name, data, true);
        private static void SetListingData<T>(this Listing listing, string dataType, T data)
        {
            if (listing is null)
            {
                throw new System.ArgumentNullException(nameof(listing));
            }

            var listingData = listing.GetListingData(dataType);
            if (listingData.DataType == null)
            {
                listingData = new ListingData
                {
                    DataType = dataType
                };
                if (listing.ListingData == null)
                {
                    listing.ListingData = new List<ListingData>();
                }
                listing.ListingData.Add(listingData);
            }

            // Remove listing data if data is null
            if (data == null || data.Equals("null"))
            {
                if (listing.ListingData != null)
                {
                    listing.ListingData.Remove(listingData);
                }
            } else
            {
                listingData.Data = Serialize(data);
            }
        }

        private static void SetListingDataAllLanguages<T>(this Listing listing, string dataType, IEnumerable<T> data, bool allowMultipleValuesPerLanguage) where T : LanguageField
        {
            if (listing is null)
            {
                throw new System.ArgumentNullException(nameof(listing));
            }

            if (data == null) data = new List<T>();

            if (listing.ListingData == null)
            {
                listing.ListingData = new List<ListingData>();
            }

            var touchedData = new HashSet<ListingData>();
            foreach (var datum in data)
            {
                if (datum == null || datum.Equals("null")) continue;

                ListingData listingData = null;
                if (!allowMultipleValuesPerLanguage)
                {
                    listingData = listing.ListingData.FirstOrDefault(d => d.DataType == dataType && d.Language == datum.CultureCode);
                }
                 
                if (listingData == null)
                {
                    listingData = new ListingData
                    {
                        DataType = dataType,
                        Language = CleanLanguage(datum.CultureCode, listing.Region?.CultureCode)
                    };
                    listing.ListingData.Add(listingData);
                }

                touchedData.Add(listingData);
                listingData.Data = Serialize(datum);
            }

            foreach (var removedData in listing.ListingData.Where(d => d.DataType == dataType && !touchedData.Contains(d)).ToList())
            {
                listing.ListingData.Remove(removedData);
            }
        }

        public static string CleanLanguage(string value, string defaultValue = null) => string.IsNullOrWhiteSpace(value) ? defaultValue ?? Startup.Configuration?["DefaultRegion:CultureCode"] : value;

        public static async Task VerifyExternalID(this Listing listing, DataEntryContext context = null)
        {
            var listingsAndSpaces = (listing.Spaces ?? new List<Listing>()).Union(new List<Listing> { listing });
            if (listingsAndSpaces.Any(l => string.IsNullOrWhiteSpace(l.ExternalID) || string.IsNullOrWhiteSpace(l.PreviewID)))
            {
                if (listing.Region == null)
                {
                    if (context == null)
                    {
                        throw new InvalidOperationException("Listing does not have an external ID. Region information is required to generate an ID. DataEntryContext was not passed to this method to retrieve the region information");
                    }
                    await context.Entry(listing).Reference(l => l.Region).LoadAsync();
                    if (string.IsNullOrWhiteSpace(listing.Region.ListingPrefix))
                    {
                        throw new InvalidOperationException($"The region with ID {listing.RegionID} has an invalid value for ListingPrefix");
                    }
                    if (string.IsNullOrWhiteSpace(listing.Region.PreviewPrefix))
                    {
                        throw new InvalidOperationException($"The region with ID {listing.RegionID} has an invalid value for PreviewPrefix");
                    }
                }
            }

            listingsAndSpaces = SetExternalIdAndPreviewId(listingsAndSpaces, listing).ToList();
        }

        private static IEnumerable<Listing> SetExternalIdAndPreviewId(IEnumerable<Listing> listingsAndSpaces, Listing listing)
        {
            foreach (var item in listingsAndSpaces)
            {
                if (item.ID != 0) 
                {
                    if (string.IsNullOrWhiteSpace(item.ExternalID))
                    {
                        item.ExternalID = $"{listing.Region.ListingPrefix}-{item.ID}";
                    }
                    if (string.IsNullOrWhiteSpace(item.PreviewID))
                    {
                        item.PreviewID = $"{listing.Region.PreviewPrefix}-{item.ID}";
                    }
                }
            }

            return listingsAndSpaces;
        }

        public static IEnumerable<IMedia> GetMedia(this Listing listing) 
        {
            var result = new List<IMedia>();
            if (listing.ListingImage != null) {
                result.AddRange(listing.ListingImage.Where(i => i != null));
            }
            
            var brochures = listing.GetListingDataArray<Brochure>();
            if (brochures != null) {
                result.AddRange(brochures.Where(b => b != null));
            }

            var epcGraphs = listing.GetListingDataArray<EpcGraph>();
            if (epcGraphs != null) {
                result.AddRange(epcGraphs.Where(g => g != null));
            }

            return result;
        }

        public static IEnumerable<AspectsEnum> ExpandListingType(this AspectsEnum value)
        {
            return value switch
            {
                AspectsEnum.sale        => new[] {AspectsEnum.sale},
                AspectsEnum.lease       => new[] {AspectsEnum.lease},
                AspectsEnum.salelease   => new[] {AspectsEnum.sale,AspectsEnum.lease},
                AspectsEnum.investment  => new[] {AspectsEnum.sale,AspectsEnum.investment},
                _                       => new AspectsEnum[0]
            };
        }
    }
}
