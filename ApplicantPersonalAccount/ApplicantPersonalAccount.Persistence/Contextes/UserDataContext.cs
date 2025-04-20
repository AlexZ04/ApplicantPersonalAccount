using ApplicantPersonalAccount.Persistence.Configurations.UserDb;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Persistence.Contextes
{
    public class UserDataContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<InfoForEventsEntity> InfoForEvents { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new InfoForEventsConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
