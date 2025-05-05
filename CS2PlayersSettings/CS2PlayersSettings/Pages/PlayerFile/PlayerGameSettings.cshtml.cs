using CS2PlayersSettings.Business;
using CS2PlayersSettings.Data.Repository.DemoCrosshairCode;
using CS2PlayersSettings.Data.Repository.DemoCrosshairCode.Structs;
using CS2PlayersSettings.Data.Repository.Entities.Players;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CS2PlayersSettings.Pages.PlayerFile
{
    public class PlayerGameSettingsModel : PageModel
    {
        private readonly PlayerBLL _playerBll;


        public string PlayerNickName { get; set; } = null!;
        [BindProperty]
        public MouseSetting MouseSettings { get; set; } = null!;
        [BindProperty]
        public CrosshairSetting CrosshairSettings { get; set; } = null!;
        public ViewmodelSetting ViewmodelSettings { get; set; } = null!;
        public VideoSetting VideoSettings { get; set; } = null!;
        public PlayerGameSettingsModel(PlayerBLL playerBLL)
        {
            _playerBll = playerBLL;
        }
        public async Task OnGet(int playerId)
        {
            if (playerId != 0)
            {
                // Get Player NickName By PlayerId
                PlayerNickName = await _playerBll.GetPlayerNickNameById(playerId);
                // Get Player Game Settings By PlayerId
                MouseSettings = await _playerBll.GetMouseSettingById(playerId);
                CrosshairSettings = await _playerBll.GetCrosshairSettingById(playerId);
                AnalyticCrosshairs(CrosshairSettings);
                ViewmodelSettings = await _playerBll.GetViewmodelSettingById(playerId);
                VideoSettings = await _playerBll.GetVideoSettingById(playerId);
            }
        }

        // Update Mouse Setting
        public async Task<IActionResult> OnPostUpdateMouseAsync([FromBody] MouseSetting mouseSettings)
        {
            try
            {
                if (mouseSettings == null || mouseSettings.PlayerId == 0 || mouseSettings.MouseId == 0)
                {
                    return new JsonResult(new { success = false, message = "Invalid data" });
                }
                await _playerBll.updatePlayerMouseSettingsById(mouseSettings.PlayerId, mouseSettings.MouseId, mouseSettings);
                return new JsonResult(new { success = true, message = "Settings updated successfully" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        // Update Crosshair Setting
        public async Task<IActionResult> OnPostUpdateCrosshairAsync([FromBody] CrosshairSetting crosshairSetting)
        {
            try
            {
                if (crosshairSetting == null || crosshairSetting.PlayerId == 0 || crosshairSetting.CrosshairId == 0)
                {
                    return new JsonResult(new { success = false, message = "Invalid data" });
                }
                await _playerBll.updatePlayerCrosshairSettingsById(crosshairSetting.PlayerId, crosshairSetting.CrosshairId, crosshairSetting);
                return new JsonResult(new { success = true, message = "Settings updated successfully" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        // Update Viewmodel Setting
        public async Task<IActionResult> OnPostUpdateViewmodelAsync([FromBody] ViewmodelSetting viewmodelSetting)
        {
            try
            {
                if (viewmodelSetting == null || viewmodelSetting.PlayerId == 0 || viewmodelSetting.ViewmodelId == 0)
                {
                    return new JsonResult(new { success = false, message = "Invalid data" });
                }
                await _playerBll.updatePlayerViewmodelSettingsById(viewmodelSetting.PlayerId, viewmodelSetting.ViewmodelId, viewmodelSetting);
                return new JsonResult(new { success = true, message = "Settings updated successfully" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        // Update Video Setting
        public async Task<IActionResult> OnPostUpdateVideoAsync([FromBody] VideoSetting videoSetting)
        {
            try
            {
                if (videoSetting == null || videoSetting.PlayerId == 0 || videoSetting.VideoId == 0)
                {
                    return new JsonResult(new { success = false, message = "Invalid data" });
                }
                //await _playerBll.updatePlayerVideoSettingsById(videoSetting.PlayerId, videoSetting.VideoId, videoSetting);
                return new JsonResult(new { success = true, message = "Settings updated successfully" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        private void AnalyticCrosshairs(CrosshairSetting crosshairSetting)
        {
            Crosshair analyticCrosshair = CS2ShareCodeConverter.DecodeCrosshairShareCode(crosshairSetting.CrosshairCode);
            crosshairSetting.CrosshairSize = analyticCrosshair.cl_crosshairsize;
            crosshairSetting.CrosshairThickness = analyticCrosshair.cl_crosshairthickness;
            crosshairSetting.CrosshairGap = analyticCrosshair.cl_crosshairgap;
            switch (analyticCrosshair.cl_crosshairstyle)
            {
                case 0:
                    crosshairSetting.CrosshairStyle = "Classic";
                    break;
                case 1:
                    crosshairSetting.CrosshairStyle = "Classic";
                    break;
                case 2:
                    crosshairSetting.CrosshairStyle = "Classic";
                    break;
                case 3:
                    crosshairSetting.CrosshairStyle = "Classic";
                    break;
                case 4:
                    crosshairSetting.CrosshairStyle = "Classic static";
                    break;
                case 5:
                    crosshairSetting.CrosshairStyle = "Legacy";
                    break;
                default:
                    break;
            }
            crosshairSetting.CrosshairColorRed = analyticCrosshair.cl_crosshaircolor_r;
            crosshairSetting.CrosshairColorGreen = analyticCrosshair.cl_crosshaircolor_g;
            crosshairSetting.CrosshairColorBlue = analyticCrosshair.cl_crosshaircolor_b;
        }
    }
}
