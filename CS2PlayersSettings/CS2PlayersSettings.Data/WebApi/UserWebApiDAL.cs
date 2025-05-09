using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.Repository.Entities.Players;
using CS2PlayersSettings.Data.Repository.Model.APIResponse;
using CS2PlayersSettings.Data.Repository.Model.User;
using CS2PlayersSettings.Data.WebApi.IUser;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;

namespace CS2PlayersSettings.Data.WebApi
{
    public class UserWebApiDAL:IFollowerRepository
    {
        private readonly PlayerDbContext _userDbContext;
        public UserWebApiDAL(PlayerDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        #region 用户注册
        public async Task<User> UserRegister(RegisterDto userReModel)
        {
            try
            {
                // 检查邮箱是否已存在
                if (await _userDbContext.Users.AnyAsync(u => u.Email == userReModel.Email))
                {
                    throw new InvalidOperationException("Email already exists.");
                }

                // 使用 Argon2 Hash 密码
                string passwordHash = Argon2.Hash(userReModel.Password);

                // 创建用户实体
                var user = new User
                {
                    Username = userReModel.Username,
                    Email = userReModel.Email,
                    PasswordHash = passwordHash
                };

                // 保存到数据库
                _userDbContext.Users.Add(user);
                await _userDbContext.SaveChangesAsync();

                return user; // 返回创建的用户
            }
            catch (Exception ex)
            {
                // 记录日志（可选，使用 ILogger）
                // _logger.LogError(ex, "Error registering user with email {Email}", userReModel.Email);
                throw; // 抛出异常，由控制器处理
            }
        }
        #endregion

        #region 用户登录
        public async Task<User?> UserLogin(LoginDto userLoModel)
        {
            try
            {
                // 通过邮箱获取用户
                var user = await _userDbContext.Users.SingleOrDefaultAsync(u => u.Email == userLoModel.Email);
                // 判断用户是否存在和密码是否正确
                if (user == null || !Argon2.Verify(user.PasswordHash, userLoModel.Password))
                    return null;
                return user;
            }
            catch (Exception ex)
            {
                // 记录日志（可选，使用 ILogger）
                // _logger.LogError(ex, "Error registering user with email {Email}", userReModel.Email);
                throw; // 抛出异常，由控制器处理
            }
        }
        #endregion

        #region 获取用户信息 BY USERID
        public async Task<User> GetUserInfoAsync(int userId)
        {
            try
            {
                if (userId == 0)
                    return null;
                return await _userDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId) ?? new User();

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 更新用户信息
        public async Task<ApiResponse<User>> UpdateUserInfoAsync(UserInfoDto updateUser, int userId)
        {
            var user = await _userDbContext.Users
                 .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return ApiResponse<User>.FailureResult("用户不存在");
            }
          
            if (string.IsNullOrWhiteSpace(updateUser.userName))
            {
                return ApiResponse<User>.FailureResult("用户名不能为空");
            }
            if (user.Username == updateUser.userName)
            {
                return ApiResponse<User>.FailureResult("用户名未更改");
            }
            // 只更新用户名
            user.Username = updateUser.userName; 
            await _userDbContext.SaveChangesAsync();

            return ApiResponse<User>.SuccessResult(user, "用户信息更新成功");
        }

        #endregion

        #region 用户关注
        public async Task<ApiResponse<bool>> FollowPlayer(int userId, int playerId)
        {
            try
            {
                ApiResponse<bool> isfolw = await IsFollowing(userId, playerId);
                // 检查是否已关注
                if (isfolw.Success)
                {
                    return ApiResponse<bool>.FailureResult("已经关注");
                }

                var follower = new Follower
                {
                    UsersId = userId,
                    PlayersId = playerId,
                    CreatedAt = DateTime.UtcNow
                };

                _userDbContext.Followers.Add(follower);
                await _userDbContext.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, "关注成功");
            }
            catch (Exception ex)
            {
                // 记录错误日志
                return ApiResponse<bool>.FailureResult($"关注失败：{ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UnfollowPlayer(int userId, int playerId)
        {
            try
            {
                var follower = await _userDbContext.Followers
                    .FirstOrDefaultAsync(f => f.UsersId == userId && f.PlayersId == playerId);

                if (follower == null)
                {
                    return ApiResponse<bool>.FailureResult("未关注");
                }

                _userDbContext.Followers.Remove(follower);
                await _userDbContext.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, "取消关注成功");
            }
            catch (Exception ex)
            {
                // 记录错误日志
                return ApiResponse<bool>.FailureResult($"取消关注失败：{ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> IsFollowing(int userId, int playerId)
        {
            try
            {
                bool isFollowing = await _userDbContext.Followers
                    .AnyAsync(f => f.UsersId == userId && f.PlayersId == playerId);
                if (isFollowing)
                {
                    return ApiResponse<bool>.SuccessResult(isFollowing,"已关注");
                }else
                {
                    return ApiResponse<bool>.FailureResult("未关注");
                }
                
            }
            catch (Exception ex)
            {
                // 记录错误日志
                return ApiResponse<bool>.FailureResult($"检查关注状态失败：{ex.Message}");
            }
        }

        public async Task<List<Player>> GetFollowedPlayers(int userId)
        {
            try
            {
                List<Player> followPlayers = await _userDbContext.Followers
                    .Where(f => f.UsersId == userId)
                    .Select(p => p.Players)
                    .ToListAsync();
                return followPlayers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<ApiResponse<List<User>>> GetFollowersOfPlayer(int playerId)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
