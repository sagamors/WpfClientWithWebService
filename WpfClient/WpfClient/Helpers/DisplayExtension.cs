using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WpfClient.Helpers
{
    public class DisplayExtension : MarkupExtension
    {
        public Type Type { get; set; }

        public string PropertyName { get; set; }

        public DisplayExtension() { }
        public DisplayExtension(string propertyName)
        {
            PropertyName = propertyName;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // (This code has zero tolerance)
            var prop = Type.GetProperty(PropertyName);
            var attributes = prop.GetCustomAttributes(typeof(DisplayAttribute), false);
            return (attributes[0] as DisplayAttribute);
        }
    }
}
