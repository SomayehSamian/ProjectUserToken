using Project.Dto;
using Project.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Contract.Services
{
    public interface IUserService
    {
        Task<string> GetUserToken(string username, string password, CancellationToken cancellationToken);
        Task<List<UserDTO>> GetUsers(CancellationToken cancellationToken);
        Task<bool> SendUserInfo(string username, string password, CancellationToken cancellationToken);
        string GenerateJwtToken(string username);
    }
}
