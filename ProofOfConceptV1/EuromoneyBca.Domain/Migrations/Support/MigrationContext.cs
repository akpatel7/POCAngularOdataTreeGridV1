using System.Data.Entity;

namespace EuromoneyBca.Domain.Migrations.Support
{
    public class MigrationContext : DbContext
    {
        public static string ConnectionString;

        public MigrationContext() : base(ConnectionString)
        {
        }

        public MigrationContext(string connectionString) : base(connectionString)
        {            
        }
    }
}

