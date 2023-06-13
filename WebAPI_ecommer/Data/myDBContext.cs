using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_ecommer.Models;

namespace WebAPI_ecommer.Data
{
    public class myDBContext: DbContext
    {
        

        public myDBContext(DbContextOptions<myDBContext> options) : base(options){ }

        #region DbSet
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> ProductCategories { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<WebAPI_ecommer.Models.CategoryModel> CategoryModel { get; set; }
        //public DbSet<User> ProductStatistics { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorProduct> VendorProducts { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<VendorProduct>()
            .HasKey(vp => new { vp.VendorId, vp.ProductId });

        }
        #endregion
    }
}
