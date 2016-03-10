using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestProtocol;

namespace WebApplication2.Repositories
{
    class ReportsRepository : IReportsRepository
    {
        private List<Report> reports;

        private ReportsRepository() : this(new UsersRepository())
        {
            
        }

        private ReportsRepository(IUsersRepository usersRepository)
        {
            reports = new List<Report>();
        }

        public IQueryable<Report> Reports
        {
            get { return reports.AsQueryable(); }
        }

        public void Add(Report report)
        {
            reports.Add(report);
        }

        public void Delete(Report report)
        {
            reports.Remove(report);
        }

        //для упрошения реализации
        public static ReportsRepository Instance { get; } = new ReportsRepository(UsersRepository.Instance);
    }
}