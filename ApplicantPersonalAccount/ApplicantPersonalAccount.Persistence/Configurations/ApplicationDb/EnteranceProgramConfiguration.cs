using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.ApplicationDb
{
    public class EnteranceProgramConfiguration : IEntityTypeConfiguration<EnteranceProgramEntity>
    {
        public void Configure(EntityTypeBuilder<EnteranceProgramEntity> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
