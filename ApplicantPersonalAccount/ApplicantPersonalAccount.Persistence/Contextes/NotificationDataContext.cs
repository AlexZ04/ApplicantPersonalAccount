using ApplicantPersonalAccount.Persistence.Configurations.NotificationDb;
using ApplicantPersonalAccount.Persistence.Entities.NotificationDb;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Persistence.Contextes
{
    public class NotificationDataContext : DbContext
    {
        public DbSet<NotificationSubscribtionEntity> Subscribers { get; set; } 

        public NotificationDataContext(DbContextOptions<NotificationDataContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NotificationSubscribtionsConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
