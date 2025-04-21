using ApplicantPersonalAccount.Persistence.Configurations.DocumentDb;
using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Persistence.Contextes
{
    public class FileDataContext : DbContext
    {
        public DbSet<DocumentEntity> Documents { get; set; }
        public DbSet<PassportInfoEntity> PassportInfos { get; set; }
        public DbSet<EducationInfoEntity> EducationInfos { get; set; }

        public FileDataContext(DbContextOptions<FileDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new PassportConfiguration());
            modelBuilder.ApplyConfiguration(new EducationInfoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
