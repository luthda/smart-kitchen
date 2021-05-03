using System;
using System.Globalization;
using System.Windows.Data;
using Hsr.CloudSolutions.SmartKitchen.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.UI.Converters
{
    public class DoorStateToBoolConverter 
        : IValueConverter
    {
        public bool Invert { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            var state = value as DoorState? ?? DoorState.Closed;
            if (Invert)
            {
                return state != DoorState.Open;
            }
            return state == DoorState.Open;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DoorState.Closed;
            }
            var boolValue = value as bool? ?? false;
            if (Invert)
            {
                boolValue = !boolValue;
            }
            return boolValue ? DoorState.Open : DoorState.Closed;
        }
    }
}
