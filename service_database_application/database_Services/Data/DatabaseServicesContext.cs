using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain_Model_App;

namespace database_Services.Data
{
    public class DatabaseServicesContext : DbContext
    {
        public DatabaseServicesContext (DbContextOptions<DatabaseServicesContext> options)
            : base(options)
        {

        }

        public DbSet<Domain_Model_App.Medic> Medic { get; set; }

        public DbSet<Domain_Model_App.Pacient> Pacient { get; set; }

        public DbSet<Domain_Model_App.Procedure> Procedure { get; set; }

        public DbSet<Domain_Model_App.User> User { get; set; }

    }
}
