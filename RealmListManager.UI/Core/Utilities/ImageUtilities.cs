using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RealmListManager.UI.Core.Utilities
{
    public static class ImageUtilities
    {
        /// <summary>
        /// Deserializes image data into a bitmap
        /// </summary>
        /// <param name="imageData">Image Data</param>
        /// <returns>Bitmap</returns>
        public static BitmapImage Deserialize(byte[] imageData)
        {
            //return (BitmapImage)new ImageSourceConverter().ConvertFrom(imageData);

            using (var stream = new MemoryStream(imageData))
            {
                var result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                return result;
            }
        }

        public static byte[] Serialize(ImageSource imageSource, BitmapEncoder encoder = null)
        {
            byte[] result;
            var bitmap = imageSource as BitmapSource;
            if (bitmap == null) return null;
            if (encoder == null)
                encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                result = stream.ToArray();
            }

            return result;
        }

        /// <summary>
        /// Loads an image file, or extracts the first icon from an executable.
        /// </summary>
        /// <param name="imagePath">File path</param>
        /// <returns>ImageSource</returns>
        public static BitmapImage ParseImageFile(string imagePath)
        {
            var extension = System.IO.Path.GetExtension(imagePath)?.ToLower();
            if (extension == null) return null;

            Image image;
            if (extension.Contains("exe"))
            {
                var icon = Icon.ExtractAssociatedIcon(imagePath);
                if (icon == null) return null;
                image = icon.ToBitmap();
            }
            else
            {
                image = System.Drawing.Image.FromFile(imagePath);
            }

            return ImageToSource(image);
        }

        /// <summary>
        /// Creates a bitmap from the specified Image
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns>Bitmap</returns>
        public static BitmapImage ImageToSource(Image image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                stream.Position = 0;

                var imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.StreamSource = stream;
                imageSource.EndInit();
                return imageSource;
            }
        }
    }
}
