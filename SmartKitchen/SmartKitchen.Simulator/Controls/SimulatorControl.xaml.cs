using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI.Templates;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Controls
{
    /// <summary>
    /// Interaction logic for SimulatorControl.xaml
    /// </summary>
    [ContentProperty(nameof(DeviceTemplates))]
    public partial class SimulatorControl : UserControl
    {
        public SimulatorControl()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            SizeChanged += OnSizeChanged;
        }

        public DynamicDataTypeBasedTemplateSelector DeviceTemplates { get; set; } = new DynamicDataTypeBasedTemplateSelector();

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!HasDevices)
            {
                return;
            }
            SimArea.Children.Clear();
            var devices = Devices;
            if (DeviceTemplates?.DataTemplates == null)
            {
                return;
            }
            foreach (var device in devices)
            {
                var template = DeviceTemplates.SelectTemplate(device.Device, this);
                if (template == null)
                {
                    continue;
                }
                var control = template.LoadContent();
                var element = control as FrameworkElement;
                if (element == null)
                {
                    return;
                }
                element.DataContext = device;
                
                ChangeSize(device, element);

                SimArea.Children.Add(element);
            }
        }

        private void ChangeSize(ISimulatorDevice device, FrameworkElement deviceControl)
        {
            Canvas.SetLeft(deviceControl, device.Coordinate.X * BackgroundImage.ActualWidth + WidthOffset);
            Canvas.SetTop(deviceControl, device.Coordinate.Y * BackgroundImage.ActualHeight + HeightOffset);

            deviceControl.Width = device.Size.X * BackgroundImage.ActualWidth;
            deviceControl.Height = device.Size.Y * BackgroundImage.ActualHeight;
        }

        private double WidthOffset => (BackgroundGrid.ActualWidth - BackgroundImage.ActualWidth)/2;
        private double HeightOffset => (BackgroundGrid.ActualHeight - BackgroundImage.ActualHeight)/2;

        private bool HasDevices => DataContext is ISimulatorDevices;

        private IEnumerable<ISimulatorDevice> Devices
        {
            get
            {
                var devices = DataContext as ISimulatorDevices;
                if (devices == null)
                {
                    return new List<ISimulatorDevice>();
                }
                return devices;
            }
        } 

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var child in SimArea.Children.OfType<FrameworkElement>())
            {
                var device = child.DataContext as ISimulatorDevice;
                if (device == null)
                {
                    continue;
                }
                ChangeSize(device, child);
            }
        }

        public static readonly DependencyProperty SimulatorBackgroundProperty = DependencyProperty.Register(
            "SimulatorBackground", typeof (ImageSource), typeof (SimulatorControl), new PropertyMetadata(default(ImageSource)));

        public ImageSource SimulatorBackground
        {
            get => (ImageSource) GetValue(SimulatorBackgroundProperty);
            set => SetValue(SimulatorBackgroundProperty, value);
        }
    }
}
