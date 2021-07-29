using ITGDevices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices.Data
{
    public class DeviceContext : DbContext
    {
        public DeviceContext(DbContextOptions<DeviceContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<UserItemRequest> UserItemRequest { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<CategoryItem> CategoryItem { get; set; }


        public DbSet<UserItem> UserItem { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<UserItemRequest>().ToTable("UserItemRequest");
            modelBuilder.Entity<Role>().ToTable("role");
            modelBuilder.Entity<UserRole>().ToTable("userRole");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Item>().ToTable("Item");
            modelBuilder.Entity<CategoryItem>().ToTable("CategoryItem");

            modelBuilder.Entity<User>(entity => {
                entity.HasIndex(e => e.username).IsUnique();
            });
            modelBuilder.Entity<Item>(entity => {
                entity.HasIndex(e => e.SerialNumber).IsUnique();
            });
        }
    }
}
