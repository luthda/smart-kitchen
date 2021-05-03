using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Hsr.CloudSolutions.SmartKitchen.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.UI.Converters
{
    public class DoorStateToVisibilityConverter 
        : IValueConverter
    {
        public DoorStateToVisibilityConverter()
        {
            Inverted = false;
        }

        public bool Inverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value as DoorState? ?? DoorState.Closed;
            var trueState = Inverted ? DoorState.Closed : DoorState.Open;
            return state == trueState ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
