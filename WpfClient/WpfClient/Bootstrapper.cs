using System;
using System.ComponentModel;
using Ninject;
using Ninject.Activation;
using WpfClient;
using WpfClient.Core;
using WpfClient.ViewModels;
using WpfClient.Windows;

namespace Tuner.Wpf
{
    public static class Bootstrapper
    {
        public static IKernel Container { private set; get; }

        public static void Initialize()
        {
            Container = new StandardKernel();
            Container.Bind<IDialogView>().To<LogonWindow>().When(ParentIsType<LogonViewModel>);
            Container.Bind<IDialogView>().To<ReportsWindow>().When(ParentIsType<ReportsViewModel>);
            Container.Bind<IDialogView>().To<AddNewReportWindow>().When(ParentIsType<AddNewReportViewModel>);
            Container.Bind<IMessageBoxService>().To<MessageBoxService>();
        }

        private static bool ParentIsType<T>(IRequest request)
        {
            return request.ParentRequest.Service.UnderlyingSystemType == typeof (T);
        }
    }
}
