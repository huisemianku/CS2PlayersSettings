using CS2PlayersSettings.Business;
using CS2PlayersSettings.Data.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CS2PlayersSettings.Pages.PlayerFile
{
    public class PlayerInfoUpdateModel : PageModel
    {
        private readonly PlayerBLL _playerBLL;

        [BindProperty]
        public Player Player { get; set; } = null!;
        [BindProperty]
        public List<Team> Teams { get; set; } = null!;

        private readonly IWebHostEnvironment _environment;

        public PlayerInfoUpdateModel(PlayerBLL playerBLL, IWebHostEnvironment environment)
        {
            _playerBLL = playerBLL;
            _environment = environment;
        }

        public async Task OnGet(int playerId)
        {
            Player = await _playerBLL.GeyPlayerDetailsById(playerId);
            Teams = await _playerBLL.GetAllTeam();
        }

        public async Task<IActionResult> OnPostUpdatePlayerAsync()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "����ʧ��" });
            }

            try
            {
                // ���������Ϣ
                await _playerBLL.UpdatePlayerInfoByPlayerIdAsync(Player.PlayerId, Player);
                return new JsonResult(new { success = true, message = "����ɹ�" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"����ʧ��: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnPostUploadImageOnlyAsync(IFormFile file, string imageType)
        {
            if (file == null || file.Length == 0)
            {
                return new JsonResult(new { success = false, message = "���ϴ���Ч�ļ�" });
            }
            try
            {
                // ����imageTypeȷ���ϴ�Ŀ¼
                string uploadsFolder;
                string relativePath;

                if (imageType == "FigureCover")
                {
                    uploadsFolder = Path.Combine(_environment.WebRootPath, "Image", "FigureCovers");
                    relativePath = "/Image/FigureCovers";
                }
                else if (imageType == "TeamLogo")
                {
                    uploadsFolder = Path.Combine(_environment.WebRootPath, "Image", "TeamLogo");
                    relativePath = "/Image/TeamLogo";
                }
                else
                {
                    return new JsonResult(new { success = false, message = "��Ч��ͼƬ����" });
                }

                // ȷ��Ŀ¼����
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // ����Ψһ�ļ���
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // �����ļ�
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // �������·��
                string relativeFilePath = $"{relativePath}/{uniqueFileName}";

                return new JsonResult(new { success = true, filePath = relativeFilePath });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"�ϴ�ʧ��: {ex.Message}" });
            }
        }


        public async Task<IActionResult> OnGetGetPlayerDataAsync(int playerId)
        {
            try
            {
                // ��ȡ�����Ϣ
                var player = await _playerBLL.GeyPlayerDetailsById(playerId);
                if (player == null)
                {
                    return NotFound();
                }

                // �����������
                return new JsonResult(new
                {
                    playerId = player.PlayerId,
                    playerName = player.PlayerName,
                    playerNickName = player.PlayerNickName,
                    playerTeam = player.PlayerTeam,
                    playerCover = player.PlayerCover,
                    playerTeamLogo = player.PlayerTeamLogo,
                    playerTopRanking = player.PlayerTopRanking,
                    playerPrizeMoney = player.PlayerPrizeMoney
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"��ȡ����ʧ��: {ex.Message}" })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
