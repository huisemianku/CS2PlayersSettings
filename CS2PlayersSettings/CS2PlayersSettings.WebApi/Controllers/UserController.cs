using CS2PlayersSettings.Business.WebApi;
using CS2PlayersSettings.Data.Repository.Entities.Players;
using CS2PlayersSettings.Data.Repository.Model.APIResponse;
using CS2PlayersSettings.Data.Repository.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [Route("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfoDto updateUser)
        {
            try
            {
                int userIdClaim = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _userWebApiBLL.UpdateUserInfoAsync(updateUser, userIdClaim);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<User>.FailureResult("更新用户信息失败，请稍后重试"));
            }
        }

        [HttpPost]
        [Route("FollowUser")]
        public async Task<IActionResult> FollowUser([FromBody]int playerId)
        {
            try
            {
                int userIdClaim = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _userWebApiBLL.FollowPlayer(userIdClaim, playerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<User>.FailureResult("关注失败，请稍后重试"));
            }
        }

        [HttpPost]
        [Route("UnFollowUser")]
        public async Task<IActionResult> UnFollowUser([FromBody] int playerId)
        {
            try
            {
                int userIdClaim = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _userWebApiBLL.UnfollowPlayer(userIdClaim, playerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<User>.FailureResult("关注失败，请稍后重试"));
            }
        }

        [HttpPost]
        [Route("IsFollowUser")]
        public async Task<IActionResult> IsFollowUser([FromBody] int playerId)
        {
            try
            {
                int userIdClaim = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _userWebApiBLL.IsFollowing(userIdClaim, playerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<User>.FailureResult("关注失败，请稍后重试"));
            }
        }

        [HttpPost]
        [Route("GetUserFollow")]
        public async Task<IActionResult> GetUserFollow()
        {
            try
            {
                int userIdClaim = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _userWebApiBLL.GetFollowedPlayers(userIdClaim);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<User>.FailureResult("关注失败，请稍后重试"));
            }
        }
    }
}
