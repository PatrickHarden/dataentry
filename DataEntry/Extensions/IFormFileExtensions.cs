using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace dataentry.Extensions
{
    public static class IFormFileExtensions
    {
        public static bool IsValidImageFormat(this IFormFile file, string fileFormats)
        {
            return fileFormats.Split(',').Any(e => e == file.ContentType);
        }

        public static string ValidateImage(this IFormFile file, string fileFormats)
        {
            string ValidationError = string.Empty;

            if (file == null)
            {
                ValidationError = "Image file required";
            }
            else if(!file.IsValidImageFormat(fileFormats))
            {
                ValidationError = "Invalid image file format";
            }
            else if(file.Length == 0)
            {
                ValidationError = "Upload image with file size greater than zero";
            }

            return ValidationError;
        }
    }
}
