using CS2_PlayerSettings.Data.Repository.Model;
using CS2PlayersSettings.Business.WebApi;
using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository.Model;
using Microsoft.AspNetCore.Mvc;

namespace CS2PlayersSettings.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly PlayerWebApiBLL _playerSettingsBLL;
        public PlayerController(PlayerWebApiBLL playerSettingsBLL)
        {
            _playerSettingsBLL = playerSettingsBLL;
        }

        [HttpGet]
        [Route("getAllPlayers")]
        public async Task<List<Player>> getAllPlayers()
        {
            return await _playerSettingsBLL.GetAllPlayersAsync();
        }

        [HttpGet]
        [Route("GetAllPlayersPage")]
        public async Task<PagedResult<Player>> GetAllPlayersPage(int page, int pageSize, string search = "")
        {
            return await _playerSettingsBLL.GetAllPlayersPageAsync(page, pageSize, search);
        }

        [HttpGet]
        [Route("GetPlayerSettingById")]
        public async Task<PlayerSettingsDto> GetPlayerSettingById(int playerId)
        {
            return await _playerSettingsBLL.GetPlayerSettingById(playerId);
        }

        [HttpGet]
        [Route("GetPlayersByNickName")]
        public async Task<List<Player>> GetPlayersByNickName(string nickName)
        {
            return await _playerSettingsBLL.GetPlayersByNickName(nickName);
        }

        [HttpGet]
        [Route("GetTeamPlayes")]
        public async Task<List<Player>> GetTeamPlayes(int teamId)
        {
            return await _playerSettingsBLL.GetTeamPlayes(teamId);
        }
        [HttpGet]
        [Route("GetTopTenteamList")]
        public async Task<List<Team>> GetTopTenteamList()
        {
            return await _playerSettingsBLL.GetTopTenteamList();
        }

        [HttpGet]
        [Route("GetPlayersById")]
        public async Task<Player> GetPlayersById(int playerId)
        {
            return await _playerSettingsBLL.GetPlayersById(playerId);
        }
    }
}
