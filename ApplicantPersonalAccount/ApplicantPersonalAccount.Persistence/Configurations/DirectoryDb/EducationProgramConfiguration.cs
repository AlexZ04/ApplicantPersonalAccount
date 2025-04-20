using ApplicantPersonalAccount.Application.OuterServices.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.DirectoryDb
{
    public class EducationProgramConfiguration : IEntityTypeConfiguration<EducationProgram>
    {
        public void Configure(EntityTypeBuilder<EducationProgram> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
