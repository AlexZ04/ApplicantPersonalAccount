using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.UserDb
{
    internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(u => u.Email)
                .IsRequired();

            builder.Property(u => u.Phone)
                .IsRequired();

            builder.Property(u => u.Gender)
                .IsRequired();

            builder.Property(u => u.Birthdate)
                .IsRequired();

            builder.Property(u => u.Address)
                .IsRequired(false);

            builder.Property(u => u.Citizenship)
                .IsRequired(false);

            builder.Property(u => u.Password)
                .IsRequired();

            builder.Property(u => u.CreateTime)
                .IsRequired();

            builder.HasOne(u => u.InfoForEvents)
               .WithOne(e => e.User)
               .HasForeignKey<InfoForEventsEntity>(e => e.UserId)
               .IsRequired();
        }
    }
}
