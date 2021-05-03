using System;
using System.Globalization;
using System.Windows.Data;

namespace Hsr.CloudSolutions.SmartKitchen.UI.Converters
{
    public class DoubleToTemperatureConverter 
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !double.TryParse(value.ToString(), out var degrees))
            {
                degrees = 0;
            }
            return $"{degrees:0.##} °C";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
