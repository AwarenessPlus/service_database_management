using Microsoft.EntityFrameworkCore;

namespace DatabaseServices.Data
{
    public class DatabaseServicesContext : DbContext
    {
        public DatabaseServicesContext(DbContextOptions<DatabaseServicesContext> options)
            : base(options)
        {
        }

        public DbSet<DomainModel.Authentication> Authentication { get; set; }

        public DbSet<DomainModel.Medic> Medic { get; set; }

        public DbSet<DomainModel.Patient> Patient { get; set; }

        public DbSet<DomainModel.Procedure> Procedure { get; set; }
        
        public DbSet<DomainModel.User> User { get; set; }

        public DbSet<DomainModel.VideoFile> VideoFile { get; set; }
    }
}