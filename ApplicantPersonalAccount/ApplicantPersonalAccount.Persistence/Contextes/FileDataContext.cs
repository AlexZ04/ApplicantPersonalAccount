using ApplicantPersonalAccount.Persistence.Configurations.DocumentDb;
using ApplicantPersonalAccount.Persistence.Entities.DocumentDb;
using Microsoft.EntityFrameworkCore;

namespace ApplicantPersonalAccount.Persistence.Contextes
{
    public class FileDataContext : DbContext
    {
        public DbSet<DocumentEntity> Documents { get; set; }

        public FileDataContext(DbContextOptions<FileDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
