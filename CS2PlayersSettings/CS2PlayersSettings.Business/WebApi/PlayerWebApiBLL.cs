using CS2_PlayerSettings.Data.Repository.Model;
using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository.Model;
using CS2PlayersSettings.Data.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Business.WebApi
{
    public class PlayerWebApiBLL
    {
        private readonly PlayerWebApiDAL _playerWebApiDAL;

        public PlayerWebApiBLL(PlayerWebApiDAL playerSettingsDAL)
        {
            _playerWebApiDAL = playerSettingsDAL;
        }

        #region 获取所有玩家
        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _playerWebApiDAL.GetAllPlayersAsync();
        }
        #endregion

        #region 获取所有玩家 分页查询
        public async Task<PagedResult<Player>> GetAllPlayersPageAsync(int page, int pageSize, string search = "")
        {
            return await _playerWebApiDAL.GetAllPlayersPageAsync(page, pageSize, search);
        }
        #endregion

        #region 根据选项ID获取鼠标设置
        /// <summary>
        /// 根据id获取鼠标设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<MouseSetting> GetPlayerMouseSettingsById(int id)
        {
            return await _playerWebApiDAL.GetPlayerMouseSettingsById(id);
        }
        #endregion

        #region 根据ID获取某个选手详情信息
        public async Task<Player> GetPlayersById(int id)
        {
            return await _playerWebApiDAL.GetPlayersById(id);
        }
        #endregion

        #region 根据Player Id获取准星设置
        public async Task<CrosshairSetting> GetPlayerCrosshairsById(int playerId)
        {
            return await _playerWebApiDAL.GetPlayerCrosshairsById(playerId);
        }
        #endregion

        #region 根据PlayerNickName获取某个选手详情信息
        public async Task<List<Player>> GetPlayersByNickName(string nickName)
        {
            return await _playerWebApiDAL.GetPlayersByNickName(nickName);
        }
        #endregion

        #region GET PLAYERS VIEWMODEL SETTINGS BY PLAYER ID
        public async Task<ViewmodelSetting> GetPlayerViewmodelById(int PlayerId)
        {
            return await _playerWebApiDAL.GetPlayerViewmodelById(PlayerId);
        }
        #endregion

        #region GET PLAYER VIDEO SETTINGS BY PLAYER ID
        public async Task<VideoSetting> GetPlayerVideoById(int PlayerId)
        {
            return await _playerWebApiDAL.GetPlayerVideoById(PlayerId);
        }

        #endregion

        #region 一次性返回Mouse、Crosshair、Viewmodel、Video四个设置
        public async Task<PlayerSettingsDto> GetPlayerSettingById(int playerId)
        {
            return await _playerWebApiDAL.GetPlayerSettingById(playerId);
        }
        #endregion

        #region 获取战队列表，以及获取战队成员
        public async Task<List<Team>> GetTopTenteamList()
        {
            return await _playerWebApiDAL.GetTopTenteamList();
        }

        public async Task<List<Player>> GetTeamPlayes(int teamId)
        {
            return await _playerWebApiDAL.GetTeamPlayes(teamId);
        }
        #endregion
    }
}
