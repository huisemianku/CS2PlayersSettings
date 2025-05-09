using System;
using System.Collections.Generic;
using CS2PlayersSettings.Data.Repository.Entities.Players;
using Microsoft.EntityFrameworkCore;

namespace CS2PlayersSettings.Data.Repository;

public partial class PlayerDbContext : DbContext
{
    public PlayerDbContext()
    {
    }

    public PlayerDbContext(DbContextOptions<PlayerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CrosshairSetting> CrosshairSettings { get; set; }

    public virtual DbSet<Follower> Followers { get; set; }

    public virtual DbSet<MouseSetting> MouseSettings { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VideoSetting> VideoSettings { get; set; }

    public virtual DbSet<ViewmodelSetting> ViewmodelSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CrosshairSetting>(entity =>
        {
            entity.HasKey(e => e.CrosshairId).HasName("PK__Crosshai__ABB07AE55BD76979");

            entity.Property(e => e.CrosshairId).HasColumnName("crosshair_id");
            entity.Property(e => e.CrosshairCode).HasMaxLength(50);
            entity.Property(e => e.CrosshairCover).HasMaxLength(50);
            entity.Property(e => e.CrosshairLastUpdateTime).HasColumnType("datetime");
            entity.Property(e => e.CrosshairStyle).HasMaxLength(10);
            entity.Property(e => e.PlayerId).HasColumnName("player_id");

            entity.HasOne(d => d.Player).WithMany(p => p.CrosshairSettings)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Crosshair__playe__4E88ABD4");
        });

        modelBuilder.Entity<Follower>(entity =>
        {
            entity.HasKey(e => e.FollowerId).HasName("PK__Follower__E3B0943A35DA329D");

            entity.HasIndex(e => e.PlayersId, "idx_player_id");

            entity.HasIndex(e => e.UsersId, "idx_user_id");

            entity.HasIndex(e => new { e.UsersId, e.PlayersId }, "unique_follow").IsUnique();

            entity.Property(e => e.FollowerId).HasColumnName("Follower_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.PlayersId).HasColumnName("Players_id");
            entity.Property(e => e.UsersId).HasColumnName("Users_id");

            entity.HasOne(d => d.Players).WithMany(p => p.Followers)
                .HasForeignKey(d => d.PlayersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Followers__Playe__57A801BA");

            entity.HasOne(d => d.Users).WithMany(p => p.Followers)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Followers__Users__56B3DD81");
        });

        modelBuilder.Entity<MouseSetting>(entity =>
        {
            entity.HasKey(e => e.MouseId).HasName("PK__MouseSet__2711F300042F7733");

            entity.Property(e => e.MouseId).HasColumnName("Mouse_Id");
            entity.Property(e => e.MouseDpi).HasColumnName("MouseDPI");
            entity.Property(e => e.MouseLastUpdateTime).HasColumnType("datetime");
            entity.Property(e => e.MouseName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Player).WithMany(p => p.MouseSettings)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MouseSett__Playe__403A8C7D");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PK__Player__4A4E74C803A1DA30");

            entity.ToTable("Player");

            entity.Property(e => e.PlayerBirthday).HasColumnType("datetime");
            entity.Property(e => e.PlayerCountry)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PlayerCountryImage)
                .HasMaxLength(120)
                .HasColumnName("playerCountryImage");
            entity.Property(e => e.PlayerCover)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.PlayerName).HasMaxLength(50);
            entity.Property(e => e.PlayerNickName).HasMaxLength(30);
            entity.Property(e => e.PlayerPrizeMoney).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PlayerTeam).HasMaxLength(50);
            entity.Property(e => e.PlayerTeamLogo)
                .HasMaxLength(120)
                .HasColumnName("playerTeamLogo");

            entity.HasOne(d => d.Team).WithMany(p => p.Players)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Player_TeamId");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK__Team__123AE79958565DCF");

            entity.ToTable("Team");

            entity.HasIndex(e => e.TeamName, "UQ__Team__4E21CAACA6DF1435").IsUnique();

            entity.Property(e => e.TeamImg).HasMaxLength(120);
            entity.Property(e => e.TeamName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CCA7FEE91");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4F1AE69EE").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053415B585B9").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Roles)
                .HasMaxLength(20)
                .HasDefaultValue("user");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<VideoSetting>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__VideoSet__E8F11E102CC78CB3");

            entity.Property(e => e.VideoId).HasColumnName("video_id");
            entity.Property(e => e.AspectRatio).HasMaxLength(20);
            entity.Property(e => e.DisplayMode).HasMaxLength(20);
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.Resolution).HasMaxLength(20);
            entity.Property(e => e.ScalingMode).HasMaxLength(20);
            entity.Property(e => e.VideoLastUpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Player).WithMany(p => p.VideoSettings)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoSett__playe__04E4BC85");
        });

        modelBuilder.Entity<ViewmodelSetting>(entity =>
        {
            entity.HasKey(e => e.ViewmodelId).HasName("PK__Viewmode__9C9E9381FEBD5127");

            entity.Property(e => e.ViewmodelId).HasColumnName("viewmodel_id");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.ViewmodelCode)
                .HasMaxLength(115)
                .HasColumnName("viewmodelCode");
            entity.Property(e => e.ViewmodelFov).HasColumnName("viewmodelFov");
            entity.Property(e => e.ViewmodelLastUpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("viewmodelLastUpdateTime");
            entity.Property(e => e.ViewmodelOffsetX).HasColumnName("viewmodelOffsetX");
            entity.Property(e => e.ViewmodelOffsetY).HasColumnName("viewmodelOffsetY");
            entity.Property(e => e.ViewmodelOffsetZ).HasColumnName("viewmodelOffsetZ");
            entity.Property(e => e.ViewmodelPresetpos).HasColumnName("viewmodelPresetpos");

            entity.HasOne(d => d.Player).WithMany(p => p.ViewmodelSettings)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Viewmodel__playe__5FB337D6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
