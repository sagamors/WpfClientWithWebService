using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using Tuner.Wpf;
using WpfBindingErrors;
using WpfClient.ViewModels;
using WpfClient.Windows;


namespace WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Bootstrapper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if DEBUG
            BindingExceptionThrower.Attach();
#endif
            var logon = Bootstrapper.Container.Get<LogonViewModel>();
            logon.ShowDialog(null);
        }
    }
}
