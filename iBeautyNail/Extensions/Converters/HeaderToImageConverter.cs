using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace iBeautyNail.Extensions.Converters
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {

        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = (string)value;

            if (path == null)
            {
                return null;
            }
            var name = GetFileFolderName(path);
            //var icon = "file.ico";

            if (string.IsNullOrEmpty(name))
            {
                //icon = "drive.ico";
                return new BitmapImage(new Uri($"pack://siteoforigin:,,,/Resources/Images/drive.ico"));
            }
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
            {
                //icon = "folder.ico";
                return new BitmapImage(new Uri($"pack://siteoforigin:,,,/Resources/Images/folder.ico"));
            }
            else
            {
                BitmapImage tmpBM = new BitmapImage();

                // BitmapImage.UriSource must be in a BeginInit/EndInit block
                tmpBM.BeginInit();
                tmpBM.UriSource = new Uri(path);
                tmpBM.CacheOption = BitmapCacheOption.OnLoad;
                tmpBM.CreateOptions = BitmapCreateOptions.DelayCreation;
                // To save significant application memory, set the DecodePixelWidth or
                // DecodePixelHeight of the BitmapImage value of the image source to the desired
                // height or width of the rendered image. If you don't do this, the application will
                // cache the image as though it were rendered as its normal size rather then just
                // the size that is displayed.
                // Note: In order to preserve aspect ratio, set DecodePixelWidth
                // or DecodePixelHeight but not both.
                tmpBM.DecodePixelWidth = 80;
                tmpBM.EndInit();

                return tmpBM;
                //return new BitmapImage(new Uri(path));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string GetFileFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }


            var normalizedPath = path.Replace('/', '\\');
            var lastIndex = normalizedPath.LastIndexOf('\\');

            if (lastIndex <= 0)
            {
                return path;
            }

            return path.Substring(lastIndex + 1);
        }
    }
}
