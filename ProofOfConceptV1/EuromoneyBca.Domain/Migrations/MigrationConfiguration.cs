using System.Data.Entity.Migrations;
using EuromoneyBca.Domain.Migrations.Support;

namespace Euromoney.Isis.Data.Migration.Migrations
{    
    public class MigrationConfiguration<T> : DbMigrationsConfiguration<T> where T : MigrationContext
    {   
        public MigrationConfiguration(string migrationNamespace)
        {                                   
            AutomaticMigrationsEnabled = false;            
            MigrationsAssembly = GetType().Assembly;
            MigrationsNamespace = migrationNamespace;            
        }       
    }
}