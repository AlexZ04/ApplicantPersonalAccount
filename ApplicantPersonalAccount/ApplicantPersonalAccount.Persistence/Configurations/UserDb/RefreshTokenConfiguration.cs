using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantPersonalAccount.Persistence.Configurations.UserDb
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Token)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(t => t.Expires)
                .IsRequired();
        }
    }
}
