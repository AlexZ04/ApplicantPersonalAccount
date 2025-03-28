using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.UserDb
{
    internal class InfoForEventsConfiguration : IEntityTypeConfiguration<InfoForEventsEntity>
    {
        public void Configure(EntityTypeBuilder<InfoForEventsEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.EducationPlace)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(u => u.SocialNetwork)
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasOne(e => e.User)
               .WithOne(u => u.InfoForEvents)
               .HasForeignKey<InfoForEventsEntity>(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
