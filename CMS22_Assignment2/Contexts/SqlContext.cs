using CMS22_Assignment2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CMS22_Assignment2.Contexts
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderRowEntity>().HasKey(or => new { or.OrderId, or.ProductId });
        }

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderRowEntity> OrdersRows { get; set; }
        public DbSet<CustomerAddressEntity> Addresses { get; set; }
    }
}
