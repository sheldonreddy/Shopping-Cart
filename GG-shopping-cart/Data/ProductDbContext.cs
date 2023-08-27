using Microsoft.EntityFrameworkCore;
using GG_shopping_cart.Entities;

namespace GG_shopping_cart.Data
{
	public class ProductDbContext: DbContext
	{
		public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options)
		{
		}
		public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<LineItem> LineItems { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .HasIndex(c => c.UserSession)
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LineItem>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LineItem>()
                .HasOne(p => p.Cart)
                .WithMany()
                .HasForeignKey(p => p.CartId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

