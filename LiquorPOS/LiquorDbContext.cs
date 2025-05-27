using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiquorPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace LiquorPOS
{
    public class LiquorDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Barcode> Barcodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // IMPORTANT: Replace with your actual connection details!
            string connectionString = "Host=100.71.49.34;Port=5432;Database=liquor_store_pos;Username=postgres;Password=postgres";
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // You can add more detailed configuration here if needed,
            // but often EF Core can figure things out from the Models.
            base.OnModelCreating(modelBuilder);
        }
    }
}