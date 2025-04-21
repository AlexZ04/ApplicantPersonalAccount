using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.DocumentDb
{
    public class EducationInfoConfiguration : IEntityTypeConfiguration<EducationInfoEntity>
    {
        public void Configure(EntityTypeBuilder<EducationInfoEntity> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
