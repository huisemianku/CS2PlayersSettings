using CS2PlayersSettings.Data.Repository.Model.User;
using CS2PlayersSettings.Data.WebApi;
using CS2PlayersSettings.Data.Repository.Entities.Users;
using CS2PlayersSettings.Data.Repository;

namespace CS2PlayersSettings.Business.WebApi
{
    public class UserWebApiBLL
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
    }
}
