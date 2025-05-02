using CS2PlayersSettings.Business.WebApi;
using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository.Model.User;
using CS2PlayersSettings.Data.WebApi;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class UserController : Controller
    {
        private readonly UserWebApiBLL _userWebApiBLL;
        private readonly IConfiguration _configuration;
        public UserController(UserWebApiBLL userWebApiBLL, IConfiguration configuration)
        {
            _userWebApiBLL = userWebApiBLL;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("UserRegister")]
        public async Task<ActionResult> UserRegister([FromBody] RegisterDto userReModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(userReModel);
            try
            {
                // 调用 DAL 注册用户
                var user = await _userWebApiBLL.UserRegister(userReModel);

                // 生成 JWT
                var token = GenerateJwtToken(user);

                return Ok(new { success = true, Message = "User registered successfully." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "Email already exists.")
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // 记录日志（可选）
                // _logger.LogError(ex, "Unexpected error during registration");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userWebApiBLL.UserLogin(model);
                if (user != null)
                {
                    var token = GenerateJwtToken(user);
                    // 设置 HttpOnly Cookie
                    Response.Cookies.Append("auth_token", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // 仅 HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.Now.AddDays(7) // 与 token 有效期一致
                    });
                    return Ok(new { success = true, Message = "Login successful."});
                }
                return Unauthorized(new { success = false, Message = "邮箱或者密码错误！！！" });

            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>

        [HttpGet("verify")]
        [Authorize]
        public IActionResult VerifyToken()
        {
            return Ok("Token is valid.");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // 清除 Cookie
            Response.Cookies.Delete("auth_token");
            return Ok(new { Message = "Logout successful." });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            // 获取key值
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            // 指定使用 HMAC SHA256 算法进行签名。 这是一种常用的对称加密算法，用于确保 JWT 的完整性和真实性。
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            /* 
                claims: 创建一个包含 JWT 声明 (Claims) 的数组。 声明是关于用户的键值对信息，例如用户的 ID、电子邮件地址、角色等。
                JwtRegisteredClaimNames.Sub: 使用 JwtRegisteredClaimNames.Sub 作为声明类型，表示 JWT 的 Subject（主题）
                通常是用户的唯一标识符。 这里使用 user.UserId.ToString() 作为主题的值。
                JwtRegisteredClaimNames.Email: 使用 JwtRegisteredClaimNames.Email 作为声明类型，
                表示用户的电子邮件地址。 使用 user.Email 作为电子邮件地址的值。
                JwtRegisteredClaimNames.Jti: 使用 JwtRegisteredClaimNames.Jti 作为声明类型，
                表示 JWT 的 JWT ID (唯一标识符)。 Guid.NewGuid().ToString() 生成一个新的 GUID 作为 JWT ID。 这可以用于防止 JWT 重放攻击。
             */
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            /*
             * 创建一个 JwtSecurityToken 对象，它代表一个 JWT。
             * 从配置文件读取issuer/audience
             * claims添加到jwt当中
             * expires：过期时间
             * signingCredentials：使用之前创建的签名凭据来签署 JWT。
             */

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7), // 支持自动登录
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
