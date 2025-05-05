using ClosedXML.Excel;
using CS2PlayersSettings.Business;
using CS2PlayersSettings.Data.Repository.Entities.Players;
using CS2PlayersSettings.Data.Repository.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace CS2PlayersSettings.Data.Repository.Helper
{
    public class ImportDataByExcel
    {
        private readonly PlayerBLL _playerBLL;
        private readonly PlayerDataService _playerDataService;
        public ImportDataByExcel(PlayerBLL playerBLL, PlayerDataService playerDataService)
        {
            _playerBLL = playerBLL;
            _playerDataService = playerDataService;

        }
        #region 通过Excel导入选手设置
        public async Task<IActionResult> GetDataByExcel(IFormFile ExcelFile)
        {
            if (ExcelFile == null || ExcelFile.Length == 0)
            {
                return new JsonResult(new { success = false, message = "请上传一个有效的 Excel 文件" });
            }

            var fileExtension = Path.GetExtension(ExcelFile.FileName).ToLower();
            if (fileExtension != ".xlsx")
            {
                return new JsonResult(new { success = false, message = "仅支持 .xlsx 文件" });
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await ExcelFile.CopyToAsync(stream);
                    using (var workbook = new XLWorkbook(stream))
                    {
                        var requiredHeaders = new List<string>
                        {
                            "Name", "NickName", "Team", "Country", "Birthday", "MouseName", "MouseDPI", "MouseSensitivity", "MouseEDPI", "ZoomSensitivity",
                            "MouseHz", "WindowsSens", "viewModelFOV", "viewModelOffsetX", "viewModelOffsetY", "viewModelOffsetZ",
                            "ViewModelPresetpos", "Resolution", "AspectRatio", "ScalingMode", "DisplayMode", "Crosshaircode"
                        };

                        var worksheet = workbook.Worksheet(1);
                        var firstRow = worksheet.Row(1);
                        var headersInExcel = firstRow.CellsUsed().Select(cell => cell.GetString().Trim()).ToList();

                        var rowCount = worksheet.RowsUsed().Count();
                        if (requiredHeaders.All(header => headersInExcel.Contains(header)) && headersInExcel.SequenceEqual(requiredHeaders))
                        {
                            if (rowCount < 2)
                            {
                                return new JsonResult(new { success = false, message = "Excel 文件中没有数据" });
                            }

                            List<Player> players = new List<Player>();
                            List<MouseSetting> mouseSettings = new List<MouseSetting>();
                            List<CrosshairSetting> crosshairSettings = new List<CrosshairSetting>();
                            List<ViewmodelSetting> viewmodelSettings = new List<ViewmodelSetting>();
                            List<VideoSetting> videoSettings = new List<VideoSetting>();

                            Dictionary<string, int> headerMap = new Dictionary<string, int>();
                            foreach (var cell in worksheet.FirstRowUsed().CellsUsed())
                            {
                                headerMap[cell.Value.ToString().Trim()] = cell.Address.ColumnNumber;
                            }

                            for (int row = 2; row <= rowCount; row++)
                            {
                                players.Add(new Player
                                {
                                    PlayerName = worksheet.Cell(row, headerMap["Name"]).GetString()?.Trim() ?? "Unknown",
                                    PlayerNickName = worksheet.Cell(row, headerMap["NickName"]).GetString()?.Trim() ?? "Unknown",
                                    PlayerTeam = worksheet.Cell(row, headerMap["Team"]).GetString()?.Trim() ?? "Unknown",
                                    PlayerCountry = worksheet.Cell(row, headerMap["Country"]).GetString()?.Trim() ?? "Unknown",
                                    PlayerBirthday = DateTime.TryParse(worksheet.Cell(row, headerMap["Birthday"]).GetString()?.Trim(), out var parsedDate) ? parsedDate : DateTime.MinValue
                                });

                                mouseSettings.Add(new MouseSetting
                                {
                                    MouseName = worksheet.Cell(row, headerMap["MouseName"]).GetString()?.Trim() ?? "Unknown",
                                    MouseDpi = int.TryParse(worksheet.Cell(row, headerMap["MouseDPI"]).GetString()?.Trim(), out var mouseDpiResult) ? mouseDpiResult : 0,
                                    MouseSensitivity = double.TryParse(worksheet.Cell(row, headerMap["MouseSensitivity"]).GetString()?.Trim(), out var mouseSensitivityResult) ? mouseSensitivityResult : 0,
                                    MouseEdpi = double.TryParse(worksheet.Cell(row, headerMap["MouseEDPI"]).GetString()?.Trim(), out var mouseEdpiResult) ? mouseEdpiResult : 0,
                                    MouseZoomSensitivity = double.TryParse(worksheet.Cell(row, headerMap["ZoomSensitivity"]).GetString()?.Trim(), out var mouseZoomSensitivityResult) ? mouseZoomSensitivityResult : 0,
                                    MouseHz = int.TryParse(worksheet.Cell(row, headerMap["MouseHz"]).GetString()?.Trim(), out var mouseHzResult) ? mouseHzResult : 0,
                                    WindowsSensitivity = int.TryParse(worksheet.Cell(row, headerMap["WindowsSens"]).GetString()?.Trim(), out var windowsSensResult) ? windowsSensResult : 0
                                });

                                viewmodelSettings.Add(new ViewmodelSetting
                                {
                                    ViewmodelFov = double.TryParse(worksheet.Cell(row, headerMap["viewModelFOV"]).GetString()?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var viewModelFovResult) ? viewModelFovResult : 0.0,
                                    ViewmodelOffsetX = double.TryParse(worksheet.Cell(row, headerMap["viewModelOffsetX"]).GetString()?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var viewModelOffsetXResult) ? viewModelOffsetXResult : 0.0,
                                    ViewmodelOffsetY = double.TryParse(worksheet.Cell(row, headerMap["viewModelOffsetY"]).GetString()?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var viewModelOffsetYResult) ? viewModelOffsetYResult : 0.0,
                                    ViewmodelOffsetZ = double.TryParse(worksheet.Cell(row, headerMap["viewModelOffsetZ"]).GetString()?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var viewModelOffsetZResult) ? viewModelOffsetZResult : 0.0,
                                    ViewmodelPresetpos = double.TryParse(worksheet.Cell(row, headerMap["ViewModelPresetpos"]).GetString()?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var viewModelPresetposResult) ? viewModelPresetposResult : 0.0
                                });

                                videoSettings.Add(new VideoSetting
                                {
                                    Resolution = worksheet.Cell(row, headerMap["Resolution"]).GetString()?.Trim() ?? "Unknown",
                                    AspectRatio = worksheet.Cell(row, headerMap["AspectRatio"]).GetString()?.Trim() ?? "Unknown",
                                    ScalingMode = worksheet.Cell(row, headerMap["ScalingMode"]).GetString()?.Trim() ?? "Unknown",
                                    DisplayMode = worksheet.Cell(row, headerMap["DisplayMode"]).GetString()?.Trim() ?? "Unknown"
                                });

                                crosshairSettings.Add(new CrosshairSetting
                                {
                                    CrosshairCode = worksheet.Cell(row, headerMap["Crosshaircode"]).GetString()?.Trim() ?? "Unknown"
                                });
                            }

                            ExcelUpsertResult result = await _playerDataService.UpsertPlayers(players, mouseSettings, crosshairSettings, viewmodelSettings, videoSettings);
                            if (result.InsertedCount == 0 && result.UpdatedCount == 0)
                            {
                                return new JsonResult(new
                                {
                                    success = true,
                                    message = $"未进行任何更新！"
                                });
                            }
                            else
                            {
                                try
                                {
                                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                                    // 构建完整的文件路径
                                    string filePath = Path.Combine(desktopPath, "update_log.txt");

                                    // 写入文件
                                    File.WriteAllText(filePath, result.UpdateLog);
                                }
                                catch (Exception)
                                {

                                    throw;
                                }
                                return new JsonResult(new
                                {
                                    success = true,
                                    message = $"新增记录数: {result.InsertedCount}, 更新记录数: {result.UpdatedCount}"
                                });
                            }
                        }
                        else
                        {
                            return new JsonResult(new { success = false, message = "Excel 表中的表头格式不正确！" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "导入失败",
                    error = ex.Message
                });
            }
        }
        #endregion



        #region 通过Excel导入战队(Value Ranking)
        public async Task<IActionResult> GetTeamDataByExcel(IFormFile ExcelFile)
        {
            if (ExcelFile == null || ExcelFile.Length == 0)
            {
                return new JsonResult(new { success = false, message = "请上传一个有效的 Excel 文件" });
            }

            var fileExtension = Path.GetExtension(ExcelFile.FileName).ToLower();
            if (fileExtension != ".xlsx")
            {
                return new JsonResult(new { success = false, message = "仅支持 .xlsx 文件" });
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await ExcelFile.CopyToAsync(stream);
                    using (var workbook = new XLWorkbook(stream))
                    {
                        var requiredHeaders = new List<string>
                     {
                        "V社排行", "战队名称"
                     };

                        var worksheet = workbook.Worksheet(1);
                        var firstRow = worksheet.Row(1);
                        var headersInExcel = firstRow.CellsUsed().Select(cell => cell.GetString().Trim()).ToList();

                        var rowCount = worksheet.RowsUsed().Count();
                        if (requiredHeaders.All(header => headersInExcel.Contains(header)) && headersInExcel.SequenceEqual(requiredHeaders))
                        {
                            if (rowCount < 2)
                            {
                                return new JsonResult(new { success = false, message = "Excel 文件中没有数据" });
                            }

                            // 初始化集合
                            List<Team> teams = new List<Team>();

                            // 读取表头映射
                            Dictionary<string, int> headerMap = new Dictionary<string, int>();
                            foreach (var cell in worksheet.FirstRowUsed().CellsUsed())
                            {
                                headerMap[cell.Value.ToString().Trim()] = cell.Address.ColumnNumber;
                            }

                            // 遍历每行并填充集合
                            for (int row = 2; row <= rowCount; row++)
                            {
                                // Player
                                if (headerMap.ContainsKey("V社排行") && headerMap.ContainsKey("战队名称"))
                                {
                                    teams.Add(new Team
                                    {
                                        TeamRanking = int.TryParse(worksheet.Cell(row, headerMap["V社排行"]).GetString()?.Trim(), out var teamRanking) ? teamRanking : 0,
                                        TeamName = worksheet.Cell(row, headerMap["战队名称"]).GetString()?.Trim() ?? "Unknown"
                                    });
                                }
                            }
                            var playersToInsert = teams
    .GroupBy(t => t.TeamName) // 按 PlayerTeam 分组
    .Select(group => group.OrderBy(player => player.TeamRanking).First()) // 按 Rank 排序，并选择排名靠前的
    .ToList();
                            int teamCount = await _playerBLL.AddTeamByExcelAsync(playersToInsert);
                            return new JsonResult(new
                            {
                                success = true,
                                message = $"成功导入 {teamCount} 条战队数据！",
                            });
                        }
                        else
                        {
                            return new JsonResult(new { success = false, message = "Excel 表中的表头格式不正确！" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "导入失败", error = ex.Message });
            }
        }
        #endregion

    }
}
