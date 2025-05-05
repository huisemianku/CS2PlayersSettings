using ClosedXML.Excel;
using CS2PlayersSettings.Business;
using CS2PlayersSettings.Data.Repository.Entities.Players;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace CS2PlayersSettings.Pages.PlayerFile
{
    public class IndexModel : PageModel
    {
        private readonly PlayerBLL _playerBLL;
        public IndexModel(PlayerBLL playerBLL)
        {
            _playerBLL = playerBLL;
        }

        public IPagedList<Player> Players { get; set; }

        // page index
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        // page size data
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        // page search data

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet(int pageIndex = 1, int pageSize = 10)
        {
            PageIndex = pageIndex > 0 ? pageIndex : 0;
            PageSize = pageSize > 0 ? pageSize : 5;
            //await _playerBLL.uCover();
            Players = await _playerBLL.GetPlayersAsync(pageIndex, pageSize, SearchTerm);
            if (Request.Headers["Accept"] == "application/json")
            {
                return new JsonResult(new
                {
                    players = Players.Select(p => new
                    {
                        playerId = p.PlayerId,
                        playerNickName = p.PlayerNickName,
                        playerCover = p.PlayerCover,
                        playerTeam = p.PlayerTeam,
                        playerTeamLogo = p.PlayerTeamLogo
                    })
                });

            }

            // 正常页面请求返回页面
            return Page();
        }

        public async Task<IActionResult> OnGetExportPlayerAsync()
        {
            try
            {
                var players = await _playerBLL.GetAllPlayersAsync();
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Players");

                    // 设置表头
                    worksheet.Cell(1, 1).Value = "Player ID";
                    worksheet.Cell(1, 2).Value = "Nickname";
                    worksheet.Cell(1, 3).Value = "Cover";
                    worksheet.Cell(1, 4).Value = "Team";

                    // 填充数据
                    for (int i = 0; i < players.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = players[i].PlayerId;
                        worksheet.Cell(i + 2, 2).Value = players[i].PlayerNickName;
                        worksheet.Cell(i + 2, 3).Value = players[i].PlayerCover;
                        worksheet.Cell(i + 2, 4).Value = players[i].PlayerTeam;
                    }

                    // 自动调整列宽
                    worksheet.Columns().AdjustToContents();

                    // 生成文件流
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Position = 0;

                        // 设置文件名
                        string fileName = $"Players_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                        // 返回文件流，触发下载
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                // 返回 JSON 错误
                return new JsonResult(new { success = false, message = $"导出失败：{ex.Message}" });
            }
        }

    }
}
