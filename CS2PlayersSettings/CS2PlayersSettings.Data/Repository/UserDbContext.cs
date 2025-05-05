using System;
using System.Collections.Generic;
using CS2PlayersSettings.Data.Repository.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace CS2PlayersSettings.Data.Repository;

public partial class UserDbContext : DbContext
{
    public UserDbContext()
    {
    }

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
