using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RestProtocol;

namespace WpfClient.Helpers
{
    public class ReportTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MessagesFromObjectDataTemplate { get; set; }
        public DataTemplate TrafficAndParkingDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MessagesFromObject)
            {
                return MessagesFromObjectDataTemplate;
            }
            if (item is TrafficAndParking)
            {
                return TrafficAndParkingDataTemplate;
            }
            return null;
        }
    }
}
