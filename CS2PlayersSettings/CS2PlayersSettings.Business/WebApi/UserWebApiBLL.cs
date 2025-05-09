using CS2PlayersSettings.Data.Repository.Model.User;
using CS2PlayersSettings.Data.WebApi;
using CS2PlayersSettings.Data.Repository.Model.APIResponse;
using CS2PlayersSettings.Data.Repository.Entities.Players;
using CS2PlayersSettings.Data.WebApi.IUser;

namespace CS2PlayersSettings.Business.WebApi
{
    public class UserWebApiBLL : IFollowerRepository
    {
        private readonly UserWebApiDAL _userWebApiDAL;

        public UserWebApiBLL(UserWebApiDAL userWebApiDAL)
        {
            _userWebApiDAL = userWebApiDAL;
        }

        #region 用户注册
        public async Task<User> UserRegister(RegisterDto userReModel)
        {
            return await _userWebApiDAL.UserRegister(userReModel);
        }
        #endregion

        #region 用户登录
        public async Task<User?> UserLogin(LoginDto userLoModel)
        {
            return await _userWebApiDAL.UserLogin(userLoModel);
        }
        #endregion

        #region 获取用户信息 BY USERID
        public async Task<User> GetUserInfoAsync(int userId)
        {
            return await _userWebApiDAL.GetUserInfoAsync(userId);
        }
        #endregion

        #region 更新用户信息
        public async Task<ApiResponse<User>> UpdateUserInfoAsync(UserInfoDto updateUser,int userId)
        {
            return await _userWebApiDAL.UpdateUserInfoAsync(updateUser, userId);
        }



        #endregion

        #region 用户关注
        public async Task<ApiResponse<bool>> FollowPlayer(int userId, int playerId)
        {
            return await _userWebApiDAL.FollowPlayer(userId, playerId);
        }

        public async Task<ApiResponse<bool>> UnfollowPlayer(int userId, int playerId)
        {
            return await _userWebApiDAL.UnfollowPlayer(userId, playerId);
        }

        public async Task<ApiResponse<bool>> IsFollowing(int userId, int playerId)
        {
            return await _userWebApiDAL.IsFollowing(userId, playerId);
        }

        public async Task<List<Player>> GetFollowedPlayers(int userId)
        {
            return await _userWebApiDAL.GetFollowedPlayers(userId);
        }

        public async Task<ApiResponse<List<User>>> GetFollowersOfPlayer(int playerId)
        {
            return await _userWebApiDAL.GetFollowersOfPlayer(playerId);
        }
        #endregion
    }
}
