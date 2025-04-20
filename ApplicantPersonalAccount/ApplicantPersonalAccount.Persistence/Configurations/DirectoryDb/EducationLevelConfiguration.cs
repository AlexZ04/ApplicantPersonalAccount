using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.DirectoryDb
{
    public class EducationLevelConfiguration : IEntityTypeConfiguration<EducationLevel>
    {
        public void Configure(EntityTypeBuilder<EducationLevel> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).ValueGeneratedNever();
        }
    }
}
