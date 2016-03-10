using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using Ninject;
using Ninject.Parameters;
using RestProtocol;
using Tuner.Wpf;
using WpfClient.Api;
using WpfClient.Core;
using WpfClient.Helpers;
using Timer = System.Timers.Timer;

namespace WpfClient.ViewModels
{
    class ReportsViewModel : DialogViewModelBase<IDialogView>
    {
        //ms
        private int PERIOD_UDPDATE_REPORTS = 500; 
        private readonly IKernel _container;
        private Timer _timer;
        public ReadOnlyObservableCollection<Report> Reports { private set; get; }
        private ObservableCollection<Report> _reports; 
        public User User { private set; get; }
        public Report SelectedReport { set; get; }
        public ICommand AddNewReportCommand {private set; get; }
        public ICommand DeleteReportCommand { private set; get; }
        public ICommand EndSessinoCommand { private set; get; }
        public ReportsViewModel([NotNull]User user, [NotNull]IKernel container, IDialogView view) : base(container, view)
        {
            ValidateHelpers.NotNull(user,nameof(user));
            ValidateHelpers.NotNull(container, nameof(container));
            _container = container;
            User = user;
            _reports = new ObservableCollection<Report>();
            Reports = new ReadOnlyObservableCollection<Report>(_reports);
            AddNewReportCommand = new RelayCommand(AddNewReport, CanDoSomthing);
            DeleteReportCommand = new RelayCommand(o => DeleteReportAsync(SelectedReport.ID), o => SelectedReport != null && CanDoSomthing(o));
            EndSessinoCommand = new RelayCommand(() =>
            {
                var logon = Bootstrapper.Container.Get<LogonViewModel>();
                DisplayedView.Close();
                logon.ShowDialog(null);
            });
            LoadReportsAsync();
        }

        public void LoadReportsAsync()
        {
            SetBusyState();
            GetReportsAsync();
        }

        public void DeleteReportAsync(int id)
        {
            SetBusyState();

            ReportsProvider.DeleteReportAsync(id).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    SetErrorState(task.Exception.AggregateMessages());
                    return;
                }
                if (task.IsCompleted)
                {
                    Dispatcher.Invoke(() =>
                    {
                        var d = _reports.FirstOrDefault(report => report.ID == id);
                        if (d != null)
                            _reports.Remove(d);
                        IsBusy = false;
                    });
                }
            });
        }

        private bool CanDoSomthing(object o) => !IsBusy && !ErrorDataLoading;

        private Task GetReportsAsync()
        {
             return ReportsProvider.GetReportsAsync(new CancellationToken()).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        if (task.IsFaulted)
                        {
                            SetErrorState(task.Exception.AggregateMessages());
                            SelectedReport = null;
                            return;
                        }
                        if (task.IsCompleted)
                        {
                            ClearStates();
                            var selected = SelectedReport;
                            _reports = new ObservableCollection<Report>(task.Result);
                            SelectedReport = selected != null ? _reports.First(report => report.ID == selected.ID) : Reports.FirstOrDefault();
                            Reports = new ReadOnlyObservableCollection<Report>(_reports);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                });
                Thread.Sleep(PERIOD_UDPDATE_REPORTS);
                GetReportsAsync();
                return task.Result;
            }); 
        }

        private void AddNewReport()
        {
            var addNewReport = Container.Get<AddNewReportViewModel>(new[] { new ConstructorArgument("user", User), new ConstructorArgument("reports", this) });
            var result = addNewReport.ShowDialog(this.DisplayedView);
            if (result == true)
                _reports.Add(addNewReport.GetReport());
        }
    }
}
