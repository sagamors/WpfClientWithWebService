using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestProtocol;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    public class ReportsController : ApiController
    {
        private readonly IReportsRepository _reportsRepository;
   
        public ReportsController()
        {
            //для упрошения реализации
            _reportsRepository = ReportsRepository.Instance;
        }

        public ReportsController(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        // GET: Reports
        public Report[] Get()
        {
            return _reportsRepository.Reports.ToArray();
        }

        // GET: Reports
        public Report[] Get(int id)
        {
            return _reportsRepository.Reports.Where(report => report.Creator.ID == id).ToArray();
        }

        public Report Post(Report report)
        {
            HttpStatusCode statucCode = HttpStatusCode.OK;
            if (_reportsRepository.Reports.Any(report1 => report.ID == report1.ID))
            {
                Report newReport = new Report(_reportsRepository.Reports.Last().ID + 1)
                {
                    Creator = report.Creator,
                    MonitoringObjects = report.MonitoringObjects,
                    Name = report.Name,
                    Parameter = report.Parameter,
                    Periodicity = report.Periodicity,
                    Time = report.Time
                };
                report = newReport;
            }
            _reportsRepository.Add(report);
            return report;
        }

        public HttpResponseMessage Delete(int id)
        {
            var report = _reportsRepository.Reports.FirstOrDefault(r => r.ID == id);
            if (report != null)
            {
                _reportsRepository.Delete(report);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}