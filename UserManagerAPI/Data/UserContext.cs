using UserManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace UserManagerAPI.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext( DbContextOptions<UserContext> opt ) : base( opt )
        {
            
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.Entity<User>()
                .HasIndex( u => u.Username )
                .IsUnique();
        }
    }
}
