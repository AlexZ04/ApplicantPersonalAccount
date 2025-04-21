using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.DocumentDb
{
    public class PassportConfiguration : IEntityTypeConfiguration<PassportInfoEntity>
    {
        public void Configure(EntityTypeBuilder<PassportInfoEntity> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
