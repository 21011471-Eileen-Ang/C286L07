using System;
using System.Collections.Generic;
using Lesson07.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson07.Services;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Client { get; set; }

    public virtual DbSet<Package> Package { get; set; }

    public virtual DbSet<Pokedex> Pokedex { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Client__3214EC07BBA1468C");

            entity.Property(e => e.CustName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMode)
                .HasMaxLength(2)
                .IsUnicode(false);

            entity.HasOne(d => d.Package).WithMany(p => p.Client)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Client__PackageI__267ABA7A");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Package__3214EC07AD201C5B");

            entity.Property(e => e.PkgName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pokedex>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pokedex__3214EC07B120B5E5");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PokeName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Type1)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Type2)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
