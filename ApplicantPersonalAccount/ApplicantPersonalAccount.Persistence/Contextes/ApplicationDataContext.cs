using ApplicantPersonalAccount.Persistence.Configurations.ApplicationDb;
using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Persistence.Contextes
{
    public class ApplicationDataContext : DbContext
    {
        public DbSet<SignedToNotificationsEntity> SignedToNotifications { get; set; }

        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SigningConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
