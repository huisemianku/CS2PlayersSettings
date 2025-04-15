using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository.Model;
using CS2PlayersSettings.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;
using CS2PlayersSettings.Data.Repository;

namespace CS2PlayersSettings.Business
{
    public class PlayerBLL
    {
        private readonly PlayerDAL _playerDAL;
        public PlayerBLL(PlayerDAL productDAL)
        {
            _playerDAL = productDAL;
        }

        public async Task uCover()
        {
            await _playerDAL.uCover();
        }

        #region 获取所有选手列表
        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _playerDAL.GetAllPlayersAsync();
        }
        #endregion

        #region 选手列表分页查询
        public async Task<IPagedList<Player>> GetPlayersAsync(int pageIndex, int pageSize, string searchTerm = "")
        {
            return await _playerDAL.GetPlayersAsync(pageIndex, pageSize, searchTerm);
        }
        #endregion

        #region 获取所有战队
        public async Task<List<Team>> GetAllTeam()
        {
            return await _playerDAL.GetAllTeam();
        }
        #endregion

        #region 根据选手id获取姓名
        public async Task<string> GetPlayerNickNameById(int playerId)
        {
            return await _playerDAL.GeyPlayerNickNameById(playerId);
        }
        #endregion

        #region 根据选手id获取详细信息
        public async Task<Player> GeyPlayerDetailsById(int playerId)
        {
            return await _playerDAL.GeyPlayerDetailsById(playerId);
        }
        #endregion

        #region 根据选手id获取选手游戏设置
        public async Task<MouseSetting> GetMouseSettingById(int playerId)
        {
            return await _playerDAL.GetMouseSettingById(playerId);
        }

        public async Task<CrosshairSetting> GetCrosshairSettingById(int playerId)
        {
            return await _playerDAL.GetCrosshairSettingById(playerId);
        }

        public async Task<ViewmodelSetting> GetViewmodelSettingById(int playerId)
        {
            return await _playerDAL.GetViewmodelSettingById(playerId);
        }

        public async Task<VideoSetting> GetVideoSettingById(int playerId)
        {
            return await _playerDAL.GetVideoSettingById(playerId);
        }
        #endregion

        #region 根据选手id和Mouse ID UPDATE MOUSESETTINGS
        public async Task updatePlayerMouseSettingsById(int playerId, int mouseId, MouseSetting mouseSetting)
        {
            await _playerDAL.updatePlayerMouseSettingsById(playerId, mouseId, mouseSetting);
        }
        #endregion

        #region 根据选手id和Mouse ID UPDATE CROSSHAIRSETTINGS
        public async Task updatePlayerCrosshairSettingsById(int playerId, int crosshairId, CrosshairSetting crosshairSetting)
        {
            await _playerDAL.updatePlayerCrosshairSettingsById(playerId, crosshairId, crosshairSetting);
        }
        #endregion

        #region 根据选手id和ViewmodelId UPDATE viewmodelSettings
        public async Task updatePlayerViewmodelSettingsById(int playerId, int viewmodelId, ViewmodelSetting viewmodelSetting)
        {
            await _playerDAL.updatePlayerViewmodelSettingsById(playerId, viewmodelId, viewmodelSetting);
        }
        #endregion

        #region 根据选手id和VideoId UPDATE videoSettings
        public async Task updatePlayerVideoSettingsById(int playerId, int videoId, VideoSetting videoSetting)
        {
            await _playerDAL.updatePlayerVideoSettingsById(playerId, videoId, videoSetting);
        }
        #endregion

        #region 更新选手信息
        public async Task UpdatePlayerInfoByPlayerIdAsync(int playerId, Player player)
        {
            await _playerDAL.UpdatePlayerInfoByPlayerIdAsync(playerId, player);
        }
        #endregion

        #region 新增选手
        public async Task<int> AddPlayerAsync(Player player)
        {
            return await _playerDAL.AddPlayerAsync(player);
        }
        #endregion

        #region 从Excel中导入数据
        public async Task<int> AddPlayerByExcelAsync(List<Player> players, List<MouseSetting> mouseSettings, List<CrosshairSetting> crosshairSettings, List<ViewmodelSetting> viewmodelSettings, List<VideoSetting> videoSettings)
        {
            return await _playerDAL.AddTeamByExcelAsync(players, mouseSettings, crosshairSettings, viewmodelSettings, videoSettings);
        }
        #endregion

        #region 从Excel中导入战队数据(V设排名) 7天一更新
        public async Task<int> AddTeamByExcelAsync(List<Team> teams)
        {
            return await _playerDAL.AddTeamByExcelAsync(teams);
        }
        #endregion
    }
}
