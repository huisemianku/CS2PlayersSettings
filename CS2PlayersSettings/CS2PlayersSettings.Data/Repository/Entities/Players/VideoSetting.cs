using System;
using System.Collections.Generic;

namespace CS2PlayersSettings.Data.Repository.Entities.Players;

public partial class VideoSetting
{
    public int VideoId { get; set; }

    public int PlayerId { get; set; }

    public string? Resolution { get; set; }

    public string? AspectRatio { get; set; }

    public string? ScalingMode { get; set; }

    public string? DisplayMode { get; set; }

    public DateTime? VideoLastUpdateTime { get; set; }

    public virtual Player Player { get; set; } = null!;
}
