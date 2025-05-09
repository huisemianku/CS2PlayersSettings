using System;
using System.Collections.Generic;

namespace CS2PlayersSettings.Data.Repository.Entities.Players;

public partial class Follower
{
    public int FollowerId { get; set; }

    public int UsersId { get; set; }

    public int PlayersId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Player Players { get; set; } = null!;

    public virtual User Users { get; set; } = null!;
}
