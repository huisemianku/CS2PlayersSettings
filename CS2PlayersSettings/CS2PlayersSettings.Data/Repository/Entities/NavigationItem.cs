using System;
using System.Collections.Generic;

namespace CS2PlayersSettings.Data.Repository.Entities;

public partial class NavigationItem
{
    public int NavId { get; set; }

    public string NavLabel { get; set; } = null!;

    public string NavUrl { get; set; } = null!;

    public string? NavTarget { get; set; }

    public bool? NavDisabled { get; set; }

    public bool? NavActive { get; set; }

    public string? NavIcon { get; set; }

    public int? NavOrder { get; set; }
}
