using System;
using System.Collections.Generic;
using CS2PlayersSettings.Data.Repository.Entities.Navbars;
using Microsoft.EntityFrameworkCore;

namespace CS2PlayersSettings.Data.Repository;

public partial class NavbarDbContext : DbContext
{
    public NavbarDbContext()
    {
    }

    public NavbarDbContext(DbContextOptions<NavbarDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NavigationItem> NavigationItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NavigationItem>(entity =>
        {
            entity.HasKey(e => e.NavId).HasName("PK__Navigati__67283A538DA7FA9D");

            entity.Property(e => e.NavActive).HasDefaultValue(false);
            entity.Property(e => e.NavDisabled).HasDefaultValue(false);
            entity.Property(e => e.NavIcon).HasMaxLength(255);
            entity.Property(e => e.NavLabel).HasMaxLength(255);
            entity.Property(e => e.NavTarget).HasMaxLength(50);
            entity.Property(e => e.NavUrl).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
