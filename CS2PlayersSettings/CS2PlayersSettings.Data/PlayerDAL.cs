using CS2PlayersSettings.Data.Repository;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;
using CS2PlayersSettings.Data.Repository.Helper;
using CS2PlayersSettings.Data.Repository.Entities.Players;

namespace CS2PlayersSettings.Data
{
    public class PlayerDAL
    {
        private readonly PlayerDbContext _playerDbContext;

        public PlayerDAL(PlayerDbContext playerDbContext)
        {
            _playerDbContext = playerDbContext;

        }

        #region 获取所有选手列表
        public async Task<List<Player>> GetAllPlayersAsync()
        {
            try
            {
                return await _playerDbContext.Players.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 选手列表分页查询
        public async Task<IPagedList<Player>> GetPlayersAsync(int pageIndex, int pageSize, string searchTerm = "")
        {
            // 构建查询
            var query = _playerDbContext.Players.AsQueryable();

            // 应用模糊查询
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.PlayerNickName.Contains(searchTerm));
            }

            // 获取总记录数
            int totalItems = await query.CountAsync();

            // 分页查询
            var items = await query
     .OrderBy(p => p.PlayerTopRanking.HasValue ? 0 : 1)  // 将 null 值排在后面
     .ThenBy(p => p.PlayerTopRanking.HasValue ? p.PlayerTopRanking : null) // 对非 null 值排序，null 值会被排在前面
     .ThenBy(p => p.Team.TeamRanking) // 对所有值按照 TeamRanking 排序
     .Select(p => new Player
     {
         PlayerId = p.PlayerId,
         PlayerNickName = p.PlayerNickName,
         PlayerCover = p.PlayerCover,
         PlayerTeam = p.PlayerTeam,
         PlayerTeamLogo = p.Team.TeamImg,
     })
     .ToPagedListAsync(pageIndex, pageSize);

            // 返回分页结果
            return items;
        }
        #endregion

        #region 获取所有战队
        public async Task<List<Team>> GetAllTeam()
        {
            try
            {
                return await _playerDbContext.Teams.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 根据选手id获取姓名
        public async Task<string> GeyPlayerNickNameById(int playerId)
        {
            try
            {
                Player player = await _playerDbContext.Players.Where(p => p.PlayerId == playerId)
                     .Select(p => new Player { PlayerNickName = p.PlayerNickName })
                     .FirstOrDefaultAsync() ?? new Player();
                return player.PlayerNickName ?? "UNKNOW";

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 根据选手id获取详细信息
        public async Task<Player> GeyPlayerDetailsById(int playerId)
        {
            try
            {
                return await _playerDbContext.Players
                    .Where(p => p.PlayerId == playerId)
                    .Select(p => new Player
                    {
                        PlayerId = p.PlayerId,
                        PlayerName = p.PlayerName,
                        PlayerNickName = p.PlayerNickName,
                        PlayerCover = p.PlayerCover,
                        PlayerTeam = p.Team.TeamName,
                        PlayerTeamLogo = p.Team.TeamImg,
                        PlayerTopRanking = p.PlayerTopRanking,
                        PlayerCountry = p.PlayerCountry,
                        PlayerPrizeMoney = p.PlayerPrizeMoney
                    })
                    .FirstOrDefaultAsync() ?? new Player();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 根据选手id获取选手游戏设置
        public async Task<MouseSetting> GetMouseSettingById(int playerId)
        {
            try
            {
                return await _playerDbContext.MouseSettings.Where(p => p.PlayerId == playerId).FirstOrDefaultAsync() ?? new MouseSetting();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<CrosshairSetting> GetCrosshairSettingById(int playerId)
        {
            try
            {
                return await _playerDbContext.CrosshairSettings.Where(p => p.PlayerId == playerId).FirstOrDefaultAsync() ?? new CrosshairSetting();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ViewmodelSetting> GetViewmodelSettingById(int playerId)
        {
            try
            {
                return await _playerDbContext.ViewmodelSettings.Where(p => p.PlayerId == playerId).FirstOrDefaultAsync() ?? new ViewmodelSetting();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<VideoSetting> GetVideoSettingById(int playerId)
        {
            try
            {
                return await _playerDbContext.VideoSettings.Where(p => p.PlayerId == playerId).FirstOrDefaultAsync() ?? new VideoSetting();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region 根据选手id和Mouse ID UPDATE MOUSESETTINGS
        public async Task updatePlayerMouseSettingsById(int playerId, int mouseId, MouseSetting mouseSetting)
        {
            try
            {
                MouseSetting mouse = await _playerDbContext.MouseSettings.Where(m => m.PlayerId == playerId && m.MouseId == mouseId).FirstOrDefaultAsync() ?? new MouseSetting();
                if (mouse != null)
                {
                    mouse.MouseName = mouseSetting.MouseName;
                    mouse.MouseDpi = mouseSetting.MouseDpi;
                    mouse.MouseSensitivity = mouseSetting.MouseSensitivity;
                    mouse.MouseEdpi = mouseSetting.MouseEdpi;
                    mouse.MouseZoomSensitivity = mouseSetting.MouseZoomSensitivity;
                    mouse.MouseHz = mouseSetting.MouseHz;
                    mouse.WindowsSensitivity = mouseSetting.WindowsSensitivity;
                    mouse.MouseLastUpdateTime = DateTime.Now;
                    await _playerDbContext.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 根据选手id和Crosshair ID UPDATE CROSSHAIRSETTINGS
        public async Task updatePlayerCrosshairSettingsById(int playerId, int crosshairId, CrosshairSetting crosshairSetting)
        {
            try
            {
                CrosshairSetting crosshair = await _playerDbContext.CrosshairSettings.Where(c => c.PlayerId == playerId && c.CrosshairId == crosshairId).FirstOrDefaultAsync() ?? new CrosshairSetting();
                if (crosshair != null)
                {
                    crosshair.CrosshairCode = crosshairSetting.CrosshairCode;
                    crosshair.CrosshairSize = crosshairSetting.CrosshairSize;
                    crosshair.CrosshairGap = crosshairSetting.CrosshairGap;
                    crosshair.CrosshairThickness = crosshairSetting.CrosshairThickness;
                    crosshair.CrosshairStyle = crosshairSetting.CrosshairStyle;
                    crosshair.CrosshairColorRed = crosshairSetting.CrosshairColorRed;
                    crosshair.CrosshairColorGreen = crosshairSetting.CrosshairColorGreen;
                    crosshair.CrosshairColorBlue = crosshairSetting.CrosshairColorBlue;
                    crosshair.CrosshairLastUpdateTime = DateTime.Now;
                    await _playerDbContext.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 根据选手id和ViewmodelId UPDATE viewmodelSettings
        public async Task updatePlayerViewmodelSettingsById(int playerId, int viewmodelId, ViewmodelSetting viewmodelSetting)
        {
            try
            {
                ViewmodelSetting viewmodel = await _playerDbContext.ViewmodelSettings.Where(v => v.PlayerId == playerId && v.ViewmodelId == viewmodelId).FirstOrDefaultAsync() ?? new ViewmodelSetting();
                if (viewmodel != null)
                {
                    viewmodel.ViewmodelFov = viewmodelSetting.ViewmodelFov;
                    viewmodel.ViewmodelOffsetX = viewmodelSetting.ViewmodelOffsetX;
                    viewmodel.ViewmodelOffsetY = viewmodelSetting.ViewmodelOffsetY;
                    viewmodel.ViewmodelOffsetZ = viewmodelSetting.ViewmodelOffsetZ;
                    viewmodel.ViewmodelPresetpos = viewmodelSetting.ViewmodelPresetpos;
                    viewmodel.ViewmodelCode = viewmodelSetting.ViewmodelCode;
                    viewmodel.ViewmodelLastUpdateTime = DateTime.Now;
                    await _playerDbContext.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 根据选手id和VideoId UPDATE videoSettings
        public async Task updatePlayerVideoSettingsById(int playerId, int videoId, VideoSetting videoSetting)
        {
            try
            {
                VideoSetting video = await _playerDbContext.VideoSettings.Where(v => v.PlayerId == playerId && v.VideoId == videoId).FirstOrDefaultAsync() ?? new VideoSetting();
                if (video != null)
                {
                    video.Resolution = videoSetting.Resolution;
                    video.AspectRatio = videoSetting.AspectRatio;
                    video.ScalingMode = videoSetting.ScalingMode;
                    video.DisplayMode = videoSetting.DisplayMode;
                    video.VideoLastUpdateTime = DateTime.Now;
                    await _playerDbContext.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 更新选手信息
        public async Task<bool> UpdatePlayerInfoByPlayerIdAsync(int playerId, Player player)
        {
            var existingPlayer = await _playerDbContext.Players
         .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (existingPlayer == null)
            {
                return false;
            }
            else
            {
                bool isModified = false;
                if (!string.Equals(existingPlayer.PlayerCover, player.PlayerCover))
                {
                    existingPlayer.PlayerCover = player.PlayerCover;
                    _playerDbContext.Entry(existingPlayer).Property(p => p.PlayerCover).IsModified = true;
                    isModified = true;
                }
                if (!string.Equals(existingPlayer.PlayerTeamLogo, player.PlayerTeamLogo))
                {
                    existingPlayer.PlayerTeamLogo = player.PlayerTeamLogo;
                    _playerDbContext.Entry(existingPlayer).Property(p => p.PlayerTeamLogo).IsModified = true;
                    isModified = true;
                }
                if (!string.Equals(existingPlayer.PlayerTeam, player.PlayerTeam))
                {
                    existingPlayer.PlayerTeam = player.PlayerTeam;
                    _playerDbContext.Entry(existingPlayer).Property(p => p.PlayerTeam).IsModified = true;
                    isModified = true;
                }
                if (!string.Equals(existingPlayer.PlayerTopRanking, player.PlayerTopRanking))
                {
                    existingPlayer.PlayerTopRanking = player.PlayerTopRanking;
                    _playerDbContext.Entry(existingPlayer).Property(p => p.PlayerTopRanking).IsModified = true;
                    isModified = true;
                }
                if (!string.Equals(existingPlayer.PlayerCountry, player.PlayerCountry))
                {
                    existingPlayer.PlayerCountry = player.PlayerCountry;
                    _playerDbContext.Entry(existingPlayer).Property(p => p.PlayerCountry).IsModified = true;
                    isModified = true;
                }
                if (!string.Equals(existingPlayer.PlayerPrizeMoney, player.PlayerPrizeMoney))
                {
                    existingPlayer.PlayerPrizeMoney = player.PlayerPrizeMoney;
                    _playerDbContext.Entry(existingPlayer).Property(p => p.PlayerPrizeMoney).IsModified = true;
                    isModified = true;
                }
                if (!isModified)
                {
                    return true;
                }
            }
            int n = await _playerDbContext.SaveChangesAsync();
            return n > 0;
        }
        #endregion

        #region 新增选手
        public async Task<int> AddPlayerAsync(Player player)
        {
            // 使用事务
            using (var transaction = await _playerDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. 添加 Player 对象
                    _playerDbContext.Players.Add(player);
                    await _playerDbContext.SaveChangesAsync();

                    // 2. 获取刚插入的 PlayerId
                    int playerId = player.PlayerId;

                    // 3. 向其他四张表插入数据
                    _playerDbContext.MouseSettings.Add(new MouseSetting { PlayerId = playerId });
                    _playerDbContext.CrosshairSettings.Add(new CrosshairSetting { PlayerId = playerId });
                    _playerDbContext.ViewmodelSettings.Add(new ViewmodelSetting { PlayerId = playerId });
                    _playerDbContext.VideoSettings.Add(new VideoSetting { PlayerId = playerId });

                    // 4. 保存所有更改
                    await _playerDbContext.SaveChangesAsync();

                    // 5. 提交事务
                    await transaction.CommitAsync();
                    return playerId;
                }
                catch (Exception ex)
                {
                    // 如果有任何错误，回滚事务
                    await transaction.RollbackAsync();
                    throw new Exception("Failed to add player and related data.", ex);
                }
            }
        }
        #endregion

        #region 从Excel导入数据
        public async Task<int> AddTeamByExcelAsync(List<Player> players, List<MouseSetting> mouseSettings, List<CrosshairSetting> crosshairSettings, List<ViewmodelSetting> viewmodelSettings, List<VideoSetting> videoSettings)
        {
            // 验证输入
            if (players == null || players.Count == 0 ||
                players.Count != mouseSettings.Count ||
                players.Count != crosshairSettings.Count ||
                players.Count != viewmodelSettings.Count ||
                players.Count != videoSettings.Count)
            {
                throw new ArgumentException("所有集合必须非空且数量一致");
            }

            using (var transaction = await _playerDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. 批量添加 Players 添加之前，先根据V社排行 进行排序
                    await _playerDbContext.Players.AddRangeAsync(players);
                    await _playerDbContext.SaveChangesAsync(); // 保存以生成 PlayerId

                    // 2. 更新相关设置的 PlayerId
                    for (int i = 0; i < players.Count; i++)
                    {
                        int playerId = players[i].PlayerId; // 获取刚插入的 PlayerId

                        mouseSettings[i].PlayerId = playerId;
                        mouseSettings[i].MouseLastUpdateTime = DateTime.Now;

                        crosshairSettings[i].PlayerId = playerId;
                        crosshairSettings[i].CrosshairLastUpdateTime = DateTime.Now;

                        viewmodelSettings[i].PlayerId = playerId;
                        viewmodelSettings[i].ViewmodelLastUpdateTime = DateTime.Now;

                        videoSettings[i].PlayerId = playerId;
                        videoSettings[i].VideoLastUpdateTime = DateTime.Now;
                    }

                    // 3. 批量添加相关设置
                    await _playerDbContext.MouseSettings.AddRangeAsync(mouseSettings);
                    await _playerDbContext.CrosshairSettings.AddRangeAsync(crosshairSettings);
                    await _playerDbContext.ViewmodelSettings.AddRangeAsync(viewmodelSettings);
                    await _playerDbContext.VideoSettings.AddRangeAsync(videoSettings);
                    await _playerDbContext.SaveChangesAsync();

                    // 4. 提交事务
                    await transaction.CommitAsync();
                    return players.Count; // 返回成功插入的记录数
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Failed to add {players.Count} players and related data.", ex);
                }
            }
        }
        #endregion

        #region 从Excel中导入战队数据(V设排名) 7天一更新
        public async Task<int> AddTeamByExcelAsync(List<Team> teams)
        {
            // 验证输入
            if (teams == null || teams.Count == 0)
            {
                throw new ArgumentException("所有集合必须非空且数量一致");
            }

            using (var transaction = await _playerDbContext.Database.BeginTransactionAsync())
            {
                try
                {

                    await _playerDbContext.Teams.AddRangeAsync(teams);
                    await _playerDbContext.SaveChangesAsync();
                    // 4. 提交事务
                    await transaction.CommitAsync();
                    return teams.Count;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Failed to add {teams.Count} teams and related data.", ex);
                }
            }
        }
        #endregion

        public async Task uCover()
        {
            await _playerDbContext.Players
                .Where(p => string.IsNullOrEmpty(p.PlayerCover))
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.PlayerCover, p => "../../Image/FigureCovers/" + p.PlayerNickName + ".webp"));
        }
    }
}
