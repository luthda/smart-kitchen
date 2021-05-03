using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hsr.CloudSolutions.SmartKitchen.UI.Controls
{
    /// <summary>
    /// Interaction logic for LabeledControl.xaml
    /// </summary>
    [ContentProperty(nameof(ControlContent))]
    public partial class LabeledControl : UserControl
    {
        public LabeledControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label", typeof (string), typeof (LabeledControl), new PropertyMetadata(default(string)));

        public string Label
        {
            get => (string) GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly DependencyProperty ControlContentProperty = DependencyProperty.Register(
            "ControlContent", typeof (object), typeof (LabeledControl), new PropertyMetadata(default(object)));

        public object ControlContent
        {
            get => GetValue(ControlContentProperty);
            set => SetValue(ControlContentProperty, value);
        }
    }
}
