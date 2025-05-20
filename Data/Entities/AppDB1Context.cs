using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Data.Entities;

public partial class AppDB1Context : DbContext
{
    public AppDB1Context()
    {
    }

    public AppDB1Context(DbContextOptions<AppDB1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<Pullout> Pullouts { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER02;database=AppDB1;User ID=admin;Password=@@Win4me@@;Trusted_Connection=true;TrustServerCertificate=True;MultipleActiveResultSets=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.ToTable("Delivery");

            entity.Property(e => e.BusinessValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.DeliveryName)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ReceiptImage)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pullout>(entity =>
        {
            entity.ToTable("Pullout");

            entity.Property(e => e.PulloutDate).HasColumnType("datetime");
            entity.Property(e => e.PulloutDescription)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PulloutName)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ReceiptImage)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Delivery).WithMany(p => p.Pullouts)
                .HasForeignKey(d => d.DeliveryId)
                .HasConstraintName("FK_Pullout_Delivery");

            entity.HasOne(d => d.Sales).WithMany(p => p.Pullouts)
                .HasForeignKey(d => d.SalesId)
                .HasConstraintName("FK_Pullout_Sales");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SalesId);

            entity.Property(e => e.BusinessValue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ReceiptImage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SalesDate).HasColumnType("datetime");
            entity.Property(e => e.SalesDescription)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SalesName)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
