using System.Collections.Generic;
using System.Linq;
using RestProtocol;

namespace WebApplication2.Repositories
{
    class UsersRepository : IUsersRepository
    {
        private static IQueryable<User> _users;

        static UsersRepository()
        {
            var users = new List<User>();
            users.Add(new User("Александр", " Пушкин",0));
            users.Add(new User("Антон ", " Чехов", 1));
            users.Add(new User("Лев ", " Толстой", 2));
            users.Add(new User("Николай ", " Гоголь", 3));
            users.Add(new User("Федор ", " Достоевский", 4));
            _users = users.AsQueryable();
        }

        public IQueryable<User> Users
        {
            get { return _users.AsQueryable(); }
        }

        public UsersRepository()
        {
            
        }

        public static UsersRepository Instance { get; } = new UsersRepository();
    }
}