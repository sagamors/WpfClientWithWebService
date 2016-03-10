using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestProtocol;
using WpfClient.CustomException;

namespace WpfClient.Api
{
    public static class UsersProvider
    {
        private static RestClient _restClient = new RestClient(Config.IP);

        /// <summary>
        /// Асинхронно возвращает всеx пользователей
        /// </summary>
        /// <exception cref="RestException"></exception>
        /// <returns>Коллекция с пользователями</returns>
        public static Task<IEnumerable<User>> GetUsersAsync(CancellationToken token)
        {
            return Task.Run(() => Task.FromResult(_restClient.MakeRequest<IEnumerable<User>>("Users")), token);
        }
    }
}
