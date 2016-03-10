using System.ComponentModel.DataAnnotations;

namespace Protocol
{
    public enum ePeriodReportCreation
    {
        [Display(Name = "День")]
        Day = 0x00,
        [Display(Name = "Неделю")]
        Week = 0x01,
        [Display(Name = "Месяц")]
        Month = 0x02
    }
    public class TrafficAndParking : ReportParameter
    {


        public ePeriodReportCreation PeriodReport { set; get; }

        public TrafficAndParking() : base("Движения и стоянки", 0x01)
        {
        }
    }
}