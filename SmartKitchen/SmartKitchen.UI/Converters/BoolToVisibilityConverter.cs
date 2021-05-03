using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hsr.CloudSolutions.SmartKitchen.UI.Converters
{
    public class BoolToVisibilityConverter 
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Hidden;
            }
            bool.TryParse(value.ToString(), out var boolValue);
            return boolValue ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
