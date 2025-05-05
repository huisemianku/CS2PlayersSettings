using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository.Entities.Players;
using CS2PlayersSettings.Data.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.RegularExpressions;


namespace CS2PlayersSettings.Data
{
  
    public class PlayerDataService
    {
        private readonly string _connectionString;
        private readonly PlayerDbContext _playerDbContext;
        private readonly ILogger<PlayerDataService> _logger;
        private static readonly DateTime DefaultDate = new DateTime(1900, 1, 1);

        public PlayerDataService(PlayerDbContext playerDbContext, IConfiguration configuration, ILogger<PlayerDataService> logger)
        {
            _playerDbContext = playerDbContext;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentException("连接字符串未在 appsettings.json 中正确配置");
            }
            _logger = logger;
        }

        public async Task<ExcelUpsertResult> UpsertPlayers(
            List<Player> players,
            List<MouseSetting> mouseSettings,
            List<CrosshairSetting> crosshairSettings,
            List<ViewmodelSetting> viewmodelSettings,
            List<VideoSetting> videoSettings)
        {
            if (players == null || players.Count == 0 ||
                players.Count != mouseSettings.Count ||
                players.Count != crosshairSettings.Count ||
                players.Count != viewmodelSettings.Count ||
                players.Count != videoSettings.Count)
            {
                throw new ArgumentException("所有集合必须非空且数量一致");
            }

            using var transaction = await _playerDbContext.Database.BeginTransactionAsync();
            try
            {
                var existingPlayers = await _playerDbContext.Players
                    .AsNoTracking()
                    .ToDictionaryAsync(p => p.PlayerNickName, StringComparer.OrdinalIgnoreCase);

                var playersToInsert = new List<Player>();
                var relatedToInsert = (Mouse: new List<MouseSetting>(),
                                      Crosshair: new List<CrosshairSetting>(),
                                      Viewmodel: new List<ViewmodelSetting>(),
                                      Video: new List<VideoSetting>());

                var playersToUpdate = new List<Player>();
                var relatedToUpdate = (Mouse: new List<MouseSetting>(),
                                      Crosshair: new List<CrosshairSetting>(),
                                      Viewmodel: new List<ViewmodelSetting>(),
                                      Video: new List<VideoSetting>());

                for (int i = 0; i < players.Count; i++)
                {
                    var newPlayer = players[i];
                    if (existingPlayers.TryGetValue(newPlayer.PlayerNickName, out var dbPlayer))
                    {
                        newPlayer.PlayerId = dbPlayer.PlayerId;
                        playersToUpdate.Add(newPlayer);
                        relatedToUpdate.Mouse.Add(mouseSettings[i]);
                        relatedToUpdate.Crosshair.Add(crosshairSettings[i]);
                        relatedToUpdate.Viewmodel.Add(viewmodelSettings[i]);
                        relatedToUpdate.Video.Add(videoSettings[i]);
                    }
                    else
                    {
                        playersToInsert.Add(newPlayer);
                        relatedToInsert.Mouse.Add(mouseSettings[i]);
                        relatedToInsert.Crosshair.Add(crosshairSettings[i]);
                        relatedToInsert.Viewmodel.Add(viewmodelSettings[i]);
                        relatedToInsert.Video.Add(videoSettings[i]);
                    }
                }

                int insertedCount = 0;
                string insertLog = string.Empty;
                if (playersToInsert.Count > 0)
                {
                    var insertResult = await InsertPlayersWithRelated(playersToInsert, relatedToInsert);
                    insertedCount = insertResult.insertedCount;
                    insertLog = insertResult.insertLog;
                }

                int updatedCount = 0;
                string updateLog = string.Empty;
                if (playersToUpdate.Count > 0)
                {
                    var updateResult = await UpdatePlayersWithRelated(playersToUpdate, relatedToUpdate);
                    updatedCount = updateResult.updatedCount;
                    updateLog = updateResult.updateLog;
                }

                await transaction.CommitAsync();
                return new ExcelUpsertResult
                {
                    InsertedCount = insertedCount,
                    UpdatedCount = updatedCount,
                    UpdateLog = $"{insertLog}\n{updateLog}".Trim()
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "更新或插入玩家失败");
                throw;
            }   
        }

        private async Task<(int insertedCount, string insertLog)> InsertPlayersWithRelated(List<Player> players,
            (List<MouseSetting> Mouse, List<CrosshairSetting> Crosshair, List<ViewmodelSetting> Viewmodel, List<VideoSetting> Video) related)
        {
            var teams = await _playerDbContext.Teams.ToListAsync();
            var insertDetails = new List<ExcelPlayerUpdateDetail>();

            foreach (var player in players)
            {
                if (Regex.IsMatch(player.PlayerTeam ?? "", @"\bTeam\b|\bEsports\b", RegexOptions.IgnoreCase))
                {
                    player.PlayerTeam = CleanTeamName(player.PlayerTeam!);
                }
                var matchTeam = teams.FirstOrDefault(t => string.Equals(t.TeamName, player.PlayerTeam,StringComparison.OrdinalIgnoreCase));
                player.TeamId = matchTeam?.TeamId ?? 413;

                player.PlayerBirthday = player.PlayerBirthday == DateTime.MinValue ? new DateTime(1900, 1, 1) : player.PlayerBirthday;
            }

            _playerDbContext.Players.AddRange(players);
            await _playerDbContext.SaveChangesAsync();

            for (int i = 0; i < players.Count; i++)
            {
                DateTime inserTime = DateTime.Now;
                var detail = new ExcelPlayerUpdateDetail { PlayerNickName = players[i].PlayerNickName, UpdateTime = inserTime };
                related.Mouse[i].PlayerId = players[i].PlayerId;
                related.Mouse[i].MouseLastUpdateTime = inserTime;
                related.Crosshair[i].PlayerId = players[i].PlayerId;
                related.Crosshair[i].CrosshairLastUpdateTime = inserTime;
                related.Viewmodel[i].PlayerId = players[i].PlayerId;
                related.Viewmodel[i].ViewmodelLastUpdateTime = inserTime;
                related.Video[i].PlayerId = players[i].PlayerId;
                related.Video[i].VideoLastUpdateTime = inserTime;


                // 确保相关设置有值，如果为空则设置默认值
                //if (string.IsNullOrEmpty(related.Mouse[i].MouseName)) related.Mouse[i].MouseName = "Unknown";
                //if (related.Mouse[i].MouseDpi == 0) related.Mouse[i].MouseDpi = 400; // 默认值示例
                //if (related.Mouse[i].MouseSensitivity == 0) related.Mouse[i].MouseSensitivity = 1.0f; // 默认值示例

                //if (string.IsNullOrEmpty(related.Crosshair[i].CrosshairCode)) related.Crosshair[i].CrosshairCode = "CSGO-Default"; // 默认值示例

                //if (related.Viewmodel[i].ViewmodelFov == 0) related.Viewmodel[i].ViewmodelFov = 60; // 默认值示例

                //related.Video[i].Resolution = ExtractFirstResolution(related.Video[i].Resolution) ?? "1920x1080"; // 默认值示例
                //related.Video[i].AspectRatio = ExtractFirstAspectRatio(related.Video[i].AspectRatio) ?? "16:9"; // 默认值示例

                // 记录插入的字段
                detail.UpdatedFields.AddRange(new[] { "Player", "MouseSettings", "CrosshairSettings", "ViewmodelSettings", "VideoSettings" });
                insertDetails.Add(detail);
            }

            _playerDbContext.MouseSettings.AddRange(related.Mouse);
            _playerDbContext.CrosshairSettings.AddRange(related.Crosshair);
            _playerDbContext.ViewmodelSettings.AddRange(related.Viewmodel);
            _playerDbContext.VideoSettings.AddRange(related.Video);
            await _playerDbContext.SaveChangesAsync();

            // 生成插入日志
            var logBuilder = new StringBuilder();
            if (insertDetails.Any())
            {
                logBuilder.AppendLine("玩家设置插入日志:");
                foreach (var detail in insertDetails)
                {
                    logBuilder.AppendLine($"玩家: {detail.PlayerNickName}");
                    logBuilder.AppendLine($"插入字段: {string.Join(", ", detail.UpdatedFields)}");
                    logBuilder.AppendLine($"插入时间: {detail.UpdateTime:yyyy-MM-dd HH:mm:ss}");
                    logBuilder.AppendLine("---");
                }
            }

            return (players.Count, logBuilder.ToString());
        }

        private async Task<(int updatedCount, string updateLog)> UpdatePlayersWithRelated(List<Player> players,
            (List<MouseSetting> Mouse, List<CrosshairSetting> Crosshair, List<ViewmodelSetting> Viewmodel, List<VideoSetting> Video) related)
        {
            var playerNickNames = players.Select(p => p.PlayerNickName).ToList();
            var dbPlayers = await _playerDbContext.Players
                .Where(p => playerNickNames.Contains(p.PlayerNickName))
                .Include(p => p.MouseSettings)
                .Include(p => p.CrosshairSettings)
                .Include(p => p.ViewmodelSettings)
                .Include(p => p.VideoSettings)
                .ToListAsync();

            var dbPlayerDict = dbPlayers.ToDictionary(p => p.PlayerNickName);
            var updateDetails = new List<ExcelPlayerUpdateDetail>();
            int updatedCount = 0;
            int index = 0;
            for (int i = 0; i < players.Count; i++)
            {
                var newPlayer = players[i];
                if (dbPlayerDict.TryGetValue(newPlayer.PlayerNickName, out var dbPlayer))
                {
                    var detail = new ExcelPlayerUpdateDetail { PlayerNickName = newPlayer.PlayerNickName };
                    bool hasChanges = false;

                    if (await UpdatePlayerFields(dbPlayer, newPlayer, detail))
                    {
                        hasChanges = true;
                    }

                    hasChanges |= UpdateOrAddRelatedEntity(dbPlayer.MouseSettings.FirstOrDefault(), related.Mouse[i], newPlayer.PlayerId, _playerDbContext.MouseSettings, detail);
                    hasChanges |= UpdateOrAddRelatedEntity(dbPlayer.CrosshairSettings.FirstOrDefault(), related.Crosshair[i], newPlayer.PlayerId, _playerDbContext.CrosshairSettings, detail);
                    hasChanges |= UpdateOrAddRelatedEntity(dbPlayer.ViewmodelSettings.FirstOrDefault(), related.Viewmodel[i], newPlayer.PlayerId, _playerDbContext.ViewmodelSettings, detail);
                    hasChanges |= UpdateOrAddRelatedEntity(dbPlayer.VideoSettings.FirstOrDefault(), related.Video[i], newPlayer.PlayerId, _playerDbContext.VideoSettings, detail, true);

                    if (hasChanges)
                    {
                        detail.UpdateTime = DateTime.Now;
                        updateDetails.Add(detail);
                        updatedCount++;
                    }
                }
                index++;
            }

            await _playerDbContext.SaveChangesAsync();

            var logBuilder = new StringBuilder();
            if (updateDetails.Any())
            {
                logBuilder.AppendLine("玩家设置更新日志:");
                foreach (var detail in updateDetails)
                {
                    logBuilder.AppendLine($"玩家: {detail.PlayerNickName}");
                    logBuilder.AppendLine($"更新字段: {string.Join(", ", detail.UpdatedFields)}");
                    logBuilder.AppendLine($"更新时间: {detail.UpdateTime:yyyy-MM-dd HH:mm:ss}");
                    logBuilder.AppendLine("---");
                }
            }
            else
            {
                logBuilder.AppendLine("无玩家设置更新。");
            }

            return (updatedCount, logBuilder.ToString());
        }

        private async Task<bool> UpdatePlayerFields(Player dbPlayer, Player newPlayer, ExcelPlayerUpdateDetail detail)
        {
            bool hasChanges = false;

            if (!string.Equals(newPlayer.PlayerName, dbPlayer.PlayerName))
            {
                dbPlayer.PlayerName = newPlayer.PlayerName;
                detail.UpdatedFields.Add("PlayerName");
                hasChanges = true;
            }
            if (!string.Equals(newPlayer.PlayerNickName, dbPlayer.PlayerNickName))
            {
                dbPlayer.PlayerNickName = newPlayer.PlayerNickName;
                detail.UpdatedFields.Add("PlayerNickName");
                hasChanges = true;
            }
            if (newPlayer.PlayerBirthday != dbPlayer.PlayerBirthday)
            {
                dbPlayer.PlayerBirthday = newPlayer.PlayerBirthday;
                detail.UpdatedFields.Add("PlayerBirthday");
                hasChanges = true;
            }
            if (!string.Equals(newPlayer.PlayerCountry, dbPlayer.PlayerCountry))
            {
                dbPlayer.PlayerCountry = newPlayer.PlayerCountry;
                detail.UpdatedFields.Add("PlayerCountry");
                hasChanges = true;
            }
            if (!string.Equals(newPlayer.PlayerTeam, dbPlayer.PlayerTeam, StringComparison.OrdinalIgnoreCase))
            {
                dbPlayer.PlayerTeam = newPlayer.PlayerTeam;
                detail.UpdatedFields.Add("PlayerTeam");
                hasChanges = true;
            }
            var teams = await _playerDbContext.Teams.ToListAsync();
            var matchTeam = teams.FirstOrDefault(t => string.Equals(t.TeamName, CleanTeamName(dbPlayer.PlayerTeam), StringComparison.OrdinalIgnoreCase));
            newPlayer.TeamId = matchTeam?.TeamId ?? 413;
            if (newPlayer.TeamId != dbPlayer.TeamId)
            {
                dbPlayer.TeamId = newPlayer.TeamId;
                detail.UpdatedFields.Add("TeamId");
                hasChanges = true;
            }

            if (hasChanges)
            {
                _playerDbContext.Entry(dbPlayer).State = EntityState.Modified;
            }
            return hasChanges;
        }

        private bool UpdateOrAddRelatedEntity<T>(T dbEntity, T newEntity, int playerId, DbSet<T> dbSet, ExcelPlayerUpdateDetail detail, bool isVideo = false) where T : class
        {
            if (newEntity == null) return false;

            bool hasChanges = false;

            if (dbEntity == null)
            {
                switch (newEntity)
                {
                    case MouseSetting mouse:
                        mouse.PlayerId = playerId;
                        _playerDbContext.MouseSettings.Add(mouse);
                        detail.UpdatedFields.Add("MouseSettings (New)");
                        hasChanges = true;
                        break;
                    case CrosshairSetting crosshair:
                        crosshair.PlayerId = playerId;
                        _playerDbContext.CrosshairSettings.Add(crosshair);
                        detail.UpdatedFields.Add("CrosshairSettings (New)");
                        hasChanges = true;
                        break;
                    case ViewmodelSetting viewmodel:
                        viewmodel.PlayerId = playerId;
                        _playerDbContext.ViewmodelSettings.Add(viewmodel);
                        detail.UpdatedFields.Add("ViewmodelSettings (New)");
                        hasChanges = true;
                        break;
                    case VideoSetting video:
                        video.PlayerId = playerId;
                        video.Resolution = ExtractFirstResolution(video.Resolution) ?? "1920x1080"; // 默认值
                        video.AspectRatio = ExtractFirstAspectRatio(video.AspectRatio) ?? "16:9"; // 默认值
                        _playerDbContext.VideoSettings.Add(video);
                        detail.UpdatedFields.Add("VideoSettings (New)");
                        hasChanges = true;
                        break;
                }
            }
            else
            {
                var dbEntry = _playerDbContext.Entry(dbEntity);

                switch (newEntity)
                {
                    case MouseSetting newMouse when dbEntity is MouseSetting dbMouse:
                        if (!string.Equals(newMouse.MouseName, dbMouse.MouseName))
                        {
                            dbMouse.MouseName = newMouse.MouseName;
                            dbMouse.MouseLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("MouseSettings.MouseName");
                            hasChanges = true;
                        }
                        if (!string.Equals(newMouse.MouseDpi.ToString(), dbMouse.MouseDpi.ToString()))
                        {
                            dbMouse.MouseDpi = newMouse.MouseDpi;
                            dbMouse.MouseLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("MouseSettings.MouseDpi");
                            hasChanges = true;
                        }
                        if (!string.Equals(newMouse.MouseSensitivity.ToString(), dbMouse.MouseSensitivity.ToString()))
                        {
                            dbMouse.MouseSensitivity = newMouse.MouseSensitivity;
                            dbMouse.MouseLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("MouseSettings.MouseSensitivity");
                            hasChanges = true;
                        }
                        if (!string.Equals(newMouse.MouseEdpi.ToString(), dbMouse.MouseEdpi.ToString()))
                        {
                            dbMouse.MouseEdpi = newMouse.MouseEdpi;
                            dbMouse.MouseLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("MouseSettings.MouseEdpi");
                            hasChanges = true;
                        }
                        if (!string.Equals(newMouse.MouseZoomSensitivity.ToString(), dbMouse.MouseZoomSensitivity.ToString()))
                        {
                            dbMouse.MouseZoomSensitivity = newMouse.MouseZoomSensitivity;
                            dbMouse.MouseLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("MouseSettings.MouseZoomSensitivity");
                            hasChanges = true;
                        }
                        if (!string.Equals(newMouse.MouseHz.ToString(), dbMouse.MouseHz.ToString()))
                        {
                            dbMouse.MouseHz = newMouse.MouseHz;
                            dbMouse.MouseLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("MouseSettings.MouseHz");
                            hasChanges = true;
                        }
                        if (!string.Equals(newMouse.WindowsSensitivity.ToString(), dbMouse.WindowsSensitivity.ToString()))
                        {
                            dbMouse.WindowsSensitivity = newMouse.WindowsSensitivity;
                            dbMouse.MouseLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("MouseSettings.WindowsSensitivity");
                            hasChanges = true;
                        }
                        break;

                    case CrosshairSetting newCrosshair when dbEntity is CrosshairSetting dbCrosshair:
                        if (!string.Equals(newCrosshair.CrosshairCode, dbCrosshair.CrosshairCode, StringComparison.OrdinalIgnoreCase))
                        {
                            dbCrosshair.CrosshairCode = newCrosshair.CrosshairCode;
                            dbCrosshair.CrosshairLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("CrosshairSettings.CrosshairCode");
                            hasChanges = true;
                        }
                        break;

                    case ViewmodelSetting newViewmodel when dbEntity is ViewmodelSetting dbViewmodel:
                        if (newViewmodel.ViewmodelFov != dbViewmodel.ViewmodelFov)
                        {
                            dbViewmodel.ViewmodelFov = newViewmodel.ViewmodelFov;
                            dbViewmodel.ViewmodelLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("ViewmodelSettings.ViewmodelFov");
                            hasChanges = true;
                        }
                        if (newViewmodel.ViewmodelOffsetX != dbViewmodel.ViewmodelOffsetX)
                        {
                            dbViewmodel.ViewmodelOffsetX = newViewmodel.ViewmodelOffsetX;
                            detail.UpdatedFields.Add("ViewmodelSettings.ViewmodelOffsetX");
                            dbViewmodel.ViewmodelLastUpdateTime = DateTime.Now;
                            hasChanges = true;
                        }
                        if (newViewmodel.ViewmodelOffsetY != dbViewmodel.ViewmodelOffsetY)
                        {
                            dbViewmodel.ViewmodelOffsetY = newViewmodel.ViewmodelOffsetY;
                            dbViewmodel.ViewmodelLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("ViewmodelSettings.ViewmodelOffsetY");
                            hasChanges = true;
                        }
                        if (newViewmodel.ViewmodelOffsetZ != dbViewmodel.ViewmodelOffsetZ)
                        {
                            dbViewmodel.ViewmodelOffsetZ = newViewmodel.ViewmodelOffsetZ;
                            dbViewmodel.ViewmodelLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("ViewmodelSettings.ViewmodelOffsetZ");
                            hasChanges = true;
                        }
                        if (newViewmodel.ViewmodelPresetpos != dbViewmodel.ViewmodelPresetpos)
                        {
                            dbViewmodel.ViewmodelPresetpos = newViewmodel.ViewmodelPresetpos;
                            dbViewmodel.ViewmodelLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("ViewmodelSettings.ViewmodelPresetpos");
                            hasChanges = true;
                        }
                        break;

                    case VideoSetting newVideo when dbEntity is VideoSetting dbVideo:
                        var cleanedResolution = ExtractFirstResolution(newVideo.Resolution);
                        var cleanedAspectRatio = ExtractFirstAspectRatio(newVideo.AspectRatio);
                        if (!string.Equals(dbVideo.Resolution, cleanedResolution, StringComparison.OrdinalIgnoreCase))
                        {
                            dbVideo.Resolution = cleanedResolution;
                            dbVideo.VideoLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("VideoSettings.Resolution");
                            hasChanges = true;
                        }
                        if (!string.Equals(dbVideo.AspectRatio, cleanedAspectRatio, StringComparison.OrdinalIgnoreCase))
                        {
                            dbVideo.AspectRatio = cleanedAspectRatio;
                            dbVideo.VideoLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("VideoSettings.AspectRatio");
                            hasChanges = true;
                        }
                        if (!string.Equals(dbVideo.ScalingMode, newVideo.ScalingMode, StringComparison.OrdinalIgnoreCase))
                        {
                            dbVideo.ScalingMode = newVideo.ScalingMode;
                            dbVideo.VideoLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("VideoSettings.ScalingMode");
                            hasChanges = true;
                        }
                        if (!string.Equals(dbVideo.DisplayMode, newVideo.DisplayMode, StringComparison.OrdinalIgnoreCase))
                        {
                            dbVideo.DisplayMode = newVideo.DisplayMode;
                            dbVideo.VideoLastUpdateTime = DateTime.Now;
                            detail.UpdatedFields.Add("VideoSettings.DisplayMode");
                            hasChanges = true;
                        }
                        break;
                }

                if (hasChanges)
                {
                    dbEntry.State = EntityState.Modified;
                }
            }

            return hasChanges;
        }

        private static string CleanTeamName(string teamName)
        {
            if (string.IsNullOrEmpty(teamName))
                return teamName;

            string cleanedName = Regex.Replace(teamName, @"\bTeam\b|\bEsports\b", "", RegexOptions.IgnoreCase);
            cleanedName = Regex.Replace(cleanedName, @"\s+", " ").Trim();
            return cleanedName;
        }

        private static string ExtractFirstAspectRatio(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            string pattern = @"^\d+:\d+";
            var match = Regex.Match(input, pattern);
            return match.Success ? match.Value : null;
        }

        private static string ExtractFirstResolution(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            string pattern = @"^\d+x\d+";
            var match = Regex.Match(input, pattern);
            return match.Success ? match.Value : null;
        }
    }
}