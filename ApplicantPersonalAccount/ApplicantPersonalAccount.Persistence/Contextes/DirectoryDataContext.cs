using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Persistence.Configurations.DirectoryDb;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Persistence.Contextes
{
    public class DirectoryDataContext : DbContext
    {
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<EducationProgram> EducationPrograms { get; set; }

        public DirectoryDataContext(DbContextOptions<DirectoryDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EducationLevelConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FacultyConfiguration());
            modelBuilder.ApplyConfiguration(new EducationProgramConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
