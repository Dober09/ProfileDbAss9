

using Microsoft.EntityFrameworkCore;
using ProfileAss.Model;

namespace ProfileAss.Data
{
    public class DatabaseContext : DbContext
    {

        private readonly string _databasePath;
        public DbSet<Profile> person { get; set; }
        
   
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) {
            // Ensure the database is created and migrated
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Explicitly map  to the "person" table
            modelBuilder.Entity<Profile>().ToTable("person");  
        }
    }
}
