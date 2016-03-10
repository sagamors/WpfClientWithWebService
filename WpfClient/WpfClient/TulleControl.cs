using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClient
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfClient"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfClient;assembly=WpfClient"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:TulleControl/>
    ///
    /// </summary>
    public class TulleControl : ContentControl
    {
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register(
            "IsShow", typeof (bool), typeof (TulleControl), new PropertyMetadata(default(bool)));

        public bool IsShow
        {
            get { return (bool) GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        public static readonly DependencyProperty TulleContentTemplateProperty = DependencyProperty.Register(
            "TulleContentTemplate", typeof (DataTemplate), typeof (TulleControl), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate TulleContentTemplate
        {
            get { return (DataTemplate) GetValue(TulleContentTemplateProperty); }
            set { SetValue(TulleContentTemplateProperty, value); }
        }

        static TulleControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TulleControl), new FrameworkPropertyMetadata(typeof(TulleControl)));
        }
    }
}
