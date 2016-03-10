using System.Linq;
using RestProtocol;

namespace WebApplication2.Repositories
{
    public interface IReportsRepository
    {
        IQueryable<Report> Reports { get; }
        void Add(Report report);
        void Delete(Report report);
    }
}