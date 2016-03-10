using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using RestProtocol;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController() : this(new UsersRepository())
        {
            
        }

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        // GET: Users
        public User[] Get()
        {
            return _usersRepository.Users.ToArray();
        }
    }
}