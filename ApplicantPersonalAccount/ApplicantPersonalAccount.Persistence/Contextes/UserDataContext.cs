using ApplicantPersonalAccount.Persistence.Configurations.UserDb;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApplicantPersonalAccount.Persistence.Contextes
{
    public class UserDataContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public UserDataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
