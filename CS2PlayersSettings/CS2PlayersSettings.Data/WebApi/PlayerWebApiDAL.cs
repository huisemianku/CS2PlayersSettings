using CS2PlayersSettings.Data.Repository.DemoCrosshairCode.Structs;
using CS2PlayersSettings.Data.Repository.DemoCrosshairCode;
using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS2_PlayerSettings.Data.Repository.Model;
using CS2PlayersSettings.Data.Repository.Model;

namespace CS2PlayersSettings.Data.WebApi
{
    public class PlayerWebApiDAL
    {
        private readonly PlayerDbContext _playerDbContext;
        public PlayerWebApiDAL(PlayerDbContext playerDbContext)
        {
            _playerDbContext = playerDbContext;
        }

        #region 获取所有选手
        // 获取所有选手
        public async Task<List<Player>> GetAllPlayersAsync()
        {
            try
            {
                return await _playerDbContext.Players
                     .OrderBy(p => p.PlayerTopRanking.HasValue ? 0 : 1)  // 将 null 值排在后面
                     .ThenBy(p => p.PlayerTopRanking.HasValue ? p.PlayerTopRanking : null) // 对非 null 值排序，null 值会被排在前面
                     .ThenBy(p => p.Team.TeamRanking) // 对所有值按照 TeamRanking 排序
                     .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 获取所有玩家 分页查询
        public async Task<PagedResult<Player>> GetAllPlayersPageAsync(int page, int pageSize, string search = "")
        {
            try
            {
                var query = _playerDbContext.Players.AsQueryable();

                // 搜索过滤
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p => p.PlayerNickName.Contains(search));
                }

                // 获取总条数
                var totalItems = await query.CountAsync();

                // 分页查询
                var players = await query
                    .OrderBy(p => p.PlayerTopRanking.HasValue ? 0 : 1)
                    .ThenBy(p => p.PlayerTopRanking.HasValue ? p.PlayerTopRanking : null)
                    .ThenBy(p => p.Team.TeamRanking)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedResult<Player>
                {
                    Items = players,
                    TotalItems = totalItems,
                    PageNumber = page,
                    PageSize = pageSize
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 根据ID获取某个选手详情信息
        public async Task<Player> GetPlayersById(int id)
        {
            if (id == 0)
            {
                return new Player();
            }
            Player playerSetDetails = await _playerDbContext.Players
                .Where(i => i.PlayerId == id)
                .FirstOrDefaultAsync() ?? new Player();
            return playerSetDetails;
        }
        #endregion

        #region 根据ID获取某个选手详情信息
        public async Task<List<Player>> GetPlayersByNickName(string nickName)
        {
            if (string.IsNullOrEmpty(nickName))
            {
                return null;
            }


            var playerSetDetails = await _playerDbContext.Players
                .Where(i => EF.Functions.Like(i.PlayerNickName, $"%{nickName}%"))
                .OrderBy(p => p.PlayerTopRanking.HasValue ? 0 : 1)
                     .ThenBy(p => p.PlayerTopRanking.HasValue ? p.PlayerTopRanking : null)
                     .ThenBy(p => p.Team.TeamRanking)
                .ToListAsync() ?? new List<Player>();

            return playerSetDetails;
        }
        #endregion

        #region 根据Player ID获取鼠标设置
        /// <summary>
        /// 根据id获取鼠标设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<MouseSetting> GetPlayerMouseSettingsById(int id)
        {
            try
            {
                if (id != 0)
                {
                    string sql = @"SELECT Mouse_Id, ms.PlayerId, p.PlayerName, MouseName, MouseDPI, 
                       MouseSensitivity, MouseEdpi, MouseZoomSensitivity, MouseHz, WindowsSensitivity,MouseLastUpdateTime
                       FROM MouseSettings ms 
                       INNER JOIN Player p ON ms.PlayerId = p.PlayerId 
                       WHERE ms.PlayerId = @id";

                    return (await _playerDbContext.MouseSettings.FromSqlRaw(sql, new SqlParameter("@id", id)).FirstOrDefaultAsync())
       ?? new MouseSetting();
                }
                return new MouseSetting()
                {
                    MouseDpi = 0,
                    MouseSensitivity = 0,
                    MouseEdpi = 0,
                    MouseZoomSensitivity = 0,
                    MouseHz = 0,
                    WindowsSensitivity = 0,
                    MouseName = string.Empty,
                    MouseLastUpdateTime = new DateTime(),
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 根据Player Id获取准星设置
        public async Task<CrosshairSetting> GetPlayerCrosshairsById(int playerId)
        {
            try
            {
                if (playerId != 0)
                {
                    CrosshairSetting crosshair = await _playerDbContext.CrosshairSettings
                        .Where(id => id.PlayerId == playerId)
                        .FirstOrDefaultAsync() ?? new CrosshairSetting();
                    Crosshair crosshairAsy = CS2ShareCodeConverter.DecodeCrosshairShareCode(crosshair.CrosshairCode!);
                    crosshair.CrosshairSize = crosshairAsy.cl_crosshairsize;
                    crosshair.CrosshairThickness = crosshairAsy.cl_crosshairthickness;
                    crosshair.CrosshairGap = crosshairAsy.cl_crosshairgap;
                    switch (crosshairAsy.cl_crosshairstyle)
                    {
                        case 0:
                            crosshair.CrosshairStyle = "经典";
                            break;
                        case 1:
                            crosshair.CrosshairStyle = "经典";
                            break;
                        case 2:
                            crosshair.CrosshairStyle = "经典";
                            break;
                        case 3:
                            crosshair.CrosshairStyle = "经典";
                            break;
                        case 4:
                            crosshair.CrosshairStyle = "经典静态";
                            break;
                        case 5:
                            crosshair.CrosshairStyle = "Legacy";
                            break;
                        default:
                            break;
                    }

                    crosshair.CrosshairColorRed = crosshairAsy.cl_crosshaircolor_r;
                    crosshair.CrosshairColorGreen = crosshairAsy.cl_crosshaircolor_g;
                    crosshair.CrosshairColorBlue = crosshairAsy.cl_crosshaircolor_b;
                    return crosshair;
                }
                return new CrosshairSetting();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GET PLAYERS VIEWMODEL SETTINGS BY PLAYER ID
        public async Task<ViewmodelSetting> GetPlayerViewmodelById(int PlayerId)
        {
            try
            {
                if (PlayerId == 0)
                    return new ViewmodelSetting();
                {
                    ViewmodelSetting viewmodel = await _playerDbContext.ViewmodelSettings
                        .Where(p => p.PlayerId == PlayerId).FirstOrDefaultAsync() ?? new ViewmodelSetting();
                    viewmodel.ViewmodelCode = $"viewmodel_fov {viewmodel.ViewmodelFov}; viewmodel_offset_x {viewmodel.ViewmodelOffsetX}; viewmodel_offset_y {viewmodel.ViewmodelOffsetY};" +
                        $" viewmodel_offset_z {viewmodel.ViewmodelOffsetZ}; viewmodel_presetpos {viewmodel.ViewmodelPresetpos};";
                    return viewmodel;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GET PLAYER VIDEO SETTINGS BY PLAYER ID
        public async Task<VideoSetting> GetPlayerVideoById(int PlayerId)
        {
            try
            {
                if (PlayerId == 0)
                    return new VideoSetting();
                {
                    return await _playerDbContext.VideoSettings
                        .Where(p => p.PlayerId == PlayerId)
                        .FirstOrDefaultAsync() ?? new VideoSetting();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region 一次性返回Mouse、Crosshair、Viewmodel、Video四个设置
        public async Task<PlayerSettingsDto> GetPlayerSettingById(int playerId)
        {
            if (playerId != 0)
            {
                var playerSettings = new PlayerSettingsDto
                {
                    MouseSettings = await GetPlayerMouseSettingsById(playerId),
                    CrosshairSettings = await GetPlayerCrosshairsById(playerId),
                    ViewmodelSettings = await GetPlayerViewmodelById(playerId),
                    VideoSettings = await GetPlayerVideoById(playerId)
                };
                return playerSettings;
            }
            return null;
        }
        #endregion

        #region 获取战队列表，以及获取战队成员
        public async Task<List<Team>> GetTopTenteamList()
        {
            try
            {
                var teams = await _playerDbContext.Teams
                         .Where(t => t.TeamRanking != null) // 确保排名字段不为空
                         .OrderBy(t => t.TeamRanking) // 按排名升序排序
                         .Take(20) // 取前20个
                         .ToListAsync(); // 执行查询并返回列表
                return teams;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<List<Player>> GetTeamPlayes(int teamId)
        {
            try
            {
                if (teamId != 0)
                {
                    var teams = await _playerDbContext.Players
                       .Where(p => p.TeamId == teamId)
                       .ToListAsync(); // 执行查询并返回列表
                    return teams;
                }
                return null;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion
    }
}
