using CS2PlayersSettings.Business;
using CS2PlayersSettings.Data.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CS2PlayersSettings.Pages.PlayerFile
{
    public class PlayerAddModel : PageModel
    {
        private readonly PlayerBLL _playerBLL;
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image/FigureCovers");
        [BindProperty]
        public Player Player { get; set; } = null!;
        public PlayerAddModel(PlayerBLL playerBLL, IWebHostEnvironment environment)
        {
            _playerBLL = playerBLL;
            _environment = environment;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAddPlayerAsync([FromBody] Player player)
        {
            if (player != null)
            {
                int playerId = await _playerBLL.AddPlayerAsync(player);
                return new JsonResult(new { success = true, message = "添加成功！", playerId = playerId });
            }
            return new JsonResult(new { success = false, message = "添加失败！" });
        }

        public async Task<IActionResult> OnPostUploadImageOnlyAsync(IFormFile file, string imageType)
        {
            if (file == null || file.Length == 0)
            {
                return new JsonResult(new { success = false, message = "请上传有效文件" });
            }
            try
            {
                // 根据imageType确定上传目录
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
                    return new JsonResult(new { success = false, message = "无效的图片类型" });
                }

                // 确保目录存在
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // 创建唯一文件名
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // 保存文件
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 返回相对路径
                string relativeFilePath = $"{relativePath}/{uniqueFileName}";

                return new JsonResult(new { success = true, filePath = relativeFilePath });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"上传失败: {ex.Message}" });
            }
        }

        public IActionResult OnPostRevert([FromBody] string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return BadRequest(new { success = false, message = "文件路径不能为空" });
                }

                // 从文件路径中获取文件名
                var fileName = Path.GetFileName(filePath);
                var fullPath = Path.Combine(_uploadPath, fileName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return new JsonResult(new { success = true, message = "文件已删除" });
                }

                return new JsonResult(new { success = false, message = "文件不存在" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"删除文件失败: {ex.Message}" });
            }
        }
    }
}
