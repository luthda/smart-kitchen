using System.Linq;
using System.Windows;

namespace Hsr.CloudSolutions.SmartKitchen.UI.Templates
{
    public class DynamicDataTypeBasedTemplateSelector 
        : DataTemplateSelectorCollection
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!DataTemplates.Any())
            {
                return base.SelectTemplate(item, container);
            }
            if (item == null)
            {
                return base.SelectTemplate(null, container);
            }
            var template = DataTemplates
                .Where(t => t.DataType != null)
                .LastOrDefault(t => t.DataType.Equals(item.GetType()));

            if (template == null)
            {
                return base.SelectTemplate(item, container);
            }
            return template;
        }
    }
}
