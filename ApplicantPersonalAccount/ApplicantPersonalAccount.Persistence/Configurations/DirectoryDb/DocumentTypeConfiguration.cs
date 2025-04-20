using ApplicantPersonalAccount.Application.OuterServices.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace ApplicantPersonalAccount.Persistence.Configurations.DirectoryDb
{
    public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever();

            builder
                .HasOne(d => d.EducationLevel)
                .WithMany();

            builder
                .HasMany(d => d.NextEducationLevels)
                .WithMany();
        }
    }
}
