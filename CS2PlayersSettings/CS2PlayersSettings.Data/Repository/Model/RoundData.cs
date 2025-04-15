using DemoFile.Game.Cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.Repository.Model
{
    public class RoundData
    {
        /// <summary>
        /// 该回合的所有击杀信息列表
        /// </summary>
        public List<KillInfo> Kills { get; set; } = new List<KillInfo>();

        /// <summary>
        /// 回合结束的原因 (例如：CTsWin, TerroristsWin, TimeExpired)
        /// </summary>
        public string? RoundEndReason { get; set; }
        /// <summary>
        /// 回合开始
        /// </summary>
        public string? RoundStart { get; set; }

        /// <summary>
        /// 获胜队伍的名称 (ClanTeamname + Teamname)
        /// </summary>
        public string? WinningTeamName { get; set; }


        public string? TeamName { get; set; }

        /// <summary>
        /// 获胜队伍的阵营 (Terrorist 或 CounterTerrorist)
        /// </summary>
        public CSTeamNumber Winner { get; set; }

        /// <summary>
        /// 恐怖分子的得分
        /// </summary>
        public int TerroristScore { get; set; }

        /// <summary>
        /// 反恐精英的得分
        /// </summary>
        public int CounterTerroristScore { get; set; }

        /// <summary>
        /// 恐怖分子队伍的名称 (ClanTeamname)
        /// </summary>
        public string? TerroristTeamName { get; set; }

        /// <summary>
        /// 反恐精英队伍的名称 (ClanTeamname)
        /// </summary>
        public string? CounterTerroristTeamName { get; set; }
        /// <summary>
        /// 恐怖分子队伍的Logo (ClanTeamname)
        /// </summary>
        public string? TerroristTeamLogo { get; set; }

        /// <summary>
        /// 反恐精英队伍的Logo (ClanTeamname)
        /// </summary>
        public string? CounterTerroristTeamLogo { get; set; }
    }

    public class KillInfo
    {
        /// <summary>
        /// 攻击者的队伍阵营 (Terrorist 或 CounterTerrorist)
        /// </summary>
        public CSTeamNumber? AttackerTeam { get; set; }

        /// <summary>
        /// 攻击者的游戏昵称
        /// </summary>
        public string? AttackerName { get; set; }

        public bool Assister { get; set; }

        /// <summary>
        /// 助攻者的游戏昵称
        /// </summary>
        public string? AssisterName { get; set; }

        /// <summary>
        /// 使用的武器名称
        /// </summary>
        public string? Weapon { get; set; }

        /// <summary>
        /// 是否爆头
        /// </summary>
        public bool Headshot { get; set; }

        /// <summary>
        /// 受害者的队伍阵营 (Terrorist 或 CounterTerrorist)
        /// </summary>
        public CSTeamNumber? VictimTeam { get; set; }

        /// <summary>
        /// 受害者的游戏昵称
        /// </summary>
        public string? VictimName { get; set; }

        /// <summary>
        /// 进攻闪光 Assistedflash
        /// </summary>
        public bool Attackerblind { get; set; }
        /// <summary>
        /// 助攻闪光
        /// </summary>
        public bool Assistedflash{get; set;}
        /// <summary>
        /// 空中击杀
        /// </summary>
        public bool Attackerinair { get; set; }
        /// <summary>
        /// 混烟击杀
        /// </summary>
        public bool Thrusmoke { get; set; }
        /// <summary>
        /// 盲狙
        /// </summary>
        public bool Noscope { get; set; }
    }
}
