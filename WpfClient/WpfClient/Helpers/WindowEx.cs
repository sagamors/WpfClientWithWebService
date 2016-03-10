using System.Windows;

namespace WpfClient.Helpers
{
    public static class WindowEx
    {
        public static readonly DependencyProperty WindowStartupLocationProperty;

        public static void SetWindowStartupLocation(DependencyObject o, WindowStartupLocation value)
        {
            o.SetValue(WindowStartupLocationProperty, value);
        }

        public static WindowStartupLocation GetWindowStartupLocation(DependencyObject o)
        {
            return (WindowStartupLocation)o.GetValue(WindowStartupLocationProperty);
        }

        static WindowEx()
        {
            WindowStartupLocationProperty = DependencyProperty.RegisterAttached("WindowStartupLocation",
                                                      typeof(WindowStartupLocation),
                                                      typeof(WindowEx),
                                                      new UIPropertyMetadata(WindowStartupLocation.Manual, OnWindowStartupLocationChanged));
        }

        private static void OnWindowStartupLocationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var window = sender as Window;
            if (window != null)
            {
                window.WindowStartupLocation = GetWindowStartupLocation(window);
            }
        }
    }
}
