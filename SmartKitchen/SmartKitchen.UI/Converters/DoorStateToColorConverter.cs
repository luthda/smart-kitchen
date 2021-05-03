using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Hsr.CloudSolutions.SmartKitchen.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.UI.Converters
{
    public class DoorStateToColorConverter 
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value as DoorState?;
            return new SolidColorBrush(GetColorFor(state));
        }

        private Color GetColorFor(DoorState? state)
        {
            if (state.HasValue)
            {
                return state == DoorState.Open ? Colors.Red : Colors.Green;
            }
            return Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
