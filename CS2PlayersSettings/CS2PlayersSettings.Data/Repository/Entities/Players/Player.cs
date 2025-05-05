using System;
using System.Collections.Generic;

namespace CS2PlayersSettings.Data.Repository.Entities.Players;

public partial class Player
{
    public int PlayerId { get; set; }

    public string? PlayerName { get; set; }

    public string? PlayerNickName { get; set; }

    public string? PlayerCover { get; set; }

    public int? PlayerTopRanking { get; set; }

    public DateTime? PlayerBirthday { get; set; }

    public string? PlayerCountry { get; set; }

    public decimal? PlayerPrizeMoney { get; set; }

    public string? PlayerTeam { get; set; }

    public string? PlayerTeamLogo { get; set; }

    public string? PlayerCountryImage { get; set; }

    public int? TeamId { get; set; }

    public virtual ICollection<CrosshairSetting> CrosshairSettings { get; set; } = new List<CrosshairSetting>();

    public virtual ICollection<MouseSetting> MouseSettings { get; set; } = new List<MouseSetting>();

    public virtual Team? Team { get; set; }

    public virtual ICollection<VideoSetting> VideoSettings { get; set; } = new List<VideoSetting>();

    public virtual ICollection<ViewmodelSetting> ViewmodelSettings { get; set; } = new List<ViewmodelSetting>();
}
