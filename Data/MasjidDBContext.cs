using GravyFoodsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.Data
{
    public class MasjidDBContext : DbContext
    {
        public MasjidDBContext(DbContextOptions<MasjidDBContext> options) : base(options)
        {
        }

        public DbSet<UserInfo> UserInfo { get; set; } = null!;
        public DbSet<Country> Country { get; set; }
        public DbSet<POSSubscription> POSSubscription { get; set; }
        public DbSet<Logging> Logging { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductSerial> ProductSerials { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductUnits> ProductUnits { get; set; }
        public DbSet<BranchInfo> BranchInfo { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }

        public DbSet<CustomerInfo> CustomerInfo { get; set; }

        public DbSet<SalesInfo> SalesInfo { get; set; }
        public DbSet<SalesDetails> SalesDetails { get; set; }
        public DbSet<ExpenseHead> ExpenseHead { get; set; }
        public DbSet<ExpenseInfo> ExpenseInfo { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>()
                .HasKey(c => new { c.UserId });

            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);


            // Relationships
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Attributes)
                .WithOne(a => a.Product)
                .HasForeignKey(a => a.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Serials)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Stocks)
                .WithOne(st => st.Product)
                .HasForeignKey(st => st.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Variants)
                .WithOne(v => v.Product)
                .HasForeignKey(v => v.ProductId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Unit)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UnitId);

            // Set custom PK for SalesInfo
            modelBuilder.Entity<CustomerInfo>()
                .HasKey(s => s.CustomerId);


            // Set custom PK for SalesInfo
            modelBuilder.Entity<SalesInfo>()
                .HasKey(s => s.SalesId);

            // Relationship: SalesInfo (1) -> SalesDetails (many)
            modelBuilder.Entity<SalesDetails>()
                .HasOne(d => d.SalesInfo)
                .WithMany(m => m.SalesDetails)
                .HasForeignKey(d => d.SalesId);

            // Optional: Configure Customer relationship if CustomerInfo.CustomerId is the PK
            modelBuilder.Entity<SalesInfo>()
                .HasOne(s => s.CustomerInfo)
                .WithMany() // assuming no navigation collection in CustomerInfo
                .HasForeignKey(s => s.CustomerId);



            base.OnModelCreating(modelBuilder);

        }




    }
}