using System;
using System.Collections.Generic;

namespace CS2PlayersSettings.Data.Repository.Entities;

public partial class MouseSetting
{
    public int MouseId { get; set; }

    public int PlayerId { get; set; }

    public string? MouseName { get; set; }

    public int? MouseDpi { get; set; }

    public double? MouseSensitivity { get; set; }

    public double? MouseEdpi { get; set; }

    public double? MouseZoomSensitivity { get; set; }

    public int? MouseHz { get; set; }

    public int? WindowsSensitivity { get; set; }

    public DateTime? MouseLastUpdateTime { get; set; }

    public virtual Player Player { get; set; } = null!;
}
