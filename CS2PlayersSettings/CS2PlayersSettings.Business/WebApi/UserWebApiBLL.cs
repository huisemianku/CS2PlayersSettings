using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository.Model.User;
using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.WebApi;
using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
