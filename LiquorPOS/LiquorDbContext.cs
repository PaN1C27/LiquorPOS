using LiquorPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace LiquorPOS;

public class LiquorDbContext : DbContext
{
    // ctor used by DI – options carry the connection string
    public LiquorDbContext(DbContextOptions<LiquorDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Barcode> Barcodes => Set<Barcode>();
    public DbSet<TaxComponent> TaxComponents => Set<TaxComponent>();
    public DbSet<ProductTaxComponent> ProductTaxComponents => Set<ProductTaxComponent>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<TaxComponent>()
          .ToTable("TaxComponent");                     // table name

        mb.Entity<TaxComponent>()
          .Property(tc => tc.ComponentId)
          .HasColumnName("component_id");

        mb.Entity<TaxComponent>()
          .Property(tc => tc.Name)
          .HasColumnName("name");

        mb.Entity<TaxComponent>()
          .Property(tc => tc.Rate)
          .HasColumnName("rate");

        mb.Entity<ProductTaxComponent>()
          .ToTable("ProductTaxComponent")
          .HasKey(l => new { l.CodeNum, l.ComponentId });

        mb.Entity<ProductTaxComponent>()
          .Property(l => l.CodeNum)
          .HasColumnName("code_num");

        mb.Entity<ProductTaxComponent>()
          .Property(l => l.ComponentId)
          .HasColumnName("component_id");

        // primary keys
        mb.Entity<Product>().HasKey(p => p.CodeNum);
        mb.Entity<TaxComponent>()
      .HasKey(tc => tc.ComponentId);          //  ← NEW
        mb.Entity<ProductTaxComponent>()
          .HasKey(l => new { l.CodeNum, l.ComponentId });

        // relationships
        mb.Entity<ProductTaxComponent>()
          .HasOne(l => l.Product)
          .WithMany()
          .HasForeignKey(l => l.CodeNum);

        mb.Entity<ProductTaxComponent>()
          .HasOne(l => l.TaxComponent)
          .WithMany(c => c.ProductLinks)
          .HasForeignKey(l => l.ComponentId);
    }
}
