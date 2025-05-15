using ApplicantPersonalAccount.Persistence.Entities.NotificationDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.NotificationDb
{
    internal class NotificationSubscribtionsConfiguration :
        IEntityTypeConfiguration<NotificationSubscribtionEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationSubscribtionEntity> builder)
        {
            builder.HasKey(s => s.Id);
        }
    }
}
