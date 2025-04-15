using CS2PlayerSettings.Data.Repository.Models;
using CS2PlayersSettings.Data.Repository.Model;
using DemoFile;
using DemoFile.Game.Cs;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace CS2PlayersSettings.Pages.Demo
{
    public class AnalysisDemoFileModel : PageModel
    {

        // .dem file
        [BindProperty]
        public IFormFile UploadedFile { get; set; }
        public List<DemoPlayer> demoCrosshair { get; set; }
        public List<DemoDataResult> demoDataResult { get; set; }

        public static RoundData? currentRoundData { get; set; }

        public List<KillInfo> KillInfo { get; set; }
        public List<RoundData> roundDataList { get; set; }

        public GameMatch gameMatch { get; set; }

        public AnalysisDemoFileModel()
        {
            demoCrosshair = new List<DemoPlayer>();
            demoDataResult = new List<DemoDataResult>();
            KillInfo = new List<KillInfo>();
            roundDataList = new List<RoundData>();
            gameMatch = new GameMatch();
        }
        public void OnGet()
        {
        }

        /// <summary>
        /// 解析Demo文件
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostUploadDemoFileAsync()
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                return BadRequest("请上传有效文件");
            }

            if (Path.GetExtension(UploadedFile.FileName).ToLower() != ".dem")
            {
                return BadRequest("仅支持 .dem 文件");
            }
            try
            {
                try
                {
                    var demo = new CsDemoParser();

                    demo.Source1GameEvents.RoundStart += e =>
                    {

                    };
                    demo.Source1GameEvents.RoundFreezeEnd += e =>
                    {
                        currentRoundData = new RoundData();
                    };

                    demo.Source1GameEvents.PlayerDeath += e =>
                    {
                        GetPlayerDeath(e);
                    };

                    demo.Source1GameEvents.RoundEnd += e =>
                    {
                        GetRoundEnd(e, demo);
                        if (demo.GameRules.CSGamePhase != CSGamePhase.MatchEnded)
                            return;
                        GetPlayerList(demo, demo.TeamTerrorist);
                        GetPlayerList(demo, demo.TeamCounterTerrorist);
                        //GetPlayerList(demo, demo.TeamSpectator);

                    };
                    using (var stream = UploadedFile.OpenReadStream())
                    {
                        var reader = DemoFileReader.Create(demo, stream);
                        //var sw = Stopwatch.StartNew();
                        await reader.ReadAllAsync();
                        //sw.Stop();
                    }

                }
                catch (Exception ex)
                {
                    return new JsonResult(new { success = false, message = $"demo解析失败: {ex.Message}" });
                }
                return new JsonResult(new
                {
                    success = true,
                    message = $"{UploadedFile.FileName}解析成功",
                    demoDataResult = demoDataResult.OrderByDescending(d => d.PlayerDamage),
                    demoCrosshair = demoCrosshair,
                    gameMatch = gameMatch,
                    gameLog = roundDataList
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"文件上传失败: {ex.Message}" });
            }
        }




        // Get Demo Match Result
        private void GetPlayerList(CsDemoParser demo, CCSTeam team)
        {
            var players = team.CSPlayerControllers
                .OrderByDescending(player => player.ActionTrackingServices!.MatchStats.Damage)
                .ToList();

            foreach (var item in players)
            {
                var matchStats = item.ActionTrackingServices?.MatchStats ?? new CSMatchStats();
                DemoPlayer demoPlayer = new DemoPlayer
                {
                    DemoPlayerName = item.PlayerName,
                    DemoPlayerCrosshairCode = item.CrosshairCodes,
                    DemoPlayerSteamId = item.SteamID.ToString(),
                    DemoPlayerCrosshairFOV = (int)item.DesiredFOV
                };
                demoCrosshair.Add(demoPlayer);

                DemoDataResult demoResult = new DemoDataResult
                {
                    PlayerPing = item.Ping.ToString(),
                    PlayerName = item.PlayerName,
                    PlayerKills = matchStats.Kills,
                    PlayerDeaths = matchStats.Deaths,
                    PlayerAssists = matchStats.Assists,
                    PlayerTripleKills = matchStats.Enemy3Ks,
                    PlayerQuadroKills = matchStats.Enemy4Ks,
                    PlayerPentaKills = matchStats.Enemy5Ks,
                    PlayerHs = (matchStats.Kills > 0 ? matchStats.HeadShotKills * 100 / matchStats.Kills : 0).ToString(),
                    PlayerDamage = matchStats.Damage,
                };
                demoDataResult.Add(demoResult);
            }
        }

        private void GetPlayerDeath(Source1PlayerDeathEvent e)
        {
            KillInfo killInfo = new KillInfo();
            // Write attacker name in the colour of their team
            //AnsiConsole.Markup($"[{TeamNumberToString(e.Attacker?.CSTeamNum)}]{e.Attacker?.PlayerName}[/]");
            killInfo.AttackerTeam = e.Attacker?.CSTeamNum;
            string assisterName = "";
            // 有助攻者
            if (!string.IsNullOrEmpty(e.Assister?.ToString()))
            {
                assisterName = e.Assister?.ToString().Replace("(PlayerConnected)", "");
                killInfo.AssisterName = assisterName;
                if (e.Assistedflash)
                {
                    killInfo.Assistedflash = e.Assistedflash;
                }
            }
            else if (e.Attackerinair)
            {
                killInfo.Attackerinair = e.Attackerinair;
            }
            else if (e.Attackerblind && e.Attackerinair)
            {
                killInfo.Attackerblind = e.Attackerblind;
                killInfo.Attackerinair = e.Attackerinair;
            }
            killInfo.AttackerName = e.Attacker?.PlayerName;

            if (e.Thrusmoke)
            {
                killInfo.Thrusmoke = e.Thrusmoke;
            }
            if (e.Noscope)
            {
                killInfo.Noscope = e.Noscope;
            }
            killInfo.Weapon = e.Weapon;
            killInfo.Headshot = e.Headshot;

            // Write the victim's name in the colour of their team
            //AnsiConsole.MarkupLine($"[{TeamNumberToString(e.Player?.CSTeamNum)}]{e.Player?.PlayerName}[/]");
            killInfo.VictimTeam = e.Player?.CSTeamNum;
            killInfo.VictimName = e.Player?.PlayerName;
            currentRoundData.Kills.Add(killInfo);
        }

        private void GetRoundEnd(Source1RoundEndEvent e, CsDemoParser demo)
        {
            demo.Source1GameEvents.RoundStart += e =>
            {
                currentRoundData.RoundStart = "回合开始";
            };
            var roundEndReason = (CSRoundEndReason)e.Reason;

            var winningTeam = (CSTeamNumber)e.Winner switch
            {
                CSTeamNumber.Terrorist => demo.TeamTerrorist,
                CSTeamNumber.CounterTerrorist => demo.TeamCounterTerrorist,
                _ => null
            };
            currentRoundData.RoundEndReason = roundEndReason.ToString();
            currentRoundData.Winner = (CSTeamNumber)e.Winner;
            currentRoundData.TeamName = winningTeam.Teamname;
            currentRoundData.WinningTeamName = winningTeam?.ClanTeamname;
            currentRoundData.TerroristTeamName = demo.TeamTerrorist.ClanTeamname;
            currentRoundData.TerroristScore = demo.TeamTerrorist.Score;
            currentRoundData.TerroristTeamLogo = demo.TeamCounterTerrorist.TeamLogoImage;
            currentRoundData.CounterTerroristTeamName = demo.TeamCounterTerrorist.ClanTeamname;
            currentRoundData.CounterTerroristScore = demo.TeamCounterTerrorist.Score;
            currentRoundData.CounterTerroristTeamLogo = demo.TeamCounterTerrorist.TeamFlagImage;


            roundDataList.Add(currentRoundData);
            var score = roundDataList.LastOrDefault();
            gameMatch.MapName = demo.ServerInfo.MapName.ToLower();
            gameMatch.HostName = demo.ServerInfo.GameSessionConfig.ServerIpAddress;
            gameMatch.TerroristName = score.TerroristTeamName;
            gameMatch.TerroristScore = score.TerroristScore;
            gameMatch.CounterTerroristName = score.CounterTerroristTeamName;
            gameMatch.CounterTerroristScore = score.CounterTerroristScore;
        }

        private static string TeamNumberToString(CSTeamNumber? csTeamNumber) => csTeamNumber switch
        {
            CSTeamNumber.Terrorist => "#fab200",
            CSTeamNumber.CounterTerrorist => "#0091d4",
            _ => "bold white",
        };

    }
}
