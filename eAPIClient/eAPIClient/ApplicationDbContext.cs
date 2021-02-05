using System;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eAPIClient.Models;


namespace eAPIClient
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //xxx

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(16,4)");
            }

            foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(string)))
            {
                property.SetCollation("Khmer_100_BIN");
            }
        }

        public DbSet<DocumentNumberModel> DocumentNumbers { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ConfigDataModel> ConfigDatas { get; set; }
        public DbSet<MenuModel> Menus { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductMenuModel> ProductMenus { get; set; }
        public DbSet<ProductPrinterModel> ProductPrinters { get; set; }
        public DbSet<ProductModifierModel> ProductModifiers { get; set; }


    }

}