using CS2PlayersSettings.Data.Repository.Entities.Players;
using CS2PlayersSettings.Data.Repository.Model.APIResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.WebApi.IUser
{
    public interface IFollowerRepository
    {
        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="playerId">玩家id</param>
        /// <returns></returns>
        Task<ApiResponse<bool>> FollowPlayer(int userId, int playerId);

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="playerId">playerid</param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UnfollowPlayer(int userId, int playerId);

        /// <summary>
        /// 是否关注
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="playerId">playerid</param>
        /// <returns></returns>
        Task<ApiResponse<bool>> IsFollowing(int userId, int playerId);

        /// <summary>
        /// 获取用户关注的所有选手
        /// </summary>
        /// <param name="userId">userid</param>
        /// <returns></returns>
        Task<List<Player>> GetFollowedPlayers(int userId);

        /// <summary>
        /// 获取所有粉丝
        /// </summary>
        /// <param name="playerId">playerid</param>
        /// <returns></returns>
        Task<ApiResponse<List<User>>> GetFollowersOfPlayer(int playerId);
    }
}
