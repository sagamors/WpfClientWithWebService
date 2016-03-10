using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using JetBrains.Annotations;
using Ninject;
using RestProtocol;
using WpfClient.Api;
using WpfClient.Core;
using WpfClient.Helpers;

namespace WpfClient.ViewModels
{
    internal class AddNewReportViewModel : OkCancelDialogViewModelBase<IDialogView>
    {
        private readonly User _user;
        private Report _newReport;
        private Report _report;
        [Required]
        public User Creator
        {
            set { _report.Creator = value; }
            get { return _report.Creator; }
        }
        [Required(ErrorMessage = "Введите имя")]
        public string Name
        {
            set { _report.Name = value; }
            get { return _report.Name; }
        }
        public ObservableCollection<MonitoringObject> AllMonitoringObjects { set; get; }
        public ObservableCollection<Sensor> AllSensors { set; get; }
        public DateTime Time
        {
            set { _report.Time = value; }
            get { return _report.Time; }
        }

        public ePeriodReport Periodicity
        {
            set { _report.Periodicity = value; }
            get { return _report.Periodicity; }
        }

        [Required(ErrorMessage = "Выберите тип отчета")]
        public ReportParameter ReportParameter
        {
            set { _report.Parameter = value; }
            get { return _report.Parameter; }
        }

        [HasElements(ErrorMessage = "Выберите хотя бы один объект для мониторинга")]
        public ObservableCollection<MonitoringObject> MonitoringObjects
        {
            set
            {
                if (_report.MonitoringObjects != null)
                    MonitoringObjects.CollectionChanged -= MonitoringObjects_CollectionChanged;
                _report.MonitoringObjects = value;
                _report.MonitoringObjects.CollectionChanged += MonitoringObjects_CollectionChanged;
            }
            get { return _report.MonitoringObjects; }
        }

        public ObservableCollection<ReportParameter> ReportParameters { private set; get; }

        public ICommand AddNewReportCommand { private set; get; }
        public AddNewReportViewModel([NotNull]ReportsViewModel reports, [NotNull]User user, [NotNull]IKernel container, IDialogView view) : base(container, view)
        {
            ValidateHelpers.NotNull(user, nameof(user));
            ValidateHelpers.NotNull(container, nameof(container));
            ValidateHelpers.NotNull(reports, nameof(reports));
            _user = user;
            _report = new Report(-1);
            Creator = user;
            Time = DateTime.Now;
            MonitoringObjects = new ObservableCollection<MonitoringObject>();
            AllMonitoringObjects = new ObservableCollection<MonitoringObject>
            {
                new MonitoringObject("о001оа178", 0x01),
                new MonitoringObject("о002оо47", 0x02),
                new MonitoringObject("a100aa777", 0x03)
            };
            AllSensors = new ObservableCollection<Sensor>
            {
                new Sensor("Топливо"),
                new Sensor("Зажигание"),
                new Sensor("Датчик удара"),
            };

            if (AllMonitoringObjects.Count > 0)
                MonitoringObjects.Add(AllMonitoringObjects.FirstOrDefault());
 
            ReportParameters = new ObservableCollection<ReportParameter> {new MessagesFromObject(), new TrafficAndParking()};
            _report.Parameter = ReportParameters.FirstOrDefault();
            var messagesFromObject = ReportParameter as MessagesFromObject;
            if (AllSensors.Count > 0 && messagesFromObject != null)
            {
                messagesFromObject.Sensors.Add(AllSensors.First());
            }

            AddNewReportCommand = new RelayCommand(AddNewReportAsync, o => !IsBusy);
            Validate();
        }

        public Report GetReport()
        {
            return _newReport;
        }

        protected override bool CanOkExecute(object o)
        {
            return !IsBusy && !ErrorDataLoading && !HasErrors;
        }

        protected override void Ok()
        {
            base.Ok();
            if (!HasErrors && CanDoSomthing(null))
            {
                SetBusyState();
                AddNewReportAsync();
            }
        }

        private void AddNewReportAsync()
        {
            ReportsProvider.AddReportAsync(_report).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        if (task.IsFaulted)
                        {
                            SetErrorState(task.Exception.AggregateMessages());
                            return;
                        }
                        if (task.IsCompleted)
                        {
                            ClearStates();
                            _newReport = task.Result;
                            DisplayedView.Ok();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                });
            });
        }

        private void MonitoringObjects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ValidateProperty(nameof(MonitoringObjects));
        }

        private bool CanDoSomthing(object o) => !IsBusy && !ErrorDataLoading;
    }
}
