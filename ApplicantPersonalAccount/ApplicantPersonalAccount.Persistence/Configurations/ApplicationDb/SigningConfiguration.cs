using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.ApplicationDb
{
    public class SigningConfiguration : IEntityTypeConfiguration<SignedToNotificationsEntity>
    {
        public void Configure(EntityTypeBuilder<SignedToNotificationsEntity> builder)
        {
            builder.HasKey(s => s.Id);
        }
    }
}
