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
    public class DesignConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string fileName = $"{value}";
            string filePath = Path.Combine(SystemPath.Designs, fileName);

            if (File.Exists(filePath))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.DelayCreation;
                image.UriSource = new Uri(filePath);
                image.EndInit();
                //return new BitmapImage(new Uri(filePath));
                return image;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
