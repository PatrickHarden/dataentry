using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace dataentry.Extensions
{
    public static class MemoryStreamExtensions
    {
        public static MemoryStream ImageResizeHandler(this MemoryStream filestream, string extension)
        {
            // only attempt to rotate jpgg/png images images
            if (extension.ToLower() == ".jpeg" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png"){
                // convert to byte array
                byte[] imageBytes = filestream.ToArray();

                // set up empty stream for new payload
                var tempStream = new MemoryStream();

                // rotate image and save it to new filestream
                using (var image = Image.Load(imageBytes, out var imageFormat))
                {
                    
                    image.Mutate(x => x.AutoOrient());
                    double resolution = image.Metadata.HorizontalResolution;
                    if ((image.Height > 1200) || (image.Width > 1600))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        { Mode = ResizeMode.Max, Size = new Size(1600, 1200)})
                        );
                    }                   
                    image.Save(tempStream, imageFormat);
                }

                // reset position and return new filestream
                tempStream.Position = 0;
                return tempStream;
            } else {
                // return original filestream if it's not a jpg/png
                return filestream;
            }
        }
    }
}
