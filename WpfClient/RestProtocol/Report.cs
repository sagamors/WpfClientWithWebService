using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace RestProtocol
{
    public enum ePeriodReport
    {
        [Display(Name = "Раз в день")]
        OnceDay = 0x00,
        [Display(Name = "Раз в неделю")]
        OnceWeek = 0x01,
        [Display(Name = "Раз в месяц")]
        OnceMonth = 0x02
    }

    public class Report
    {
        public int ID { set; get; }
        public User Creator { set; get; }
        public string Name { set; get; }
        public ObservableCollection<MonitoringObject> MonitoringObjects { set; get; }
        public DateTime Time { set; get; }
        public ePeriodReport Periodicity { set; get; }
        public ReportParameter Parameter { set; get; }

        public Report(int id)
        {
            ID = id;
            MonitoringObjects = new ObservableCollection<MonitoringObject>();
        }
    }
}
