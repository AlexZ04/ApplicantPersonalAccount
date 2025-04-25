using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.ApplicationDb
{
    public class EnteranceConfiguration : IEntityTypeConfiguration<EnteranceEntity>
    {
        public void Configure(EntityTypeBuilder<EnteranceEntity> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
