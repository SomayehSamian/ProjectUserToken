using Project.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Contract.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUser(string userName, string Password, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllUser(CancellationToken cancellationToken);
    }
}
