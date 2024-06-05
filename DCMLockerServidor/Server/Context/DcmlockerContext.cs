using System;
using System.Collections.Generic;
using DCMLockerServidor.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DCMLockerServidor.Server.Context;

public partial class DcmlockerContext : DbContext
{
    public DcmlockerContext()
    {
    }

    public DcmlockerContext(DbContextOptions<DcmlockerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Box> Boxes { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Locker> Lockers { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //=> optionsBuilder.UseMySql("server=localhost;database=DCMLocker;user=root;password=m}'ZzHJcD(5*vYy,rSp8kb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
    //=> optionsBuilder.UseMySql("server=localhost;database=DCMLocker;user=root;password=anselmo", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
    //=> optionsBuilder.UseMySql("server=localhost;database=DCMLocker;user=root;password=pedro1029", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Box>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("boxes")
                .UseCollation("utf8mb4_unicode_520_ci");

            entity.HasIndex(e => e.IdLocker, "FK_Boxes_IdLocker");

            entity.HasIndex(e => e.IdSize, "FK_Boxes_IdSize");

            entity.Property(e => e.Box1).HasColumnName("Box");
            entity.Property(e => e.LastUpdateTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasColumnType("text");

            entity.HasOne(d => d.IdLockerNavigation).WithMany(p => p.Boxes)
                .HasForeignKey(d => d.IdLocker)
                .HasConstraintName("FK_Boxes_IdLocker");

            entity.HasOne(d => d.IdSizeNavigation).WithMany(p => p.Boxes)
                .HasForeignKey(d => d.IdSize)
                .HasConstraintName("FK_Boxes_IdSize");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("empresas")
                .UseCollation("utf8mb4_unicode_520_ci");

            entity.Property(e => e.Nombre).HasColumnType("text");
            entity.Property(e => e.TokenEmpresa).HasColumnType("text");
        });

        modelBuilder.Entity<Locker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("lockers")
                .UseCollation("utf8mb4_unicode_520_ci");

            entity.HasIndex(e => e.Empresa, "FK_Lockers_Empresa");

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.LastUpdateTime).HasColumnType("datetime");
            entity.Property(e => e.NroSerieLocker).HasColumnType("text");
            entity.Property(e => e.Status).HasColumnType("text");

            entity.HasOne(d => d.EmpresaNavigation).WithMany(p => p.Lockers)
                .HasForeignKey(d => d.Empresa)
                .HasConstraintName("FK_Lockers_Empresa");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sizes")
                .UseCollation("utf8mb4_unicode_520_ci");

            entity.Property(e => e.Nombre).HasColumnType("text");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("tokens")
                .UseCollation("utf8mb4_unicode_520_ci");

            entity.HasIndex(e => e.IdBox, "FK_Tokens_IdBox");

            entity.HasIndex(e => e.IdLocker, "FK_Tokens_IdLocker");

            entity.HasIndex(e => e.IdSize, "FK_Tokens_IdSize");

            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.Modo).HasColumnType("text");
            entity.Property(e => e.Token1).HasColumnName("Token");

            entity.HasOne(d => d.IdBoxNavigation).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.IdBox)
                .HasConstraintName("FK_Tokens_IdBox");

            entity.HasOne(d => d.IdLockerNavigation).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.IdLocker)
                .HasConstraintName("FK_Tokens_IdLocker");

            entity.HasOne(d => d.IdSizeNavigation).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.IdSize)
                .HasConstraintName("FK_Tokens_IdSize");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
