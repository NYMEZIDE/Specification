using Microsoft.EntityFrameworkCore;
using Specifications.IntegrationTests.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Specifications.IntegrationTests.Infrastructure
{
    public class OrdersDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Orders.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(builder =>
            {
                builder.HasKey(o => o.Id);
                builder.Property(o => o.Status)
                    .HasConversion(
                        o => o.ToString(),
                        o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o));

                builder.HasMany(o => o.Products).WithOne().HasForeignKey(p => p.OrderId);
            });

            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasKey(p => new { p.OrderId, p.Id });
            });
        }
    }
}
