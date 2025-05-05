using System;
using System.Collections.Generic;

namespace CS2PlayersSettings.Data.Repository.Entities.Players;

public partial class ViewmodelSetting
{
    public int ViewmodelId { get; set; }

    public int PlayerId { get; set; }

    public double? ViewmodelFov { get; set; }

    public double? ViewmodelOffsetX { get; set; }

    public double? ViewmodelOffsetY { get; set; }

    public double? ViewmodelOffsetZ { get; set; }

    public double? ViewmodelPresetpos { get; set; }

    public string? ViewmodelCode { get; set; }

    public DateTime? ViewmodelLastUpdateTime { get; set; }

    public virtual Player Player { get; set; } = null!;
}
