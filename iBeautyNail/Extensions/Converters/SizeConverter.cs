using System;
using System.Globalization;
using System.Windows.Data;

namespace iBeautyNail.Extensions.Converters
{
    public class SizeConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            double _actualValue;
            double _paramValue;

            if (double.TryParse(value.ToString(), out _actualValue))
            {
                if (double.TryParse(parameter.ToString(), out _paramValue))
                    return _actualValue * _paramValue;
                else
                    return _actualValue * 2;
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
