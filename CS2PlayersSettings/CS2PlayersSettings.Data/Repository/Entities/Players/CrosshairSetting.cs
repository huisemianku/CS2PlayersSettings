using System;
using System.Collections.Generic;

namespace CS2PlayersSettings.Data.Repository.Entities.Players;

public partial class CrosshairSetting
{
    public int CrosshairId { get; set; }

    public int PlayerId { get; set; }

    public string? CrosshairCover { get; set; }

    public double? CrosshairSize { get; set; }

    public double? CrosshairThickness { get; set; }

    public double? CrosshairGap { get; set; }

    public string? CrosshairStyle { get; set; }

    public int? CrosshairColorRed { get; set; }

    public int? CrosshairColorGreen { get; set; }

    public int? CrosshairColorBlue { get; set; }

    public string? CrosshairCode { get; set; }

    public DateTime? CrosshairLastUpdateTime { get; set; }

    public virtual Player Player { get; set; } = null!;
}
