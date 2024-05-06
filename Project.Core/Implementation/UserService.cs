using Project.Core.Contract.Repository;
using Project.Core.Contract.Services;
using Project.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Project.Dto;
using System.Threading;
using Project.Core.Contract;

namespace Project.Core.Implementation
{
    public class UserService: IUserService
    {
        private readonly IConfiguration _configuration;

        private readonly IUserRepository _userRepository;

        private readonly IRabbitMQService _rabbitMQService;

        public UserService(IUserRepository userRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _rabbitMQService = new Sender.RabbitMQService("localhost", "Inbox");
        }

        public async Task<string> GetUserToken(string username, string password, CancellationToken cancellationToken)
        {
            User user = await GetUser(username, password, cancellationToken);

            if (user is null)
                throw new Exception("username or password not valid");
            else
                return GenerateJwtToken(username);
        }
        public async Task<List<UserDTO>> GetUsers(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUser(cancellationToken);
            return users.Select(a=>new UserDTO() { 
                Id = a.Id,
                FirstName=a.FirstName,
                LastName =a.LastName,
                Email =a.Email,
                Password = a.Password,
                UserName =a.UserName
            }).ToList<UserDTO>();
        }

        public async Task<bool> SendUserInfo(string username, string password, CancellationToken cancellationToken)
        {
            User user = await GetUser(username, password, cancellationToken);

            if (user is null)
                throw new Exception("Username or Password not Valid");
            else
            {

                try
                {
                    _rabbitMQService.SendMessage(user);
                }
                catch
                {
                    throw new Exception("Send Message is Fail......");
                }
                return true;
                
            }


        }
        private async Task<User> GetUser(string username, string password, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUser(username, password, cancellationToken);
        }
        public string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
