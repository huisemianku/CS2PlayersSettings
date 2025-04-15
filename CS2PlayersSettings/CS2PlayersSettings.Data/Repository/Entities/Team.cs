using System;
using System.Collections.Generic;

namespace CS2PlayersSettings.Data.Repository.Entities;

public partial class Team
{
    public int TeamId { get; set; }

    public string TeamName { get; set; } = null!;

    public int? TeamRanking { get; set; }

    public string? TeamImg { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
