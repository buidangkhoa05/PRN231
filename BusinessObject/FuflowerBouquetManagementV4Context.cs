using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObject;

public partial class FuflowerBouquetManagementV4Context : DbContext
{
    public FuflowerBouquetManagementV4Context()
    {
    }

    public FuflowerBouquetManagementV4Context(DbContextOptions<FuflowerBouquetManagementV4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<FlowerBouquet> FlowerBouquets { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config.GetConnectionString("DefaultConnection");
        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Customer__A4AE64B84B19B6D4");

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.AccountStatus).HasDefaultValue((byte)1);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2BCF154F64");

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
        });

        modelBuilder.Entity<FlowerBouquet>(entity =>
        {
            entity.HasKey(e => e.FlowerBouquetId).HasName("PK__FlowerBo__A13913016CFABD6D");

            entity.Property(e => e.FlowerBouquetId).ValueGeneratedNever();

            entity.HasOne(d => d.Category).WithMany(p => p.FlowerBouquets).HasConstraintName("FK__FlowerBou__Categ__2F10007B");

            entity.HasOne(d => d.Supplier).WithMany(p => p.FlowerBouquets)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__FlowerBou__Suppl__300424B4");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF66918AA5");

            entity.Property(e => e.OrderStatus).IsFixedLength();

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Order__CustomerI__30F848ED");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.FlowerBouquetId }).HasName("PK__OrderDet__C983CA9F53E38433");

            entity.HasOne(d => d.FlowerBouquet).WithMany(p => p.OrderDetails).HasConstraintName("FK__OrderDeta__Flowe__31EC6D26");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__32E0915F");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.SupplierId).ValueGeneratedNever();
            entity.Property(e => e.Telephone).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
