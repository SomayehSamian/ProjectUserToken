using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.Core.Contract.Services;
using Project.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;


namespace Project.API.Controllers.User
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    //[ApiVersion("1.0"), RoutePrefix("v{version:apiVersion}/User")]
    public class UserController : ControllerBase
    {

        public IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet, Route(nameof(GetUserToken))]
        public async Task<string> GetUserToken(string username, string password, CancellationToken cancellationToken)
        {
            return await _userService.GetUserToken(username, password, cancellationToken);
        }

        [HttpGet, Route(nameof(GetUsers))]
        public async Task<List<UserDTO>> GetUsers(CancellationToken cancellationToken)
        {
            return await _userService.GetUsers(cancellationToken);
        }

        [HttpPost, Route(nameof(SendToRabbit))]
        [AllowAnonymous]
        public async Task<bool> SendToRabbit(string username, string password, CancellationToken cancellationToken)
        {
           
           return await _userService.SendUserInfo(username, password, cancellationToken);
        }

    }
}
