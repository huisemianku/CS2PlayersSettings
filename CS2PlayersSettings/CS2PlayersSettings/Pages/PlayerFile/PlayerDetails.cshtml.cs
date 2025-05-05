using CS2PlayersSettings.Business;
using CS2PlayersSettings.Data.Repository.Entities.Players;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CS2PlayersSettings.Pages.PlayerFile
{
    public class PlayerDetailsModel : PageModel
    {
        private readonly PlayerBLL _playerBLL;
        public Player Player { get; set; } = null!;

        public PlayerDetailsModel(PlayerBLL playerBLL)
        {
            _playerBLL = playerBLL;
        }
        public async Task OnGet(int playerId)
        {
            Player =  await _playerBLL.GeyPlayerDetailsById(playerId);
        }
    }
}
