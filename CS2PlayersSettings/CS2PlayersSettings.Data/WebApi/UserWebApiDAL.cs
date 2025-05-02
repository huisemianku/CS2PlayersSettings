using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository.Model.User;
using DemoFile.Game.Cs;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.WebApi
{
    public class UserWebApiDAL
    {
        private readonly PlayerDbContext _playerDbContext;
        public UserWebApiDAL(PlayerDbContext playerDbContext)
        {
            _playerDbContext = playerDbContext;
        }

        #region 用户注册
        public async Task<User> UserRegister(RegisterDto userReModel)
        {
            try
            {
                // 检查邮箱是否已存在
                if (await _playerDbContext.Users.AnyAsync(u => u.Email == userReModel.Email))
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
                _playerDbContext.Users.Add(user);
                await _playerDbContext.SaveChangesAsync();

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
                var user = await _playerDbContext.Users.SingleOrDefaultAsync(u => u.Email == userLoModel.Email);
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
    }
}
