using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp
{
    public class PropertyMapperOptions
    {

        public enum AmenitiesSourceEnum
        {
            FromProperty,
            FromAvailabilities,
            FromPropertyPropertyAmenity
        }

        public Dictionary<string, string> AmenitiesSource { get; set; }

        public AmenitiesSourceEnum AmenitiesSourceForSite(string homeSiteId)
        {
            string resultString;
            AmenitiesSourceEnum result = AmenitiesSourceEnum.FromProperty;

            if (AmenitiesSource != null)
            {
                if (!string.IsNullOrWhiteSpace(homeSiteId))
                {
                    if (AmenitiesSource.TryGetValue(homeSiteId, out resultString))
                    {
                        if (Enum.TryParse(resultString, false, out result))
                        {
                            return result;
                        }
                    }
                }
                if (AmenitiesSource.TryGetValue("default", out resultString))
                {
                    if (Enum.TryParse(resultString, false, out result))
                    {
                        return result;
                    }
                }
            }

            return result;
        }
    }
}