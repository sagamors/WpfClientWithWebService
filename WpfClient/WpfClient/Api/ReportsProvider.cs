using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestProtocol;
using WpfClient.CustomException;


namespace WpfClient.Api
{
    public static class ReportsProvider
    {
        private static RestClient _restClient = new RestClient(Config.IP);

        /// <summary>
        /// Асинхронно возвращает все доступные отчеты
        /// </summary>
        /// <exception cref="RestException"></exception>
        /// <returns>Коллекция с отчетами</returns>
        public static Task<IEnumerable<Report>> GetReportsAsync(CancellationToken token)
        {
            return Task.Run(() => Task.FromResult(GetReports()), token);
        }

        /// <summary>
        /// Возвращает все доступные отчеты
        /// </summary>
        /// <exception cref="RestException"></exception>
        /// <returns>Коллекция с отчетами</returns>
        public static IEnumerable<Report> GetReports()
        {
            _restClient.Method = HttpVerb.GET;
            _restClient.PostData = null;
            return _restClient.MakeRequest<IEnumerable<Report>>("Reports");
        }

        /// <summary>
        /// Возвращает все доступные отчеты
        /// </summary>
        /// <exception cref="RestException"></exception>
        /// <returns>Коллекция с отчетами</returns>
        public static IEnumerable<Report> GetReports(int id)
        {
            _restClient.Method = HttpVerb.GET;
            _restClient.PostData = null;
            return _restClient.MakeRequest<IEnumerable<Report>>($"Reports/{id}");
        }

        /// <summary>
        /// Удаляет отчет
        /// </summary>
        /// <param name="id">ID отчета</param>
        /// <returns>Результат</returns>
        public static string DeleteReport(int id)
        {
            _restClient.Method = HttpVerb.DELETE;
            _restClient.PostData = null;
            return _restClient.MakeRequest($"Reports/{id}");
        }

        /// <summary>
        /// Асинхронно удаляет отчет
        /// </summary>
        /// <param name="id">ID отчета</param>
        /// <exception cref="RestException"></exception>
        /// <returns>Результат</returns>
        public static Task<string> DeleteReportAsync(int id)
        {
            return Task.Run(() => Task.FromResult(DeleteReport(id)));
        }

        /// <summary>
        /// Добавляет новый отчет
        /// </summary>
        /// <param name="id">ID отчета</param>
        /// <exception cref="RestException"></exception>
        /// <returns>Результат</returns>
        public static Report AddReport(Report report)
        {
            return _restClient.Post<Report>("Reports",report);
        }

        /// <summary>
        /// Асинхронно добавляет новый отчет
        /// </summary>
        /// <param name="id">ID отчета</param>
        /// <exception cref="RestException"></exception>
        /// <returns>Результат</returns>
        public static Task<Report> AddReportAsync(Report report)
        {
            return Task.Run(() => Task.FromResult(AddReport(report)));
        }
    }
}
