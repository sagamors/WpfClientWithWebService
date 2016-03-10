using System;
using System.Linq;
using RestProtocol;

namespace WebApplication2.Repositories
{
    public interface IUsersRepository
    {
        IQueryable<User> Users { get; }
    }
}