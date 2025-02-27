

using Microsoft.EntityFrameworkCore;
using ProfileAss.Model;

namespace ProfileAss.Data
{
    public class DatabaseContext : DbContext
    {

        public DbSet<Profile> person { get; set; }

        public DbSet<ProductItem> products { get; set; }
        
        public DbSet<Basket> baskets  { get; set; }
        public DbSet<BasketItem> basketItems { get; set; }
   
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) {
            // Ensure the database is created and migrated
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Explicitly map  to the "person" table
            modelBuilder.Entity<Profile>().ToTable("person");
            modelBuilder.Entity<ProductItem>().ToTable("product");
            //map the basket tables
            modelBuilder.Entity<Basket>().ToTable("basket");
            modelBuilder.Entity<BasketItem>().ToTable("basket_item");

            // Update the Basket-Profile relationship configuration
            modelBuilder.Entity<Basket>()
                .HasOne(b => b.Profile)
                .WithMany(p => p.Baskets)  // Specify the navigation property
                .HasForeignKey(b => b.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Basket>()
                .HasMany(b => b.BasketItems)
                .WithOne(bi => bi.Basket)
                .HasForeignKey(bi => bi.BasketId);

            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.ProductItem)
                .WithMany()
                .HasForeignKey(bi => bi.ProductItemId);
        }
    }
}
