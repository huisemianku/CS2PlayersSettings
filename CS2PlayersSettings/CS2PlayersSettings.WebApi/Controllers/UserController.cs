using CS2PlayersSettings.Business.WebApi;
using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository.Entities.Users;
using CS2PlayersSettings.Data.Repository.Model.User;
using CS2PlayersSettings.Data.WebApi;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static CProductInfo_SetRichPresenceLocalization_Request.Types;

namespace CS2PlayersSettings.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 验证
    public class UserController : Controller
    {
        private readonly UserWebApiBLL _userWebApiBLL;
        public UserController(UserWebApiBLL userWebApiBLL)
        {
            _userWebApiBLL = userWebApiBLL;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var user = await _userWebApiBLL.GetUserInfoAsync(userId);

            if (user == null)
                return NotFound("用户不存在");

            return Ok(new { user.Username, user.Email });
        }
    }
}
